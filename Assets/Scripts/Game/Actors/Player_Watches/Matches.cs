using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(VelocityFromController))]
public class Matches : Burnable
{
    #region Members
    [SerializeField] private MeshRenderer m_Gfx;
    [SerializeField] private GameObject m_FirePrefab;

    [SerializeField] private float m_burnSpeed;
    [SerializeField] private float m_normalSpeed;

    [Header("To debug")]
    [SerializeField] private Material m_materialOnFire;

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

        Speed                          = m_normalSpeed;
    }

    private void Start()
    {
        Controller = Controller;
    }

    private void InstantiateFire()
    {
        GameObject fire = Instantiate(m_FirePrefab);
        fire.transform.parent = transform;
        fire.transform.localPosition = Vector2.zero;
    }

    private void SetMaterialOnFire()
    {
        m_Gfx.GetComponent<MeshRenderer>().material = m_materialOnFire;
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
            SetMaterialOnFire();
            InstantiateFire();
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
                m_NavMeshAgent.SetDestination(new Vector3(Random.Range(-4, 4), 0f, Random.Range(-4, 4)));
    }
    #endregion
}
