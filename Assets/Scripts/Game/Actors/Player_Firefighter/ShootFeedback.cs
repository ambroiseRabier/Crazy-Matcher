using UnityEngine;

namespace Assets.Scripts.Game.Actors.Player_Firefighter {

    /// <summary>
    /// 
    /// </summary>
    public class ShootFeedback : MonoBehaviour {

        [SerializeField] private float m_defaultScale;
        [SerializeField] private float m_shootScale;
        [SerializeField] private VelocityFromController m_fireFigtherVelocityFromController;

        protected void Update() {
            if (m_fireFigtherVelocityFromController.Controller != null && 
                m_fireFigtherVelocityFromController.Controller.Fire) {
                transform.localScale = new Vector2(m_shootScale, m_shootScale);
            } else {
                transform.localScale = new Vector2(m_defaultScale, m_defaultScale);
            }
        }
    }
}