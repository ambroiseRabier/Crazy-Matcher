using System;
using UnityEngine;

namespace Com.IsartDigital.Utils.Game.Camera {

    /// <summary>
    /// Comportement à ajouter à la Caméra
    /// Permet de toujours avoir la Safe Zone visible
    /// 
    /// @version : 0.1.0
    /// TODO: appliquer sur un event onResize
    /// </summary>
    public class ShowAll : MonoBehaviour {

        public uint SafeZoneWidth = 2048;
        public uint SafeZoneHeight = 1366;
        public uint pixelPerUnit = 100;

        protected void Awake() {

            UnityEngine.Camera lCamera = GetComponent<UnityEngine.Camera>();
                   
            if (lCamera==null) throw new Exception("Le GameObject sur lequel est placé le composant ShowAll doit posséder un composant Camera.");
            else if (!lCamera.orthographic) throw new Exception("La Camera doit être orthographique pour que le comportement ShowAll s'applique.");
            else lCamera.orthographicSize = Screen.height / Math.Min((float)Screen.width / SafeZoneWidth, (float)Screen.height / SafeZoneHeight) / 2 / pixelPerUnit;
        }
        
    }
}