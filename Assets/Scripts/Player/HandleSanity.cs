using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player {
    public class HandleSanity : MonoBehaviour {
        [SerializeField] private Light2D playerLight;
        private bool _isIntensityIncreasing;
        private Light2D _torchLight2D;

        private void Start() {
            StartCoroutine(HandleLightRadius());
        }

        private void Update() {
            if (_torchLight2D) {
                var torchIntensity = _torchLight2D.intensity;
                _isIntensityIncreasing = !(torchIntensity <= 0.05f);
            } else {
                _isIntensityIncreasing = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Torch")) {
                _torchLight2D = col.gameObject.transform.Find("FlameLight").GetComponent<Light2D>();
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            if (col.gameObject.CompareTag("Torch")) {
                _torchLight2D = null;
            }
        }

        private IEnumerator HandleLightRadius() {
            while (true) {
                yield return new WaitForSeconds(0.05f);

                if (_isIntensityIncreasing) {
                    if (playerLight.pointLightInnerRadius <= 10f) playerLight.pointLightInnerRadius += 0.05f;
                    else playerLight.pointLightInnerRadius = 10f;

                    if (playerLight.pointLightOuterRadius <= 20) playerLight.pointLightOuterRadius += 0.1f;
                    else playerLight.pointLightOuterRadius = 20f;
                } else {
                    if (playerLight.pointLightInnerRadius > 1f) playerLight.pointLightInnerRadius -= 0.02f;
                    else playerLight.pointLightInnerRadius = 1f;

                    if (playerLight.pointLightOuterRadius > 2f) playerLight.pointLightOuterRadius -= 0.04f;
                    else playerLight.pointLightOuterRadius = 2f;
                }
            }
        }
    }
}