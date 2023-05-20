using AYellowpaper.SerializedCollections;
using Plugins.SerializedCollections.Runtime.Scripts;
using UnityEngine;

namespace Data {
    [CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/ Base Data")]
    public class PlayerData : ScriptableObject {
        [Header("CapsuleCollider Data")]
        [SerializedDictionary("Capsule Type", "[offset, size]")]
        public SerializedDictionary<string, Vector2[]> capsuleProps = new() {
            { "normal", new Vector2[] { new(0.1056232f, -0.1030393f), new(1.061398f, 1.941665f) } },
            { "groundSlide", new Vector2[] { new(-0.0825872f, -0.07872185f), new(1.967494f, 0.709112f) } }
        };

        [Header("Move State")]
        public float movementVelocity = 10f;

        [Header("Ground Slide State")]
        public float groundSlideVelocity = 15f;
        public float groundSlideMinTime = 0.5f;

        [Header("Jump State")]
        public float jumpVelocity = 15f;
        public float maxSafeFallSpeed = -25f;
        [SerializedDictionary("Distance Between Walls", "Gravity Force")]
        public SerializedDictionary<int, float> gravityForce;

        [Header("Wall Jump State")]
        [Space(10f)]
        [SerializedDictionary("Distance Between Walls", "Wall Jump Duration")]
        public SerializedDictionary<int, float> wallJumpDur;
        [SerializedDictionary("Distance Between Walls", "Wall Jump Force")]
        public SerializedDictionary<int, Vector2> wallJumpForce;

        [Header("Wall Slide State")]
        [Space(10f)]
        [Tooltip("Not implemented yet")]
        public float wallSlideVelocity = 3f;
        [SerializedDictionary("Distance Between Walls", "Wall Slide Hang Duration")]
        public SerializedDictionary<int, float> wallSlideHangDuration;

        [Header("Dash State")]
        public float dashCoolDown;
        public float dashTime;
        public float dashSpeed;
        public float distBetweenAfterImgs;

        [Header("Check Variables")]
        public Vector2 capsuleSize = new(0.425f, 0.05f);
        public LayerMask groundMask;

        [Header("Player Stats")]
        public float maxHp;
        public float maxSanity;
        public float fallDamage;
    }
}