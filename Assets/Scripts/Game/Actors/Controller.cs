using Assets.Scripts.Game.Actors;
using UnityEngine;

public class Controller : MonoBehaviour {
    [Tooltip("Rewired override options bellow.")]
    [SerializeField] private bool m_useRewired;
    [Header("Whitout Rewired")]
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
    [HideInInspector] public ControllerRewired rewiredController;

    private void Start () {
        rewiredController = gameObject.GetComponent<ControllerRewired>();
    }

    private void Update()
    {
        if (m_useRewired)
            UseRewired();
        else
            UseDefaultInputManager();
    }

    private void UseRewired () {
        m_Joystick = rewiredController.m_move;
        Fire = rewiredController.m_fire;

        m_triggerLeft = rewiredController.m_triggerLeft;
        m_triggerRight = rewiredController.m_triggerRight;
    }

    private void UseDefaultInputManager () {
        m_Joystick.x = Input.GetAxis(m_HorizontalInputName);
        m_Joystick.y = Input.GetAxis(m_VerticalInputName);
        Fire = Input.GetButton(m_FireInputName);

        m_triggerLeft = Input.GetAxis(m_triggerLeftName);
        m_triggerRight = Input.GetAxis(m_triggerRightName);
    }
}
