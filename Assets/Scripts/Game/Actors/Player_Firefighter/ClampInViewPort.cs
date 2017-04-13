using UnityEngine;

namespace Assets.Scripts.Game.Actors.Player_Firefighter {

    /// <summary>
    /// 
    /// </summary>
    public class ClampInViewPort : MonoBehaviour {

        protected void Start() {

        }

        protected void Update() {
            Vector3 pos = UnityEngine.Camera.main.WorldToViewportPoint(transform.position);
            pos.x = Mathf.Clamp01(pos.x);
            pos.y = Mathf.Clamp01(pos.y);
            transform.position = UnityEngine.Camera.main.ViewportToWorldPoint(pos);
        }
    }
}