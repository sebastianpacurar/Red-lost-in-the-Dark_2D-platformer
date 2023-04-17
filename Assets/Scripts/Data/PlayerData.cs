using UnityEngine;

namespace Data {
    [CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/ Base Data")]
    public class PlayerData : ScriptableObject {
        [Header("Move State")]
        public float movementVelocity = 10f;

        [Header("Jump State")]
        public float jumpVelocity = 10f;
        public int amountOfJumps = 1;

        [Header("Check Variables")]
        public Vector2 capsuleSize = new(0.425f, 0.05f);
        public LayerMask groundMask;
    }
}