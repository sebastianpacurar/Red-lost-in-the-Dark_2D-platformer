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
        [SerializeField] private float wallJumpDuration;
        [SerializeField] private float wallSlidingSpeed;

        [ReadOnlyProp] [SerializeField] private float xInputVal;
        [ReadOnlyProp] [SerializeField] private float cachedXInputVal;
        [ReadOnlyProp] [SerializeField] private bool isGrounded;
        [ReadOnlyProp] [SerializeField] private bool isWallActive;
        [ReadOnlyProp] [SerializeField] private bool isWallClimbing;
        [ReadOnlyProp] [SerializeField] private bool isSliding;
        [ReadOnlyProp] [SerializeField] private float wallSlideMaxSpeed;
        [ReadOnlyProp] [SerializeField] private bool isWallJumpInitiated;
        [ReadOnlyProp] [SerializeField] private bool isWallJumpInProgress;
        [ReadOnlyProp] [SerializeField] private float wallJumpDirection;
        [ReadOnlyProp] [SerializeField] private bool isFalling;

        [ReadOnlyProp] [SerializeField] private bool isAttacking;

        private PlayerControls _controls;
        private CinemachineVirtualCamera _cineMachineCam;
        private Rigidbody2D _rb;
        private Animator _animator;
        private SpriteRenderer _sr;

        private static readonly int State = Animator.StringToHash("state");

        private void Awake() {
            _controls = new PlayerControls();
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Start() {
            _cineMachineCam = GameObject.FindGameObjectWithTag("CM2D").GetComponent<CinemachineVirtualCamera>();
            _cineMachineCam.Follow = transform;
        }

        private void Update() {
            isGrounded = Physics2D.OverlapCapsule(groundChecker.position, new Vector2(0.425f, 0.05f), CapsuleDirection2D.Horizontal, 0, groundLayer);
            isWallActive = Physics2D.OverlapCapsule(wallChecker.position, new Vector2(0.05f, 0.425f), CapsuleDirection2D.Vertical, 0, groundLayer);

            if (isGrounded) {
                isFalling = false;
            }

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
                isFalling = false;
                _sr.flipX = true;
            } else if (isAttacking) {
                _sr.flipX = true;
            } else {
                _sr.flipX = false;
            }

            HandleAnimations();
        }

        private void FixedUpdate() {
            if (isSliding) {
                // allow to slide upwards if simple jump is triggered while facing a wall, else prevent the player from sliding upwards
                wallSlideMaxSpeed = isWallClimbing ? float.MaxValue : 0f;
                _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -wallSlidingSpeed, wallSlideMaxSpeed));
            }

            if (isWallJumpInitiated) {
                // set the next condition to true
                isWallJumpInProgress = true;

                // set direction only once, the first time when isWallJumpInitiated is triggered as true
                if (wallJumpDirection.Equals(0)) wallJumpDirection = -transform.localScale.x;

                // add wall jump force
                _rb.AddForce(new Vector2(wallJumpForce.x * wallJumpDirection, wallJumpForce.y), ForceMode2D.Impulse);
            } else if (isWallJumpInProgress) {
                // apply jump force 
                _rb.velocity = new Vector2(wallJumpForce.x * wallJumpDirection, _rb.velocity.y);

                // set the current condition to false, and set direction to 0, when player is in mid air
                if (_rb.velocity.y <= 9.5f) {
                    isWallJumpInProgress = false;
                    wallJumpDirection = 0f;
                }
            } else {
                _rb.velocity = new Vector2(xInputVal * moveSpeed, _rb.velocity.y);
            }

            // clamp ascend speed to 12f;
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, float.MinValue, 12f));

            // reduce Air Time through gravity value
            if (_rb.velocity.y < 0f && !isGrounded) {
                _rb.gravityScale = 2f;
            } else if (_rb.velocity.y > 5f && _rb.velocity.y > 0f) {
                _rb.gravityScale = 1.75f;
            } else {
                _rb.gravityScale = 1f;
            }

            FlipPlayerScale();
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
            if (isGrounded && !isAttacking) {
                // if grounded apply simple jump
                _rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

                // perform slide upwards when jumping when facing a wall close by
                if (isWallActive) isWallClimbing = true;
            } else if (isSliding) {
                isWallClimbing = false;
                // if sliding start wallJumping process
                isWallJumpInitiated = true;
                Invoke(nameof(StopWallJump), wallJumpDuration);
            }
        }

        // prevent double wall jump in mid air
        private void StopWallJump() {
            isWallJumpInitiated = false;
        }

        private void HandleAnimations() {
            var state = AnimationState.Idle;

            if (xInputVal != 0f && isGrounded) state = AnimationState.Run;
            if (_rb.velocity.y > 0f && !isGrounded) state = AnimationState.Ascend;
            if (_rb.velocity.y < 0f && !isGrounded) state = AnimationState.Descend;
            if (isFalling) state = AnimationState.Fall;
            if (isSliding) state = AnimationState.Slide;

            if (isAttacking) {
                state = Random.Range(6, 9) switch {
                    6 => AnimationState.FirstAttack,
                    7 => AnimationState.SecondAttack,
                    8 => AnimationState.ThirdAttack,
                    _ => AnimationState.FirstAttack
                };
            }

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

        private void OnEnable() {
            _controls.Player.Move.Enable();
            _controls.Player.Jump.Enable();
            _controls.Player.Attack.Enable();
            _controls.Player.Move.performed += Move;
            _controls.Player.Move.canceled += Move;
            _controls.Player.Jump.performed += Jump;
            _controls.Player.Attack.performed += Attack;
        }

        private void OnDisable() {
            _controls.Player.Move.performed -= Move;
            _controls.Player.Move.canceled -= Move;
            _controls.Player.Jump.performed -= Jump;
            _controls.Player.Attack.performed -= Attack;
            _controls.Player.Attack.Disable();
            _controls.Player.Move.Disable();
            _controls.Player.Jump.Disable();
        }

        // called in Descend Animation as event
        public void SetIsFallingTrue() {
            isFalling = true;
        }

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
            ThirdAttack
        }
    }
}