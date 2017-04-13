using UnityEngine;

public class TestMatches : MonoBehaviour {

    [SerializeField] private Matches m_matches;

	private void Start () {
        m_matches.TryStartBurn();
    }
}
