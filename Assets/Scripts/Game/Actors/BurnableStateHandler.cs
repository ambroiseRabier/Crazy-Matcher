using System.Collections.Generic;
using UnityEngine;

public class BurnableStateHandler : MonoBehaviour {
    [SerializeField] private List<GameObject> m_states = new List<GameObject>();
    [SerializeField] private Burnable burnable;

	private void Awake () {
        burnable.OnBurnRatioProgress += Burnable_OnBurnRatioProgress;
        EnableStateFromBurnRatio(burnable.BurnRatio);
    }

    private void Burnable_OnBurnRatioProgress(Burnable burnable, float newBurnRatio)
    {
        EnableStateFromBurnRatio(newBurnRatio);
    }

    private void EnableStateFromBurnRatio(float burnRatio)
    {
        EnableStateFromIndex(Mathf.RoundToInt(burnRatio * (m_states.Count - 1)));
    }

    private void EnableStateFromIndex(int index)
    {
        for (int i = m_states.Count-1; i > -1; i--)
            m_states[i].SetActive(index == i);
    }

    private void OnDestroy()
    {
        burnable.OnBurnRatioProgress -= Burnable_OnBurnRatioProgress;
    }
}
