using UnityEngine;

namespace Assets.Scripts.Game {

    /// <summary>
    /// SPEED variable IS NOT TO BE SETTED on the matches, (change burn speed on matches scripts)
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class VelocityFromControllerMatche : VelocityFromController {

        /*[SerializeField]*/ private float m_turnSpeed = 0.1f; // 0 to 1;
        /*[SerializeField] */private float m_maxTurnAnglePerFrame = 15f;
        /*[SerializeField] */private float m_maxTurnAnglePerFrameCar = 5f;
        //private Vector3 previousPosition = Vector3.zero; // don't use transform.rotation dummy, get the direction from last position
        private float previousInputAngle; // don't use transform.rotation dummy, get the direction from last position
        private Vector3 previousDirection = Vector3.zero;
        private bool firstInputSet = false;

        private Burnable m_burnableComponent;
        [SerializeField] private AnimationCurve speedBurnCurve;
        private float burnRatio;

        protected void Start () {
            m_burnableComponent = gameObject.GetComponent<Burnable>();
            m_burnableComponent.OnBurnRatioProgress += BurnableComponent_OnBurnRatioProgress;
        }

        protected void Update () {
                
        }

        override protected void FixedUpdate () {
            if (m_Controller && m_Controller.Joystick.normalized != Vector3.zero) {
                //SixthTest();
                if (firstInputSet) {


                    //ThirdTest();
                    //FourthTest(); // choose one
                    SeventhController(); // choosen one whit "speed growing whit death"
                    //FirstTest();
                    //FifthTest();
                } else {
                    firstInputSet = true;
                    //previousInputAngle = Mathf.Atan2(m_Controller.Joystick.normalized.y, m_Controller.Joystick.normalized.x);
                    previousDirection = m_Controller.Joystick.normalized;
                    m_currentDirection = m_Controller.Joystick.normalized;
                }

            }
        }

        private void BurnableComponent_OnBurnRatioProgress(Burnable burnable, float newBurnRatio) {
            burnRatio = newBurnRatio;
        }

        void SeventhController() {
            print(speedBurnCurve.Evaluate(burnRatio));
            m_Rigidbody.velocity = m_Controller.Joystick.normalized * m_Speed * speedBurnCurve.Evaluate(burnRatio);
        }

        //physic control, whit max speed
        void SixthTest () {
            m_Rigidbody.AddForce(previousDirection * m_Speed);
            m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, previousDirection * m_Speed, m_turnSpeed);
            if (m_Controller.Joystick.normalized != Vector3.zero)
                previousDirection = m_Controller.Joystick.normalized;
        }

        //car control
        void FifthTest () {
            float angleDiff = m_Controller.Joystick.normalized.x * m_maxTurnAnglePerFrameCar; // avec vitesse 200, mais collision galère
            Vector3 newDirection = Quaternion.Euler(0, 0, angleDiff) * previousDirection;
            float newAngleDiff = Vector3.Angle(newDirection, m_Controller.Joystick.normalized);

            m_Rigidbody.velocity = newDirection.normalized * m_Speed;
            previousDirection = newDirection.normalized;
        }

        //normal control
        void FourthTest () {
            m_Rigidbody.velocity = m_Controller.Joystick.normalized * m_Speed;
        }

        // physic control, no alway max speed
        void ThirdTest() {
            m_Rigidbody.AddForce(m_Controller.Joystick.normalized * m_Speed);
            m_Rigidbody.velocity = Vector3.Lerp(m_Rigidbody.velocity, m_Controller.Joystick.normalized * m_Speed, m_turnSpeed);
        }

        // limited angle turn
        void FirstTest () {
            //Vector3 newDirection = Vector3.zero;
            //float maxTurnPerFrameRad = m_maxTurnAnglePerFrame * Mathf.PI / 180;
            //float inputDirectionAngle = Mathf.Atan2(m_Controller.Joystick.normalized.y, m_Controller.Joystick.normalized.x);
            //float limitedDirectionAngle = Mathf.Lerp(previousInputAngle, inputDirectionAngle, 0.5f);//Mathf.Clamp(previousInputAngle - inputDirectionAngle, -maxTurnPerFrameRad, maxTurnPerFrameRad);
            float angleDiff = Vector3.Angle(previousDirection, m_Controller.Joystick.normalized); // can be a mess if z is different
            Debug.Log(angleDiff);
            angleDiff = Mathf.Clamp(angleDiff, -m_maxTurnAnglePerFrame, m_maxTurnAnglePerFrame);
            Debug.Log("after " + angleDiff);
            Vector3 newDirection = Quaternion.Euler(0, 0, -angleDiff) * previousDirection;
            float newAngleDiff = Vector3.Angle(newDirection, m_Controller.Joystick.normalized);
            Debug.Log("new " + newAngleDiff);

            /*newDirection = new Vector3(
                Mathf.Cos(limitedDirectionAngle),
                Mathf.Sin(limitedDirectionAngle),
                0
            );*/

            m_Rigidbody.velocity = newDirection.normalized * m_Speed;
            //previousInputAngle = limitedDirectionAngle;
            previousDirection = newDirection.normalized;
        }

        float m_maxAngleRotation = 15f;   // Défini comme membre sérializé
        Vector3 m_currentDirection; // Actuelle direction, défini comme membre

        // not working
        void SecondTest () {

            float currentRadian = Mathf.Atan2(m_currentDirection.y, m_currentDirection.x);
            float wantedRadian = Mathf.Atan2(m_Controller.Joystick.y, m_Controller.Joystick.x);
            float currentToWantedRadian = wantedRadian - currentRadian;
            float maxRadianRotation = m_maxAngleRotation * Mathf.Deg2Rad;
            float newRadian = currentRadian + Mathf.Clamp(currentToWantedRadian, maxRadianRotation, -maxRadianRotation);

            Vector3 newDirection = new Vector3(
              Mathf.Cos(newRadian),
              Mathf.Sin(newRadian)
            );

            m_Rigidbody.velocity = newDirection * m_Speed;
        }

        Vector2 rotateVector (Vector2 pVec, float pRad) {
            Vector2 copyTransform = pVec;//transform.rotation.eulerAngles;
            //Vector2 unitVector = Vector3.Normalize(copyTransform);
           /* Debug.Log("rotation: ");
            Debug.Log(Math.Atan2(unitVector.y, unitVector.x) / Mathf.PI * 180);*/
            Vector2 unitVectorRotated = Quaternion.Euler(0, 0, pRad *180/Mathf.PI) * copyTransform;
            return unitVectorRotated;
            /*Debug.Log("rotation -45: ");
            Debug.Log(Math.Atan2(unitVectorRotated.y, unitVectorRotated.x) / Mathf.PI * 180);*/
        }
    }
}