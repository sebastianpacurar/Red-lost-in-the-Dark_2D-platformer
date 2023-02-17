using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player {
    public class HandleSanity : MonoBehaviour {
        [SerializeField] private Light2D playerLight;
        [SerializeField] private bool isIntensityIncreasing;
        [SerializeField] private Light2D torchLight;

        private void Start() {
            StartCoroutine(HandleLightRadius());
        }

        private void Update() {
            if (!torchLight) return;
            var torchIntensity = torchLight.intensity;
            isIntensityIncreasing = !(torchIntensity <= 0.05f);
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Torch")) {
                torchLight = col.gameObject.transform.Find("FlameLight").GetComponent<Light2D>();
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            if (col.gameObject.CompareTag("Torch")) {
                torchLight = null;
            }
        }

        private IEnumerator HandleLightRadius() {
            while (true) {
                yield return new WaitForSeconds(0.05f);

                if (isIntensityIncreasing) {
                    if (playerLight.pointLightInnerRadius < 8f) playerLight.pointLightInnerRadius += 0.02f;
                    else playerLight.pointLightInnerRadius = 8f;

                    if (playerLight.pointLightOuterRadius < 16f) playerLight.pointLightOuterRadius += 0.04f;
                    else playerLight.pointLightOuterRadius = 16f;
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