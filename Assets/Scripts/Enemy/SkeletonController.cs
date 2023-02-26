using System;
using CustomAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy {
    public class SkeletonController : MonoBehaviour {
        [SerializeField] private GameObject hitCapsuleObj;

        [SerializeField] private float minRandomWalk;
        [SerializeField] private float maxRandomWalk;
        [SerializeField] private float minRandomRun;
        [SerializeField] private float maxRandomRun;

        [SerializeField] public float walkSpeed;
        [SerializeField] public float runSpeed;

        [SerializeField] private float rayCastRange;
        [SerializeField] private float minAllowedDistance;
        [SerializeField] private float maxAllowedDistance;

        [ReadOnlyProp] [SerializeField] private bool isPlayerDetected;
        [ReadOnlyProp] [SerializeField] private float dirX;

        [ReadOnlyProp] [SerializeField] private bool isPlayerFacingSelf;
        [ReadOnlyProp] [SerializeField] private bool isWalking;
        [ReadOnlyProp] [SerializeField] private bool isRunning;
        [ReadOnlyProp] [SerializeField] private float currentSpeed;

        [ReadOnlyProp] [SerializeField] private bool isAttacking;

        [ReadOnlyProp] [SerializeField] private bool isDeathTriggered;

        private Rigidbody2D _rb;
        private Animator _animator;
        private Transform _playerTransform;
        private static readonly int State = Animator.StringToHash("state");
        private static readonly int Death = Animator.StringToHash("death");

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start() {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            // generate random values for walk and run speed
            walkSpeed = Random.Range(minInclusive: minRandomWalk, maxInclusive: maxRandomWalk);
            runSpeed = Random.Range(minInclusive: minRandomRun, maxInclusive: maxRandomRun);
        }

        private void FlipSkeletonScale() {
            if (isDeathTriggered) return;

            if (dirX < 0f) {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            } else if (dirX > 0f) {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        private void Update() {
            // kill when too far away from player
            if (Vector2.Distance(_playerTransform.position, transform.position) > maxAllowedDistance) {
                TriggerDeath();
            }

            HandlePlayerDetection();

            if (isPlayerDetected) {
                HandleMovementConstraints();
                FlipSkeletonScale();
            }

            HandleAnimations();
        }

        private void FixedUpdate() {
            if (!isPlayerDetected || isDeathTriggered) {
                _rb.velocity = Vector2.zero;
            } else {
                _rb.velocity = new Vector2(currentSpeed * Mathf.Round(dirX), _rb.velocity.y);
            }
        }

        // initiate Death process when hit by player or touched by Checkpoint radius
        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("PlayerHitArea") || col.gameObject.CompareTag("CheckpointTorch")) {
                TriggerDeath();
            }
        }

        private void HandleMovementConstraints() {
            if (Vector2.Distance(transform.position, _playerTransform.position) < minAllowedDistance) {
                if (!isAttacking) {
                    isAttacking = true;
                } else {
                    currentSpeed = 0f;
                    isWalking = false;
                    isRunning = false;
                }
            } else {
                isWalking = isPlayerFacingSelf;
                isRunning = !isPlayerFacingSelf;
                currentSpeed = isPlayerFacingSelf ? walkSpeed : runSpeed;
                isAttacking = false;
            }
        }

        private void HandlePlayerDetection() {
            var pos = transform.position;
            var playerPos = _playerTransform.position;
            var playerScale = _playerTransform.localScale;
            var direction = playerPos - pos;

            // dirX is the direction from skeleton (self) towards player
            dirX = direction.normalized.x;

            // used to swap between walk and run speed 
            isPlayerFacingSelf = Vector2.Dot(transform.localScale, playerScale).Equals(0f);

            var rayMask = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Character");
            var rayInfo = Physics2D.RaycastAll(pos, direction, rayCastRange, rayMask);

            // isPlayerDetected is true only if there is no wall between the player and the skeleton
            if (rayInfo.Length > 1) {
                var playerIndex = Array.FindIndex(rayInfo[1..], obj => obj.collider.CompareTag("Player"));
                var wallIndex = Array.FindLastIndex(rayInfo[1..], obj => obj.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")));

                if (playerIndex >= 0) {
                    if (playerIndex < wallIndex || wallIndex == -1) {
                        isPlayerDetected = true;
                        return;
                    }
                }
            }

            isPlayerDetected = false;
        }

        private void HandleAnimations() {
            var state = AnimationState.Idle;

            // if player is not detected, allow idle mode
            if (isPlayerDetected) {
                if (isWalking) {
                    state = AnimationState.Walk;
                } else if (isRunning) {
                    state = AnimationState.Run;
                } else if (isAttacking) {
                    state = Random.Range(3, 5) switch {
                        3 => AnimationState.FirstAttack,
                        4 => AnimationState.SecondAttack,
                        _ => AnimationState.FirstAttack,
                    };
                }
            }

            _animator.SetInteger(State, (int)state);
        }

        private void TriggerDeath() {
            _animator.SetTrigger(Death);
            isDeathTriggered = true;
        }

        // called in every Attack animation as event
        public void StopAttackEvent() {
            isAttacking = false;
            hitCapsuleObj.GetComponent<CapsuleCollider2D>().enabled = false;
        }

        // called in Death animation, after all keyframes + 2 have been reached 
        public void DestroySelf() {
            Destroy(gameObject);
        }

        private enum AnimationState {
            Idle,
            Walk,
            Run,
            FirstAttack,
            SecondAttack,
        }
    }
}