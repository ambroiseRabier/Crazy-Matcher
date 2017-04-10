using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(VelocityFromController))]
public class Matches : MonoBehaviour {

    #region Members
    [SerializeField] private bool m_IsOnFire = true;
    [SerializeField] private MeshRenderer m_Gfx;
    [SerializeField] private GameObject m_FirePrefab;

    [Header("To debug")]
    [SerializeField] private Material m_materialOnFire;

    private NavMeshAgent m_NavMeshAgent;
    private VelocityFromController m_VelocityFromController;
    #endregion

    #region Properties
    public float Speed
    {
        get
        {
            return m_VelocityFromController.Speed;
        }
        set
        {
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
        }
    }

    public bool HasController
    {
        get
        {
            return m_VelocityFromController.HasController;
        }
    }

    public bool IsOnFire { get { return m_IsOnFire; } }
    #endregion

    #region Fire
    private void Start () {
        CheckIsOnFire();
        m_NavMeshAgent                 = GetComponent<NavMeshAgent>();
        m_VelocityFromController       = GetComponent<VelocityFromController>();
        m_NavMeshAgent.speed           = Speed;
    }

    private void CheckIsOnFire()
    {
        if (m_IsOnFire)
        {
            SetMaterialOnFire();
            InstantiateFire();
        }
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

    public void Alight()
    {
        m_IsOnFire = true;
        CheckIsOnFire();
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
