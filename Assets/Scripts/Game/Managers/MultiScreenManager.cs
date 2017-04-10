using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MultiScreenManager<T> : BaseManager<T> where T : Component
{
    #region Variables

    private List<GameObject> m_screens;

    #endregion


    #region Start

    protected override IEnumerator CoroutineStart()
    {
        yield return null;
    }

    protected void InitScreens(List<GameObject> pScreens)
    {
        m_screens = pScreens;
    }

    protected override void InitEvent()
    {

    }

    #endregion


    #region List Manager

    protected void AddScreen(GameObject screen)
    {
        m_screens.Add(screen);
    }

    #endregion


    #region Screen Function

    protected void EnableOnlyScreen(GameObject screen)
    {
        DisableAllScreen();
        ToggleScreen(screen, true);
    }

    protected void EnableOnlyScreens(List<GameObject> screens)
    {
        DisableAllScreen();
        foreach (GameObject screen in screens)
        {
            ToggleScreen(screen, true);
        }
    }

    protected void ToggleScreen(GameObject screen, bool active)
    {
        screen.SetActive(active);
    }

    protected void DisableAllScreen()
    {
        foreach (GameObject screen in m_screens)
        {
            ToggleScreen(screen, false);
        }
    }

    #endregion
}

