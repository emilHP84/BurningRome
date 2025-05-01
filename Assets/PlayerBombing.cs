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
        if (canBomb && GAMEPLAY.access.PlayerControl)
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
        Vector3 bombPos = transform.position;
        bombPos.x = Mathf.Round(bombPos.x);
        bombPos.z = Mathf.Round(bombPos.z);
        bombPos.y = 2f;
        activeBombs.Add(Instantiate(bombPrefab,bombPos,transform.rotation));
        remainingBomb--;
        if (bombFx) Instantiate(bombFx,bombPos,transform.rotation);
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
