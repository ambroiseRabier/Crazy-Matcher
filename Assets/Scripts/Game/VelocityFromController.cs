using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class VelocityFromController : MonoBehaviour {

    [SerializeField] private float m_Speed = 10;
    [SerializeField] private Controller m_Controller;

    private Rigidbody2D m_Rigidbody;

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
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate () {
        if (m_Controller)
        {
            m_Rigidbody.velocity = m_Controller.Joystick.normalized * m_Speed;
        } 
    }
}