using UnityEngine;

public class TestStarter : MonoBehaviour {
	private void Start () {
        Starter.instance.StartStarterThenPerform(() => { Debug.Log("animationended"); });
    }
}
