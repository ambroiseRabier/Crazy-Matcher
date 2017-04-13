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
    [SerializeField]
    private GameObject m_winScreen;
    
    

    [Header("HUD")]
    [SerializeField]
    private Slider m_waterBar;

    #endregion


    #region Start

    protected override IEnumerator CoroutineStart()
    {
        InitScreens(new List<GameObject>(new GameObject[] {
            m_titleScreen,
            m_menu,
            m_pauseMenu,
            m_HUD,
            m_winScreen
        }));

        InitPauseMenu();

        IsReady = true;
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
        GlobalEventBus.onTeamWin.AddListener(OnTeamWin);
        GlobalEventBus.onWaterChange.AddListener(OnWaterChange);
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

    private void OnInitLevel()
    {
        EnableOnlyScreen(m_HUD);
        CloseWinScreen();
    }

    private void OnTittleScreen()
    {
        EnableOnlyScreen(m_titleScreen);
        CloseWinScreen();
    }

    private void OnPause()
    {
        EnableOnlyScreen(m_pauseMenu);
    }

    private void OnTeamWin(GameManager.Team team)
    {
        EnableOnlyScreen(m_winScreen);
        WinScreen.instance.SetWinnerTeam(team);
        OpenWinScreen();
    }

    private void OnResume()
    {
        EnableOnlyScreen(m_HUD);
    }

    private void OnWaterChange(float newValue) 
    {
        if (m_waterBar != null)
            m_waterBar.value = newValue;
    }


    #endregion


    private void OpenWinScreen()
    {
        if (!WinScreen.instance.IsOpened)
        {
            WinScreen.instance.Open();
        }
    }

    private void CloseWinScreen()
    {
        if (WinScreen.instance == null) {
            Debug.LogWarning("WinScreen.instance is null");
            return;
        }
        if (WinScreen.instance.IsOpened)
        {
            WinScreen.instance.Close();
        }
    }

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

    public void PlayClick(int level)
    {
        GlobalEventBus.onLoadingScene.Invoke(level);
    }

    public void ResumeClick()
    {
        Resume();
    }

    public void MenuClick()
    {
        GlobalEventBus.onLoadingScene.Invoke(0);
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

