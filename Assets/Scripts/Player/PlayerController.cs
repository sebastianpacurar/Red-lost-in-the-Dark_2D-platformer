using Cinemachine;
using CustomAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Player {
    public class PlayerController : MonoBehaviour {
        [SerializeField] private Transform groundChecker;
        [SerializeField] private Transform wallChecker;
        [SerializeField] private LayerMask groundLayer;

        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpForce;

        [SerializeField] private Vector2 wallJumpForce;
        [SerializeField] private Vector2 wallClimbForce;
        [SerializeField] private float wallClimbDefaultDuration;
        [SerializeField] private float wallJumpDefaultDuration;
        [SerializeField] private float wallSlidingSpeed;
        [SerializeField] private float maxAscendSpeed;

        [ReadOnlyProp] [SerializeField] private bool isJumpPressed;

        [ReadOnlyProp] [SerializeField] private float xInputVal;
        [ReadOnlyProp] [SerializeField] private float cachedXInputVal;
        [ReadOnlyProp] [SerializeField] private bool isGrounded;
        [ReadOnlyProp] [SerializeField] private bool isWallActive;
        [ReadOnlyProp] [SerializeField] private bool isSliding;
        [ReadOnlyProp] [SerializeField] private float wallSlideMaxSpeed;
        [ReadOnlyProp] [SerializeField] private float wallJumpDirection;
        [ReadOnlyProp] [SerializeField] private bool isWallJumpInit;
        [ReadOnlyProp] [SerializeField] private float wallJumpDurationCd;
        [ReadOnlyProp] [SerializeField] private bool canAutoWallJump;
        [ReadOnlyProp] [SerializeField] private bool isWallClimbInit;
        [ReadOnlyProp] [SerializeField] private float wallClimbDurationCd;
        [ReadOnlyProp] [SerializeField] private float wallClimbCurrDur;
        [ReadOnlyProp] [SerializeField] private bool canAutoWallClimb;
        [ReadOnlyProp] [SerializeField] private bool isAscending;
        [ReadOnlyProp] [SerializeField] private bool isFalling;

        [ReadOnlyProp] [SerializeField] private bool isAttacking;
        [ReadOnlyProp] [SerializeField] private bool isDead;
        [ReadOnlyProp] [SerializeField] private Vector3 checkpointPos;

        private PlayerControls _controls;
        private CinemachineVirtualCamera _cineMachineCam;
        private Rigidbody2D _rb;
        private Animator _animator;
        private SpriteRenderer _sr;
        private HandleHpSanity _stats;
        private RaycastHit2D _hitLeft;
        private RaycastHit2D _hitRight;

        private static readonly int State = Animator.StringToHash("state");

        private void Awake() {
            _controls = new PlayerControls();
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sr = GetComponent<SpriteRenderer>();
            _stats = GetComponent<HandleHpSanity>();
        }


        private void Start() {
            _cineMachineCam = GameObject.FindGameObjectWithTag("CM2D").GetComponent<CinemachineVirtualCamera>();
            _cineMachineCam.Follow = transform;

            checkpointPos = transform.position;
        }

        private void Update() {
            isGrounded = Physics2D.OverlapCapsule(groundChecker.position, new Vector2(0.425f, 0.05f), CapsuleDirection2D.Horizontal, 0, groundLayer);
            isWallActive = Physics2D.OverlapCapsule(wallChecker.position, new Vector2(0.05f, 0.425f), CapsuleDirection2D.Vertical, 0, groundLayer);

            var pos = transform.position;
            _hitLeft = Physics2D.Raycast(pos, Vector2.left, 5.1f, groundLayer);
            _hitRight = Physics2D.Raycast(pos, Vector2.right, 5.1f, groundLayer);

            HandleWallClimbDurationVal();

            // if (isGrounded) {
            //     isFalling = false;
            // }

            if (isAttacking) {
                xInputVal = 0f;
                _sr.flipX = true;
            } else {
                xInputVal = cachedXInputVal;
                _sr.flipX = false;
            }

            if (isWallActive && !isGrounded) {
                isSliding = true;
            } else {
                isSliding = false;
            }

            if (isSliding) {
                // isFalling = false;
                _sr.flipX = true;
            } else if (isAttacking) {
                _sr.flipX = true;
            } else {
                _sr.flipX = false;
            }

            if (_stats.HealthPoints == 0) {
                isDead = true;
            }

            HandleAnimations();
            HandleWallClimbDurationCd();
            HandleWallJumpDurationCd();
        }

        private void FixedUpdate() {
            // jump section
            if (isGrounded && !isAttacking && isJumpPressed) {
                // if grounded apply simple jump
                _rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }

            if (isSliding && isJumpPressed) {
                if (_hitLeft && _hitRight && !isWallClimbInit) {
                    isWallClimbInit = true;
                } else if ((!_hitLeft || !_hitRight) && !isWallJumpInit) {
                    isWallJumpInit = true;
                } else {
                    _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -wallSlidingSpeed, wallSlideMaxSpeed));
                }
            }

            // NOTE: Treat Wall Jump and Wall Climb differently. 
            if (isWallJumpInit) {
                // Wall Jump
                canAutoWallJump = true;

                if (wallJumpDirection.Equals(0)) {
                    wallJumpDirection = -transform.localScale.x;
                }

                if (wallJumpDurationCd < wallJumpDefaultDuration) {
                    _rb.AddForce(new Vector2(wallJumpForce.x * wallJumpDirection, wallJumpForce.y), ForceMode2D.Impulse);
                } else {
                    _rb.velocity = new Vector2(xInputVal * moveSpeed, _rb.velocity.y);
                }
            } else if (isWallClimbInit) {
                // Wall Climb
                canAutoWallClimb = true;

                if (wallJumpDirection.Equals(0)) {
                    wallJumpDirection = -transform.localScale.x;
                }

                _rb.AddForce(new Vector2(wallClimbForce.x * wallJumpDirection, wallClimbForce.y), ForceMode2D.Impulse);
            } else {
                _rb.velocity = new Vector2(xInputVal * moveSpeed, _rb.velocity.y);
            }

            // clamp ascend speed to 12f;
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, float.MinValue, maxAscendSpeed));

            // reduce Air Time through gravity value
            _rb.gravityScale = _rb.velocity.y switch {
                < 0f when !isGrounded => 2f,
                > 5f and > 0f => 1.75f,
                _ => 1f
            };

            // prevent player from sliding upwards the wall
            if (isSliding && _rb.velocity.y > 0f) {
                _rb.velocity = new Vector2(_rb.velocity.x, 0f);
            }

            var product = Vector2.Dot(_rb.velocity, wallJumpForce);

            isAscending = product > 0f && !isSliding && !isGrounded;
            isFalling = product < -10f && !isSliding && !isGrounded;
            Debug.Log(Vector2.Dot(_rb.velocity, wallJumpForce));

            // block inputs and set booleans handled by animations to false
            if (isDead) {
                _rb.velocity = Vector2.zero;
                isFalling = false;
                isSliding = false;
                isAttacking = false;
            }

            FlipPlayerScale();
        }

        private void HandleWallClimbDurationCd() {
            if (canAutoWallClimb) {
                wallClimbDurationCd += Time.deltaTime;

                if (wallClimbDurationCd > wallClimbCurrDur) {
                    isWallClimbInit = false;
                    canAutoWallClimb = false;
                    wallClimbDurationCd = 0f;
                    wallJumpDirection = 0f;
                }
            }
        }

        private void HandleWallJumpDurationCd() {
            if (canAutoWallJump) {
                wallJumpDurationCd += Time.deltaTime;

                if (wallJumpDurationCd > wallJumpDefaultDuration * 2) {
                    isWallJumpInit = false;
                    canAutoWallJump = false;
                    wallJumpDurationCd = 0f;
                    wallJumpDirection = 0f;
                }
            }
        }


        // NOTE: Increases/Decreases wallClimbDuration in case there are narrow halls to climb upwards
        // NOTE: Value is provided based on the distance between the walls
        private void HandleWallClimbDurationVal() {
            // in case both walls are detected, make value dynamic, else set to default
            if (_hitLeft && _hitRight) {
                var dist = _hitLeft.distance + _hitRight.distance;

                wallClimbCurrDur = dist switch {
                    < 2 => 0.275f, // 2 tiles distance between walls
                    > 2f and < 3f => 0.375f, // 3 tiles distance between walls
                    > 3f and < 5f => 0.525f, // 4 tiles distance between walls
                    _ => wallClimbDefaultDuration
                };
            } else {
                wallClimbCurrDur = wallClimbDefaultDuration;
            }
        }

        // set the checkpoint on the next triggered torch
        // TODO: marked torches should also be treated, so previous checkpoints should remain inactive
        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("CheckpointTorch")) {
                if (!col.transform.position.Equals(checkpointPos)) {
                    checkpointPos = col.transform.position;
                }
            }
        }


        private void Move(InputAction.CallbackContext ctx) {
            switch (ctx.phase) {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    xInputVal = _controls.Player.Move.ReadValue<float>();
                    cachedXInputVal = xInputVal;
                    break;
                case InputActionPhase.Canceled:
                    xInputVal = 0f;
                    cachedXInputVal = 0f;
                    break;
            }
        }

        private void Attack(InputAction.CallbackContext ctx) {
            if (isGrounded && !isAttacking) isAttacking = true;
        }

        private void Jump(InputAction.CallbackContext ctx) {
            switch (ctx.phase) {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    isJumpPressed = true;
                    break;
                case InputActionPhase.Canceled:
                    isJumpPressed = false;
                    break;
            }
        }

        private void HandleAnimations() {
            var state = AnimationState.Idle;

            if (xInputVal != 0f && isGrounded) state = AnimationState.Run;

            // state = _rb.velocity.y switch {
            //     > 0f when !isGrounded => AnimationState.Ascend,
            //     < 0f and > -1f when !isGrounded => AnimationState.Descend,
            //     _ => state
            // };

            if (isSliding) state = AnimationState.Slide;
            if (isAscending) state = AnimationState.Ascend;
            if (isFalling) state = AnimationState.Fall;

            if (isAttacking) {
                state = Random.Range(6, 9) switch {
                    6 => AnimationState.FirstAttack,
                    7 => AnimationState.SecondAttack,
                    8 => AnimationState.ThirdAttack,
                    _ => AnimationState.FirstAttack
                };
            }

            if (isDead) state = AnimationState.Death;

            _animator.SetInteger(State, (int)state);
        }

        // x is -1   if   facing left
        // x is  1   if   facing right
        private void FlipPlayerScale() {
            if (_rb.velocity.x < 0f) {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            } else if (_rb.velocity.x > 0f) {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        // in case the player dies, restart from last checkpoint
        public void RestartFromCheckpoint() {
            transform.position = checkpointPos;
            _stats.HealthPoints = _stats.maxHp;
            isDead = false;
        }

        private void OnEnable() {
            _controls.Player.Move.Enable();
            _controls.Player.Jump.Enable();
            _controls.Player.Attack.Enable();
            _controls.Player.Move.performed += Move;
            _controls.Player.Move.canceled += Move;
            _controls.Player.Jump.performed += Jump;
            _controls.Player.Jump.canceled += Jump;
            _controls.Player.Attack.performed += Attack;
        }

        private void OnDisable() {
            _controls.Player.Move.performed -= Move;
            _controls.Player.Move.canceled -= Move;
            _controls.Player.Jump.performed -= Jump;
            _controls.Player.Jump.canceled -= Jump;
            _controls.Player.Attack.performed -= Attack;
            _controls.Player.Attack.Disable();
            _controls.Player.Move.Disable();
            _controls.Player.Jump.Disable();
        }

        //TODO:  broke entire animator because of this
        // // called in Descend Animation as event
        // public void SetIsFallingTrue() {
        //     isFalling = true;
        // }

        // called in every Attack animation as event
        public void StopAttackEvent() {
            isAttacking = false;
        }

        private enum AnimationState {
            Idle,
            Run,
            Ascend,
            Descend,
            Fall,
            Slide,
            FirstAttack,
            SecondAttack,
            ThirdAttack,
            Death
        }
    }
}