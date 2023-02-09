using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerController : MonoBehaviour {
        [SerializeField] private Transform groundCheckerPos, wallCheckerPos;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float climbSpeed;

        private PlayerAnimState _animState;
        private PlayerControls _controls;
        private Rigidbody2D _rb;
        private Animator _animator;

        private float _moveVal;
        private bool _isJumpOn;
        private bool _isClimbOn;
        private static readonly int State = Animator.StringToHash("state");


        private void Awake() {
            _controls = new PlayerControls();
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate() {
            if (IsGrounded() && _isJumpOn) {
                var velocity = _rb.velocity;
                velocity = new Vector2(velocity.x, velocity.y + jumpForce);
                _rb.velocity = velocity;
                _isJumpOn = false;
            }
            _rb.velocity = new Vector2(_moveVal * moveSpeed, _rb.velocity.y);

            PerformWallClimbing();
            SlideOffWall();

            HandleAnimations();
            FlipHero();
        }

        private void PerformWallClimbing() {
            if (!_isClimbOn) return;
            var velocity = _rb.velocity;
            velocity = new Vector2(velocity.x, velocity.y + climbSpeed);
            _rb.velocity = velocity;
        }

        private void SlideOffWall() {
            if (_isClimbOn || IsGrounded() || !IsNextToWall()) return;
            var velocity = _rb.velocity;
            velocity = new Vector2(velocity.x, velocity.y - climbSpeed);
            _rb.velocity = velocity;
        }

        private void Move(InputAction.CallbackContext ctx) {
            switch (ctx.phase) {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    _moveVal = _controls.Player.Move.ReadValue<float>();
                    break;
                case InputActionPhase.Canceled:
                    _moveVal = 0;
                    break;
            }
        }

        private void Jump(InputAction.CallbackContext ctx) {
            _isJumpOn = true;
        }

        private void ClimbWall(InputAction.CallbackContext ctx) {
            if (!IsNextToWall()) return;
            switch (ctx.phase) {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    _isClimbOn = true;
                    break;
                case InputActionPhase.Canceled:
                    _isClimbOn = false;
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

        private void HandleAnimations() {
            if (_rb.velocity.x is < 0f or > 0f && IsGrounded()) {
                _animState = PlayerAnimState.Run;
            } else if (_rb.velocity.y > 0f && !IsGrounded()) {
                _animState = PlayerAnimState.Jump;
            } else if (_rb.velocity.y < 0f && !IsGrounded()) {
                _animState = PlayerAnimState.Fall;
            } else {
                _animState = PlayerAnimState.IdleOne;
            }

            if (_isClimbOn) {
                _animState = PlayerAnimState.WallClimb;
            }

            if (IsNextToWall() && !IsGrounded()) {
                _animState = PlayerAnimState.WallSlide;
            }

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