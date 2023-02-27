using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu {
    public class MainMenu : MonoBehaviour {
        public void StartTutorial() {
            SceneManager.LoadScene("Tutorial");
        }

        public void StartDemoLevel() {
            SceneManager.LoadScene("DemoLevel");
        }
    }
}