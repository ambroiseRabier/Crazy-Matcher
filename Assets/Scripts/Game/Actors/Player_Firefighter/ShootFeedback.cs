using UnityEngine;

namespace Assets.Scripts.Game.Actors.Player_Firefighter {

    /// <summary>
    /// 
    /// </summary>
    public class ShootFeedback : MonoBehaviour {

        [SerializeField] private float m_defaultScale;
        [SerializeField] private float m_shootScale;
        [SerializeField] private GameObject m_fireFigther;
        private Controller m_myControl;

        protected void Start() {
            m_myControl = m_fireFigther.GetComponent<ShootWater>().Controller;
        }

        protected void Update() {
            if (m_myControl.Fire) {
                transform.localScale = new Vector2(m_shootScale, m_shootScale);
            } else {
                transform.localScale = new Vector2(m_defaultScale, m_defaultScale);
            }
        }
    }
}