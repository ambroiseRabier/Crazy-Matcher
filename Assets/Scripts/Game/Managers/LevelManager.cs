using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Managers {

    /// <summary>
    /// 
    /// </summary>
    public class LevelManager : MonoBehaviour {

        /// <summary>
        /// Choose a level (a scene in fact) in those listed in GameManager (in unity editor)
        /// This number will be used in code
        /// </summary>
        public static int desiredLevel { get; private set; }

        [SerializeField] protected Dropdown levelSelectTitleScreen;
        //[SerializeField] protected Dropdown levelSelectWinscreen; 
        // problem whit hiding the dropdown, UI seems entirely managed by animations transitions.

        protected void Awake () {
            desiredLevel = 1;
        }

        protected void Start() {
            levelSelectTitleScreen.onValueChanged.AddListener(onDropdown);
            //levelSelectWinscreen.onValueChanged.AddListener(onDropdown);
        }

        protected void onDropdown (int pInt) {
            desiredLevel = pInt+1; //+1 because first level value returned is 0, and 0 is the main scene
        }

        protected void OnDestroy () {
            levelSelectTitleScreen.onValueChanged.RemoveListener(onDropdown);
            //levelSelectWinscreen.onValueChanged.RemoveListener(onDropdown);
        }

    }
}