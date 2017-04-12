using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(VelocityFromController))]
public class Matches : Burnable
{
    #region Members

    [SerializeField] private float m_burnSpeed;
    [SerializeField] private float m_normalSpeed;

    [SerializeField] private float m_rangeX;
    [SerializeField] private float m_rangeY;

    private float m_speed;
    private NavMeshAgent m_NavMeshAgent;
    private VelocityFromController m_VelocityFromController;
    #endregion

    #region Properties
    public float Speed
    {
        get
        {
            return m_speed;
        }
        set
        {
            m_speed              = value;
            m_NavMeshAgent.speed = value;
            m_VelocityFromController.Speed = value;
        }
    }

    public Controller Controller
    {
        get
        {
            return m_VelocityFromController.Controller;
        }
        set
        {
            m_VelocityFromController.Controller = value;

            if (value)
            {
                m_NavMeshAgent.enabled = false;
                Vector3 position = transform.position;
                position.z = 0;
                transform.position = position;
            }
            else
            {
                m_NavMeshAgent.enabled = true;
            }
        }
    }

    public bool HasController
    {
        get
        {
            return m_VelocityFromController.HasController;
        }
    }

    #endregion

    #region Fire
    private void Awake () {
        m_NavMeshAgent                 = GetComponent<NavMeshAgent>();
        m_VelocityFromController       = GetComponent<VelocityFromController>();
        m_NavMeshAgent.updateRotation = false; 
        Speed                          = m_normalSpeed;
    }

    private void Start()
    {
        Controller = Controller;
    }
    

    /// <summary>
    /// If burned or already burning, don't start burn again and return false
    /// else return true
    /// </summary>
    /// <returns></returns>
    public override bool TryStartBurn()
    {
        if (base.TryStartBurn())
        {
            Speed = m_burnSpeed;

            return true;
        }

        return false;
    }

    #endregion

    #region Movement
    private void Update()
    {
        if (!HasController)
            if (m_NavMeshAgent.remainingDistance < 0.3f)
            {
                Vector3 position = transform.position;
                Vector3 destination = new Vector3(Random.Range(position.x - m_rangeX, position.x + m_rangeX), Random.Range(position.y - m_rangeY, position.y + m_rangeY), 0f);
                m_NavMeshAgent.SetDestination(destination);

            }
    }
    #endregion
}
