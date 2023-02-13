using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour {
    [SerializeField] private Transform groundChecker;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] private float xInputVal;
    private PlayerControls _controls;
    private Rigidbody2D _rb;
    private Animator _animator;
    private AnimationState _state;

    private bool _isJumpPressed;
    private static readonly int State = Animator.StringToHash("state");


    private void Awake() {
        _controls = new PlayerControls();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        _rb.velocity = new Vector2(xInputVal * moveSpeed, _rb.velocity.y);

        if (_isJumpPressed) {
            _rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            _isJumpPressed = false;
        }

        FlipPlayer();
        HandleAnimations();
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCapsule(groundChecker.position, new Vector2(0.425f, 0.05f), CapsuleDirection2D.Horizontal, 0, groundLayer);
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

    private void Jump(InputAction.CallbackContext ctx) {
        if (IsGrounded()) _isJumpPressed = true;
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

    private void HandleAnimations() {
        _state = AnimationState.Idle;
        if (_rb.velocity.y > 0f && !IsGrounded()) _state = AnimationState.Ascend;
        if (_rb.velocity.x != 0f && IsGrounded()) _state = AnimationState.Run;
        if (_rb.velocity.y < 0f && !IsGrounded()) _state = AnimationState.Descend;

        _animator.SetInteger(State, (int)_state);
    }

    private void FlipPlayer() {
        if (_rb.velocity.x < 0f) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        } else if (_rb.velocity.x > 0f) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}