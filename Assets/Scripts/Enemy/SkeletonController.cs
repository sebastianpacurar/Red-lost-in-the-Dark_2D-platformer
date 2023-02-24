using CustomAttributes;
using UnityEngine;

namespace Enemy {
    public class SkeletonController : MonoBehaviour {
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;

        [SerializeField] private float minDistanceFromPlayer;

        [ReadOnlyProp] [SerializeField] private bool isPlayerDetected;
        [ReadOnlyProp] [SerializeField] private float dirX;

        [ReadOnlyProp] [SerializeField] private bool isPlayerFacingSelf;
        [ReadOnlyProp] [SerializeField] private bool isWalking;
        [ReadOnlyProp] [SerializeField] private bool isRunning;
        [ReadOnlyProp] [SerializeField] private float currentSpeed;

        [ReadOnlyProp] [SerializeField] private bool isAttacking;

        private Rigidbody2D _rb;
        private Animator _animator;
        private Transform _playerTransform;
        private static readonly int State = Animator.StringToHash("state");

        private void Awake() {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start() {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void FlipSkeletonScale() {
            if (dirX < 0f) {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            } else if (dirX > 0f) {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        private void Update() {
            HandlePlayerDetection();

            if (isPlayerDetected) {
                HandleMovementConstraints();
                FlipSkeletonScale();
                HandleAnimations();
            }
        }

        private void FixedUpdate() {
            if (!isPlayerDetected) return;
            _rb.velocity = new Vector2(currentSpeed * Mathf.Round(dirX), _rb.velocity.y);
        }

        private void HandleMovementConstraints() {
            if (Vector2.Distance(transform.position, _playerTransform.position) < minDistanceFromPlayer && isPlayerFacingSelf && !isAttacking) {
                currentSpeed = 0f;
                isWalking = false;
                isRunning = false;
                isAttacking = true;
            } else if (!isAttacking) {
                isWalking = isPlayerFacingSelf;
                isRunning = !isPlayerFacingSelf;
                currentSpeed = isPlayerFacingSelf ? walkSpeed : runSpeed;
            }
        }

        private void HandlePlayerDetection() {
            var pos = transform.position;
            var playerPos = _playerTransform.position;
            var playerScale = _playerTransform.localScale;
            var direction = playerPos - pos;

            dirX = direction.normalized.x;
            isPlayerFacingSelf = Vector2.Dot(transform.localScale, playerScale).Equals(0f);

            var rayMask = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Character");
            var rayInfo = Physics2D.RaycastAll(pos, direction, 50f, rayMask);

            if (rayInfo.Length > 1) {
                if (rayInfo[1].collider.CompareTag("Player")) {
                    isPlayerDetected = true;
                    return;
                }
            }

            isPlayerDetected = false;
        }

        private void HandleAnimations() {
            var state = AnimationState.Idle;

            if (isWalking) state = AnimationState.Walk;
            if (isRunning) state = AnimationState.Run;

            if (isAttacking)
                state = Random.Range(3, 5) switch {
                    3 => AnimationState.FirstAttack,
                    4 => AnimationState.SecondAttack,
                    _ => AnimationState.FirstAttack,
                };

            _animator.SetInteger(State, (int)state);
        }

        // called in every Attack animation as event
        public void StopAttackEvent() {
            isAttacking = false;
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