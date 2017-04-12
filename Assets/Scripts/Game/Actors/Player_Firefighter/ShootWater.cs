using UnityEngine;
using System;
using UnityEditor;

namespace Assets.Scripts.Game.Actors.Player_Firefighter {

    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(VelocityFromController))]
    public class ShootWater : MonoBehaviour {

        [SerializeField] float m_SHOOT_POWER = 30f;

        [SerializeField] private float CONTROL_ROTATION_FACTOR = 6f; // 4.5
        [SerializeField] private float DISPERSION_FACTOR = 4f; //3

        private static ShootWater _instance;

        [SerializeField] private GameObject m_WaterPrefab;
        private VelocityFromController m_VelocityFromController;

        [SerializeField] private float m_waterResourceMAX = 200f;
        [SerializeField] private float m_waterResource = 0f;

        [SerializeField] private float WATER_GAIN_FACTOR = 5f; // * (0 to 1) (per trigger pressed and released)
        [SerializeField] private bool WATER_GAIN_INSTANT = false; // different algorythme, instant is more opti.
        private float previousTriggerLeftValue = 0;
        private float previousTriggerRightValue = 0;
        private float totalTriggerLeftValue = 0;
        private float totalTriggerRightValue = 0;

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
            m_VelocityFromController = GetComponent<VelocityFromController>();
        }

        protected void Start () {
            UpdateWaterResource();
            transform.position = new Vector3(transform.position.x, transform.position.y, -400);
        }

        protected void Update () {
            if (m_VelocityFromController.Controller.Fire && m_waterResource > 0) {
                m_waterResource -= 1;
                UpdateWaterResource();
                InstantiateWater();
            } // else feedback plus d'eau !
            if (WATER_GAIN_INSTANT)
                waterGainInstant();
            else
                waterGain();
        }

        protected void waterGain () {
            // only take a positiv value when pressing (trigger distance in this frame)
            float triggerLeftDiff = m_VelocityFromController.Controller.m_triggerLeft - previousTriggerLeftValue;
            float triggerRightDiff = m_VelocityFromController.Controller.m_triggerRight - previousTriggerRightValue;

            // push value in waterbar
            if (triggerLeftDiff < 0) {
                addWater(-triggerLeftDiff * WATER_GAIN_FACTOR);
            }
            if (triggerRightDiff < 0) {
                addWater(-triggerRightDiff * WATER_GAIN_FACTOR);
            }
            if (triggerLeftDiff < 0 || triggerRightDiff < 0)
                UpdateWaterResource();

            // remember previous trigger value
            previousTriggerLeftValue = m_VelocityFromController.Controller.m_triggerLeft;
            previousTriggerRightValue = m_VelocityFromController.Controller.m_triggerRight;
        }

        protected void waterGainInstant () {
            // only take a positiv value when pressing (trigger distance in this frame)
            float triggerLeftDiff = m_VelocityFromController.Controller.m_triggerLeft - previousTriggerLeftValue;
            float triggerRightDiff = m_VelocityFromController.Controller.m_triggerRight - previousTriggerRightValue;

            // check if relaesed
            bool isRealeasingLeft = m_VelocityFromController.Controller.m_triggerLeft < previousTriggerLeftValue;
            bool isRealeasingRight = m_VelocityFromController.Controller.m_triggerRight < previousTriggerRightValue;

            // add to total value (trigger distance)
            totalTriggerLeftValue += Mathf.Max(0, triggerLeftDiff);
            totalTriggerRightValue += Mathf.Max(0, triggerRightDiff);

            // push value in waterbar
            if (isRealeasingLeft && totalTriggerLeftValue > 0) {
                addWater(totalTriggerLeftValue * WATER_GAIN_FACTOR);
                //Debug.Log(totalTriggerLeftValue * WATER_GAIN_FACTOR); to see the gain for one trigger
                totalTriggerLeftValue = 0;
            }
            if (isRealeasingRight && totalTriggerRightValue > 0) {
                addWater(totalTriggerRightValue * WATER_GAIN_FACTOR);
                totalTriggerRightValue = 0;
            }
            if (isRealeasingLeft || isRealeasingRight)
                UpdateWaterResource();

            // remember previous trigger value
            previousTriggerLeftValue = m_VelocityFromController.Controller.m_triggerLeft;
            previousTriggerRightValue = m_VelocityFromController.Controller.m_triggerRight;
        }

        protected void addWater (float quantity) {
            m_waterResource = Mathf.Min(
                m_waterResource + quantity,
                m_waterResourceMAX
            );
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