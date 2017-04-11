using UnityEngine;
using System;
using UnityEditor;

namespace Assets.Scripts.Game.Actors.Player_Firefighter {

    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(VelocityFromController))]
    public class ShootWater : MonoBehaviour {

        [SerializeField] float m_SHOOT_POWER = 2f;

        [SerializeField] private float CONTROL_ROTATION_FACTOR = 0.05f;
        [SerializeField] private float DISPERSION_FACTOR = 0.1f;

        private static ShootWater _instance;

        [SerializeField] private GameObject m_WaterPrefab;
        private VelocityFromController m_VelocityFromController;

        [SerializeField] private float m_waterResourceMAX = 200f;
        [SerializeField] private float m_waterResource = 100f;

        public Controller Controller {
            get {
                return m_VelocityFromController.Controller;
            }
            set {
                m_VelocityFromController.Controller = value;
            }
        }

        /// <summary>
        /// instance unique de la classe     
        /// </summary>
        public static ShootWater instance {
            get {
                return _instance;
            }
        }

        protected void Awake () {
            if (_instance != null) {
                throw new Exception("Tentative de création d'une autre instance de ShootWater alors que c'est un singleton.");
            }
            _instance = this;
        }

        protected void Start () {
            m_VelocityFromController = GetComponent<VelocityFromController>();
            UpdateWaterResource();
        }

        protected void Update () {
            if (m_VelocityFromController.Controller.Fire && m_waterResource > 0) {
                m_waterResource -= 1;
                UpdateWaterResource();
                InstantiateWater();
            } // else feedback plus d'eau !


        }

        protected void UpdateWaterResource () {
            Events.GlobalEventBus.onWaterChange.Invoke(m_waterResource / m_waterResourceMAX);
        }

        private void InstantiateWater () {
            GameObject water = Instantiate(m_WaterPrefab, transform.parent);
            //PrefabUtility.InstantiatePrefab(m_WaterPrefab) as GameObject;
            water.transform.position = transform.position;
            water.GetComponent<Rigidbody>().AddForce(
                new Vector3(
                    Controller.Joystick.x * CONTROL_ROTATION_FACTOR + UnityEngine.Random.Range(-DISPERSION_FACTOR, DISPERSION_FACTOR),
                    Controller.Joystick.y * CONTROL_ROTATION_FACTOR + UnityEngine.Random.Range(-DISPERSION_FACTOR, DISPERSION_FACTOR),
                    m_SHOOT_POWER
                ) * 500
            ); //Controller.Joystick
            //Debug.Log(Controller.Joystick);
        }

        protected void OnDestroy () {
            _instance = null;
        }
    }
}