using UnityEngine;
using GAF.Core;

namespace Assets.Scripts.Game.Actors.Player_Watches {

    /// <summary>
    /// 
    /// </summary>
    public class MatchesLifeFeedback : MonoBehaviour {

        [SerializeField] private GAFBakedMovieClip m_gafHead;
        [SerializeField] private AnimationCurve m_headScalePerBurnRatio;

        private Vector2 m_gafHeadStartScale;

        protected void Start() {
            m_gafHeadStartScale = m_gafHead.transform.localScale;
        }

        protected void Update() {
            Vector2 newScale = new Vector2(
                m_gafHeadStartScale.x * m_headScalePerBurnRatio.Evaluate(gameObject.GetComponent<Matches>().BurnRatio),
                m_gafHeadStartScale.y * m_headScalePerBurnRatio.Evaluate(gameObject.GetComponent<Matches>().BurnRatio)
            );

            m_gafHead.transform.localScale = newScale;
        }
    }
}