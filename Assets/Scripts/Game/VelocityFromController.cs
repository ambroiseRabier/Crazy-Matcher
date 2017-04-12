using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VelocityFromController : MonoBehaviour {

    [SerializeField] private float m_Speed = 10;
    [SerializeField] private Controller m_Controller;

    private Rigidbody m_Rigidbody;

    public Controller Controller
    {
        get
        {
            return m_Controller;
        }
        set
        {
            m_Controller = value;

            m_Rigidbody.isKinematic = !value;
        }
    }
    public bool HasController
    {
        get
        {
            return m_Controller != null;
        }
    }

    public float Speed {
        get
        {
            return m_Speed;
        }
        set
        {
            m_Speed = value;
        }
    }

    protected void Awake () {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    protected void FixedUpdate () {
        if (m_Controller)
        {
            print(m_Controller.Joystick.normalized * m_Speed);
            m_Rigidbody.velocity = m_Controller.Joystick.normalized * m_Speed;

        }
    }
}