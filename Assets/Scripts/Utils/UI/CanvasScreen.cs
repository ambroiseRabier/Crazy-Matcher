using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CanvasScreen : MonoBehaviour {
    private Animator m_animator;

    private static readonly int IS_OPENED_PARAM = Animator.StringToHash("IsOpened");

    public bool IsOpened {
        get
        {
            return m_animator.GetBool(IS_OPENED_PARAM);
        }
        private set
        {
            m_animator.SetBool(IS_OPENED_PARAM, value);
        }
    }

    private void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_animator.Play("Close", 0, 1f);
    }

    public void Open()
    {
        IsOpened = true;
    }

    public void Close()
    {
        IsOpened = false;
    }
}
