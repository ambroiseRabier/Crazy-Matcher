using UnityEngine;

namespace Assets.Scripts.Game.Actors.Water {

    /// <summary>
    /// 
    /// </summary>
    public class Water : MonoBehaviour {

        [SerializeField]
        private float reduceSpeed = 0.003f; // change to lifeTime ?

        protected void Start () {
        }

        protected void Update () {
            Vector3 lScale = gameObject.transform.localScale;

            lScale.z = Mathf.Max(0, lScale.z - reduceSpeed);
            lScale.x = Mathf.Max(0, lScale.x - reduceSpeed);

            if (lScale.z == 0) {
                DestroyImmediate(this);
                return;
            }

            gameObject.transform.localScale = lScale;
        }

        private void OnTriggerEnter (Collider collision) {
            Matches watches = collision.gameObject.GetComponent<Matches>();

            if (watches != null && watches.IsOnFire)
                watches.Extinct();
        }


    }
}