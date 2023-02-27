using System.Collections;
using CustomAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial {
    public class SwapImageSprites : MonoBehaviour {
        [SerializeField] private float timeMultiplier;
        [SerializeField] private Image[] targetImg;
        [SerializeField] private Sprite[] keyUnpressed;
        [SerializeField] private Sprite[] keyPressed;

        [ReadOnlyProp] [SerializeField] private bool isPressedSelected;

        private void OnEnable() {
            StartCoroutine(nameof(SwapImages));
        }

        private void OnDisable() {
            StopCoroutine(nameof(SwapImages));
        }

        private IEnumerator SwapImages() {
            while (true) {
                yield return new WaitForSeconds(timeMultiplier);
                var final = isPressedSelected ? keyUnpressed : keyPressed;

                for (var i = 0; i < targetImg.Length; i++) {
                    targetImg[i].sprite = final[i];
                }

                isPressedSelected = !isPressedSelected;
            }
        }
    }
}