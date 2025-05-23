using UnityEngine;
using Rewired;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class GAMEPLAY : MonoBehaviour
{
    class PlayerData
    {
        public int playerID;
        public Controller controller;
        public int keyboardSide;
    }
    List<PlayerData> persistentPlayers = new List<PlayerData>();

    public static GAMEPLAY access;

    public bool PlayerControl = false;
    public GameplayState CurrentState;
    public float timeToJoin = 3f;
    public float timeToSuddenDeath = 60f;
    int totalPlayers = 0;
    public int TotalPlayers
    {
        get { return totalPlayers; }
        set { totalPlayers = value; }
    }

    int alivePlayers = 0;
    float timer;
    [SerializeField]GameObject[] playerPrefabs;
    [SerializeField] List<GameObject> alivePlayersList = new List<GameObject>();
    [SerializeField] TextMeshProUGUI Countdown;
    
    private void OnEnable()
    {
        EVENTS.OnGameStart += LaunchGameplayBoucle;
        EVENTS.OnPlayerDeath += RemovePlayerNumber;
        EVENTS.OnGameplay += GamePlayStarted;
        EVENTS.OnAfterGameStart += GameplayAfterFirstBattle;
        ReInput.ControllerPreDisconnectEvent += CheckDisconnect;
    }



    private void OnDisable()
    {
        EVENTS.OnGameStart -= LaunchGameplayBoucle;
        EVENTS.OnPlayerDeath -= RemovePlayerNumber;
        EVENTS.OnGameplay -= GamePlayStarted;
        EVENTS.OnAfterGameStart += GameplayAfterFirstBattle;
        ReInput.ControllerPreDisconnectEvent -= CheckDisconnect;
    }

    private void Start()
    {
        access = this;
        EnterState(GameplayState.off);
        if (MENU.SCRIPT.AlreadyStartFirstGame == false)
        {
            DestroyAllPlayers();
        }
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
        Debug.Log("💀 Player died: " + deadID + " — Players alive: " + alivePlayers);
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
        if (newState == CurrentState) return;
        ResetTimer();
        CurrentState = newState;

        switch(newState) // Fonction Start quand on rentre dans un nouvel état
        {
            case GameplayState.off:
                Debug.Log("IsOFF");
                Countdown.transform.parent.parent.gameObject.SetActive(false);
                PlayerControl = false;
                if(MENU.SCRIPT.AlreadyStartFirstGame == false)
                {
                    DestroyAllPlayers();
                }            
                ResetAllControllers();
                GAME.MANAGER.SwitchTo(State.menu);
            break;

            case GameplayState.joining:
                Debug.Log("IsJoining");
                Debug.Log("TotalPlayers = "+totalPlayers);
                PlayerControl = false;
                GAME.MANAGER.SwitchTo(State.waiting);
            break;

            case GameplayState.battle:
                EVENTS.OnGameplay -= GamePlayStarted;
                Debug.Log("IsBattle");
                Countdown.transform.parent.parent.gameObject.SetActive(false);
                PlayerControl = true;
                alivePlayers = totalPlayers;
                Debug.Log("🟢 Battle Start — alivePlayers = " + alivePlayers);
                EVENTS.InvokeBattleStart();
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
               
                if (alivePlayers < 2)
                {
                    EVENTS.InvokeDestroyAllBombs();
                    EVENTS.InvokeOnVictory();
                    EnterState(GameplayState.end);
                }
            break;

            case GameplayState.suddenDeath:
                if (alivePlayers < 2)
                {
                    EVENTS.InvokeDestroyAllBombs();
                    EVENTS.InvokeOnVictory();
                    EnterState(GameplayState.end);
                }
            break;

            case GameplayState.end:
                EVENTS.InvokeEndGame();
                EVENTS.InvokeDestroyAllBombs();
                if (timer>4f)
                {
                    //SceneLoader.access.LoadScene(1, 1, 0.25f, 1, false, 0.5f); // ⚠️ IL FAUDRAIT QUE L'ÉCRAN DE FIN NE SOIT PAS UNE SCÈNE À PART MAIS UN SIMPLE MENU
                    GAME.MANAGER.SwitchTo(State.menu);
                    EnterState(GameplayState.off);
                    //persistentPlayers.Clear();
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
        DestroyAllPlayers();
        Debug.Log("🔁 Rematch: persistentPlayers count = " + persistentPlayers.Count);
        foreach (PlayerData data in persistentPlayers)
        {
            Debug.Log("🔁 Player in rematch: ID = " + data.playerID);
            Player player = ReInput.players.GetPlayer(data.playerID);
            //InstantiatePlayerInScene(data.playerID);
            AddPlayerController(data.playerID,data.controller);
            if (data.controller.type == ControllerType.Keyboard)
            {
                player.controllers.maps.SetMapsEnabled(true, ControllerType.Keyboard, 0, data.keyboardSide);
                if (data.keyboardSide == 1) rightKeyboardAssigned = true;
                if (data.keyboardSide == 2) leftKeyboardAssigned = true;
            }
            else if (data.controller.type == ControllerType.Joystick)
            {
                player.controllers.maps.SetMapsEnabled(true, ControllerType.Joystick, 0);
            }
            //AssignControllerToPlayer(data.playerID, data.controller);
            ReInput.players.GetPlayer(data.playerID).isPlaying = true;
        }
        totalPlayers = persistentPlayers.Count;
        //alivePlayers = totalPlayers;
        EnterState(GameplayState.battle);
    }

    public void LaunchGameplayBoucle()
    {
        EnterState(GameplayState.joining);
    }

    public void GameplayAfterFirstBattle()

    {
        Rematch();
    }



    //-----------------------------------------------------------------------//
    #endregion

    void InstantiatePlayerInScene(int playerID)
    {
        foreach (var player in FindObjectsOfType<PlayerManager>())
        {
            if (player.PlayerID == playerID)
            {
                Debug.LogWarning($"⚠️ Duplicate PlayerID {playerID} detected, destroying old instance");
                Destroy(player.gameObject);
            }
        }
        GameObject newPlayer = Instantiate(playerPrefabs[playerID], playerPrefabs[playerID].transform.localPosition, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(newPlayer,SceneManager.GetActiveScene());
        alivePlayersList.Add(newPlayer);
        Debug.Log("CreatePlayer"+playerID);
        EVENTS.InvokePlayerSpawn(playerID);
    }

    void DestroyAllPlayers()
    {
        for (int i=0; i<alivePlayersList.Count;i++) Destroy(alivePlayersList[i]);
        alivePlayersList = new List<GameObject>();
        alivePlayers = totalPlayers = 0;
    }



    // 🎮 CONTROLLER ASSIGNMENT -----------------------------------------------------------------------------------------------------------------------------------------
    Controller[] activeControllers = new Controller[4];
    bool leftKeyboardAssigned, rightKeyboardAssigned = false;

    void ResetAllControllers()
    {
        totalPlayers = 0;
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
        systemPlayer.controllers.AddController(ControllerType.Keyboard,0,false);
        leftKeyboardAssigned = rightKeyboardAssigned = false;
        //Debug.Log("Reset all controllers   SystemPlayer " + ReInput.players.GetSystemPlayer().controllers);
    }



    void ListenNewControllers()
    {
        if (ReInput.controllers.GetAnyButtonDown())
        {
            Controller lastActive = ReInput.controllers.GetLastActiveController();
            int newPlayer = FirstAvailablePlayer();
            if (newPlayer<0) return; // all players already have a controller, abort

            if (lastActive.type==ControllerType.Keyboard)
            {
                if (leftKeyboardAssigned==false && ReInput.controllers.Keyboard.GetKeyDown(KeyCode.Space)) AddPlayerKeyboard(newPlayer,2, lastActive);
                else if (rightKeyboardAssigned==false && ReInput.controllers.Keyboard.GetKeyDown(KeyCode.Return)) AddPlayerKeyboard(newPlayer,1, lastActive);
            }

            if (lastActive.type==ControllerType.Joystick && !activeControllers.Contains(lastActive))
            {
                AddPlayerController(newPlayer, lastActive);
            }
        }
    }


    void AddPlayerKeyboard(int playerID, int keyboardSide, Controller keyboard)
    {
        persistentPlayers.Add(new PlayerData
        {
            playerID = playerID,
            controller = keyboard,
            keyboardSide = keyboardSide
        });
        Debug.Log("ADD KEYBOARD PLAYER "+playerID+"   keyboardside "+keyboardSide);
        if (keyboardSide==2) leftKeyboardAssigned = true;
        if (keyboardSide==1) rightKeyboardAssigned = true;
        ReInput.players.GetPlayer(playerID).controllers.maps.SetMapsEnabled(true, ControllerType.Keyboard,0, keyboardSide);
        AddPlayerController(playerID, keyboard);
    }


    void AddPlayerController(int playerID, Controller controller)
    {
        InstantiatePlayerInScene(playerID);
        ResetTimer();
        AddNewPlayer(playerID, controller);
        ReInput.players.GetPlayer(playerID).isPlaying = true;
    }


    int FirstAvailablePlayer()
    {
        for (int i=0;i<4;i++)
        {
            if (activeControllers[i]==null) return i;
        }
        return -1;
    }


    void AddNewPlayer(int playerID, Controller controller)
    {
        totalPlayers++;
        if(!persistentPlayers.Exists(x=>x.playerID == playerID))
        {
            persistentPlayers.Add(new PlayerData { playerID = playerID, controller = controller });
        }
        AssignControllerToPlayer(playerID, controller);
    }


    void AssignControllerToPlayer(int playerID, Controller controller)
    {
        
        bool removeFromOtherPlayers = true;
        if (controller.type==ControllerType.Keyboard) removeFromOtherPlayers = false;
        Player thisPlayer = ReInput.players.GetPlayer(playerID);
        thisPlayer.controllers.ClearAllControllers();
        thisPlayer.controllers.AddController(controller, removeFromOtherPlayers);
        activeControllers[playerID] = controller;
        Debug.Log("🎮" + controller.name + " to Player" + playerID);
    }


    void CheckDisconnect(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("🎮 DISCONNECTED "+args.controller.name);
        if (CurrentState==GameplayState.off || CurrentState==GameplayState.joining) return;
        if (args.controller.type!=ControllerType.Joystick) return;
        if (activeControllers.Contains(args.controller))
        {
            int index = System.Array.IndexOf(activeControllers, args.controller);
            activeControllers[index] = null;
            Debug.Log("🎮 PLAYER"+index+" HAS NO CONTROLLER!");
            StartCoroutine(WaitControllerReconnect(index));
        }
    }

    bool allPlayersHaveController = true;

    IEnumerator WaitControllerReconnect(int playerID)
    {
        Debug.Log("🎮 WAITING FOR PLAYER"+playerID+" CONTROLLER TO CONNECT");
        while (activeControllers[playerID]==null)
        {
            if (ReInput.controllers.GetAnyButtonDown())
            {
                Controller lastActive = ReInput.controllers.GetLastActiveController();

                if (lastActive.type==ControllerType.Joystick && !activeControllers.Contains(lastActive))
                {
                    AssignControllerToPlayer(playerID, lastActive);
                }
            }
            yield return null;
        }
    }



} // FIN DU SCRIPT

