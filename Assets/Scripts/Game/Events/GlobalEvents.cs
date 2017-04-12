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
        public class SceneEvent : UnityEvent<int> { }

        [System.Serializable]
        public class TeamEvent : UnityEvent<GameManager.Team> { }

        [System.Serializable]
        public class WaterBar : UnityEvent<float> { }

    }
}
