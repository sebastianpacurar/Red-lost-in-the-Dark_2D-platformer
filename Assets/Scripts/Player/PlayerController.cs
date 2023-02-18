using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Player {
    public class PlayerController : MonoBehaviour {
        [Header("OverlapShape Checkers")]
        [SerializeField] private Transform groundChecker;
        [SerializeField] private Transform wallChecker;
        [SerializeField] private LayerMask groundLayer;

        [Header("Move Related X-Axis")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float xInputVal;

        [Header("Move Related Y-Axis")]
        [SerializeField] private bool isGrounded;
        [SerializeField] private float jumpForce;

        [Header("WallJumping Related")]
        [SerializeField] private Vector2 wallJumpForce;
        [SerializeField] private bool isWallActive;
        [SerializeField] private bool isSliding;
        [SerializeField] private float wallSlidingSpeed;
        [SerializeField] private bool isWallJumping;
        [SerializeField] private bool isWallJumpInProgress;
        [SerializeField] private float wallJumpDuration;
        [SerializeField] private float wallJumpDirection;
        [SerializeField] private bool isWallClimbing;

        private PlayerControls _controls;
        private Rigidbody2D _rb;
        private Animator _animator;
        private SpriteRenderer _sr;
        private AnimationState _state;

        [SerializeField] private bool isFalling;

        private static readonly int State = Animator.StringToHash("state");

        private void Awake() {
            _controls = new PlayerControls();
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sr = GetComponent<SpriteRenderer>();
        }

        private void Update() {
            isGrounded = Physics2D.OverlapCapsule(groundChecker.position, new Vector2(0.425f, 0.05f), CapsuleDirection2D.Horizontal, 0, groundLayer);
            isWallActive = Physics2D.OverlapCapsule(wallChecker.position, new Vector2(0.05f, 0.425f), CapsuleDirection2D.Vertical, 0, groundLayer);

            if (isGrounded) {
                isFalling = false;
            }

            if (isWallActive && !isGrounded) {
                isSliding = true;
            } else {
                isSliding = false;
            }

            if (isSliding) {
                isFalling = false;
                _sr.flipX = true;
            } else {
                _sr.flipX = false;
            }
        }

        private void FixedUpdate() {
            if (isSliding) {
                // allow to slide upwards if simple jump is triggered while facing a wall, else prevent the player from sliding upwards
                var maxY = isWallClimbing ? float.MaxValue : 0f;
                _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -wallSlidingSpeed, maxY));
            }

            if (isWallJumping) {
                // set the next condition to true
                isWallJumpInProgress = true;

                // set direction only once, the first time when isWallJumping is triggered as true
                if (wallJumpDirection.Equals(0)) wallJumpDirection = -transform.localScale.x;

                // add wall jump force
                _rb.AddForce(new Vector2(wallJumpForce.x * wallJumpDirection, wallJumpForce.y), ForceMode2D.Impulse);
            } else if (isWallJumpInProgress) {
                // apply jump force 
                _rb.velocity = new Vector2(wallJumpForce.x * wallJumpDirection, _rb.velocity.y);

                // set the current condition to false, and set direction to 0, when player is in mid air
                if (_rb.velocity.y <= 7.5f) {
                    isWallJumpInProgress = false;
                    wallJumpDirection = 0f;
                }
            } else {
                _rb.velocity = new Vector2(xInputVal * moveSpeed, _rb.velocity.y);
            }

            // clamp ascend speed to 10f;
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, float.MinValue, 10f));

            // reduce Air Time through gravity value
            if (_rb.velocity.y < 0f && !isGrounded) {
                _rb.gravityScale = 1.5f;
            } else {
                _rb.gravityScale = 1f;
            }

            FlipPlayerScale();
            HandleAnimations();
        }

        private void Move(InputAction.CallbackContext ctx) {
            switch (ctx.phase) {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    xInputVal = _controls.Player.Move.ReadValue<float>();
                    break;
                case InputActionPhase.Canceled:
                    xInputVal = 0f;
                    break;
            }
        }

        // TODO: issue with jumping when next to wall (
        private void Jump(InputAction.CallbackContext ctx) {
            if (isGrounded) {
                // if grounded apply simple jump
                _rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

                // perform slide upwards when jumping when facing a wall close by
                if (isWallActive) isWallClimbing = true;
            } else if (isSliding) {
                isWallClimbing = false;
                // if sliding start wallJumping process
                isWallJumping = true;
                Invoke(nameof(StopWallJump), wallJumpDuration);
            }
        }

        // prevent double wall jump in mid air
        private void StopWallJump() {
            isWallJumping = false;
        }

        private void HandleAnimations() {
            _state = AnimationState.Idle;
            if (_rb.velocity.x != 0f && isGrounded) _state = AnimationState.Run;
            if (_rb.velocity.y > 0f && !isGrounded) _state = AnimationState.Ascend;
            if (_rb.velocity.y < 0f && !isGrounded) _state = AnimationState.Descend;
            if (isFalling) _state = AnimationState.Fall;
            if (isSliding) _state = AnimationState.Slide;
            _animator.SetInteger(State, (int)_state);
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
            _controls.Player.Move.performed += Move;
            _controls.Player.Move.canceled += Move;
            _controls.Player.Jump.performed += Jump;
        }

        private void OnDisable() {
            _controls.Player.Move.performed -= Move;
            _controls.Player.Move.canceled -= Move;
            _controls.Player.Jump.performed -= Jump;
            _controls.Player.Move.Disable();
            _controls.Player.Jump.Disable();
        }

        // called in Descend Animation as event
        public void SetIsFallingTrue() {
            isFalling = true;
        }
    }
}