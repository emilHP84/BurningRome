using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDetect, ICollisionable, IExplodable
{
    [SerializeField] GameObject Fx_DeathPlayer,fxspawn,fx_BlessByGod;
    [Header("GAME SYSTEM")]
    [SerializeField] private int playerID;
    public Collider PlayerCollider;
    PlayerMovement Movement => GetComponent<PlayerMovement>();


    public int PlayerID
    {
        get { return playerID; }
        private set { playerID = value; }
    }


    [Header("DEATH")]
    [SerializeField] private bool isAlive;
    public bool IsAlive
    {
        get { return isAlive; }
        private set { isAlive = value; }
    }
    [SerializeField] private float deathTime;
    public bool Invincible { get { return invincible; } }
    bool invincible = false;



    private void OnEnable()
    {
        EVENTS.OnVictory += OnVictory;
        Spawn();
    }

    private void OnDisable()
    {
        EVENTS.OnVictory -= OnVictory;
    }

    void Spawn()
    {
        isAlive = true;
        invincible = false;
        invincibilityTime = 0;
        PlayerCollider.enabled = true;
        if(fxspawn)Instantiate(fxspawn,transform.position,Quaternion.identity);
    }



    IEnumerator DeathSequenceRoutine(float deathTime)
    {
        if (isAlive == false) yield break;
        isAlive = false;
        Movement.DeathPlaying();
        EVENTS.InvokePlayerDeath(PlayerID);
        Instantiate(Fx_DeathPlayer,transform.position,Quaternion.identity);
        PlayerCollider.enabled = false;
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

    void OnVictory(int winnerID)
    {
        if (PlayerID == winnerID)
        {
            if (victoryRoutine != null) StopCoroutine(victoryRoutine);
            victoryRoutine = StartCoroutine(WaitThenCallCameraZoom());
        }
    }

    Coroutine victoryRoutine;

    IEnumerator WaitThenCallCameraZoom()
    {
        yield return new WaitForSeconds(3f);
        EVENTS.InvokeOnCallCamera(this.gameObject);
    }



    public void OnCollisionWith(ICollisionable collisionable)
    {

    }

    public void Explode()
    {
        if (!invincible && isAlive && GAME.MANAGER.CurrentState==State.gameplay)
        {
            StartCoroutine(DeathSequenceRoutine(deathTime));
        }
    }

    public void InvincibilityFor(float duration)
    {
        Debug.Log("Invincibility");
        invincibilityTime = duration;
        StartCoroutine(WaitForInvincibilityEnd());
        fx_BlessByGod.SetActive(true);
    }

    float invincibilityTime = 0;

    IEnumerator WaitForInvincibilityEnd()
    {
        invincible = true;
        while (invincibilityTime > 0)
        {
            if (GAME.MANAGER.CurrentState == State.gameplay) invincibilityTime -= Time.deltaTime;
            yield return null;
        }
        invincible = false;
        fx_BlessByGod.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (invincible)
        {
            Obstacle obstacle = collision.GetComponentInParent<Obstacle>();
            if(obstacle!=null) obstacle.ExplodeWithoutPU();
        }
    }


    public void OnDetectionWith(IDetect detect)
    {
        throw new NotImplementedException();
    }
}// FIN DU SCRIPT
