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
    int RemainingBombs{ get{return remainingBombs;} set{remainingBombs = Mathf.Clamp(value,0,maxBomb);} }
    int remainingBombs = 1;

    [SerializeField] GameObject bombPrefab;
    List<BombManager> activeBombs = new List<BombManager>();
    [SerializeField] GameObject bombFx, noBombFx;
    [SerializeField] private int explosionRange = 1;
    public int ExplosionRange => explosionRange;
    int manualDetonation = 0;
    BombManager manualBomb;
    bool nextBombIsHadesFire = false;

    float bombExplodeDelay = 1;

    void Update()
    {
        if (canBomb && GAMEPLAY.access.PlayerControl)
        {
            if (player.GetButtonDown("Bomb"))
            {
                if (RemainingBombs>0)
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
                            manualBomb.SetManualDetonation(true);
                            manualDetonation = 1;
                        }
                    }
                }
                else
                {
                    if (noBombFx)
                    {
                        Instantiate(noBombFx, transform.position, transform.rotation); 
                    }
                }
            }
        }

        for (int i =0; i<activeBombs.Count; i++)
        {
            if(activeBombs[i]==null)
            {
                RemainingBombs++;
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
        BombManager newBomb = Instantiate(bombPrefab,bombPos,transform.rotation).GetComponent<BombManager>();
        activeBombs.Add(newBomb);
        newBomb.SetDelay(bombExplodeDelay);
        newBomb.SetExplosionRange(explosionRange);
        newBomb.SetBombOwner(this);
        if (nextBombIsHadesFire)
        {
            newBomb.SetFlameDuration(5f);
            nextBombIsHadesFire = false;
        }
        RemainingBombs--;
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
        activeBombs = new List<BombManager>();
        RemainingBombs = maxBomb;
    }

    public void BombExploded(BombManager bomb)
    {
        if (activeBombs.Contains(bomb))
        {
            RemainingBombs++;
            activeBombs.Remove(bomb);
        }
    }

    public void ChangeMaxBomb(int desired)
    {
        if (desired<1) desired = 1;
        int difference = desired-maxBomb;
        maxBomb = desired;
        RemainingBombs += difference;
    }

    public void ChangeRange(int desired)
    {
        explosionRange += Mathf.Clamp(desired,1,99);
    }

    public void AddManualDetonation()
    {
        manualDetonation=2;
    }

    public void Switchdelay(float newBombDelay)
    {
        bombExplodeDelay = newBombDelay;
    }

    public void BombPercing()
    {
        bombPrefab.GetComponent<BombManager>().SetPiercing(true);
    }

    public void NextBombIsHadesFire()
    {
        nextBombIsHadesFire = true;
    }

} // FIN DU SCRIPT
