using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player {
    public class StatsManager : MonoBehaviour {
        public float HealthPoints { get; set; }
        public float SanityPoints { get; set; }

        public float MaxHp = 100f;
        public float MaxSanity = 15f;
        [SerializeField] private Light2D playerLight;


        private void Start() {
            HealthPoints = MaxHp;
            StartCoroutine(nameof(DecreaseHp));
        }

        private void Update() {
            SanityPoints = playerLight.pointLightInnerRadius;
        }

        private IEnumerator DecreaseHp() {
            while (true) {
                yield return new WaitForSeconds(1);

                if (playerLight.pointLightInnerRadius <= 1) {
                    HealthPoints--;
                }
            }
        }
    }
}