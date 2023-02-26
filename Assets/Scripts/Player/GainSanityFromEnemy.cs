using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player {
    public class GainSanityFromEnemy : MonoBehaviour {
        [SerializeField] private Light2D playerLight;
        [SerializeField] private float gainedSanityPoints;


        // when enemy is hit, increase sanity and mark enemy as untagged
        private void OnTriggerEnter2D(Collider2D col) {
            if (!col.gameObject.CompareTag("Enemy")) return;
            playerLight.pointLightOuterRadius += gainedSanityPoints;
            col.gameObject.tag = "Untagged";
        }
    }
}