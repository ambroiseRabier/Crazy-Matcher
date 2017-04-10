using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Matches : MonoBehaviour {

    #region Members
    [SerializeField] private float m_Speed;
    [SerializeField] private Controller m_Controller;
    [SerializeField] private NavMeshAgent m_NavMeshAgent;

    [SerializeField] private bool m_IsOnFire = true;
    [SerializeField] private MeshRenderer m_Gfx;
    [SerializeField] private GameObject m_FirePrefab;

    [Header("To debug")]
    [SerializeField] private Material m_materialOnFire;
    #endregion

    #region Properties
    public Controller Controller
    {
        get
        {
            return m_Controller;
        }
        set
        {
            m_Controller = value;
        }
    }

    public bool IsOnFire { get { return m_IsOnFire; } }
    #endregion

    #region Fire
    private void Start () {
        CheckIsOnFire();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_NavMeshAgent.speed = m_Speed;
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
        if (m_Controller)
        {
            transform.position += m_Controller.Joystick * m_Speed * Time.deltaTime;
        } else
        {
            if (m_NavMeshAgent.remainingDistance < 0.3f)
                m_NavMeshAgent.SetDestination(new Vector3(Random.Range(-4, 4), 0f, Random.Range(-4, 4)));
        }
    }
    #endregion
}
