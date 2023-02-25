using CustomAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player {
    public class SpawnEnemies : MonoBehaviour {
        [SerializeField] private GameObject skeletonPrefab;
        [SerializeField] private GameObject skeletonContainer;

        [SerializeField] private GameObject spawnPoints;
        [SerializeField] private GameObject leftSide;
        [SerializeField] private GameObject rightSide;

        [SerializeField] private float minRandSpawnTime;
        [SerializeField] private float maxRandSpawnTime;

        [ReadOnlyProp] [SerializeField] private float spawnCdValue;
        [ReadOnlyProp] [SerializeField] private float spawnTime;

        private Light2D _playerLight;
        private HandleHpSanity _handleHpSanity;
        private Rigidbody2D _rb;

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Start() {
            _handleHpSanity = GetComponent<HandleHpSanity>();
        }

        private void Update() {
            // prevent spawnPoints from swapping locations when player flips X scale to -1
            spawnPoints.transform.localScale = transform.localScale;

            // handle Spawning Enemies: spawn skeleton when current sanity is lower than half of max sanity (when progress bar is 50%)
            if (_handleHpSanity.SanityPoints <= _handleHpSanity.maxSanity / 2) {
                spawnCdValue += Time.deltaTime;

                if (spawnCdValue >= spawnTime) {
                    SpawnSkeleton();
                    spawnCdValue = 0f;
                    spawnTime = Random.Range(minInclusive: minRandSpawnTime, maxInclusive: maxRandSpawnTime); // randomize spawn time 
                }
            } else {
                spawnCdValue = 0f;
            }
        }

        private void SpawnSkeleton() {
            // defaults to left as player starts in the scene facing towards right
            var spawnLoc = leftSide.transform.position;
            // make spawn point happen only from behind the player
            if (_rb.velocity.x < 0f) spawnLoc = rightSide.transform.position;

            Instantiate(skeletonPrefab, spawnLoc, Quaternion.identity, skeletonContainer.transform);
        }
    }
}