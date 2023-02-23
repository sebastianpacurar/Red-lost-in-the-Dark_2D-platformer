using UnityEngine;

namespace Enemy {
    public class SkeletonController : MonoBehaviour {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float jumpForce;

        private Rigidbody2D _rb;
        private Animator _animator;
        private Transform _playerTransform;

        private void Start() {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void FlipSkeletonScale() {
            if (_rb.velocity.x < 0f) {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            } else if (_rb.velocity.x > 0f) {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        private void Update() { }

        private void HandlePlayerDetection() {
            var pos = transform.position;
            var direction = _playerTransform.position - pos;
            var rayMask = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Character");
            var rayInfo = Physics2D.RaycastAll(pos, direction, 50f, rayMask);
            
        }

        private void HandleAnimations() {
            var state = AnimationState.Idle;
        }

        private enum AnimationState {
            Idle,
            Run,
            Ascend,
            Descend,
            Fall,
            Slide,
            FirstAttack,
            SecondAttack,
            ThirdAttack
        }
    }
}