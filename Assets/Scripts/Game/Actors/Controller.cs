using UnityEngine;

public class Controller : MonoBehaviour {
    [SerializeField] private string m_HorizontalInputName;
    [SerializeField] private string m_VerticalInputName;
    [SerializeField] private string m_FireInputName;

    private Vector3 m_Joystick = Vector3.zero;
    public Vector3 Joystick { get { return m_Joystick; }}
    public bool Fire { get; private set; }

    private void Update()
    {
        m_Joystick.x = Input.GetAxis(m_HorizontalInputName);
        m_Joystick.z = Input.GetAxis(m_VerticalInputName);
        Fire         = Input.GetButton(m_FireInputName);
    }
}
