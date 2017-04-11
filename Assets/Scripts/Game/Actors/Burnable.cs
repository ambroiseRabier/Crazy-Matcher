using System.Collections;
using UnityEngine;

public delegate void BurnableEventHandler(Burnable burnable);

public abstract class Burnable : MonoBehaviour
{
    [SerializeField] private GameObject m_FirePrefab;

    public event BurnableEventHandler OnStartBurn;
    public event BurnableEventHandler OnBurned;
    public event BurnableEventHandler OnExtinguished;
    
    [SerializeField] private MeshRenderer m_Gfx;

    [Header("To debug")]
    [SerializeField]
    private Material m_materialOnFire;



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
            OnBurnRatioProgress(BurnRatio = burnTimer / m_BurnTime);
            yield return null;
            burnTimer += Time.deltaTime;
        }

        IsBurning = false;
        IsBurned  = true;
        BurnRatio = 1f;

        if (OnBurned != null)
            OnBurned(this);
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

        StopCoroutine(m_StartedBurnCoroutine);
        IsBurning = false;
        OnExtinguished(this);
        return true;
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

    protected virtual void OnBurnRatioProgress(float burnRatioProgress){}
}
