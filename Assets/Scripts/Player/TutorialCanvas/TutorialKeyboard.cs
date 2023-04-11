using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Plugins.SerializedCollections.Runtime.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Player.TutorialCanvas {
    public class TutorialKeyboard : MonoBehaviour {
        private PlayerControls _controls;

        [SerializedDictionary("Key Type (string)", "Pressed/Unpressed")]
        public SerializedDictionary<String, Sprite[]> keyboardKeys;

        [SerializedDictionary("Space Key Segments (UI.Image)", "Pressed/Unpressed")]
        public SerializedDictionary<Image, Sprite[]> spaceKeys;

        [SerializeField] private Image imgLeft;
        [SerializeField] private Image imgRight;
        [SerializeField] private Image imgZ;

        [SerializeField] private float xInputVal;
        [SerializeField] private bool isJumpPressed;
        [SerializeField] private bool isAttackPressed;

        private void Awake() {
            _controls = new PlayerControls();
        }

        private void Update() {
            // handle Arrow Keys
            switch (xInputVal) {
                case < 0:
                    imgLeft.sprite = keyboardKeys[KeyType.LeftKey.ToString()][0];
                    imgRight.sprite = keyboardKeys[KeyType.RightKey.ToString()][1];
                    break;
                case > 0:
                    imgLeft.sprite = keyboardKeys[KeyType.LeftKey.ToString()][1];
                    imgRight.sprite = keyboardKeys[KeyType.RightKey.ToString()][0];
                    break;
                default:
                    imgLeft.sprite = keyboardKeys[KeyType.LeftKey.ToString()][1];
                    imgRight.sprite = keyboardKeys[KeyType.RightKey.ToString()][1];
                    break;
            }


            // handle Jump Key Segments
            // if kvp.Value[0]  = pressed; kvp.Value[1] = not pressed.
            foreach (KeyValuePair<Image, Sprite[]> kvp in spaceKeys) {
                kvp.Key.sprite = isJumpPressed ? kvp.Value[0] : kvp.Value[1];
            }

            // handle Z Key
            imgZ.sprite = isAttackPressed ? keyboardKeys[KeyType.ZKey.ToString()][0] : keyboardKeys[KeyType.ZKey.ToString()][1];
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


        private enum KeyType {
            LeftKey,
            RightKey,
            ZKey,
        }
    }
}