using System.Runtime.InteropServices;
using UnityEngine;

namespace Menu.OpenLinks {
    public class OpenLinks : MonoBehaviour {
        [DllImport("__Internal")]
        private static extern void OpenTab(string url);

        private static void OpenUrl(string url) {
            // run only from WebGl builds
            #if !UNITY_EDITOR && UNITY_WEBGL
			OpenTab(url);
            #endif
        }

        //TODO: add proper link here
        public void GoToSebyGit() {
            OpenUrl("https://seby-pacu.itch.io/");
        }

        public void GoToLegnopos() {
            OpenUrl("https://legnops.itch.io/");
        }

        public void GoToSanctumpixel() {
            OpenUrl("https://sanctumpixel.itch.io/");
        }

        public void GoToAdamatomic() {
            OpenUrl("https://adamatomic.itch.io/");
        }
        
        public void GoToBrullov() {
            OpenUrl("https://brullov.itch.io/");
        }

        public void GoToCaz() {
            OpenUrl("https://cazwolf.itch.io/");
        }

        public void GoToNicolemariet() {
            OpenUrl("https://nicolemariet.itch.io/");
        }
    }
}