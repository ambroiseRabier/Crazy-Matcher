using System.Collections;
using UnityEngine;

public class TestStarter : MonoBehaviour {
	private IEnumerator Start () {
        yield return null;
        Starter.instance.StartStarterThenPerformOnEnd(() => {
            Debug.Log("animationended");
            Starter.instance.StartStarterThenPerformOnEnd(() => Debug.Log("Ahaha"));
        });
    }
}
