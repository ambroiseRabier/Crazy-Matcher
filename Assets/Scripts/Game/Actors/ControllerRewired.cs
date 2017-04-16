using UnityEngine;
using Rewired;
using System.Collections;
using Events;
using System;

namespace Assets.Scripts.Game.Actors {

    [System.Serializable]
    struct VibrationSettings {
        [Tooltip("0 to 1")] public float left;
        [Tooltip("0 to 1")] public float right;
        [Tooltip("In seconds")] public float duration;
    }

    /// <summary>
    /// 
    /// </summary>
    public class ControllerRewired : MonoBehaviour {

        [SerializeField] private int playerId;
        public Vector2 m_move { get; private set; }
        public bool m_fire { get; private set; }
        public float m_triggerLeft { get; private set; }
        public float m_triggerRight { get; private set; }
        private Player m_player;
        protected float endTime;
        protected bool vibrationEndIsRunning;
        /// <summary>
        /// Had to add this because i get stuck on vibration on winscreen sometimes
        /// seem like i change body after the winscreen event ?
        /// </summary>
        protected bool allowVibration;

        protected void Awake () {
            m_player = ReInput.players.GetPlayer(playerId);
            m_move = Vector2.zero;
        }

        protected void Start () {
            GlobalEventBus.onTeamWin.AddListener(OnTeamWin);
            GlobalEventBus.onRestartGame.AddListener(OnRestartGame);
        }

        protected void Update () {
            GetInput();
        }

        private void GetInput () {
            // get input by name or action id
            m_move = new Vector2(
                m_player.GetAxis("Move Horizontal"),
                m_player.GetAxis("Move Vertical")
            );           
            m_fire = m_player.GetButton("Fire");
            m_triggerLeft = m_player.GetAxis("Press Trigger Left");
            m_triggerRight = m_player.GetAxis("Press Trigger Right");
        }

        private void OnRestartGame () {
            allowVibration = true;
        }

        private void OnTeamWin (GameManager.Team arg0) {
            allowVibration = false;
            StopVibration();
        }

        public void SetVibration (float leftMotor = 0f, float rightMotor = 0f, float timeSeconds = 0.2f) {
            if (timeSeconds == 0 || !allowVibration)
                return;


            foreach (Joystick j in m_player.controllers.Joysticks) {
                if (!j.supportsVibration)
                    continue;
                j.SetVibration(
                    Mathf.Clamp01(leftMotor), 
                    Mathf.Clamp01(rightMotor)
                );
                // left and right motor and left and right, but are different type of motor :(, right is quick, left is slow vibration.
                
            }
            // another way of doing this, but whitout spam vibration support
            //StartCoroutine("StartVibration", time);
            //Invoke(function, time)

            endTime = Time.fixedTime + timeSeconds;
            if (!vibrationEndIsRunning)
                StartCoroutine("CheckVibrationEnd");
        }

        protected IEnumerator CheckVibrationEnd () {
            vibrationEndIsRunning = true;
            while(Time.fixedTime < endTime) {
                yield return null;
            }
            StopVibration();
            vibrationEndIsRunning = false;
        }

        // another way of doing this, but whitout spam vibration support
        /*protected IEnumerator StartVibration (float duration) {
            yield return new WaitForSecondsRealtime(duration);
            StopVibration();
        }*/

        protected void StopVibration () {
            foreach (Joystick j in m_player.controllers.Joysticks) {
                j.StopVibration();
            }
        }

        protected void OnDestroy () {
            StopCoroutine("CheckVibrationEnd");
            StopVibration();
        }

    }
}