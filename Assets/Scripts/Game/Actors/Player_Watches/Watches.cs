using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watches : MonoBehaviour {

    // In editor for debug
    [SerializeField] 
    private bool m_onFire = true; 

    [SerializeField]
    private GameObject m_gfx;

    [SerializeField]
    private GameObject m_fire;

    [Header("To debug")]
    [SerializeField]
    private Material m_materialOnFire;

    public bool OnFire
    {
        get
        {
            return m_onFire;
        }
    }

    void Start () {
        CheckIsOnFire();
    }

    private void CheckIsOnFire()
    {
        if (m_onFire)
        {
            SetMaterialOnFire();
            InstantiateFire();
        }
    }

    private void InstantiateFire()
    {
        GameObject fire = Instantiate(m_fire);
        fire.transform.parent = transform;
        fire.transform.localPosition = Vector2.zero;
    }

    private void SetMaterialOnFire()
    {
        m_gfx.GetComponent<MeshRenderer>().material = m_materialOnFire;
    }

    public void Alight()
    {
        m_onFire = true;
        CheckIsOnFire();
    }





}
