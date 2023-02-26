using System.Collections;
using CustomAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Torch {
    public class Checkpoint : MonoBehaviour {
        [SerializeField] private GameObject flameObject;
        [SerializeField] private Animator flameAnimation;

        [SerializeField] private float sineFreq;
        [SerializeField] private float sineAmp;

        [SerializeField] private float minInner;
        [SerializeField] private float maxInner;
        [SerializeField] private float minOuter;
        [SerializeField] private float maxOuter;
        [SerializeField] private float minFallOff;
        [SerializeField] private float maxFallOff;

        [SerializeField] private float timeMultiplier;
        [SerializeField] private float increaseUnit;

        [SerializeField] private ParticleSystem sparklesPs;
        [SerializeField] private ParticleSystem hollowPs;

        [ReadOnlyProp] [SerializeField] private bool isMarked;

        private Light2D _light2D, _playerLight;
        private float _initialOuterRadius, _initialInnerRadius;

        private CircleCollider2D _circleCollider2D;
        private ParticleSystem.MainModule _sparkleMain;
        private ParticleSystem.ShapeModule _sparkleShape, _hollowShape;

        private TorchAnimState _state;
        private static readonly int State = Animator.StringToHash("state");

        private void Awake() {
            _light2D = flameObject.GetComponent<Light2D>();
            _circleCollider2D = GetComponent<CircleCollider2D>();

            _sparkleMain = sparklesPs.main;
            _sparkleShape = sparklesPs.shape;
            _hollowShape = hollowPs.shape;
        }

        private void Start() {
            _initialInnerRadius = _light2D.pointLightInnerRadius;
            _initialOuterRadius = _light2D.pointLightOuterRadius;
        }

        private void Update() {
            HandleAnimationsAndParticles();

            if (!isMarked) {
                _light2D.pointLightInnerRadius = Mathf.Sin(Time.time * sineFreq) * sineAmp + _initialInnerRadius;
                _light2D.pointLightOuterRadius = Mathf.Sin(Time.time * sineFreq) * sineAmp + _initialOuterRadius;
            }

            if (_light2D.pointLightInnerRadius.Equals(maxInner)) {
                StopCoroutine(nameof(TriggerCheckpoint));
            }
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Player") && !isMarked) {
                StartCoroutine(nameof(TriggerCheckpoint));
                isMarked = true;
            }
        }

        // handle the lights and collider to form a safe zone
        private IEnumerator TriggerCheckpoint() {
            while (true) {
                yield return new WaitForSeconds(timeMultiplier);

                _light2D.pointLightInnerRadius += increaseUnit;
                _light2D.pointLightOuterRadius += increaseUnit;

                _circleCollider2D.radius += increaseUnit;
                _hollowShape.radius += increaseUnit * 2;
                _light2D.falloffIntensity -= increaseUnit / 3;

                _light2D.pointLightInnerRadius = Mathf.Clamp(_light2D.pointLightInnerRadius, minInner, maxInner);
                _light2D.pointLightOuterRadius = Mathf.Clamp(_light2D.pointLightOuterRadius, minOuter, maxOuter);
                _circleCollider2D.radius = Mathf.Clamp(_circleCollider2D.radius, minInner, maxInner);
                _hollowShape.radius = Mathf.Clamp(_hollowShape.radius, minInner / 2, maxInner);
                _light2D.falloffIntensity = Mathf.Clamp(_light2D.falloffIntensity, minFallOff, maxFallOff);
            }
        }

        // update based on light inner radius
        private void HandleAnimationsAndParticles() {
            _state = TorchAnimState.Loop4;

            switch (_light2D.pointLightInnerRadius) {
                case > 5f and < 7.5f:
                    _state = TorchAnimState.Loop3;

                    _sparkleMain.startLifetime = 0.5f;
                    _sparkleMain.startSpeed = 1f;
                    _sparkleMain.startSize = 0.05f;

                    _sparkleShape.radius = 0.5f;

                    break;
                case >= 7.5f and < 10f:
                    _state = TorchAnimState.Loop2;

                    _sparkleMain.startLifetime = 0.6f;
                    _sparkleMain.startSpeed = 1f;
                    _sparkleMain.startSize = 0.075f;

                    _sparkleShape.radius = 0.75f;

                    break;
                case >= 10 and < 15:
                    _state = TorchAnimState.Loop1;

                    _sparkleMain.startLifetime = 0.7f;
                    _sparkleMain.startSpeed = 1.5f;
                    _sparkleMain.startSize  = 0.1f;

                    _sparkleShape.radius = 1f;

                    break;
                case >= 15:
                    _state = TorchAnimState.Loop0;

                    _sparkleMain.startLifetime = 0.8f;
                    _sparkleMain.startSpeed = 2f;
                    _sparkleMain.startSize  = 0.125f;

                    _sparkleShape.radius = 1.5f;
                    break;
            }

            flameAnimation.SetInteger(State, (int)_state);
        }
    }
}