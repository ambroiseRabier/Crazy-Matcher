using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Events;
using Utils;

public class GameManager : Singleton<GameManager>
{

    #region Variables

    private const float DEFAULT_GAME_TIME_SCALE = 1;

    private float m_gameTimeScale;

    #endregion


    #region Start

    IEnumerator Start()
    {
        InitVariables();

        while (!(UIManager.instance.isReady))
            yield return null;

        InitEvent();

        TitleScreen();

    }

    #endregion


    #region Initialized

    private void InitEvent()
    {
        GlobalEventBus.onPause.AddListener(OnPause);
        GlobalEventBus.onResume.AddListener(OnResume);
        GlobalEventBus.onInitLevel.AddListener(OnInitLevel);
    }


    private void InitVariables()
    {
        m_gameTimeScale = DEFAULT_GAME_TIME_SCALE;
    }

    #endregion


    #region Game

    void Update()
    {

    }

    private void StartLevel()
    {
        GlobalEventBus.onStartLevel.Invoke();
    }
    
    private void Menu()
    {
        GlobalEventBus.onMenu.Invoke();
    }

    private void TitleScreen()
    {
        GlobalEventBus.onTitleScreen.Invoke();
    }

    #endregion


    #region Events

    private void OnPause()
    {
        Pause();
    }

    private void OnResume()
    {
        Unpause();
    }

    private void OnInitLevel(string levelName)
    {
        //TODO 
        GlobalEventBus.onStartLevel.Invoke();
    } 

    #endregion


    #region Time Scale Manager

    private void Pause()
    {
        Time.timeScale = 0;
    }

    private void Unpause()
    {
        Time.timeScale = m_gameTimeScale;
    }

    #endregion
}

