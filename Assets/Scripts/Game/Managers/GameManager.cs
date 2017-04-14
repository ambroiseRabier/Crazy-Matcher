using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Events;
using Utils;
using System.Collections.Generic;
using Assets.Scripts.Game.Actors.Player_Firefighter;
using Random = UnityEngine.Random;
using Assets.Scripts.Game;
using System;

public class GameManager : Singleton<GameManager>
{

    #region Variables


    [SerializeField] private List<string> m_sceneName;

    private int m_currentLevel;

    [SerializeField] private float m_timeBeforePlayerChange = 3f;
    private Matches m_currentPlayerMatches;
    
    [SerializeField] private Controller m_controllerP1; 
    [SerializeField] private Controller m_controllerP2;

    private AudioSource m_MusicAudioSource;

    [SerializeField] private AudioClip titleAudioClip;
    [SerializeField] private AudioClip BGMAudioClip;
    [SerializeField] private AudioClip matchesVictoryAudioClip;
    [SerializeField] private AudioClip grammyVictoryAudioClip;



    private const float DEFAULT_GAME_TIME_SCALE = 1;

    private float m_gameTimeScale;

    private int m_gameCount;
    private int m_scoreFireFightP1;
    private int m_scoreMatchesP1;
    private int m_scoreFireFightP2;
    private int m_scoreMatchesP2;
    private bool m_p1IsMatches;

    private Dictionary<AudioClip, AudioSource> m_audioClipsToAudioSource = new Dictionary<AudioClip, AudioSource>();

    public enum GameState
    {
        TITLE_SCREEN,
        MENU,
        IN_GAME,
        WIN_SCREEN,
        CREDIT_MENU,
        INPUT_SCREEN
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
        StartCoroutine(ControllerUpdate());

    }

    internal void PlaySound(object m_startFireSoundClip)
    {
        throw new NotImplementedException();
    }

    #endregion


    #region Initialized

    private void InitEvent()
    {
        GlobalEventBus.onPause.AddListener(OnPause);
        GlobalEventBus.onResume.AddListener(OnResume);
        GlobalEventBus.onInitLevel.AddListener(OnInitLevel);

        GlobalEventBus.onInputScreen.AddListener(OnInputScreen);
        GlobalEventBus.onCreditScreen.AddListener(OnCreditMenu);

        GlobalEventBus.onStartLevel.AddListener(OnStartLevel);
        GlobalEventBus.onTeamWin.AddListener(OnTeamWin);

        GlobalEventBus.onLoadingScene.AddListener(OnLoadingScene);
        GlobalEventBus.onTitleScreen.AddListener(OnTitleScreen);
        GlobalEventBus.onMenu.AddListener(OnMenu);
        GlobalEventBus.onRestartGame.AddListener(OnRestartGame);
    }


    private void InitVariables()
    {
        m_MusicAudioSource = GetComponent<AudioSource>(); 
        m_potentialPlayers = new List<Matches>();
        m_gameTimeScale = DEFAULT_GAME_TIME_SCALE;
    }


    private void InitCurrentScene()
    {
        m_currentLevel = m_sceneName.IndexOf(SceneManager.GetActiveScene().name);
        if (m_currentLevel == 0)
        {
            GlobalEventBus.onMenu.Invoke();
        }
        else
        {
            GlobalEventBus.onInitLevel.Invoke();
        }
    }

    private void InitGame()
    {
        m_gameCount = 0;
        m_scoreFireFightP1 = 0;
        m_scoreMatchesP1 = 0;
        m_scoreFireFightP2 = 0;
        m_scoreMatchesP2 = 0;
        m_p1IsMatches = true;
    }

    #endregion


    #region Game

    protected override void Awake()
    {
        base.Awake();
    }

    IEnumerator ControllerUpdate()
    {
        while (true)
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
            else if (m_currentGameState == GameState.WIN_SCREEN)
            {
                CheckWinScreenButtonPress();
            }
            else if (m_currentGameState == GameState.INPUT_SCREEN)
            {
                CheckInputScreenPress();
            }
            else if (m_currentGameState == GameState.CREDIT_MENU)
            {
                CheckCreditMenuPress();
            }

            yield return null;
        }
    }

    private void CheckInputScreenPress()
    {
        if (Input.GetButtonDown("Fire2_P1") || Input.GetButtonDown("Fire2_P2"))
        {
            InputScreen.instance.Close(GlobalEventBus.onMenu.Invoke);
            
        }
    }

    private void CheckCreditMenuPress()
    {
        if (Input.GetButtonDown("Fire2_P1") || Input.GetButtonDown("Fire2_P2"))
        {
            CreditsScreen.instance.Close(GlobalEventBus.onMenu.Invoke);
        }
    }

    void Update()
    {
    }

    private void CheckPressStart()
    {
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Fire1_P1"))
        {

            
            //CinematicIntroduction.instance.Close(GlobalEventBus.onMenu.Invoke);
            CinematicIntroduction.instance.Close(() => { GlobalEventBus.onLoadingScene.Invoke(1); });

        }
        else if (Input.GetButtonDown("Fire2_P1"))
        {
            CinematicIntroduction.instance.Close(GlobalEventBus.onMenu.Invoke);
            //QuitApplication();
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        GetAudioSourceFromAudioClip(audioClip).Play();
    }

    private AudioSource GetAudioSourceFromAudioClip(AudioClip audioClip)
    {
        AudioSource audioSource;

        if(!m_audioClipsToAudioSource.TryGetValue(audioClip, out audioSource))
        {
            audioSource = m_audioClipsToAudioSource[audioClip] = gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioClip;
        }

        return audioSource;
    }

    private void CheckMenuButtonPress()
    {
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Fire1_P1"))
        {
            TitleScreen.instance.Close(GlobalEventBus.onTitleScreen.Invoke);
            
            //GlobalEventBus.onLoadingScene.Invoke(1);
        }
        else if (Input.GetButtonDown("Fire2_P1"))
        {
            QuitApplication();
            //GlobalEventBus.onTitleScreen.Invoke();
        }
        else if (Input.GetButtonDown("Xbox_X"))
        {
            GlobalEventBus.onInputScreen.Invoke();
        }
        else if (Input.GetButtonDown("Xbox_Y"))
        {
            GlobalEventBus.onCreditScreen.Invoke();
        }

    }

    private void CheckWinScreenButtonPress()
    {
        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Fire1_P1"))
        {
            WinScreen.instance.Close();
            GlobalEventBus.onRestartGame.Invoke();
        }
        else if (Input.GetButtonDown("Fire2_P1"))
        {
            WinScreen.instance.Close();
            GlobalEventBus.onLoadingScene.Invoke(0);
        }
    }

    private void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
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
        
        if (m_p1IsMatches)
        {
            m_currentPlayerMatches.Controller = m_controllerP1;
        }
        else
        {
            m_currentPlayerMatches.Controller = m_controllerP2;
        }

        m_currentPlayerMatches.TryStartBurn();
    }

    private void InitPlayerFireFight()
    {
        ShootWater fireFight = FindObjectOfType<ShootWater>();

        if (m_p1IsMatches)
        {
            fireFight.Controller = m_controllerP2;
        }
        else
        {
            fireFight.Controller = m_controllerP1;
        }
    }

    private void ChangePlayer(Matches matches)
    {
        m_currentPlayerMatches.Controller = null;
        m_currentPlayerMatches.Die();
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
        if (m_burnObjectifsCount >= m_objectifs.Length - 1)
            GlobalEventBus.onTeamWin.Invoke(Team.MATCHES);
    }

    private void OnMatchesBurn(Burnable burnable)
    {
        Matches matches = (Matches)burnable;
        if (matches.matchesBurnMe && matches != m_currentPlayerMatches && matches.matchesBurnMe.IsControlByPlayer)
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

    private void TitleScreen2()
    {
        GlobalEventBus.onTitleScreen.Invoke();
    }

    private void UpdatePlayer()
    {
        if (m_p1IsMatches)
        {
            VSIntroductionScreen.instance.SetP1Team(Team.MATCHES);
            VSIntroductionScreen.instance.SetP2Team(Team.FIRE_FIGHTER);
        }
        else
        {
            VSIntroductionScreen.instance.SetP1Team(Team.FIRE_FIGHTER);
            VSIntroductionScreen.instance.SetP2Team(Team.MATCHES);
        }
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

    private void UpdateUIScore()
    {
        HUD.instance.P1ScoreComp.SetNFireFighterWin(m_scoreFireFightP1);
        HUD.instance.P1ScoreComp.SetNMatchesWin(m_scoreMatchesP1);

        HUD.instance.P2ScoreComp.SetNFireFighterWin(m_scoreFireFightP2);
        HUD.instance.P2ScoreComp.SetNMatchesWin(m_scoreMatchesP2);
    }

    private void OnInitLevel()
    {
        PlayMusic(BGMAudioClip);
        UpdatePlayer();
        UpdateUIScore();

        m_currentGameState = GameState.IN_GAME;
        Time.timeScale = 0;

        Timer.DelayThenPerform(1.25f, () => {
            VSIntroductionScreen.instance.Close(()=>{
                Timer.DelayThenPerform(0.25f, () =>{
                    Starter.instance.StartStarterThenPerformOnEnd(GlobalEventBus.onStartLevel.Invoke);
                });
            });
        });
    }
    
    private void OnInputScreen()
    {
        m_currentGameState = GameState.INPUT_SCREEN;
    }

    private void OnCreditMenu()
    {
        m_currentGameState = GameState.CREDIT_MENU;
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
        PlayMusic(titleAudioClip);
        InitGame();
        m_currentGameState = GameState.MENU;
    }

    private void OnRestartGame()
    {
        m_p1IsMatches = !m_p1IsMatches;
        UpdatePlayer();
        GlobalEventBus.onLoadingScene.Invoke(1);
    }

    private void OnStartLevel()
    {
        Time.timeScale = 1;
        StartLevel();
    }

    private void OnTeamWin(Team teamWin)
    {
        Time.timeScale = 0;
        if (teamWin == Team.FIRE_FIGHTER)
        {
            PlayMusic(matchesVictoryAudioClip);

            if (m_p1IsMatches)
                m_scoreFireFightP2++;
            else
                m_scoreFireFightP1++;
        }
        else
        {
            PlayMusic(grammyVictoryAudioClip);

            if (m_p1IsMatches)
                m_scoreMatchesP1++;
            else
                m_scoreMatchesP2++;
        }

        Timer.DelayThenPerform(2, () => { m_currentGameState = GameState.WIN_SCREEN; });
        UpdateUIScore();
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

    #region Music

    private string m_currentSoundName = "";
    private void PlayMusic(AudioClip music)
    {
        if (m_MusicAudioSource.isPlaying)
        {
            if (m_currentSoundName != music.name)
            {
                m_currentSoundName = music.name;
                StartCoroutine(Fade(m_MusicAudioSource, 1, 0, 0.5f, () =>
                {
                    m_MusicAudioSource.clip = music;
                    m_MusicAudioSource.Play();
                    StartCoroutine(Fade(m_MusicAudioSource, 0, 1, 0.5f));
                }));
            }
        }
        else
        {
            m_currentSoundName = music.name;
            m_MusicAudioSource.PlayOneShot(music);
        }
    }

    private IEnumerator Fade(AudioSource audioSource, float startVolume, float endVolume, float FadeTime, Action callback = null)
    {
        audioSource.volume = startVolume;

        float endTime = Time.unscaledTime + FadeTime;

        while (Time.unscaledTime < endTime)
        {
            float percent = (FadeTime - (endTime - Time.unscaledTime)) / FadeTime;
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, percent);

            yield return null;
        }

        audioSource.volume = endVolume;

        if (callback != null)
        {
            callback();
        }
    }

    #endregion
}

