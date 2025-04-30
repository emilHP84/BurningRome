using UnityEngine;
using UnityEngine.SceneManagement;

public class GAMEPLAY : MonoBehaviour
{
    public GameplayState State;

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
        time = 0;
    }

    private void Update()
    {
        if (State is GameplayState.joining || State is GameplayState.gameplay)
        {
            IncreaseTimer();
        }
        SwitchGameplayState();
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
        playerNumber++;
        time = 0;
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
    private void SwitchGameplayState()
    {
        switch (State)
        {
            case GameplayState.off:
                if (SceneManager.GetActiveScene().buildIndex == 1) SwitchToJoining();
                    break;

            case GameplayState.joining:
                if (time >= timeToJoin && playerNumber >= 2)
                {
                    IncreaseTimer();
                    SwitchToGameplay();
                }
                break;

            case GameplayState.gameplay:
                if (time >= timeToSuddenDeath)
                {
                    IncreaseTimer();
                    SwitchToSuddenDeath();
                }
                break;

            case GameplayState.suddenDeath:
                EVENTS.InvokeOnSuddenDeath(this, new System.EventArgs());
                if (playerNumber == 0 || playerNumber == 1)
                {
                    SwitchToEnd();
                }
                break;

            case GameplayState.end:
                SceneLoader.access.LoadScene(0, 1, 0.25f, 1, false, 0.5f);
                SwitchToOff();
                break;
        }
    }

    public void SwitchToOff()
    {
         State = GameplayState.off;
    }

    public void SwitchToJoining()
    {
        ResetTimer();
        State = GameplayState.joining;
    }

    public void SwitchToGameplay()
    {
        ResetTimer();
        State = GameplayState.gameplay;
    }

    public void SwitchToSuddenDeath()
    {
        State = GameplayState.suddenDeath;
    }

    public void SwitchToEnd()
    {
        State = GameplayState.end;
    }

    //-----------------------------------------------------------------------//
    #endregion

    private void OnDisable()
    {
        EVENTS.OnDeathEventHandler -= RemovePlayerNumber;
        EVENTS.OnPlayerConnectEventHandler -= AddPlayerNumber;
    }
}

