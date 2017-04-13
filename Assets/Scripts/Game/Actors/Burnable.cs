using System.Collections;
using UnityEngine;

public delegate void BurnableEventHandler(Burnable burnable);
public delegate void BurnableBurnProgressEventHandler(Burnable burnable, float newBurnRatio);

public abstract class Burnable : MonoBehaviour
{
    [SerializeField] private GameObject m_FirePrefab;

    public event BurnableEventHandler OnStartBurn;
    public event BurnableEventHandler OnBurned;
    public event BurnableEventHandler OnExtinguished;
    public event BurnableBurnProgressEventHandler OnBurnRatioProgress;
    

    public Matches matchesBurnMe;

    [SerializeField] private MeshRenderer m_Gfx;

    [Header("To debug")]
    [SerializeField]
    private Material m_materialOnFire;

    protected GameObject m_fire;


    public bool IsBurning { get; private set; }
    public bool IsBurned { get; private set; }

    /// <summary>
    /// Time to burn completely
    /// </summary>
    public float BurnTime
    {
        get
        {
            return m_BurnTime;
        }
        set
        {
            m_BurnTime = value;
        }
    }

    /// <summary>
    /// Burn ratio state. If completely burned, BurnRatio = 1f
    /// </summary>
    public float BurnRatio { get; private set; }

    [SerializeField] private float m_BurnTime;
    [SerializeField] private AudioClip m_startFireSoundClip;

    private Coroutine m_StartedBurnCoroutine;

    /// <summary>
    /// If burned or already burning, don't start burn again and return false
    /// else return true
    /// </summary>
    /// <returns></returns>
    public virtual bool TryStartBurn()
    {
        if (IsBurning || IsBurned)
            return false;

        if (OnStartBurn != null)
            OnStartBurn(this);

        GameManager.instance.PlaySound(m_startFireSoundClip);
        SetMaterialOnFire();
        InstantiateFire();

        m_StartedBurnCoroutine = StartCoroutine(BurnCoroutine());
        return true;
    }

    protected IEnumerator BurnCoroutine()
    {
        IsBurning = true;

        float burnTimer = 0;

        while (burnTimer < m_BurnTime)
        {
            if (OnBurnRatioProgress != null)
                OnBurnRatioProgress(this, BurnRatio = burnTimer / m_BurnTime);

            yield return null;
            burnTimer += Time.deltaTime;
        }

        TryExtinguish();
        IsBurning = false;
        IsBurned  = true;
        BurnRatio = 1f;

        //if (OnBurned != null)
        //    OnBurned(this);
    }

    /// <summary>
    /// If not burning or is burned, return false
    /// if is burning and not burned, return true
    /// </summary>
    /// <returns></returns>
    public virtual bool TryExtinguish()
    {
        if (!IsBurning || IsBurned)
            return false;

        //StopCoroutine(m_StartedBurnCoroutine);
        IsBurning = false;

        if (OnExtinguished != null)
            OnExtinguished(this);

        return true;
    }


    protected virtual void InstantiateFire()
    {
        m_fire = Instantiate(m_FirePrefab);
        m_fire.transform.parent = transform;
        m_fire.transform.localPosition = Vector2.zero;
    }


    private void SetMaterialOnFire()
    {
        m_Gfx.GetComponent<MeshRenderer>().material = m_materialOnFire;
    }
}
