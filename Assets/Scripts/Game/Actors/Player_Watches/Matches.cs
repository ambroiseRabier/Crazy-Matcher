using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(VelocityFromController))]
public class Matches : Burnable
{
    #region Members

    [SerializeField] private float m_burnSpeed;
    [SerializeField] private float m_normalSpeed;

    private float m_speed;
    //private NavMeshAgent m_NavMeshAgent;
    private VelocityFromController m_VelocityFromController;


    private Vector2 m_currentDirection;
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
            //m_NavMeshAgent.speed = value;
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
                //m_NavMeshAgent.enabled = false;
                Vector3 position = transform.position;
                position.z = 0;
                transform.position = position;
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

        m_currentDirection = RandomDirection();
        //m_NavMeshAgent                 = GetComponent<NavMeshAgent>();
        m_VelocityFromController       = GetComponent<VelocityFromController>();

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
        {
            RandomMovement();
        }
            //if (m_NavMeshAgent.remainingDistance < 0.3f)
            //    m_NavMeshAgent.SetDestination(new Vector3(Random.Range(-4, 4), 0f, Random.Range(-4, 4)));
    }

    public float randomlol;

    private void RandomMovement()
    {
        float randomAngle = Random.Range(-randomlol, randomlol);

        float angleRad = Vector2.Angle(Vector2.zero, m_currentDirection);
        angleRad += randomAngle;

        print(angleRad);
        m_currentDirection = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        
        GetComponent<Rigidbody2D>().velocity = m_currentDirection * m_speed;

    }

    private Vector2 RandomDirection()
    {
        return (new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized;
    }
    #endregion
}
