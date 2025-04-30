using UnityEngine;
using Rewired;
using System.Collections.Generic;

public class PlayerBombing : MonoBehaviour
{
    [SerializeField] int playerID = 0;
    Player player;
    bool canBomb = false;
    [SerializeField] int maxBomb = 1;
    int remainingBomb = 1;

    [SerializeField] GameObject bombPrefab;
    List<GameObject> activeBombs = new List<GameObject>();
    [SerializeField] GameObject bombFx, noBombFx;

    void Update()
    {
        if (canBomb)
        {
            if (player.GetButtonDown("Bomb"))
            {
                if (remainingBomb>0)
                {
                    NewBomb();
                }
                else
                {
                    if (bombFx) Instantiate(noBombFx,transform.position,transform.rotation);
                }
            }
        }

        for (int i =0; i<activeBombs.Count; i++)
        {
            if(activeBombs[i]==null)
            {
                remainingBomb++;
                activeBombs.RemoveAt(i);
            }
        }


    }

    void NewBomb()
    {
        if (bombPrefab==null) return;
        activeBombs.Add(Instantiate(bombPrefab,transform.position+Vector3.up,transform.rotation));
        remainingBomb--;
        if (bombFx) Instantiate(bombFx,transform.position,transform.rotation);
    }

    void OnEnable()
    {
        player = ReInput.players.GetPlayer(playerID);
        EVENTS.OnGameplay += Activate;
        EVENTS.OnGameplayExit += Disable;
        if (GAME.MANAGER.CurrentState==State.gameplay) Activate();
    }

    void OnDisable()
    {
        EVENTS.OnGameplay -= Activate;
        EVENTS.OnGameplayExit -= Disable;
    }

    void Activate()
    {
        canBomb = true;
        ResetBombs();
    }

    void Disable()
    {
        canBomb = false;
        ResetBombs();
    }

    void ResetBombs()
    {
        activeBombs = new List<GameObject>();
        remainingBomb = maxBomb;
    }

    public void ChangeMaxBomb(int desired)
    {
        if (desired<0) desired = 0;
        maxBomb = desired;
        if (remainingBomb>maxBomb) remainingBomb=maxBomb;
        if (activeBombs.Count>maxBomb) activeBombs = new List<GameObject>();
    }

} // FIN DU SCRIPT
