using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject menu;

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

        Instantiate(menu);
        var playerObj = Instantiate(player);

        switch (scene.name) {
            case "Tutorial":
                playerObj.transform.position = new Vector3(-33.25f, 0f, 1f);
                playerObj.transform.Find("TutorialCanvas").gameObject.SetActive(true);
                break;
            case "DemoLevel":
                playerObj.transform.position = new Vector3(-130f, 5f, 1f);
                playerObj.transform.Find("TutorialCanvas").gameObject.SetActive(false);
                break;
        }
    }
}