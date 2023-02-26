using System.Collections;
using CustomAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player {
    public class HandleHpSanity : MonoBehaviour {
        [SerializeField] private Light2D playerLight;
        [SerializeField] private float timeMultiplier = 0.03f;

        [SerializeField] private float minOuterRadius = 2f;
        [SerializeField] private float maxOuterRadius = 30f;
        [SerializeField] private float outerRadiusIncreaseUnit = 0.1f;
        [SerializeField] private float outerRadiusDecreaseUnit = 0.04f;

        [SerializeField] private float hpIncreaseUnit;
        [SerializeField] public float maxHp;

        [ReadOnlyProp] [SerializeField] private float currentHp;
        [ReadOnlyProp] [SerializeField] private float currentSanity;
        [ReadOnlyProp] [SerializeField] public float minSanity;
        [ReadOnlyProp] public float maxSanity;

        [ReadOnlyProp] [SerializeField] private bool isIntensityIncreasing;
        [ReadOnlyProp] [SerializeField] private string torchTag;

        public float HealthPoints { get; set; }
        public float SanityPoints { get; set; }

        private Light2D _torchLight2D;
        private bool _isHitByEnemy;

        private void Awake() {
            maxSanity = maxOuterRadius;
            minSanity = minOuterRadius;
            HealthPoints = maxHp;
        }

        private void Start() {
            StartCoroutine(HandleLightRadius());
        }

        private void Update() {
            SanityPoints = playerLight.pointLightOuterRadius;
            currentHp = HealthPoints;
            currentSanity = SanityPoints;

            if (_torchLight2D) {
                var torchIntensity = _torchLight2D.intensity;
                isIntensityIncreasing = !(torchIntensity <= 0.05f);
            } else {
                isIntensityIncreasing = false;
            }

            if (!_isHitByEnemy) return;
            HealthPoints--;
            _isHitByEnemy = false;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("LightTorch") || col.gameObject.CompareTag("HealthTorch") || col.gameObject.CompareTag("CheckpointTorch")) {
                torchTag = col.gameObject.tag;
                _torchLight2D = col.gameObject.transform.Find("FlameLight").GetComponent<Light2D>();
            }

            if (col.gameObject.CompareTag("EnemyHitArea")) {
                _isHitByEnemy = true;
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            if (col.gameObject.CompareTag("LightTorch") || col.gameObject.CompareTag("HealthTorch") || col.gameObject.CompareTag("CheckpointTorch")) {
                torchTag = "";
                _torchLight2D = null;
            }

            if (col.gameObject.CompareTag("EnemyHitArea")) {
                _isHitByEnemy = false;
            }
        }

        private IEnumerator HandleLightRadius() {
            while (true) {
                yield return new WaitForSeconds(timeMultiplier);

                if (isIntensityIncreasing) {
                    switch (torchTag) {
                        case "LightTorch":
                            playerLight.pointLightOuterRadius += outerRadiusIncreaseUnit;
                            break;
                        case "HealthTorch":
                            HealthPoints += hpIncreaseUnit;
                            break;
                        case "CheckpointTorch":
                            HealthPoints += hpIncreaseUnit * 2;
                            playerLight.pointLightOuterRadius += outerRadiusIncreaseUnit * 2;
                            break;
                    }
                } else {
                    playerLight.pointLightOuterRadius -= outerRadiusDecreaseUnit;
                }

                playerLight.pointLightOuterRadius = Mathf.Clamp(playerLight.pointLightOuterRadius, minOuterRadius, maxOuterRadius);
                HealthPoints = Mathf.Clamp(HealthPoints, 0, maxHp);
            }
        }
    }
}