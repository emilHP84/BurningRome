using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    private CustomPlayerJoin m_customPlayerJoin;
    [SerializeField] private List<GameObject> PlayerAlive = new List<GameObject>();

    private void OnEnable()
    {
        EVENTS.OnDeathEventHandler += OnDeath;
    }

    private void Awake()
    {
        
    }

    private void Start()
    {
        m_customPlayerJoin = GetComponent<CustomPlayerJoin>();
        SetPlayerAlive();
    }

    private void Update()
    {

    }





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
        if (PlayerAlive[playerID].gameObject.GetComponent<PlayerManager>().Hited == false &&
            playerID == PlayerAlive[playerID-1].gameObject.GetComponent<PlayerManager>().PlayerID)
        {
            PlayerAlive.Remove(PlayerAlive[playerID-1]);
        }
    }
    //-----------------------------------------------------------------------//





    //-vérifie si il reste un joueur, si oui lancer Victoire()---------------//
    public void CheckVictory()
    {
        if(PlayerAlive.Count == 1) 
        { 
            SetVictory();
        }
    }

    public void SetVictory()
    {
        SceneManager.LoadScene("VictoryScene");
    }
    //-----------------------------------------------------------------------//





    private void OnDisable()
    {
        
    }
}
