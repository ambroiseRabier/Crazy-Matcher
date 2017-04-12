using UnityEngine;

namespace Assets.Scripts.Game.Camera {

    /// <summary>
    /// 
    /// </summary>
    public class ScreenShake : MonoBehaviour {

        private float m_amplitude = 0.01f;
        private float m_amplitudeCompensator = 10f;
        private float m_frequency = 10f; //ms
        private Vector3 originalPos;

        protected void Start() {
            originalPos = transform.position;
        }

        protected void Update() {
            Vector3 newPos = transform.position;

            if (Mathf.Round(Time.fixedTime*1000) % m_frequency == 0) {
                newPos.x += Random.Range(0, m_amplitude);
                newPos.y += Random.Range(0, m_amplitude);
                newPos.x -= (newPos.x - originalPos.x) / m_amplitudeCompensator;
                newPos.y -= (newPos.y - originalPos.y) / m_amplitudeCompensator;
            } /*else { // useless ?
                newPos.x -= (newPos.x - originalPos.x) / 10;
                newPos.y -= (newPos.y - originalPos.y) / 10;
            }*/

            transform.position = newPos;
        }
    }
}