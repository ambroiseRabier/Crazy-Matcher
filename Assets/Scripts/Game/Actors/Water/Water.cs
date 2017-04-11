using UnityEngine;

namespace Assets.Scripts.Game.Actors.Water {

    /// <summary>
    /// 
    /// </summary>
    public class Water : MonoBehaviour {

        [SerializeField]
        private float m_Z_INDEX_SNAP = -0.5f;
        [SerializeField]
        private Transform m_ground;
        [SerializeField]
        private float reduceSpeed = 0.003f; // change to lifeTime ?
        private Rigidbody rb;
        

        protected void Start () {
            rb = gameObject.GetComponent<Rigidbody>();
            m_ground = GameObject.Find("Level").transform;
        }

        protected void Update () {
            Vector3 lScale = gameObject.transform.localScale;

            lScale.y = Mathf.Max(0, lScale.y - reduceSpeed);
            lScale.x = Mathf.Max(0, lScale.x - reduceSpeed);

            if (lScale.y == 0) {
                DestroyImmediate(this);
                return;
            }

            gameObject.transform.localScale = lScale;
            CheckCollisionGround();
        }

        private void OnTriggerEnter (Collider collision) {
            Burnable burnable = collision.gameObject.GetComponent<Burnable>();

            if (burnable)
                burnable.TryExtinguish();
        }

        private void CheckCollisionGround() {
            if (transform.position.z >= m_ground.position.z) {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    m_Z_INDEX_SNAP
                );
                rb.isKinematic = true;
            }
        }


    }
}