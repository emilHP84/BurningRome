using UnityEngine;
using Rewired;
using System.Collections.Generic;

public class PlayerBombing : MonoBehaviour
{
    [SerializeField] int playerID = 0;
    Player player;
    bool canBomb = false;
    public int MaxBomb{get{return maxBomb;}}
    [SerializeField] int maxBomb = 1;
    int remainingBomb = 1;

    [SerializeField] GameObject bombPrefab;
    List<BombManager> activeBombs = new List<BombManager>();
    [SerializeField] GameObject bombFx, noBombFx;
    [SerializeField] private int explosionRange = 1;
    public int ExplosionRange => explosionRange;
    int manualDetonation = 0;
    BombManager manualBomb;

    float delay = 1;

    void Update()
    {
        if (canBomb && GAMEPLAY.access.PlayerControl)
        {
            if (player.GetButtonDown("Bomb"))
            {
                if (remainingBomb>0)
                {
                    if (manualDetonation<1) NewBomb();
                    else
                    {
                        if(manualDetonation<2)
                        {
                            if (manualBomb!=null) manualBomb.Explode();
                            else NewBomb();
                            manualDetonation = 0;
                        }
                        else
                        {
                            NewBomb();
                            manualBomb = activeBombs[activeBombs.Count-1];
                            manualBomb.ChangeBombState(true);
                            manualDetonation = 1;
                        }
                    }
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
                if(remainingBomb>maxBomb) remainingBomb = maxBomb;
                activeBombs.RemoveAt(i);
            }
        }
    }

    void NewBomb()
    {
        bombPrefab.GetComponent<BombManager>().DelayExplose = delay;
        if (bombPrefab==null) return;
        Vector3 bombPos = transform.position;
        bombPos.x = Mathf.Round(bombPos.x);
        bombPos.z = Mathf.Round(bombPos.z);
        bombPos.y = 2f;
        BombManager newBomb = Instantiate(bombPrefab,bombPos,transform.rotation).GetComponent<BombManager>();
        activeBombs.Add(newBomb);
        newBomb.GetComponent<BombManager>().SetExplosionRange(explosionRange);
        remainingBomb--;
        if (bombFx) Instantiate(bombFx,bombPos,transform.rotation);
        delay = 1;
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
        activeBombs = new List<BombManager>();
        remainingBomb = maxBomb;
    }

    public void ChangeMaxBomb(int desired)
    {
        if (desired<1) desired = 1;
        maxBomb = desired;
        remainingBomb = desired;
        if (remainingBomb>maxBomb) remainingBomb=maxBomb;
        //if (activeBombs.Count>maxBomb) activeBombs = new List<BombManager>();
    }

    public void ChangeRange(int desired)
    {
        explosionRange += Mathf.Clamp(desired,1,99);
    }

    public void AddManualDetonation()
    {
        manualDetonation=2;
    }

    public void Switchdelay()
    {
        delay = 5;
    }

    public void BombPercing()
    {
        bombPrefab.GetComponent<BombManager>().IsPercing = true;
    }

} // FIN DU SCRIPT
