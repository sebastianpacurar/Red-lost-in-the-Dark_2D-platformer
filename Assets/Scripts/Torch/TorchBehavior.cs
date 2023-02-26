using System.Collections;
using CustomAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Torch {
    public class TorchBehavior : MonoBehaviour {
        [SerializeField] private GameObject flameObject;
        [SerializeField] private Animator flameAnimation;

        [SerializeField] private float sineFreq;
        [SerializeField] private float sineAmp;

        [SerializeField] private float timeMultiplier;
        [SerializeField] private float increaseUnit;
        [SerializeField] private float decreaseUnit;
        [SerializeField] private float minIntensity;
        [SerializeField] private float maxIntensity;

        [SerializeField] private ParticleSystem sparklesPs;
        [SerializeField] private ParticleSystem smokePs;

        [ReadOnlyProp] [SerializeField] private bool isIntensityIncreasing;
        [ReadOnlyProp] [SerializeField] private float lightIntensityValue;
        [ReadOnlyProp] [SerializeField] private float outerRadiusValue;

        private Light2D _light2D;
        private float _initialOuterRadius;
        private ParticleSystem.MainModule _sparklesMain, _smokeMain;
        private TorchAnimState _state;
        private static readonly int State = Animator.StringToHash("state");

        private void Awake() {
            _light2D = flameObject.GetComponent<Light2D>();
            _sparklesMain = sparklesPs.main;
            _smokeMain = smokePs.main;
        }

        private void Start() {
            StartCoroutine(HandleLightIntensity());
            _initialOuterRadius = _light2D.pointLightOuterRadius;


            isIntensityIncreasing = true;
        }

        private void Update() {
            lightIntensityValue = _light2D.intensity;
            outerRadiusValue = _light2D.pointLightOuterRadius;

            HandleAnimationsAndParticles();

            _light2D.pointLightOuterRadius = Mathf.Sin(Time.time * sineFreq) * sineAmp + _initialOuterRadius;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Player")) {
                isIntensityIncreasing = false;
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            if (col.gameObject.CompareTag("Player")) {
                isIntensityIncreasing = true;
            }
        }

        private IEnumerator HandleLightIntensity() {
            while (true) {
                yield return new WaitForSeconds(timeMultiplier);

                if (isIntensityIncreasing) {
                    _light2D.intensity += increaseUnit;
                } else {
                    _light2D.intensity -= decreaseUnit;
                }

                _light2D.intensity = Mathf.Clamp(_light2D.intensity, minIntensity, maxIntensity);
            }
        }

        // update based on light intensity
        private void HandleAnimationsAndParticles() {
            _state = TorchAnimState.Loop0;

            switch (_light2D.intensity) {
                case < 4 and > 3:
                    _state = TorchAnimState.Loop1;
                    _sparklesMain.startSize = 0.075f;
                    _smokeMain.startSize = 0.075f;
                    break;
                case < 3 and > 2:
                    _state = TorchAnimState.Loop2;
                    _sparklesMain.startSize = 0.05f;
                    _smokeMain.startSize = 0.05f;
                    break;
                case < 2 and > 1:
                    _state = TorchAnimState.Loop3;
                    _sparklesMain.startSize = 0.025f;
                    _smokeMain.startSize = 0.025f;
                    break;
                case < 1 and > 0:
                    _state = TorchAnimState.Loop4;
                    _sparklesMain.startSize = 0f;
                    _smokeMain.startSize = 0f;
                    break;
            }

            flameAnimation.SetInteger(State, (int)_state);
        }
    }
}