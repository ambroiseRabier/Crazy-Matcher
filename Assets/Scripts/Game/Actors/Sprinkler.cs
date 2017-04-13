using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprinkler : MonoBehaviour {

    [SerializeField] private GameObject m_sprinklerWaterPrefab;
    private GameObject m_sprinklerWater;

    [SerializeField] private float m_fillSpeed = 0.1f;
    [SerializeField] private float m_emptyingSpeed = 0.1f;
    private float m_fillRatio = 0f;

    private bool isFull;

    private void Awake()
    {
        ResetFill();
    }

    private void ResetFill()
    {
        m_fillRatio = 0f;
        isFull = false;
    }

    public void TryToFill()
    {
        if (!isFull)
        {
            m_fillRatio += m_fillSpeed;
            print(m_fillRatio);
            if (m_fillRatio >= 1)
            {
                Full();
            }
        }
    }

    private void Full()
    {
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
