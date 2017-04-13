using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectif : Burnable {

    [SerializeField] private GameObject m_gfxFlame1;
    [SerializeField] private GameObject m_gfxFlame2;
    [SerializeField] private GameObject m_gfxFlame3;

    private void Awake()
    {
        m_gfxFlame1.SetActive(false);
        m_gfxFlame2.SetActive(false);
        m_gfxFlame3.SetActive(false);
    }

    public override bool TryStartBurn()
    {
        if (base.TryStartBurn())
        {
            m_gfxFlame1.SetActive(true);
            Timer.DelayThenPerform(0.25f, () => { m_gfxFlame2.SetActive(true);});
            Timer.DelayThenPerform(0.5f, () => { m_gfxFlame3.SetActive(true);});

            return true;
        }

        return false;
    }
}
