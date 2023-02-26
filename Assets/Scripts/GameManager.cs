using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    [SerializeField] private GameObject player;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        if (scene.name.Equals("MainMenu")) return;
        Instantiate(player, new Vector3(-10, -26, 0), Quaternion.identity);
    }
}