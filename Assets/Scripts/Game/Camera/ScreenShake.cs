using UnityEngine;

namespace Assets.Scripts.Game.Camera {

    [System.Serializable]
    public struct ScreenShakeParams {
        public float m_amplitude;
        public float m_amplitudeCompensator;
        public float m_frequency;
    }

    /// <summary>
    /// 
    /// </summary>
    public class ScreenShake : MonoBehaviour {


        [SerializeField] private ScreenShakeParams[] m_shakeParamsPerObjectivOnFire;

        /*[SerializeField] private float m_amplitude = 0.01f;
        [SerializeField] private float m_amplitudeCompensator = 10f;
        [SerializeField] private float m_frequency = 10f; //ms*/
        private int m_currentShakeParamIndex;
        private Vector3 originalPos;

        private bool inGame;

        protected void Start() {
            originalPos = transform.position;
            Events.GlobalEventBus.onTeamWin.AddListener(OnTeamWin);
            inGame = true; // this work because the scene is reloaded when the level end.
        }

        protected void Update() {
            ScreenShakeParams program = m_shakeParamsPerObjectivOnFire[m_currentShakeParamIndex];
            if (program.m_amplitude != 0 &&
                program.m_amplitudeCompensator != 0 &&
                program.m_frequency != 0 &&
                inGame) {
                Shake(program);
            }


            UpdateCurrentShakeIndex();
        }

        protected void Shake (ScreenShakeParams program) {
            Vector3 newPos = transform.position;

            if (Mathf.Round(Time.fixedTime * 1000) % program.m_frequency == 0) {
                newPos.x += Random.Range(-program.m_amplitude, program.m_amplitude);
                newPos.y += Random.Range(-program.m_amplitude, program.m_amplitude);
                newPos.x -= (newPos.x - originalPos.x) / program.m_amplitudeCompensator;
                newPos.y -= (newPos.y - originalPos.y) / program.m_amplitudeCompensator;
            } /*else { // useless ?
                newPos.x -= (newPos.x - originalPos.x) / 10;
                newPos.y -= (newPos.y - originalPos.y) / 10;
            }*/

            transform.position = newPos;
        }

        protected void OnTeamWin (GameManager.Team team) {
            inGame = false;
            originalPos = transform.position;
        }

        protected void UpdateCurrentShakeIndex () {
            m_currentShakeParamIndex = GetBurningObjectiveNumber();
        }

        protected int GetBurningObjectiveNumber () {
            Objectif[] list = GameObject.FindObjectsOfType<Objectif>();
            
            int count = 0;
            foreach (Objectif element in list) {
                if (element.isBurning2)
                    count++;
            }

            return count;
        }

    }
}