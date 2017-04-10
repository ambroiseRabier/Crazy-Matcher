using UnityEngine;
using System;

namespace Assets.Scripts.Game {

    /// <summary>
    /// 
    /// </summary>
    public class PlayerMove : MonoBehaviour {

        private static PlayerMove _instance;
        private Rigidbody2D rb;
        [SerializeField]
        private float SPEED = 10;

        /// <summary>
        /// instance unique de la classe     
        /// </summary>
        public static PlayerMove instance {
            get {
                return _instance;
            }
        }

        protected void Awake () {
            if (_instance != null) {
                throw new Exception("Tentative de création d'une autre instance de PlayerMove alors que c'est un singleton.");
            }
            _instance = this;
        }

        protected void Start () {
            rb = GetComponent<Rigidbody2D>();

            Debug.Log("fdsfds");
            //Debug.Log(Input.GetAxis("Horizontal_P1"));
        }

        protected void Update () {
            //Debug.Log(Input.GetAxis("Vertical_P1"));
        }

        protected void FixedUpdate () {
            // "doom" speed, i know
            rb.velocity = new Vector2(
                Input.GetAxis("Horizontal") * SPEED,
                Input.GetAxis("Vertical") * SPEED
            );
        }

        protected void OnDestroy () {
            _instance = null;
        }
    }
}