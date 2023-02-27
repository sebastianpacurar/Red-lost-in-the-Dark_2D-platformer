using Player;
using Player.TutorialCanvas;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Menu {
    public class PauseMenu : MonoBehaviour {
        [SerializeField] private GameObject panel;

        private PlayerController _controller;
        private PlayerControls _controls;
        private TutorialKeyboard _tutorialKeyboard;

        private void Awake() {
            _controls = new PlayerControls();
        }

        private void Start() {
            _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _tutorialKeyboard = GameObject.FindGameObjectWithTag("Player").transform.parent.GetChild(0).GetChild(0).GetComponent<TutorialKeyboard>();
        }

        private void PauseMenuActivate() {
            Time.timeScale = 0;
            _controller.enabled = false;
            _tutorialKeyboard.enabled = false;
            panel.SetActive(true);
        }

        // called as unity event for CancelBtn object
        public void PauseMenuDeactivate() {
            Time.timeScale = 1;
            _controller.enabled = true;
            _tutorialKeyboard.enabled = true;
            panel.SetActive(false);
        }

        public void GoToMainMenu() {
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }

        private void ToggleMenu(InputAction.CallbackContext ctx) {
            if (panel.activeSelf) {
                PauseMenuDeactivate();
            } else {
                PauseMenuActivate();
            }
        }

        private void OnEnable() {
            _controls.UI.Pause.Enable();
            _controls.UI.Pause.performed += ToggleMenu;
        }

        private void OnDisable() {
            _controls.UI.Pause.performed -= ToggleMenu;
            _controls.UI.Pause.Disable();
        }
    }
}