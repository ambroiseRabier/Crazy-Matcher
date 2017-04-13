using Assets.Scripts.Game;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(VelocityFromControllerMatche))]
public class Matches : Burnable
{
    #region Members

    [SerializeField] private bool m_startPlayer = false;

    [SerializeField] private float m_burnSpeed;
    [SerializeField] private float m_normalSpeed;
    [SerializeField] private AudioClip m_audioClip;

    [SerializeField] private Vector2 m_rangeNormal;
    [SerializeField] private Vector2 m_rangeBurning;
    [SerializeField] private float m_minDistToNavMeshDestination = 30f; // la distance à laquelle le navMesAgent décide qu'il a atteint sa destination
    [SerializeField] private GameObject m_GFXContainer;
    [SerializeField] private GameObject m_idleGFX;
    [SerializeField] private GameObject m_walkGFX;
    [SerializeField] private GameObject m_runGFX;

    private float m_speed;
    private NavMeshAgent m_NavMeshAgent;
    private VelocityFromControllerMatche m_VelocityFromController;

    [SerializeField] private AnimationCurve speedBurnCurve;
    private float burnRatio;
    #endregion

    #region Properties
    public bool StartPlayer
    {
        get
        {
            return m_startPlayer;
        }
    }

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
            m_NavMeshAgent.acceleration = value;
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


    public bool IsControlByPlayer
    {
        get
        {
            return Controller != null;
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
        m_VelocityFromController       = GetComponent<VelocityFromControllerMatche>();
        m_NavMeshAgent.updateRotation = false; 
        Speed                          = m_normalSpeed;
        AwakeMovement();
        DisableAllGFX();
    }
    
    private void DisableAllGFX()
    {
        m_idleGFX.SetActive(false);
        m_walkGFX.SetActive(false);
        m_runGFX.SetActive(false);
    }

    private void EnableGfx(GameObject gfx)
    {
        DisableAllGFX();
        gfx.SetActive(true);
    }

    private void Start()
    {
        Controller = Controller; // (wtf), to call the setter one time
        StartMove();
        OnBurnRatioProgress += BurnableComponent_OnBurnRatioProgress;

        EnableGfx(m_idleGFX);
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

            EnableGfx(m_runGFX);

            return true;
        }

        return false;
    }


    public override bool TryExtinguish()
    {
        if (base.TryExtinguish())
        {
            print("EXTINGUISH");
            Die();
            return true;
        }

        return false;
    }

    protected override void InstantiateFire()
    {
        base.InstantiateFire();
        m_fire.GetComponent<Burner>().fireOwner = this;
    }

    private void BurnableComponent_OnBurnRatioProgress(Burnable burnable, float newBurnRatio) {
        burnRatio = newBurnRatio;
    }

    #endregion

    public void Die()
    {
        GameManager.instance.PlaySound(m_audioClip);
        Destroy(gameObject);
    }

    #region Movement

    private string state = "move";
    [SerializeField] private float PAUSE_TIME_RANDOM_RANGE_MIN = 1000f;
    [SerializeField] private float PAUSE_TIME_RANDOM_RANGE_MAX = 1500f;
    [SerializeField] private float PAUSE_WAIT_PROBABILITY = 1f; // probabilité d'une pause après avoir atteint sa destination.
    private float waitCount = 0f;
    private float currentPauseTime;
    private float currentWaitProb;

    private void StartMove () {
        state = "move";
        SetNextDesination();
    }

    private void Update () {
        // isBurned et isBurning inversé. peut-être plus maintenant ? 12/04
        /*if (IsBurning)
            Debug.Log("IsBurning " +IsBurning);
        if (IsBurned)
            Debug.Log("IsBurned " + IsBurned);*/

        Speed = m_burnSpeed * speedBurnCurve.Evaluate(burnRatio); 

        if (!HasController) {

            if (state == "move") {
                checkIfDestinationReached();
            } else if (state == "wait") {
                Wait();
            }

        }

    }

    private void checkIfDestinationReached () {
        // make sure next destination is m_minDistToNavMeshDestination distance far away from last destination
        if (m_NavMeshAgent.remainingDistance < m_minDistToNavMeshDestination) {
            //print("checkIfDestinationReached");

            if (!IsBurning)
                CheckIfWait();

            if (state == "move")
                SetNextDesination();
        }
    }

    // only one chance per destination reached
    private void CheckIfWait() {
        
        bool lWait = false;
        if (currentWaitProb != 0) {
            lWait = currentWaitProb > Random.Range(0f, 1f);
            state = lWait ? "wait" : "move";

            /*if (lWait)
                print("WAIT");*/
        }
        
        if (lWait)
        {
            m_NavMeshAgent.isStopped = true;
            EnableGfx(m_idleGFX);
        }

    }

    private void AwakeMovement() 
    {
        currentPauseTime = Random.Range(PAUSE_TIME_RANDOM_RANGE_MIN, PAUSE_TIME_RANDOM_RANGE_MAX);
        currentWaitProb = PAUSE_WAIT_PROBABILITY;
    }

    private void Wait () {        
        if (waitCount >= currentPauseTime) {
            EnableGfx(m_walkGFX);
            SetNextDesination();
            waitCount = 0f;
            AwakeMovement();
            return; // waitCount stay to 0
        }

        waitCount += Time.deltaTime * 1000;
    }

    private void SetNextDesination()
    {
        state = "move";
        m_NavMeshAgent.isStopped = false;
        Vector3 position = transform.position;
        Vector2 usingRange = IsBurning ? m_rangeBurning : m_rangeNormal;
        Vector3 destination = new Vector3(
            Random.Range(-usingRange.x, usingRange.y),
            Random.Range(-usingRange.x, usingRange.y),
            0f
        );
        //Debug.Log(destination);
        // add min value to destination so it won't instant reach.
        // minimum m_minDistToNavMeshDestination * 1.1 distance to travel
        destination.x += Mathf.Sign(destination.x) * m_minDistToNavMeshDestination * 1.1f;
        destination.y += Mathf.Sign(destination.y) * m_minDistToNavMeshDestination * 1.1f;

        //Debug.Log(destination);

        destination = new Vector3(
            position.x + destination.x,
            position.y + destination.y,
            0f
        );


        m_NavMeshAgent.SetDestination(destination);
    }

    #endregion
}
