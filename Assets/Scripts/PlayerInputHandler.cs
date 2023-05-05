using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour {
    private PlayerControls _controls;
    public static PlayerInputHandler Instance { get; private set; }

    public int MovementInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool GroundSlideInput { get; private set; }

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _groundSlideAction;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        
        _controls = new PlayerControls();
        _moveAction = _controls.Player.Move;
        _jumpAction = _controls.Player.Jump;
        _groundSlideAction = _controls.Player.GroundSlide;
    }

    private void OnMoveInput(InputAction.CallbackContext ctx) {
        switch (ctx.phase) {
            case InputActionPhase.Started:
            case InputActionPhase.Performed:
                MovementInput = (int)_moveAction.ReadValue<float>();
                break;
            case InputActionPhase.Canceled:
                MovementInput = 0;
                break;
        }
    }

    private void OnJumpInput(InputAction.CallbackContext ctx) {
        switch (ctx.phase) {
            case InputActionPhase.Started:
            case InputActionPhase.Performed:
                JumpInput = true;
                break;
            case InputActionPhase.Canceled:
                JumpInput = false;
                break;
        }
    }

    private void OnGroundSlideInput(InputAction.CallbackContext ctx) {
        switch (ctx.phase) {
            case InputActionPhase.Started:
            case InputActionPhase.Performed:
                GroundSlideInput = true;
                break;
            case InputActionPhase.Canceled:
                GroundSlideInput = false;
                break;
        }
    }

    private void OnEnable() {
        _controls.Enable();
        _moveAction.performed += OnMoveInput;
        _moveAction.canceled += OnMoveInput;
        _jumpAction.performed += OnJumpInput;
        _jumpAction.canceled += OnJumpInput;
        _groundSlideAction.performed += OnGroundSlideInput;
        _groundSlideAction.canceled += OnGroundSlideInput;
    }

    private void OnDisable() {
        _moveAction.performed -= OnMoveInput;
        _moveAction.canceled -= OnMoveInput;
        _jumpAction.performed -= OnJumpInput;
        _jumpAction.canceled -= OnJumpInput;
        _groundSlideAction.performed -= OnGroundSlideInput;
        _groundSlideAction.canceled -= OnGroundSlideInput;
        _controls.Disable();
    }
}