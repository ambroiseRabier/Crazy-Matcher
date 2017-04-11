using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMatches : MonoBehaviour {

    [SerializeField] private Matches m_matches;

	// Use this for initialization
	void Start () {
        m_matches.TryStartBurn();

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
