using UnityEngine;

public class TranslateFromBurnRatio : MonoBehaviour {
    [SerializeField] private Burnable m_burnable;
    [SerializeField] private Transform m_targetedTransform;
    private Vector3 m_localPositionOnStart;
	
	private void Awake () {
        m_localPositionOnStart = transform.localPosition;
        m_burnable.OnBurnRatioProgress += Burnable_OnBurnRatioProgress;
	}

    private void Burnable_OnBurnRatioProgress(Burnable burnable, float newBurnRatio)
    {
        transform.localPosition = Vector3.Lerp(m_localPositionOnStart, m_targetedTransform.localPosition, newBurnRatio);
    }

    private void OnDestroy () {
        m_burnable.OnBurnRatioProgress -= Burnable_OnBurnRatioProgress;
    }
}
