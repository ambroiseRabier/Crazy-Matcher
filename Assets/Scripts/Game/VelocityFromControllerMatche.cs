using UnityEngine;

namespace Assets.Scripts.Game {

    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class VelocityFromControllerMatche : VelocityFromController {

        [SerializeField] private float m_maxTurnAnglePerFrame = 3f;
        private Vector3 previousPosition = Vector3.zero; // don't use transform.rotation dummy, get the direction from last position
        private float previousInputAngle; // don't use transform.rotation dummy, get the direction from last position
        private bool firstInputSet = false;

        protected void Update () {
                
        }

        override protected void FixedUpdate () {

            if (m_Controller) {
                if (firstInputSet && m_Controller.Joystick.normalized != Vector3.zero) {
                    //print(transform.position);
                    // take a little time to have a controller. (don't know why)

                    Vector3 newDirection = Vector3.zero;
                    //Vector3 currentDirection = previousPosition - transform.position;
                    //float currentDirectionAngle = Mathf.Atan2(currentDirection.y, currentDirection.x);
                    float inputDirectionAngle = Mathf.Atan2(m_Controller.Joystick.normalized.y, m_Controller.Joystick.normalized.x);
                    //float newDirectionAngle = currentDirectionAngle + Mathf.Clamp(inputDirectionAngle, -m_maxTurnAnglePerFrame, m_maxTurnAnglePerFrame);

                    //print(inputDirectionAngle);
                    //print("angle " + currentDirectionAngle);

                    newDirection = new Vector3(
                        Mathf.Cos(inputDirectionAngle),
                        Mathf.Sin(inputDirectionAngle),
                        0
                    );
                    //print("old" + previousPosition);
                    m_Rigidbody.velocity = newDirection * m_Speed;
                    //previousPosition = transform.position;
                    //print("new" + previousPosition);
                    previousInputAngle = inputDirectionAngle;

                } else {
                    if (m_Controller.Joystick.normalized != Vector3.zero) {
                        firstInputSet = true;
                        previousInputAngle = Mathf.Atan2(m_Controller.Joystick.normalized.y, m_Controller.Joystick.normalized.x);
                    }
                        
                }
                
            }
            
        }
    }
}