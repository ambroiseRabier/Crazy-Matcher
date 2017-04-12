using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Events;
using Utils;
using System.Collections.Generic;
using Assets.Scripts.Game.Actors.Player_Firefighter;

public class GameManager : Singleton<GameManager>
{

    #region Variables


    [SerializeField] private List<string> m_sceneName;

    private int m_currentLevel;

    [SerializeField] private float m_timeBeforePlayerChange = 3f;
    private Matches m_currentPlayerMatches;
    
    [SerializeField] private Controller m_controllerP1; 
    [SerializeField] private Controller m_controllerP2; 

    private const float DEFAULT_GAME_TIME_SCALE = 1;

    private float m_gameTimeScale;

    public enum GameState
    {
        TITLE_SCREEN,
        MENU,
        IN_GAME
    }

    private GameState m_currentGameState;

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

        InitCurrentScene();

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

        GlobalEventBus.onLoadingScene.AddListener(OnLoadingScene);
        GlobalEventBus.onTitleScreen.AddListener(OnTitleScreen);
        GlobalEventBus.onMenu.AddListener(OnMenu);
    }


    private void InitVariables()
    {

        m_potentialPlayers = new List<Matches>();
        m_gameTimeScale = DEFAULT_GAME_TIME_SCALE;
    }


    private void InitCurrentScene()
    {
        m_currentLevel = m_sceneName.IndexOf(SceneManager.GetActiveScene().name);
        if (m_currentLevel == 0)
        {
            GlobalEventBus.onTitleScreen.Invoke();
        }
        else
        {
            GlobalEventBus.onInitLevel.Invoke();
        }
    }

    #endregion


    #region Game

    void Update()
    {
        // DEGUEULASSE NE PAS REPRODUIRE
        if (m_currentGameState == GameState.TITLE_SCREEN)
        {
            CheckPressStart();
        }
        else if (m_currentGameState == GameState.MENU)
        {
            CheckMenuButtonPress();
        }
    }

    private void CheckPressStart()
    {
        print(Input.GetButtonDown("Submit"));
        if (Input.GetButtonDown("Submit"))
        {
            GlobalEventBus.onMenu.Invoke();
        }
    }

    private void CheckMenuButtonPress()
    {
        if (Input.GetButtonDown("Fire1_P1"))
        {
            GlobalEventBus.onLoadingScene.Invoke(1);
        }

    }

    private void StartLevel()
    {
        m_burnObjectifsCount = 0;

        FindMatches();
        FindObjectifs();

        InitPlayerMatches();
        InitPlayerFireFight();
    }

    private void InitPlayerMatches()
    {
        m_currentPlayerMatches.Controller = m_controllerP1;
        m_currentPlayerMatches.TryStartBurn();
    }

    private void InitPlayerFireFight()
    {
        ShootWater fireFight = FindObjectOfType<ShootWater>();
        //fireFight.Controller = m_controllerP2;
        fireFight.Controller = m_controllerP1; // TO DEBUG
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
            objectif.OnStartBurn += OnObjectifBurn;
        }
    }

    private void FindMatches()
    {
        Matches[] matchesList = FindObjectsOfType<Matches>();

        foreach (Matches matches in matchesList)
        {
            if (matches.StartPlayer)
            {
                m_currentPlayerMatches = matches;
            }

            matches.OnStartBurn += OnMatchesBurn;
            matches.OnExtinguished += OnMatchesExtinguished;
        }

    }

    private void OnObjectifBurn(Burnable burnable)
    {
        m_burnObjectifsCount++;
        if (m_burnObjectifsCount >= m_objectifs.Length)
            GlobalEventBus.onTeamWin.Invoke(Team.MATCHES);
    }

    private void OnMatchesBurn(Burnable burnable)
    {
        Matches matches = (Matches)burnable;
        if (matches != m_currentPlayerMatches && matches.matchesBurnMe.IsControlByPlayer)
        {
            AddPotentialPlayers(matches);
        }
    }

    private void OnMatchesExtinguished(Burnable burnable)
    {
        Matches matches = (Matches)burnable;
        RemovePotentialPlayers(matches);
        CheckWinFireFighter();
    }

    private void CheckWinFireFighter()
    {
        if (!MatchesInFire())
        {
            GlobalEventBus.onTeamWin.Invoke(Team.FIRE_FIGHTER);
        }
    }

    private bool MatchesInFire()
    {
        Matches[] matchesList = FindObjectsOfType<Matches>();

        foreach (Matches matches in matchesList)
        {
            if (matches.IsBurning)
            {
                return true;
            }
        }

        return false;

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

    private void RemovePotentialPlayers(Matches matches)
    {
        m_potentialPlayers.Remove(matches);
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
        if (m_potentialPlayers.Count != 0)
        {
            int randomIndex = Random.Range(0, m_potentialPlayers.Count - 1);
            ChangePlayer(m_potentialPlayers[randomIndex]);
        }
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

    private void OnInitLevel()
    {
        //TODO 
        GlobalEventBus.onStartLevel.Invoke();
    }

    private void OnLoadingScene(int sceneNumber = -1)
    {
        if (sceneNumber != -1)
        {
            m_currentLevel = sceneNumber;
        }
        else
        {
            m_currentLevel = m_sceneName.IndexOf(SceneManager.GetActiveScene().name);
        }
        StartCoroutine(StartLoadingScene(m_sceneName[m_currentLevel]));
    }

    private void OnTitleScreen()
    {
        m_currentGameState = GameState.TITLE_SCREEN;
    }

    private void OnMenu()
    {
        m_currentGameState = GameState.MENU;
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


    #region Loading Scene

    IEnumerator StartLoadingScene(string nameScene)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(nameScene);
        yield return async;
        OnSceneReady();
    }

    private void OnSceneReady()
    {
        if (m_currentLevel == 0)
        {
            GlobalEventBus.onMenu.Invoke();
        }
        else
        {
            GlobalEventBus.onInitLevel.Invoke();
        }
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

