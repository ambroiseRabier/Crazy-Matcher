using UnityEngine;
using GAF.Core;

namespace Assets.Scripts.Game.Actors.Water {

    /// <summary>
    /// 
    /// </summary>
    public class Water : MonoBehaviour {

        // todo: put serializedfield on the same line
        [SerializeField]
        private float m_Z_INDEX_SNAP = -0.5f;
        [SerializeField]
        private Transform m_ground;
        [SerializeField]
        private float reduceSpeed = 0.003f; // change to lifeTime ?
        [SerializeField]
        private GameObject m_waterSprite;
        [SerializeField]
        private GameObject m_waterSplashSprite;
        [SerializeField]
        private AnimationCurve alphaByHeight;
        [SerializeField]
        private GAFMovieClip splashAnimation;
        [SerializeField]
        private GAFBakedMovieClip projectileAnimation;

        private Rigidbody rb;
        private Renderer m_renderer;
        private SpriteRenderer[] m_renderers;
        private float m_startZDistance;
        private bool groundHitted;

        protected void Awake () {
            m_renderer = gameObject.GetComponent<Renderer>();
            rb = gameObject.GetComponent<Rigidbody>();
            m_renderers = gameObject.transform.Find("GFX").GetComponentsInChildren<SpriteRenderer>();
            m_waterSplashSprite.GetComponent<Renderer>().enabled = false; // useless
            m_waterSprite.GetComponent<Renderer>().enabled = true; // useless
        }

        protected void Start () {
            // cannot add the ground by link in editor :/
            m_ground = GameObject.Find("Level").transform;
            m_startZDistance = transform.position.z;
            splashAnimation.gameObject.SetActive(false);
            projectileAnimation.gameObject.SetActive(true);
        }

        protected void Update () {
            //Vector3 lScale = gameObject.transform.localScale;
            
            /*lScale.y = Mathf.Max(0, lScale.y - reduceSpeed);
            lScale.x = Mathf.Max(0, lScale.x - reduceSpeed);

            if (lScale.y == 0) {
                DestroyImmediate(gameObject);
                return;
            }*/

            //if (splashAnimation.currentFrameNumber)
            if (!splashAnimation.isPlaying() && groundHitted) {
                DestroyImmediate(gameObject);
                return;
            }

            //gameObject.transform.localScale = lScale;
            CheckCollisionGround();
            UpdateHeightFeedBack();
        }
        
        private void OnTriggerEnter (Collider collision) {
            Matches matches = collision.gameObject.GetComponent<Matches>();

            if (matches)
                matches.TryExtinguish(true);
            else {
                Sprinkler sprinkler = collision.gameObject.GetComponent<Sprinkler>();
                if (sprinkler) {
                    sprinkler.TryToFill();
                }
            }
        }

        private void CheckCollisionGround () {
            if (transform.position.z >= m_ground.position.z) {
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    m_Z_INDEX_SNAP
                );
                rb.isKinematic = true;
                SetAlphas(1f);
                ChangeSpriteToSplash();
                PlaySplash();

                groundHitted = true;
            }
        }

        private void UpdateHeightFeedBack () {
            // this calcul seem a bit wrong, but in global it work.
            //Debug.Log("("+m_startZDistance+" - "+transform.position.z+") / "+m_startZDistance);

            // this one only work for sprite renderer, not GAF
            //SetAlphas(alphaByHeight.Evaluate(Mathf.Clamp((m_startZDistance - transform.position.z) / m_startZDistance, 0, 1))); // ((-400) - (-380))/(-400) // ((-total) - (-current))/(-total) // m_startZDistance / transform.position.z

            float newScale = alphaByHeight.Evaluate(Mathf.Clamp((m_startZDistance - transform.position.z) / m_startZDistance, 0, 1));
            transform.localScale = new Vector3(newScale, newScale, 1);
        }

        public void SetAlphas (float newAlpha) {
            foreach (SpriteRenderer r in m_renderers) {
                r.color = new Vector4(r.color.r, r.color.g, r.color.b, newAlpha);
            }
        }

        public void ChangeSpriteToSplash() {
            // give better names to the sprites objects
            m_waterSplashSprite.GetComponent<Renderer>().enabled = true;
            m_waterSprite.GetComponent<Renderer>().enabled = false;
        }

        private void PlaySplash () {
            //splashAnimation.GetComponent<Renderer>().enabled = true;
            projectileAnimation.gameObject.SetActive(false);
            splashAnimation.gameObject.SetActive(true);
            splashAnimation.play();
        }

        /*public static void SetAlpha(this Material material, float value) {
            Color color = material.color;
            color.a = value;
            material.color = color;
        }*/


    }
}