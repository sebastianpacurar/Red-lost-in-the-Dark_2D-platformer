using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Player.TutorialCanvas {
    public class TutorialKeyboard : MonoBehaviour {
        private PlayerControls _controls;
        private Camera _mainCam;

        [SerializeField] private UnityEngine.Canvas canvas;
        [SerializeField] private Sprite[] leftKeysUnpressed;
        [SerializeField] private Sprite[] leftKeysPressed;
        [SerializeField] private Sprite[] rightKeysUnpressed;
        [SerializeField] private Sprite[] rightKeysPressed;
        [SerializeField] private Sprite[] shiftKeyPressed;
        [SerializeField] private Sprite[] shiftKeyUnpressed;
        [SerializeField] private Sprite[] spaceKeyPressed;
        [SerializeField] private Sprite[] spaceKeyUnpressed;

        [SerializeField] private Image imgA;
        [SerializeField] private Image imgLeft;

        [SerializeField] private Image imgD;
        [SerializeField] private Image imgRight;

        [SerializeField] private Image[] imgShift;
        [SerializeField] private Image[] imgSpace;


        [SerializeField] private float xInputVal;
        [SerializeField] private bool isJumpPressed;
        [SerializeField] private bool isAttackPressed;

        private void Awake() {
            _controls = new PlayerControls();
        }

        private void Start() {
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = _mainCam;
        }

        private void Update() {
            switch (xInputVal) {
                case < 0:
                    imgA.sprite = leftKeysPressed[0];
                    imgLeft.sprite = leftKeysPressed[1];
                    imgD.sprite = rightKeysUnpressed[0];
                    imgRight.sprite = rightKeysUnpressed[1];
                    break;
                case > 0:
                    imgA.sprite = leftKeysUnpressed[0];
                    imgLeft.sprite = leftKeysUnpressed[1];
                    imgD.sprite = rightKeysPressed[0];
                    imgRight.sprite = rightKeysPressed[1];
                    break;
                default:
                    imgA.sprite = leftKeysUnpressed[0];
                    imgLeft.sprite = leftKeysUnpressed[1];
                    imgD.sprite = rightKeysUnpressed[0];
                    imgRight.sprite = rightKeysUnpressed[1];
                    break;
            }

            if (isJumpPressed)
                for (var i = 0; i < imgSpace.Length; i++)
                    imgSpace[i].sprite = spaceKeyPressed[i];
            else
                for (var i = 0; i < imgSpace.Length; i++)
                    imgSpace[i].sprite = spaceKeyUnpressed[i];


            if (isAttackPressed)
                for (var i = 0; i < imgShift.Length; i++)
                    imgShift[i].sprite = shiftKeyPressed[i];
            else
                for (var i = 0; i < imgShift.Length; i++)
                    imgShift[i].sprite = shiftKeyUnpressed[i];
        }

        private void Move(InputAction.CallbackContext ctx) {
            xInputVal = _controls.Player.Move.ReadValue<float>();
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

        private void Attack(InputAction.CallbackContext ctx) {
            switch (ctx.phase) {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    isAttackPressed = true;
                    break;
                case InputActionPhase.Canceled:
                    isAttackPressed = false;
                    break;
            }
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
            _controls.Player.Attack.canceled += Attack;
        }

        private void OnDisable() {
            _controls.Player.Move.performed -= Move;
            _controls.Player.Move.canceled -= Move;
            _controls.Player.Jump.performed -= Jump;
            _controls.Player.Jump.canceled -= Jump;
            _controls.Player.Jump.canceled -= Jump;
            _controls.Player.Attack.performed -= Attack;
            _controls.Player.Attack.canceled -= Attack;
            _controls.Player.Attack.Disable();
            _controls.Player.Move.Disable();
            _controls.Player.Jump.Disable();
        }
    }
}