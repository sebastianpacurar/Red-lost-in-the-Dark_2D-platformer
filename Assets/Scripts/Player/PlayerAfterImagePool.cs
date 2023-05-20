using UnityEngine;
using System.Collections.Generic;

namespace Player {
    public class PlayerAfterImagePool : MonoBehaviour {
        [SerializeField] private GameObject afterImagePrefab;
        private readonly Queue<GameObject> _availableObjects = new();
        public static PlayerAfterImagePool Instance { get; private set; }

        private void Awake() {
            Instance = this;
            GrowPool();
        }

        private void GrowPool() {
            for (int i = 0; i < 5; i++) {
                var instanceToAdd = Instantiate(afterImagePrefab, transform, true);
                AddToPool(instanceToAdd);
            }
        }

        public void AddToPool(GameObject instance) {
            instance.SetActive(false);
            _availableObjects.Enqueue(instance);
        }

        public GameObject GetFromPool() {
            if (_availableObjects.Count == 0) {
                GrowPool();
            }
            var instance = _availableObjects.Dequeue();
            instance.SetActive(true);
            return instance;
        }
    }
}