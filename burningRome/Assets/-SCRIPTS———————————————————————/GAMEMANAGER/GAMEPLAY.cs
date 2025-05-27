using UnityEngine;
using Rewired;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

class PlayerData
{
    public int playerID;
    public Controller controller;
    public int score;
    public GameObject gameObjectInstance;
    public PlayerManager manager;
}

public class GAMEPLAY : MonoBehaviour
{
    public static GAMEPLAY access;
    List<PlayerData> players = new List<PlayerData>();
    List<PlayerData> alivePlayersList = new List<PlayerData>();
    public bool PlayerControl = false;
    public GameplayState CurrentState;
    public float timeToJoin = 3f;
    public float timeToSuddenDeath = 60f;
    public int AlivePlayers
    {
        get { return alivePlayersList.Count; }
    }
    public int TotalPlayers
    {
        get { return players.Count; }
    }
    float timer;
    [SerializeField] GameObject[] playerPrefabs;



    void Awake()
    {
        access = this;
    }

    private void OnEnable()
    {
        EVENTS.OnGameStart += LaunchGameplayBoucle;
        EVENTS.OnPlayerDeath += PlayerDeath;
        EVENTS.OnAfterGameStart += GameplayAfterFirstBattle;
        ReInput.ControllerPreDisconnectEvent += CheckDisconnect;
        EVENTS.OnRematch += Rematch;
    }



    private void OnDisable()
    {
        EVENTS.OnGameStart -= LaunchGameplayBoucle;
        EVENTS.OnPlayerDeath -= PlayerDeath;
        EVENTS.OnAfterGameStart += GameplayAfterFirstBattle;
        ReInput.ControllerPreDisconnectEvent -= CheckDisconnect;
        EVENTS.OnRematch -= Rematch;
    }

    private void Start()
    {
        EnterState(GameplayState.off);
        ResetAllControllers();
    }

    private void Update()
    {
        IncreaseTimer();
        UpdateCurrentState();
    }



    private void PlayerDeath(int deadID)
    {
        Debug.Log("💀 Player died: " + deadID);
        for (int i = 0; i < alivePlayersList.Count; i++)
        {
            if (alivePlayersList[i].playerID == deadID) alivePlayersList.RemoveAt(i);
        }
        //foreach (PlayerData deadPlayer in alivePlayersList) if (deadPlayer.playerID == deadID) alivePlayersList.Remove(deadPlayer);
        if (AlivePlayers < 2) EnterState(GameplayState.battleOver);
    }

    public int GetPlayerScore(int playerID)
    {
        if (players.Count <= playerID) return -1;
        else return players[playerID].score;
    }



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
        if (newState == CurrentState) return;
        ResetTimer();
        CurrentState = newState;

        switch (newState) // Fonction Start quand on rentre dans un nouvel état
        {
            case GameplayState.off:
                Debug.Log("IsOFF");
                PlayerControl = false;
                InBattle = false;
                GAME.MANAGER.SwitchTo(State.menu);
                break;

            case GameplayState.joining:
                Debug.Log("IsJoining");
                PlayerControl = false;
                GAME.MANAGER.SwitchTo(State.menu);
                ResetAllPlayers();
                ResetAllControllers();
                EVENTS.InvokeJoiningStart();
                break;

            case GameplayState.battle:
                Debug.Log("IsBattle");
                PlayerControl = true;
                gameEnded = false;
                Debug.Log("🟢 Battle Start — alivePlayers = " + AlivePlayers);
                GAME.MANAGER.SwitchTo(State.gameplay);
                EVENTS.InvokeBattleStart();
                break;

            case GameplayState.suddenDeath:
                Debug.Log("IsSuddenDeath");
                EVENTS.InvokeSuddenDeathStart();
                break;

            case GameplayState.battleOver:
                PlayerControl = false;
                break;

            case GameplayState.end:
                Debug.Log("IsEnd");
                InBattle = false;
                GAME.MANAGER.SwitchTo(State.menu);
                EVENTS.InvokeScoreDisplay();
                break;
        }
    }

    private void UpdateCurrentState() // L'Update de ma machine à état
    {
        switch (CurrentState)
        {
            case GameplayState.off:
            break;

            case GameplayState.joining:
                if (TotalPlayers < 4) ListenNewControllers();
            break;

            case GameplayState.battle:
                if (timer >= timeToSuddenDeath) EnterState(GameplayState.suddenDeath);
            break;

            case GameplayState.suddenDeath:
            break;

            case GameplayState.battleOver:
                if (timer > 0.1f && gameEnded == false)
                {
                    gameEnded = true;
                    EVENTS.InvokeEndGame();
                    if (AlivePlayers > 0)
                    {
                        alivePlayersList[0].score++;
                        EVENTS.InvokeOnVictory(alivePlayersList[0].playerID);
                    }
                    GAME.MANAGER.SwitchTo(State.waiting);
                }

                if (timer > 6f)
                {
                    EnterState(GameplayState.end);
                }
            break;

            case GameplayState.end:
            break;
        }
    }



    bool gameEnded = false;

    public void OnStartClick()
    {
        if (TotalPlayers < 2) return;
        EVENTS.InvokeGameStart();
    }
    public void CheckActivePlayers(int activeplayers)
    {

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

    void Rematch()
    {
        StartCoroutine(RematchRoutine());
    }

    IEnumerator RematchRoutine()
    {
        foreach (PlayerData alivePlayer in alivePlayersList) Destroy(alivePlayer.gameObjectInstance);
        alivePlayersList = new List<PlayerData>();

        foreach (PlayerData playingPlayer in players)
        {
            alivePlayersList.Add(playingPlayer);
            GameObject restartingPlayer = InstantiatePlayerInScene(playingPlayer.playerID);
        }
        yield return null;
        while (Black.screen.IsWorking) yield return null;
        EVENTS.InvokeGameStart();
        EnterState(GameplayState.battle);
    }





    bool InBattle = false;
    void LaunchGameplayBoucle()
    {
        if (InBattle) return;
        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        if (InBattle) yield break;
        InBattle = true;
        EVENTS.InvokeJoiningDone();
        yield return new WaitForSeconds(1f);
        MENU.SCRIPT.MenusList(false);
        EnterState(GameplayState.battle);
    }
    public void LaunchJoin()
    {
        EnterState(GameplayState.joining);
    }

    public void GameplayAfterFirstBattle()
    {
        Rematch();
    }



    //-----------------------------------------------------------------------//
    #endregion


    void ResetAllPlayers()
    {
        foreach (PlayerData player in players) Destroy(player.gameObjectInstance);
        alivePlayersList = new List<PlayerData>();
        players = new List<PlayerData>();
    }


    GameObject InstantiatePlayerInScene(int playerID)
    {
        Debug.Log("CreatePlayer" + playerID);
        GameObject newPlayer = Instantiate(playerPrefabs[playerID], playerPrefabs[playerID].transform.localPosition, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(newPlayer, SceneManager.GetActiveScene());
        return newPlayer;
    }





    // 🎮 CONTROLLER ASSIGNMENT -----------------------------------------------------------------------------------------------------------------------------------------
    Controller[] activeControllers = new Controller[4];
    bool leftKeyboardAssigned, rightKeyboardAssigned = false;

    void ResetAllControllers()
    {
        activeControllers = new Controller[4];
        foreach (Player player in ReInput.players.GetPlayers())
        {
            player.controllers.maps.SetMapsEnabled(false, ControllerType.Keyboard, 0, 1);
            player.controllers.maps.SetMapsEnabled(false, ControllerType.Keyboard, 0, 2);
            player.controllers.ClearAllControllers();
            player.isPlaying = false;
        }
        Player systemPlayer = ReInput.players.GetSystemPlayer();
        systemPlayer.controllers.ClearAllControllers();
        systemPlayer.controllers.AddController(ControllerType.Mouse, 0, true);
        systemPlayer.controllers.AddController(ControllerType.Keyboard, 0, false);
        leftKeyboardAssigned = rightKeyboardAssigned = false;
        //Debug.Log("Reset all controllers   SystemPlayer " + ReInput.players.GetSystemPlayer().controllers);
    }



    void ListenNewControllers()
    {
        if (ReInput.controllers.GetAnyButtonDown())
        {
            Controller lastActive = ReInput.controllers.GetLastActiveController();
            int newPlayer = FirstAvailablePlayer();
            if (newPlayer < 0) return; // all players already have a controller, abort

            if (lastActive.type == ControllerType.Keyboard)
            {
                if (leftKeyboardAssigned == false && ReInput.controllers.Keyboard.GetKeyDown(KeyCode.Space)) AddPlayerKeyboard(newPlayer, 2, lastActive);
                else if (rightKeyboardAssigned == false && ReInput.controllers.Keyboard.GetKeyDown(KeyCode.Return)) AddPlayerKeyboard(newPlayer, 1, lastActive);
            }

            if (lastActive.type == ControllerType.Joystick && !activeControllers.Contains(lastActive))
            {
                AddPlayerController(newPlayer, lastActive);
            }
        }
    }


    void AddPlayerKeyboard(int playerID, int keyboardSide, Controller keyboard)
    {
        Debug.Log("ADD KEYBOARD PLAYER " + playerID + "   keyboardside " + keyboardSide);
        if (keyboardSide == 2) leftKeyboardAssigned = true;
        if (keyboardSide == 1) rightKeyboardAssigned = true;
        ReInput.players.GetPlayer(playerID).controllers.maps.SetMapsEnabled(true, ControllerType.Keyboard, 0, keyboardSide);
        AddPlayerController(playerID, keyboard);
    }


    void AddPlayerController(int playerID, Controller controller)
    {
        ResetTimer();
        AddNewPlayer(playerID, controller);
        ReInput.players.GetPlayer(playerID).isPlaying = true;
    }


    int FirstAvailablePlayer()
    {
        for (int i = 0; i < 4; i++)
        {
            if (activeControllers[i] == null) return i;
        }
        return -1;
    }


    void AddNewPlayer(int playerID, Controller controller)
    {
        if (!players.Exists(x => x.playerID == playerID))
        {
            GameObject newPlayerInstance = InstantiatePlayerInScene(playerID);
            PlayerData newPlayerData = new PlayerData
            {
                playerID = playerID,
                controller = controller,
                gameObjectInstance = newPlayerInstance,
                score = 0,
                manager = newPlayerInstance.GetComponentInChildren<PlayerManager>()
            };
            AssignControllerToPlayer(playerID, controller);
            players.Add(newPlayerData);
            alivePlayersList.Add(newPlayerData);
            EVENTS.InvokePlayerSpawn(playerID);
        }
        else Debug.Log("❌ERROR PLAYER" + playerID + " ALREADY EXISTS!");
    }


    void AssignControllerToPlayer(int playerID, Controller controller)
    {
        bool removeFromOtherPlayers = true;
        if (controller.type == ControllerType.Keyboard) removeFromOtherPlayers = false;
        Player thisPlayer = ReInput.players.GetPlayer(playerID);
        thisPlayer.controllers.ClearAllControllers();
        thisPlayer.controllers.AddController(controller, removeFromOtherPlayers);
        activeControllers[playerID] = controller;
        Debug.Log("🎮" + controller.name + " to Player" + playerID);
    }


    void CheckDisconnect(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("🎮 DISCONNECTED " + args.controller.name);
        if (CurrentState == GameplayState.off || CurrentState == GameplayState.joining) return;
        if (args.controller.type != ControllerType.Joystick) return;
        if (activeControllers.Contains(args.controller))
        {
            int index = System.Array.IndexOf(activeControllers, args.controller);
            activeControllers[index] = null;
            Debug.Log("🎮 PLAYER" + index + " HAS NO CONTROLLER!");
            StartCoroutine(WaitControllerReconnect(index));
        }
    }

    IEnumerator WaitControllerReconnect(int playerID)
    {
        Debug.Log("🎮 WAITING FOR PLAYER" + playerID + " CONTROLLER TO CONNECT");
        while (activeControllers[playerID] == null)
        {
            if (ReInput.controllers.GetAnyButtonDown())
            {
                Controller lastActive = ReInput.controllers.GetLastActiveController();

                if (lastActive.type == ControllerType.Joystick && !activeControllers.Contains(lastActive))
                {
                    AssignControllerToPlayer(playerID, lastActive);
                }
            }
            yield return null;
        }
    }



} // FIN DU SCRIPT

