using UnityEngine;

namespace Menu {
    public class EnableMenu : MonoBehaviour {
        private Camera _mainCam;
        [SerializeField] private Canvas canvas;

        private void Start() {
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = _mainCam;
        }
    }
}