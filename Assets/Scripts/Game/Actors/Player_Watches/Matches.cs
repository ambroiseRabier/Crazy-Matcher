using UnityEngine;

public class Matches : MonoBehaviour {

    // In editor for debug
    [SerializeField] private bool m_IsOnFire = true; 
    [SerializeField] private MeshRenderer m_Gfx;
    [SerializeField] private GameObject m_FirePrefab;

    [Header("To debug")]
    [SerializeField] private Material m_materialOnFire;

    public bool IsOnFire { get { return m_IsOnFire; } }

    private void Start () {
        CheckIsOnFire();
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
}
