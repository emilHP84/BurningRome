using UnityEngine;

public class GAME: MonoBehaviour
{

// PUBLIC METHODS --------------------------------------------------------------------------------------------------

    public void SwitchTo(State desiredState) // <-- CALL THIS TO CHANGE THE GAME STATE (with GAME.MANAGER.SwitchTo(desired))
    {
        SwitchTo(desiredState, -1f);
    }

    public void SwitchTo(State desiredState, float waitingTime) // <-- You can specify a waiting duration before entering the new state
    {
        nextState = desiredState;

        if (waitingTime<0)
        {
            waitDuration = 0f;
            waitTimer = 1f;
            EnterState(desiredState);
        }
        else
        {
            waitDuration = waitingTime;
            waitTimer = 0;
            EnterState(State.waiting);
        }
    }

    public void Pause() // <-- Call this to pause the game
    {
        if (CurrentState==State.gameplay) pauseScript.Pause();
    }

    public void Resume() // <-- Call this to resume the game from pause
    {
        if (CurrentState==State.paused) pauseScript.Resume();
    }


// -----------------------------------------------------------------------------------------------------------------


    // THIS IS THE STATE MACHINE THAT ONLY HANDLES THE CURRENT GAME STATE
    public static GAME MANAGER; // call from anywhere using: GAME.MANAGER
    public State CurrentState{get{return gameState;}}
    [SerializeField]State gameState;
    float waitDuration, waitTimer = 0;
    State nextState;
    public Transform gameCam,menuCam;
    PauseScript pauseScript =>GetComponentInChildren<PauseScript>();

    [RuntimeInitializeOnLoadMethod]
    static void Singleton() // THIS METHOD INSTANTIATES THE GAME MANAGER PREFAB (IN RESOURCES FOLDER) WHEN THE GAME RUNS
    {
        if (MANAGER!=null) return;
        string gmName = "👑 — GAME MANAGER ——————————————————————————————————";
        GameObject gm = Instantiate(Resources.Load(gmName, typeof(GameObject))) as GameObject;
        gm.name = gmName;
        DontDestroyOnLoad(gm);
        MANAGER = gm.GetComponent<GAME>();
        Debug.Log(MANAGER);
    }

    void OnEnable()
    {
        DestroyIfDuplicate(); // Security to prevent multiple game managers
    }

    void DestroyIfDuplicate()
    {
        if(MANAGER!=null && MANAGER!=this)
        {
            Debug.Log("⚠️ ERROR! MORE THAN ONE GAME MANAGER IN SCENE!");
            DestroyImmediate(gameObject);
        }
    }

    void Start()
    {
        SwitchTo(State.waiting);
        EVENTS.InvokeInitialization();
    }

    void Update()
    {
        UpdateCurrentState();
    }





// GAME STATE MACHINE --------------------------------------------------------------------------
    
    void EnterState(State desiredState) // <-- Start Function of the CurrentState
    {
        if (desiredState==gameState) // Can't enter the same state. Rebooting the state.
        {
            RebootState();
            return;
        }

        ExitCurrentState(); // Exit previous state
        gameState = desiredState; // Applying new state
  
        switch (gameState) // Notify everyone of the new state
        {
            case State.menu: EVENTS.InvokeMenu(); break;
            case State.gameplay: EVENTS.InvokeGameplay(); break;
            case State.paused: EVENTS.InvokeGamePause(); break;
            case State.waiting: EVENTS.InvokeWaiting(); break;
        }
    }

    void UpdateCurrentState() // <-- Update Function of the CurrentState
    {
        switch (gameState)
        {
            case State.menu: UpdateInMenu(); break;
            case State.gameplay: UpdateInGameplay(); break;
            case State.paused: UpdateInPause(); break;
            case State.waiting: UpdateInWaiting(); break;
        }
    }

    void ExitCurrentState() // <-- Exit Function of the CurrentState
    {
        switch (gameState) // Notify everyone we are leaving the current state
        {
            case State.menu: EVENTS.InvokeMenuExit(); break;
            case State.gameplay: EVENTS.InvokeGameplayExit(); break;
            case State.paused: EVENTS.InvokeGamePauseExit();break;
            case State.waiting: EVENTS.InvokeWaitingExit(); break;
        }
    }

    void RebootState() // <-- Specific Start Function for entering the state we already are in.
    {
        switch (gameState) // Notify everyone we are entering the same state again.
        {
            case State.menu: EVENTS.InvokeMenuReboot(); break;
            case State.gameplay: EVENTS.InvokeGameplayReboot(); break;
            case State.paused: EVENTS.InvokeGamePauseReboot(); break;
            case State.waiting: EVENTS.InvokeWaitingReboot(); break;
        }
    }


// STATE MACHINE END ---------------------------------------------------------------------------




    void UpdateInMenu()
    {

    }

    void UpdateInGameplay()
    {

    }

    void UpdateInPause()
    {

    }

    void UpdateInWaiting()
    {
        waitTimer += Time.unscaledDeltaTime; // Optional timer in wait state
        if (waitTimer>waitDuration && nextState!=State.waiting) EnterState(nextState);
    }


} // SCRIPT END

