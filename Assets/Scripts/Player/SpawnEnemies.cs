using System;
using CustomAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player {
    public class SpawnEnemies : MonoBehaviour {
        [SerializeField] private GameObject skeletonPrefab;
        [SerializeField] private GameObject skeletonContainer;
        [SerializeField] private int maxAllowedInContainer;

        [SerializeField] private Transform pointsContainer;
        [SerializeField] private Transform leftSide;
        [SerializeField] private Transform rightSide;
        [SerializeField] private float xOffset;
        [SerializeField] private LayerMask groundLayer;

        [SerializeField] private float minRandSpawnTime;
        [SerializeField] private float maxRandSpawnTime;

        [ReadOnlyProp] [SerializeField] private float spawnCdValue;
        [ReadOnlyProp] [SerializeField] private float spawnTime;

        [ReadOnlyProp] [SerializeField] private bool isLeftAvailable;
        [ReadOnlyProp] [SerializeField] private bool isRightAvailable;
        [ReadOnlyProp] [SerializeField] private bool safeZone;

        [ReadOnlyProp] [SerializeField] private int skeletonsCount;

        private HandleHpSanity _handleHpSanity;


        private void Start() {
            _handleHpSanity = GetComponent<HandleHpSanity>();
        }
        
        private void Update() {
            skeletonsCount = skeletonContainer.transform.childCount;

            // prevent pointsContainer from swapping locations when player flips X scale to -1
            pointsContainer.transform.localScale = transform.localScale;

            isLeftAvailable = Physics2D.OverlapCapsule(leftSide.position, new Vector2(0.5f, 3f), CapsuleDirection2D.Vertical, 0, groundLayer);
            isRightAvailable = Physics2D.OverlapCapsule(rightSide.position, new Vector2(0.5f, 3f), CapsuleDirection2D.Vertical, 0, groundLayer);

            HandleSpawnPointsPositions();
            ValidateSpawnPoints();
            SpawnEnemy();
        }

        private void OnTriggerStay2D(Collider2D other) {
            if (other.gameObject.CompareTag("CheckpointTorch")) {
                safeZone = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.CompareTag("CheckpointTorch")) {
                safeZone = false;
            }
        }


        // set the points to close in on the player, when sanity is dropping
        private void HandleSpawnPointsPositions() {
            var posX = (_handleHpSanity.maxSanity / 2) + xOffset;

            if (_handleHpSanity.SanityPoints <= _handleHpSanity.maxSanity / 2) {
                posX = _handleHpSanity.SanityPoints + xOffset;
            }

            leftSide.localPosition = new Vector3(-posX, 0, 0);
            rightSide.localPosition = new Vector3(posX, 0, 0);
        }


        // prevent spawning enemies on the other side of the wall if it's the case
        private void ValidateSpawnPoints() {
            var pos = transform.position;
            var leftPos = leftSide.position;
            var rightPos = rightSide.position;
            var rayMask = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Character") | 1 << LayerMask.NameToLayer("SpawnPoints");

            var rayInfoLeft = Physics2D.RaycastAll(pos, leftPos - pos, Vector2.Distance(pos, leftPos), rayMask);
            var rayInfoRight = Physics2D.RaycastAll(pos, rightPos - pos, Vector2.Distance(pos, rightPos), rayMask);

            var leftWallIndex = Array.FindIndex(rayInfoLeft[1..], obj => obj.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")));
            var leftPointIndex = Array.FindIndex(rayInfoLeft[1..], obj => obj.collider.gameObject.layer.Equals(LayerMask.NameToLayer("SpawnPoints")));

            var rightWallIndex = Array.FindIndex(rayInfoRight[1..], obj => obj.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")));
            var rightPointIndex = Array.FindIndex(rayInfoRight[1..], obj => obj.collider.gameObject.layer.Equals(LayerMask.NameToLayer("SpawnPoints")));

            // if there is a wall between the player and the target spawn point, then set to false
            if (leftWallIndex > -1 && leftWallIndex < leftPointIndex) isLeftAvailable = false;
            if (rightWallIndex > -1 && rightWallIndex < rightPointIndex) isRightAvailable = false;
        }


        // handle Spawning Enemies: spawn skeleton when current sanity is lower than half of max sanity (when progress bar is 50%)
        private void SpawnEnemy() {
            if (safeZone) return; // skip when in the Checkpoint radius
            if (skeletonsCount == maxAllowedInContainer) return; // skip when container reached max capacity
            if (!isLeftAvailable && !isRightAvailable) return; // skip when no spawning points are available

            // if sanity is smaller than mid (max/2) begin spawning process
            if (_handleHpSanity.SanityPoints <= _handleHpSanity.maxSanity / 2) {
                // spawn enemies from behind, in case both spawn points are available
                var spawnLoc = transform.localScale.x switch {
                    -1f when isRightAvailable => rightSide.position,
                    1f when isLeftAvailable => leftSide.position,
                    _ => isLeftAvailable ? leftSide.position : rightSide.position // defaults to the only available side
                };

                spawnCdValue += Time.deltaTime;

                if (spawnCdValue >= spawnTime) {
                    Instantiate(skeletonPrefab, spawnLoc, Quaternion.identity, skeletonContainer.transform);
                    spawnCdValue = 0f;
                    spawnTime = Random.Range(minInclusive: minRandSpawnTime, maxInclusive: maxRandSpawnTime); // randomize spawn time 
                }
            } else {
                // if sanity is larger than mid (max/2), keep the spawnCdValue to 0
                spawnCdValue = 0f;
            }
        }
    }
}