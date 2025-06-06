using UnityEngine;
using Rewired;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class GAMEPLAY : MonoBehaviour
{
    public static GAMEPLAY access;
    public bool PlayerControl = false;
    public GameplayState CurrentState;
    public float timeToJoin = 3f;
    public float timeToSuddenDeath = 60f;
    int totalPlayers = 0;
    int alivePlayers = 0;
    float timer;
    [SerializeField]GameObject[] playerPrefabs;
    List<GameObject> alivePlayersList = new List<GameObject>();
    [SerializeField] TextMeshProUGUI Countdown;
    
    private void OnEnable()
    {
        EVENTS.OnPlayerDeath += RemovePlayerNumber;
        EVENTS.OnGameplay += GamePlayStarted;
    }



    private void OnDisable()
    {
        EVENTS.OnPlayerDeath -= RemovePlayerNumber;
        EVENTS.OnGameplay -= GamePlayStarted;

    }

    private void Start()
    {
        access = this;
        EnterState(GameplayState.off);
        DestroyAllPlayers();
        ResetAllControllers();
    }

    private void Update()
    {
        IncreaseTimer();
        UpdateCurrentState();
    }


    private void GamePlayStarted()
    {
        EnterState(GameplayState.joining);
    }

    #region PLAYER NUMBER
    //-----------------------------------------------------------------------//
    //-PLAYER-NUMBER---------------------------------------------------------//
    //-----------------------------------------------------------------------//

    private void RemovePlayerNumber(int deadID)
    {
        alivePlayers--;
    }

    //-----------------------------------------------------------------------//
    #endregion

    #region TIME
    //-----------------------------------------------------------------------//
    //-TIME-SYSTEM-----------------------------------------------------------//
    //-----------------------------------------------------------------------//

    private void IncreaseTimer()
    {
        timer += Time.deltaTime;
    }

    private void ResetTimer()
    {
        timer = 0;
    }

    //-----------------------------------------------------------------------//
    #endregion

    #region STATE
    //-----------------------------------------------------------------------//
    //-STATE-GAMEPLAY-SYSTEM-------------------------------------------------//
    //-----------------------------------------------------------------------//
    void EnterState(GameplayState newState)
    {
        ResetTimer();
        CurrentState = newState;

        switch(newState) // Fonction Start quand on rentre dans un nouvel état
        {
            case GameplayState.off:
                Debug.Log("IsOFF");
                Countdown.transform.parent.parent.gameObject.SetActive(false);
                PlayerControl = false;
                DestroyAllPlayers();
                ResetAllControllers();
                GAME.MANAGER.SwitchTo(State.menu);
            break;

            case GameplayState.joining:
                Debug.Log("IsJoining");
                if (totalPlayers>0) CreatePlayer(0); else Debug.Log("Je t'encule thérèse");
                PlayerControl = false;
                GAME.MANAGER.SwitchTo(State.waiting);
            break;

            case GameplayState.battle:
                EVENTS.OnGameplay -= GamePlayStarted;
                Debug.Log("IsBattle");
                Countdown.transform.parent.parent.gameObject.SetActive(false);
                PlayerControl = true;
                EVENTS.InvokeBattleStart();
                alivePlayers = totalPlayers;
                GAME.MANAGER.SwitchTo(State.gameplay);
            break;

            case GameplayState.suddenDeath:
                Debug.Log("IsSuddenDeath");
                EVENTS.InvokeSuddenDeathStart();
            break;

            case GameplayState.end:
                Debug.Log("IsEnd");
                PlayerControl = false;
                GAME.MANAGER.SwitchTo(State.waiting);
                // ⚠️ ICI IL FAUDRAIT DÉSACTIVER TOUTES LES BOMBES ACTUELLEMENT À L'ÉCRAN
            break;
        }
    }
    
    private void UpdateCurrentState() // L'Update de ma machine à état
    {
        switch (CurrentState)
        {
            case GameplayState.off:
                if (activeControllers[0] == null) GetFirstPlayer();
            break;

            case GameplayState.joining:
                if (totalPlayers > 1)
                {
                    Countdown.transform.parent.parent.gameObject.SetActive(true);
                    Countdown.text = Mathf.CeilToInt(timeToJoin - timer).ToString();
                }
                if (timer >= timeToJoin && totalPlayers > 1)
                {
                    EnterState(GameplayState.battle);
                }
                if (totalPlayers<4) ListenNewControllers();
            break;

            case GameplayState.battle:
                if (timer >= timeToSuddenDeath) EnterState(GameplayState.suddenDeath);
                if (alivePlayers<2) EnterState(GameplayState.end);
            break;

            case GameplayState.suddenDeath:
                if (alivePlayers<2) EnterState(GameplayState.end);
            break;

            case GameplayState.end:
                if (timer>3f)
                {
                    SceneLoader.access.LoadScene(1, 1, 0.25f, 1, false, 0.5f); // ⚠️ IL FAUDRAIT QUE L'ÉCRAN DE FIN NE SOIT PAS UNE SCÈNE À PART MAIS UN SIMPLE MENU
                    GAME.MANAGER.SwitchTo(State.menu);
                    EnterState(GameplayState.off);
                }
            break;
        }
    }

    public void WaitPlayersToJoin()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        SceneLoader.access.LoadScene(SceneManager.GetActiveScene().buildIndex, 1, 0.25f, 1, false, 0.5f);
        while (SceneLoader.access.IsLoading) yield return null;
        while (Black.screen.IsWorking) yield return null;
        EnterState(GameplayState.joining);
    }

    public void Rematch()
    {
        EnterState(GameplayState.battle);
    }




    //-----------------------------------------------------------------------//
    #endregion

    void CreatePlayer(int playerID)
    {
        GameObject newPlayer = Instantiate(playerPrefabs[playerID], playerPrefabs[playerID].transform.localPosition, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(newPlayer,SceneManager.GetActiveScene());
        alivePlayersList.Add(newPlayer);
        Debug.Log("CreatePlayer"+playerID);
    }

    void DestroyAllPlayers()
    {
        for (int i=0; i<alivePlayersList.Count;i++) Destroy(alivePlayersList[i]);
        alivePlayersList = new List<GameObject>();
        alivePlayers = totalPlayers = 0;
    }



    // 🎮 CONTROLLER ASSIGNMENT
    Controller[] activeControllers = new Controller[4];


    void ResetAllControllers()
    {
        totalPlayers = 0;
        activeControllers = new Controller[4];
        foreach (Player player in ReInput.players.GetPlayers())
        {
            player.controllers.ClearAllControllers();
            player.isPlaying = false;
        }
        ReInput.players.GetPlayer(0).controllers.AddController(ControllerType.Mouse, 0, true);
    }

    void GetFirstPlayer()
    {
        if (ReInput.controllers.GetAnyButton())
        {
            Controller lastActive = ReInput.controllers.GetLastActiveController();
            if (lastActive.type == ControllerType.Mouse) return;
            AssignControllerToPlayer(0, lastActive);
        }
    }

    void ListenNewControllers()
    {
        if (ReInput.controllers.GetAnyButton())
        {
            Controller lastActive = ReInput.controllers.GetLastActiveController();
            if (!activeControllers.Contains(lastActive) && lastActive.type!=ControllerType.Mouse)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (activeControllers[i] == null)
                    {
                        CreatePlayer(i);
                        ResetTimer();
                        AssignControllerToPlayer(i, lastActive);
                        ReInput.players.GetPlayer(i).isPlaying = true;
                        break;
                    }
                }
            }
        }
    }


    void AssignControllerToPlayer(int playerID, Controller controller)
    {
        totalPlayers++;
        ReInput.players.GetPlayer(playerID).controllers.AddController(controller, true);
        activeControllers[playerID] = controller;
        Debug.Log("🎮" + controller.name + " to Player" + playerID);
    }



} // FIN DU SCRIPT

