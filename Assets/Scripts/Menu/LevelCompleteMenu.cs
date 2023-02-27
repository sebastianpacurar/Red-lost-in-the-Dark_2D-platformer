using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu {
    public class LevelCompleteMenu : MonoBehaviour {
        [SerializeField] private GameObject panel;

        public void RestartLevel() {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}