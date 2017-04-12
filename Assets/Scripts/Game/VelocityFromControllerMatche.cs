using UnityEngine;

namespace Assets.Scripts.Game {

    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class VelocityFromControllerMatche : VelocityFromController {

        [SerializeField] private float m_maxTurnAnglePerFrame = 3f;

        override protected void FixedUpdate () {
            Vector3 newDirection;
            float currentDirectionAngle = Mathf.Atan2(transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.x);
            float inputDirectionAngle = Mathf.Atan2(m_Controller.Joystick.normalized.y, m_Controller.Joystick.normalized.x);
            float newDirectionAngle = currentDirectionAngle + Mathf.Clamp(inputDirectionAngle, -m_maxTurnAnglePerFrame, m_maxTurnAnglePerFrame);



            newDirection = new Vector2(
                Mathf.Cos(currentDirectionAngle),
                Mathf.Sin(currentDirectionAngle)
            );

            m_Rigidbody.velocity = newDirection * m_Speed;
        }
    }
}