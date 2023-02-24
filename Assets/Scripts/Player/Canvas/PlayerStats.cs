using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Canvas {
    public class PlayerStats : MonoBehaviour {
        private Camera _mainCam;
        private HandleHpSanity _stats;

        [SerializeField] private GameObject playerObj;
        [SerializeField] private UnityEngine.Canvas canvas;
        [SerializeField] private Image hpBar;
        [SerializeField] private Image sanityBar;
        [SerializeField] private TextMeshProUGUI hpPercentage;
        [SerializeField] private TextMeshProUGUI sanityPercentage;


        private void Start() {
            _stats = playerObj.GetComponent<HandleHpSanity>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = _mainCam;
        }

        private void Update() {
            UpdateHpBar();
            UpdateSanityBar();
        }

        private void UpdateHpBar() {
            var res = _stats.HealthPoints / _stats.maxHp;
            hpBar.fillAmount = res;
            hpPercentage.text = $"{(int)(res * 100)}%";
        }

        private void UpdateSanityBar() {
            var res = _stats.SanityPoints / _stats.maxSanity;
            sanityBar.fillAmount = res;
            sanityPercentage.text = $"{(int)(res * 100)}%";
        }
    }
}