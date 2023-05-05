using PlayerFiniteStateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Canvas {
    public class PlayerStats : MonoBehaviour {
        private Camera _mainCam;
        private PlayerScript _playerScript;

        [SerializeField] private GameObject playerObj;
        [SerializeField] private UnityEngine.Canvas canvas;
        [SerializeField] private Image hpBar;
        [SerializeField] private Image sanityBar;
        [SerializeField] private TextMeshProUGUI hpPercentage;
        [SerializeField] private TextMeshProUGUI sanityPercentage;


        private void Start() {
            _playerScript = playerObj.GetComponent<PlayerScript>();
            _mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = _mainCam;
        }

        private void Update() {
            UpdateHpBar();
            UpdateSanityBar();
        }

        private void UpdateHpBar() {
            var res = _playerScript.HealthPoints / _playerScript.playerData.maxHp;
            hpBar.fillAmount = res;
            hpPercentage.text = $"{(int)(res * 100)}%";
        }

        private void UpdateSanityBar() {
            var res = _playerScript.SanityPoints / _playerScript.playerData.maxSanity;
            sanityBar.fillAmount = res;
            sanityPercentage.text = $"{(int)(res * 100)}%";
        }
    }
}