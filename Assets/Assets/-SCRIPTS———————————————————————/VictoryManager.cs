using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    private CustomPlayerJoin m_customPlayerJoin;
    [SerializeField] private List<GameObject> PlayerAlive = new List<GameObject>();

    float PlayerNumber = 4; // temporaire le 4

    private void OnEnable()
    {
        EVENTS.OnDeathEventHandler += OnDeath;
        EVENTS.OnPlayerConnectEventHandler += SetNewNumberOfPlayer;
    }

    private void Awake()
    {

    }

    private void Start()
    {
        //Emilien: à réplacer plus tard
        // PlayerNumber = 0;

        m_customPlayerJoin = GetComponent<CustomPlayerJoin>();
        SetPlayerAlive();
    }

    private void Update()
    {

    }

    //-ajout des joueur présent dans le la partit----------------------------//
    void SetNewNumberOfPlayer(object invoker, int playerIndex)
    {
        PlayerNumber++;
        m_customPlayerJoin.playerPrefabs[playerIndex].gameObject.SetActive(true);
    }

    //-----------------------------------------------------------------------//


    //-ajout des joueur présent dans le CustomPlayerJoin---------------------//
    public void SetPlayerAlive()
    {
        for (int i = 0; i < 4; i++)
        {
            AddNewPlayer(i);
        }
    }

    public void AddNewPlayer(int playerNumber)
    {
        PlayerAlive.Add(m_customPlayerJoin.playerPrefabs[playerNumber]);
    }

    //-----------------------------------------------------------------------//





    //-retier le joueur de la liste des joueur encore vivant-----------------//
    void OnDeath(object invoker, int e)
    {
        SetLooser(e);
        CheckVictory();
    }

    public void SetLooser(int playerID)
    {
        Debug.Log("playezrID" + playerID);

        PlayerAlive[playerID - 1].gameObject.SetActive(false);
        PlayerAlive[playerID - 1] = null;


        //if (PlayerAlive[playerID - 1].gameObject.GetComponent<PlayerManager>().Hited == false && playerID == PlayerAlive[playerID - 1].gameObject.GetComponent<PlayerManager>().PlayerID)
        //{

        //}
        //else return;
    }
    //-----------------------------------------------------------------------//





    //-vérifie si il reste un joueur, si oui lancer Victoire()---------------//
    public void CheckVictory()
    {
        for(int i = 0; i < 3 ; i++) 
        {
            if (PlayerAlive[i] == null)
            {
                PlayerNumber--;
            }
        }

        if (PlayerNumber == 1)
        {
            SetVictory();
        }
        else return;
    }

    public void SetVictory()
    {
        SceneLoader.access.LoadScene(3);
    }
    //-----------------------------------------------------------------------//





    private void OnDisable()
    {
        EVENTS.OnDeathEventHandler -= OnDeath;
        EVENTS.OnPlayerConnectEventHandler -= SetNewNumberOfPlayer;
    }
}
