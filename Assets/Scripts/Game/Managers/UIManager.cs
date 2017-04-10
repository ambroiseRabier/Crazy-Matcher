using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Events;
using System.Collections.Generic;
using Utils;

public class UIManager : MultiScreenManager<UIManager>
{
    #region Variables

    [Header("Menu Screen")]
    [SerializeField]
    private GameObject m_titleScreen;
    [SerializeField]
    private GameObject m_menu;
    [SerializeField]
    private GameObject m_pauseMenu;
    [SerializeField]
    private GameObject m_HUD;


    [Header("Menu In Game")]
    [SerializeField]
    private GameObject m_btnPause;

    #endregion


    #region Start

    protected override IEnumerator CoroutineStart()
    {
        InitScreens(new List<GameObject>(new GameObject[] {
            m_titleScreen,
            m_menu,
            m_pauseMenu,
            m_HUD
        }));

        InitPauseMenu();
        InitPauseButton();

        isReady = true;
        yield return null;
    }

    #endregion


    #region Initialized

    private void InitPauseMenu()
    {
        m_pauseMenu.SetActive(false);
    }

    protected override void InitEvent()
    {
        GlobalEventBus.onMenu.AddListener(OnMenu);
        GlobalEventBus.onInitLevel.AddListener(OnInitLevel);
        GlobalEventBus.onTitleScreen.AddListener(OnTittleScreen);
        GlobalEventBus.onResume.AddListener(OnResume);
        GlobalEventBus.onPause.AddListener(OnPause);
    }

    private void InitPauseButton()
    {
        m_btnPause.SetActive(true);
    }

    #endregion


    #region Update

    void Update()
    {
    }

    #endregion


    #region Events

    private void OnMenu()
    {
        EnableOnlyScreen(m_menu);
    }

    private void OnInitLevel(string levelName)
    {
        EnableOnlyScreen(m_HUD);
        InitPauseButton();
    }

    private void OnTittleScreen()
    {
        EnableOnlyScreen(m_titleScreen);
    }

    private void OnPause()
    {
        EnableOnlyScreen(m_pauseMenu);
    }

    private void OnResume()
    {
        EnableOnlyScreen(m_HUD);
    }


    #endregion


    #region Pause Manager

    private void Pause()
    {
        GlobalEventBus.onPause.Invoke();
    }

    private void Resume()
    {
        GlobalEventBus.onResume.Invoke();
    }

    #endregion
    
    

    #region Button Callback

    public void PlayClick(string levelName)
    {
        GlobalEventBus.onInitLevel.Invoke(levelName);
    }

    public void ResumeClick()
    {
        Resume();
    }

    public void MenuClick()
    {
        GlobalEventBus.onMenu.Invoke();
    }

    public void PauseClick()
    {
        m_HUD.SetActive(false);
        Pause();
    }

    public void RestartClick()
    {

    }

    #endregion

}

