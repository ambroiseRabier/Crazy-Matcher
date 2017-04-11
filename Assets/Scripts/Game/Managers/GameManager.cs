using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Events;
using Utils;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{

    #region Variables

    private const float DEFAULT_GAME_TIME_SCALE = 1;

    private float m_gameTimeScale;

    public enum Team
    {
        FIRE_FIGHTER,
        MATCHES
    }

    //private List<Objectif> m_objectifs;
    private int m_burnObjectifsCount;
    private Objectif[] m_objectifs;

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

        GlobalEventBus.onStartLevel.AddListener(OnStartLevel);
        GlobalEventBus.onTeamWin.AddListener(OnTeamWin);
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
        m_burnObjectifsCount = 0;
        FindObjectifs();
    }

    private void FindObjectifs()
    {
        //if (m_objectifs != null)
        //    m_objectifs = new List<Objectif>();
        //else
        //    m_objectifs.Clear();

        m_objectifs = FindObjectsOfType<Objectif>();
        
        foreach (Objectif objectif in m_objectifs)
        {
            print(objectif.name);
            objectif.OnStartBurn += OnObjectifBurn;
        }
    }

    private void OnObjectifBurn(Burnable burnable)
    {
        print("OnObjectifBurn");
        m_burnObjectifsCount++;
        if (m_burnObjectifsCount >= m_objectifs.Length)
            GlobalEventBus.onTeamWin.Invoke(Team.MATCHES);
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

    private void OnStartLevel()
    {
        StartLevel();
    }

    private void OnTeamWin(Team teamWin)
    {
        print(teamWin);
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

