using System.Collections;
using CustomAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Torch {
    public class HealthTorchBehavior : MonoBehaviour {
        private Light2D _light2D;
        private float _initialOuterRadius;
        private TorchAnimState _state;
        private static readonly int State = Animator.StringToHash("state");

        [SerializeField] private GameObject flameObject;
        [SerializeField] private Animator flameAnimation;

        [SerializeField] private float sineFreq;
        [SerializeField] private float sineAmp;

        [SerializeField] private float timeMultiplier;
        [SerializeField] private float increaseUnit;
        [SerializeField] private float decreaseUnit;
        [SerializeField] private float minIntensity;
        [SerializeField] private float maxIntensity;

        [ReadOnlyProp] [SerializeField] private bool isTriggered;
        [ReadOnlyProp] [SerializeField] private float lightIntensityValue;
        [ReadOnlyProp] [SerializeField] private float outerRadiusValue;

        private void Awake() {
            _light2D = flameObject.GetComponent<Light2D>();
        }

        private void Start() {
            StartCoroutine(HandleLightIntensity());
            _initialOuterRadius = _light2D.pointLightOuterRadius;

            isTriggered = true;
        }
        
        private void Update() {
            lightIntensityValue = _light2D.intensity;
            outerRadiusValue = _light2D.pointLightOuterRadius;

            HandleAnimations();

            _light2D.pointLightOuterRadius = Mathf.Sin(Time.time * sineFreq) * sineAmp + _initialOuterRadius;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Player")) {
                isTriggered = false;
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            if (col.gameObject.CompareTag("Player")) {
                isTriggered = true;
            }
        }

        //TODO: change logic to fit for health
        private IEnumerator HandleLightIntensity() {
            while (true) {
                yield return new WaitForSeconds(timeMultiplier);

                if (isTriggered) {
                    if (_light2D.intensity <= maxIntensity) _light2D.intensity += increaseUnit;
                    else _light2D.intensity = maxIntensity;
                } else {
                    if (_light2D.intensity >= minIntensity) _light2D.intensity -= decreaseUnit;
                    else _light2D.intensity = minIntensity;
                }
            }
        }

        private void HandleAnimations() {
            _state = TorchAnimState.Loop0;

            if (_light2D.intensity < 4) _state = TorchAnimState.Loop1;
            if (_light2D.intensity < 3) _state = TorchAnimState.Loop2;
            if (_light2D.intensity < 2) _state = TorchAnimState.Loop3;
            if (_light2D.intensity < 1) _state = TorchAnimState.Loop4;

            flameAnimation.SetInteger(State, (int)_state);
        }
    }
}