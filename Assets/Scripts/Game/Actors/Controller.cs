using UnityEngine;

public class Controller : MonoBehaviour {
    [SerializeField] private string m_HorizontalInputName;
    [SerializeField] private string m_VerticalInputName;
    [SerializeField] private string m_FireInputName;
    [SerializeField] private string m_triggerLeftName;
    [SerializeField] private string m_triggerRightName;

    private Vector3 m_Joystick = Vector3.zero;
    public Vector3 Joystick { get { return m_Joystick; }}
    public bool Fire { get; private set; }
    public float m_triggerLeft { get; private set; }
    public float m_triggerRight { get; private set; }

    private void Update()
    {
        m_Joystick.x    = Input.GetAxis(m_HorizontalInputName);
        m_Joystick.y    = Input.GetAxis(m_VerticalInputName);
        Fire            = Input.GetButton(m_FireInputName);
        m_triggerLeft   = Input.GetAxis(m_triggerLeftName);
        m_triggerRight  = Input.GetAxis(m_triggerRightName);
    }
}
