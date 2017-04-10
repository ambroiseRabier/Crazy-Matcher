using UnityEngine;

public class Matches : MonoBehaviour {

    // In editor for debug
    [SerializeField] private bool m_OnFire = true; 
    [SerializeField] private GameObject m_Gfx;
    [SerializeField] private GameObject m_Fire;

    [Header("To debug")]
    [SerializeField] private Material m_materialOnFire;

    public bool OnFire
    {
        get
        {
            return m_OnFire;
        }
    }

    private void Start () {
        CheckIsOnFire();
    }

    private void CheckIsOnFire()
    {
        if (m_OnFire)
        {
            SetMaterialOnFire();
            InstantiateFire();
        }
    }

    private void InstantiateFire()
    {
        GameObject fire = Instantiate(m_Fire);
        fire.transform.parent = transform;
        fire.transform.localPosition = Vector2.zero;
    }

    private void SetMaterialOnFire()
    {
        m_Gfx.GetComponent<MeshRenderer>().material = m_materialOnFire;
    }

    public void Alight()
    {
        m_OnFire = true;
        CheckIsOnFire();
    }
}
