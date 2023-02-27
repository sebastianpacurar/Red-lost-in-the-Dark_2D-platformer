using UnityEngine;

namespace Tutorial {
    public class RevealSignContent : MonoBehaviour {
        private GameObject _canvasObj;

        private void Start() {
            _canvasObj = transform.GetChild(0).gameObject;
            _canvasObj.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D col) {
            if (col.gameObject.CompareTag("Player")) {
                _canvasObj.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                _canvasObj.SetActive(false);
            }
        }
    }
}