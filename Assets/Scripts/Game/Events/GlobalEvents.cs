using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Events
{
    public static class GlobalEvents
    {

        [System.Serializable]
        public class BaseGameEvent : UnityEvent { }

        [System.Serializable]
        public class InitLevelEvent : UnityEvent<string> { }

        [System.Serializable]
        public class TeamEvent : UnityEvent<GameManager.Team> { }

    }
}
