using System.Collections;
using UnityEngine;

public class TestStarter : MonoBehaviour {
	private IEnumerator Start () {
        yield return null;
        Starter.instance.StartStarterThenPerform(() => {
            Debug.Log("animationended");
            Starter.instance.StartStarterThenPerform(() => Debug.Log("Ahaha"));
        });
    }
}
