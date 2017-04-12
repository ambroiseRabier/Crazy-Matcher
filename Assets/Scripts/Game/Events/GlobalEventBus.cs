using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Utils;
using System;


namespace Events
{

    public static class GlobalEventBus
    {
        public static GlobalEvents.BaseGameEvent onTitleScreen = new GlobalEvents.BaseGameEvent();
        public static GlobalEvents.BaseGameEvent onMenu        = new GlobalEvents.BaseGameEvent();
        public static GlobalEvents.BaseGameEvent onPause       = new GlobalEvents.BaseGameEvent();
        public static GlobalEvents.BaseGameEvent onResume      = new GlobalEvents.BaseGameEvent();
        public static GlobalEvents.BaseGameEvent onStartLevel  = new GlobalEvents.BaseGameEvent();
        public static GlobalEvents.BaseGameEvent onRestartGame = new GlobalEvents.BaseGameEvent();

        public static GlobalEvents.TeamEvent onTeamWin = new GlobalEvents.TeamEvent();

        public static GlobalEvents.BaseGameEvent onInitLevel   = new GlobalEvents.BaseGameEvent();
        public static GlobalEvents.SceneEvent onLoadingScene   = new GlobalEvents.SceneEvent();
        
        public static GlobalEvents.WaterBar onWaterChange = new GlobalEvents.WaterBar();
        
    }

}