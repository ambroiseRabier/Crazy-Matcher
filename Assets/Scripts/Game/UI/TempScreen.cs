using System;
using UnityEngine;

public class TempScreen<T> : CanvasScreen<T> where T : Component {
    private void Start()
    {
        Close();
    }

    public new bool IsOpened { get { return gameObject.activeInHierarchy; } }

    public new void Open(Action action = null)
    {
        gameObject.SetActive(true);
    }
    public new void Close(Action action = null)
    {
        gameObject.SetActive(false);
    }
}
