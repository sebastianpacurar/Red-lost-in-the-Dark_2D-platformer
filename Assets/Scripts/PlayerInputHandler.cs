using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour {
    private PlayerControls _controls;

    public int MovementInput { get; private set; }
    public bool JumpInput { get; private set; }
    
    private void Awake() {
        _controls = new PlayerControls();
    }

    private void OnMoveInput(InputAction.CallbackContext ctx) {
        switch (ctx.phase) {
            case InputActionPhase.Started:
            case InputActionPhase.Performed:
                MovementInput = (int)_controls.Player.Move.ReadValue<float>();
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

    public void SetJumpInputFalse() {
        JumpInput = false;
    }

    private void OnEnable() {
        _controls.Enable();
        _controls.Player.Move.performed += OnMoveInput;
        _controls.Player.Move.canceled += OnMoveInput;
        _controls.Player.Jump.performed += OnJumpInput;
        _controls.Player.Jump.canceled += OnJumpInput;
    }

    private void OnDisable() {
        _controls.Player.Move.performed -= OnMoveInput;
        _controls.Player.Move.canceled -= OnMoveInput;
        _controls.Player.Jump.performed -= OnJumpInput;
        _controls.Player.Jump.canceled -= OnJumpInput;
        _controls.Disable();
    }
}