using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerController : MonoBehaviour {
        private PlayerAnimState _animState;
        private PlayerControls _controls;
        private Rigidbody2D _rb;
        private Animator _animator;
        private static readonly int State = Animator.StringToHash("state");

        [Header("Shape Casts")]
        [SerializeField] private Transform groundCheckerPos;
        [SerializeField] private Transform wallCheckerPos;
        [SerializeField] private Transform wallEdgeChecker;
        [SerializeField] private LayerMask groundLayer;

        [Space(10)]
        [Header("Physics Related")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float climbSpeed;

        [Space(10)]
        [Header("For Debugging Purposes")]
        [SerializeField] private float moveVal;
        [SerializeField] private bool isJumpPressed;

        [Space(10)]
        [Header("Free Jump Related")]
        [SerializeField] private bool isAscending;
        [SerializeField] private bool isDescending;

        [Space(10)]
        [Header("Wall Climbing Related")]
        [SerializeField] private bool isClimbOn;
        [SerializeField] private bool isSlideOn;
        [SerializeField] private bool isHangingOnEdge;


        private void Awake() {
            _controls = new PlayerControls();
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update() {
            if (!IsGrounded() && IsNextToWall() && !isClimbOn && !isHangingOnEdge) {
                isSlideOn = true;
            } else {
                isSlideOn = false;
            }

            // if player is facing the wall and the key which runs towards the wall is pressed, prevent move val to be non zero
            if ((moveVal < 0 && transform.rotation.y.Equals(-1) || (moveVal > 0 && transform.rotation.y.Equals(0f))) && IsNextToWall()) {
                moveVal = 0;
            }

            if (_rb.velocity.y > 0f && !IsGrounded() && !IsNextToWall()) {
                isAscending = true;
            } else {
                isAscending = false;
            }

            if (_rb.velocity.y < 0f && !IsGrounded() && !IsNextToWall()) {
                isDescending = true;
            } else {
                isDescending = false;
            }
        }

        private void FixedUpdate() {
            if (IsGrounded() && isJumpPressed) {
                var velocity = _rb.velocity;
                velocity = new Vector2(velocity.x, velocity.y + jumpForce);
                _rb.velocity = velocity;
                isJumpPressed = false;
                isAscending = true;
            }
            _rb.velocity = new Vector2(moveVal * moveSpeed, _rb.velocity.y);

            PerformWallClimbing();
            PerformEdgeHanging();
            SlideOffWall();

            HandleAnimations();
            FlipHero();

            if (isHangingOnEdge) {
                _rb.gravityScale = 0f;
                _rb.velocity = Vector2.zero;
            } else {
                _rb.gravityScale = 1f;
            }
        }

        private void PerformWallClimbing() {
            if (!isClimbOn || !IsNextToWall() || isHangingOnEdge) return;
            var velocity = _rb.velocity;
            velocity = new Vector2(velocity.x, climbSpeed);
            _rb.velocity = velocity;
        }

        private void PerformEdgeHanging() {
            if (isClimbOn && !IsWallEdgeActive()) {
                isClimbOn = false;
                isHangingOnEdge = true;
            } else if (!IsNextToWall()) {
                isHangingOnEdge = false;
            }
        }

        private void SlideOffWall() {
            if (isClimbOn || IsGrounded() || !IsNextToWall() || isHangingOnEdge) return;
            var velocity = _rb.velocity;
            velocity = new Vector2(velocity.x, -climbSpeed);
            _rb.velocity = velocity;
        }

        private void Move(InputAction.CallbackContext ctx) {
            if (isClimbOn || isSlideOn || isHangingOnEdge) {
                moveVal = 0;
                return;
            }

            switch (ctx.phase) {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    moveVal = _controls.Player.Move.ReadValue<float>();
                    break;
                case InputActionPhase.Canceled:
                    moveVal = 0;
                    break;
            }
        }

        private void Jump(InputAction.CallbackContext ctx) {
            isJumpPressed = IsGrounded();
        }

        private void ClimbWall(InputAction.CallbackContext ctx) {
            switch (ctx.phase) {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    if (IsNextToWall()) {
                        isClimbOn = true;
                    }
                    break;
                case InputActionPhase.Canceled:
                    isClimbOn = false;
                    break;
            }
        }

        private bool IsGrounded() {
            var val = Physics2D.OverlapCapsule(groundCheckerPos.position, new Vector2(0.6f, 0.05f), CapsuleDirection2D.Horizontal, 0, groundLayer);
            return val;
        }

        private bool IsNextToWall() {
            var val = Physics2D.OverlapCapsule(wallCheckerPos.position, new Vector2(0.1f, 1f), CapsuleDirection2D.Vertical, 0, groundLayer);
            return val;
        }

        private bool IsWallEdgeActive() {
            var val = Physics2D.OverlapCircle(wallEdgeChecker.position, 0.05f, groundLayer);
            return val;
        }

        private void HandleAnimations() {
            _animState = PlayerAnimState.IdleOne;

            if (moveVal != 0 && IsGrounded() && _rb.velocity.x != 0) _animState = PlayerAnimState.Run;
            if (isAscending) _animState = PlayerAnimState.Jump;
            if (isDescending) _animState = PlayerAnimState.Fall;
            if (isClimbOn) _animState = PlayerAnimState.WallClimb;
            if (isSlideOn) _animState = PlayerAnimState.WallSlide;
            if (isHangingOnEdge) _animState = PlayerAnimState.CornerGrab;

            _animator.SetInteger(State, (int)_animState);
        }

        private void FlipHero() {
            switch (_rb.velocity.x) {
                case < 0f:
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    break;
                case > 0f:
                    transform.rotation = Quaternion.Euler(0f, 0, 0f);
                    break;
            }
        }

        private void OnEnable() {
            _controls.Player.Move.Enable();
            _controls.Player.Jump.Enable();
            _controls.Player.ClimbWall.Enable();

            _controls.Player.Move.performed += Move;
            _controls.Player.Move.canceled += Move;
            _controls.Player.Jump.performed += Jump;
            _controls.Player.ClimbWall.performed += ClimbWall;
            _controls.Player.ClimbWall.canceled += ClimbWall;
        }

        private void OnDisable() {
            _controls.Player.Move.performed -= Move;
            _controls.Player.Move.canceled -= Move;
            _controls.Player.Jump.performed -= Jump;
            _controls.Player.ClimbWall.performed -= ClimbWall;
            _controls.Player.ClimbWall.canceled -= ClimbWall;

            _controls.Player.Move.Disable();
            _controls.Player.Jump.Disable();
            _controls.Player.ClimbWall.Disable();
        }
    }
}