using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Events;
using Utils;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{

    #region Variables

    [SerializeField] private float m_timeBeforePlayerChange = 3f;
    [SerializeField] private Matches m_currentPlayerMatches;
    
    [SerializeField] private Controller m_controllerP1; 
    [SerializeField] private Controller m_controllerP2; 

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

    private List<Matches> m_potentialPlayers;

    #endregion


    #region Start

    IEnumerator Start()
    {
        InitVariables();

        while (!(UIManager.instance.IsReady))
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

        m_potentialPlayers = new List<Matches>();
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
        InitPlayerMatches();
        FindObjectifs();
        FindMatches();
    }

    private void InitPlayerMatches()
    {
        m_currentPlayerMatches.Controller = m_controllerP1;
        m_currentPlayerMatches.TryStartBurn();
    }

    private void ChangePlayer(Matches matches)
    {
        m_currentPlayerMatches.Controller = null;
        m_currentPlayerMatches = matches;
        InitPlayerMatches();
    }

    private void FindObjectifs()
    {
        m_objectifs = FindObjectsOfType<Objectif>();
        
        foreach (Objectif objectif in m_objectifs)
        {
            print(objectif.name);
            objectif.OnStartBurn += OnObjectifBurn;
        }
    }

    private void FindMatches()
    {
        Matches[] matchesList = FindObjectsOfType<Matches>();

        foreach (Matches matches in matchesList)
        {
            matches.OnStartBurn += OnMatchesBurn;
        }

    }

    private void OnObjectifBurn(Burnable burnable)
    {
        m_burnObjectifsCount++;
        print(burnable is Matches);
        if (m_burnObjectifsCount >= m_objectifs.Length)
            GlobalEventBus.onTeamWin.Invoke(Team.MATCHES);
    }

    private void OnMatchesBurn(Burnable burnable)
    {
        Matches matches = (Matches)burnable;
        print(matches.matchesBurnMe.IsControlByPlayer);
        if (matches.matchesBurnMe.IsControlByPlayer)
        {
            AddPotentialPlayers(matches);
        }
    }

    private void AddPotentialPlayers(Matches matches)
    {
        m_potentialPlayers.Add(matches);
        print(m_potentialPlayers.Count);
        if (m_potentialPlayers.Count == 1)
        {
            StartCoroutine(StartTimerPotentialPlayers());
        }
    }

    private IEnumerator StartTimerPotentialPlayers()
    {
        float time = 0f;
        while (time < m_timeBeforePlayerChange)
        {
            time += Time.deltaTime;
            yield return null;
        }

        ChangeRandomPlayer();
    }

    private void ChangeRandomPlayer()
    {
        print("CHANGE PLAYER");
        int randomIndex = Random.Range(0, m_potentialPlayers.Count - 1);
        ChangePlayer(m_potentialPlayers[randomIndex]);
        m_potentialPlayers.Clear();
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

