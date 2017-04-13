using System;
using UnityEngine;

public class TestCanvasScreen : MonoBehaviour {
	private void Start () {
        WinScreen.instance.Open(OnOpened);
	}

    private void OnOpened()
    {
        Debug.Log("Onpened ! :D");
        //() => WinScreen.instance.Close()
    }
}
