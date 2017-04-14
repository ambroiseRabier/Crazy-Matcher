using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : MonoBehaviour {

    [SerializeField] private GameObject m_sprinklerWaterPrefab;

    [SerializeField] private GameObject m_gfxIdle;
    [SerializeField] private GameObject m_gfxFill;

    private GameObject m_sprinklerWater;

    [SerializeField] private AudioClip m_audioClip;
    [SerializeField] private float m_fillSpeed;
    [SerializeField] private float m_emptyingSpeed;
    private float m_fillRatio;

    private bool isFull;

    private void Awake()
    {
        ResetFill();
        DisableAllGfx();
    }

    private void Start()
    {
        EnableGfx(m_gfxIdle);
    }

    private void DisableAllGfx()
    {
        m_gfxIdle.SetActive(false);
        m_gfxFill.SetActive(false);
    }

    private void EnableGfx(GameObject gfx)
    {
        DisableAllGfx();
        gfx.SetActive(true);
    }

    private void ResetFill()
    {
        m_fillRatio = 0f;
        isFull = false;
        m_isBeingFilled = false;
    }

    private bool m_isBeingFilled;

    public void TryToFill()
    {

        if (!isFull)
        {
            EnableGfx(m_gfxFill);
            if (!m_isBeingFilled)
            {
                m_isBeingFilled = true;
                Timer.DelayThenPerform(1, () => { EnableGfx(m_gfxIdle); m_isBeingFilled = false; });
            }

            m_fillRatio += m_fillSpeed * Time.deltaTime;
            if (m_fillRatio >= 1)
            {
                Full();
            }
        }
    }

    private void Full()
    {
        GameManager.instance.PlaySound(m_audioClip);
        isFull = true;
        InstantiateSprinklerWater();
    }

    private void Empty()
    {
        ResetFill();
        DestroySprinklerWater();
    }


    private void InstantiateSprinklerWater()
    {
        m_sprinklerWater = Instantiate(m_sprinklerWaterPrefab);
        m_sprinklerWater.transform.parent = transform;
        m_sprinklerWater.transform.localPosition = Vector2.zero;
    }

    private void DestroySprinklerWater()
    {
        Destroy(m_sprinklerWater);
        m_sprinklerWater = null;
    }

    private void Update()
    {
        if (isFull)
        {
            m_fillRatio -= m_emptyingSpeed * Time.deltaTime;
            if (m_fillRatio <= 0)
            {
                Empty();
            }
        }
    }
}
