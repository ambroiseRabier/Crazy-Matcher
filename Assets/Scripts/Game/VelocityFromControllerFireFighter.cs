using Assets.Scripts.Game.Actors;
using Events;
using UnityEngine;

namespace Assets.Scripts.Game {

    /// <summary>
    /// 
    /// </summary>
    public class VelocityFromControllerFireFighter : VelocityFromController {

        [SerializeField] private VibrationSettings vibrationOnWaterKillMatche;

        protected void Start() {
            GlobalEventBus.onWaterKillMatcheByPlayer.AddListener(OnWaterKillMatcheByPlayer);
        }

        protected void OnWaterKillMatcheByPlayer() {
            m_Controller.rewiredController.SetVibration(
                vibrationOnWaterKillMatche.left,
                vibrationOnWaterKillMatche.right,
                vibrationOnWaterKillMatche.duration
            );
        }
    }
}