using UnityEngine;

namespace Data {
    [CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/ Base Data")]
    public class PlayerData : ScriptableObject {
        [Header("Move State")]
        public float movementVelocity = 10f;

        [Header("Jump State")]
        public float jumpVelocity = 10f;
        public int amountOfJumps = 1;

        [Header("Wall Slide State")]
        public float slideVelocity = 3f;

        [Header("Wall Jump State")]
        public float wallJumpDefaultDuration = 0.3f;
        public float wallClimbDefaultDuration = 0.15f;
        public Vector2 wallJumpForce = new(0.5f, 5f);
        public Vector2 wallClimbForce = new(3f, 15f);

        [Header("Check Variables")]
        public Vector2 capsuleSize = new(0.425f, 0.05f);
        public LayerMask groundMask;
    }
}