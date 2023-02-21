using System.Collections;
using CustomAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player {
    public class HandleSanity : MonoBehaviour {
        [SerializeField] private Light2D playerLight;
        [SerializeField] private float timeMultiplier = 0.03f;

        [SerializeField] private float minInnerRadius = 1f;
        [SerializeField] private float maxInnerRadius = 15f;
        [SerializeField] private float innerRadiusIncreaseVal = 0.05f;
        [SerializeField] private float innerRadiusDecreaseVal = 0.02f;

        [SerializeField] private float minOuterRadius = 2f;
        [SerializeField] private float maxOuterRadius = 30f;
        [SerializeField] private float outerRadiusIncreaseVal = 0.1f;
        [SerializeField] private float outerRadiusDecreaseVal = 0.04f;

        [ReadOnlyProp] [SerializeField] private bool isIntensityIncreasing;

        private Light2D _torchLight2D;

        private void Start() {
            StartCoroutine(HandleLightRadius());
        }

        private void Update() {
            if (_torchLight2D) {
                var torchIntensity = _torchLight2D.intensity;
                isIntensityIncreasing = !(torchIntensity <= 0.05f);
            } else {
                isIntensityIncreasing = false;
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
                yield return new WaitForSeconds(timeMultiplier);

                if (isIntensityIncreasing) {
                    if (playerLight.pointLightInnerRadius <= maxInnerRadius) playerLight.pointLightInnerRadius += innerRadiusIncreaseVal;
                    else playerLight.pointLightInnerRadius = maxInnerRadius;

                    if (playerLight.pointLightOuterRadius <= maxOuterRadius) playerLight.pointLightOuterRadius += outerRadiusIncreaseVal;
                    else playerLight.pointLightOuterRadius = maxOuterRadius;
                } else {
                    if (playerLight.pointLightInnerRadius >= minInnerRadius) playerLight.pointLightInnerRadius -= innerRadiusDecreaseVal;
                    else playerLight.pointLightInnerRadius = minInnerRadius;

                    if (playerLight.pointLightOuterRadius >= minOuterRadius) playerLight.pointLightOuterRadius -= outerRadiusDecreaseVal;
                    else playerLight.pointLightOuterRadius = minOuterRadius;
                }
            }
        }
    }
}