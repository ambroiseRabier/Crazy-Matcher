using UnityEngine;
using Rewired;

namespace Assets.Scripts.Game.Actors {

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

        protected void Awake () {
            m_player = ReInput.players.GetPlayer(playerId);
            m_move = Vector2.zero;
        }

        protected void Start () {

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

    }
}