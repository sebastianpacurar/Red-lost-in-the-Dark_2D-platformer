using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Torch {
    public class Torch : MonoBehaviour {
        [SerializeField] private GameObject flameObject;
        private Light2D _light2D;
        private bool _isIntensityIncreasing;

        private void Awake() {
            _light2D = flameObject.GetComponent<Light2D>();
        }


        private void Start() {
            StartCoroutine(HandleLightIntensity());
            _isIntensityIncreasing = true;
        }


        void Update() {
            _light2D.pointLightOuterRadius = Mathf.Lerp(9f, 12f, 25f * Time.deltaTime);
        }


        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Player")) {
                _isIntensityIncreasing = false;
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            if (col.gameObject.CompareTag("Player")) {
                _isIntensityIncreasing = true;
            }
        }

        private IEnumerator HandleLightIntensity() {
            while (true) {
                yield return new WaitForSeconds(0.1f);

                if (_isIntensityIncreasing) {
                    if (_light2D.intensity < 2.75f) _light2D.intensity += 0.05f;
                    else _light2D.intensity = 2.75f;
                } else {
                    if (_light2D.intensity > 0.05f) _light2D.intensity -= 0.05f;
                    else _light2D.intensity = 0.05f;
                }
            }
        }
    }
}