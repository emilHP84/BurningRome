using UnityEngine;

public class GAMEPLAY : MonoBehaviour
{
    public GameplayState CurrentState;

    public float timeToJoin = 10;
    public float timeToSuddenDeath = 60;

    [SerializeField] private int playerNumber;

    [SerializeField]private float time;


    private void OnEnable()
    {
        EVENTS.OnDeathEventHandler += RemovePlayerNumber;
        EVENTS.OnPlayerConnectEventHandler += AddPlayerNumber;
    }

    private void Start()
    {
        EnterState(GameplayState.off);
    }

    private void Update()
    {
        IncreaseTimer();
        UpdateCurrentState();
    }

    #region PLAYER NUMBER
    //-----------------------------------------------------------------------//
    //-PLAYER-NUMBER---------------------------------------------------------//
    //-----------------------------------------------------------------------//

    private void RemovePlayerNumber(object invoker, int e)
    {
        playerNumber--;
    }

    private void AddPlayerNumber(object invoker, int e)
    {
        ResetTimer();
        playerNumber++;
    }

    //-----------------------------------------------------------------------//
    #endregion

    #region TIME
    //-----------------------------------------------------------------------//
    //-TIME-SYSTEM-----------------------------------------------------------//
    //-----------------------------------------------------------------------//

    private void IncreaseTimer()
    {
        time += Time.deltaTime;
    }

    private void ResetTimer()
    {
        time = 0;
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
                GAME.MANAGER.SwitchTo(State.menu);
            break;

            case GameplayState.joining:
                GAME.MANAGER.SwitchTo(State.gameplay);
            break;

            case GameplayState.gameplay:
                GAME.MANAGER.SwitchTo(State.gameplay);
            break;

            case GameplayState.suddenDeath:
                EVENTS.InvokeOnSuddenDeath(this, new System.EventArgs());
            break;

            case GameplayState.end:
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
                if (time >= timeToJoin && playerNumber > 1) EnterState(GameplayState.gameplay);
            break;

            case GameplayState.gameplay:
                if (time >= timeToSuddenDeath) EnterState(GameplayState.suddenDeath);
                if (playerNumber<2) EnterState(GameplayState.end);
            break;

            case GameplayState.suddenDeath:
                if (playerNumber<2) EnterState(GameplayState.end);
            break;

            case GameplayState.end:
                if (time>3f)
                {
                    SceneLoader.access.LoadScene(0, 1, 0.25f, 1, false, 0.5f);
                    GAME.MANAGER.SwitchTo(State.menu);
                    EnterState(GameplayState.off);
                }
            break;
        }
    }

    public void WaitPlayersToJoin()
    {
        EnterState(GameplayState.joining);
    }

    public void Rematch()
    {
        EnterState(GameplayState.gameplay);
    }




    //-----------------------------------------------------------------------//
    #endregion

    private void OnDisable()
    {
        EVENTS.OnDeathEventHandler -= RemovePlayerNumber;
        EVENTS.OnPlayerConnectEventHandler -= AddPlayerNumber;
    }
} // FIN DU SCRIPT

