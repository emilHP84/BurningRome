using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDetect, ICollisionable, IExplodable
{
    [SerializeField] GameObject Fx_DeathPlayer,fxspawn;
    [Header("GAME SYSTEM")]
    [SerializeField] private int playerID;
    public Collider PlayerCollider;
    PlayerAnim anim => GetComponent<PlayerAnim>();
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

    private void Awake()
    {
        if(fxspawn)Instantiate(fxspawn,transform.position,Quaternion.identity);
    }

    private void OnEnable()
    {
        isAlive = true;
        invincible = false;
        invincibilityTime = 0;
        EVENTS.OnVictory += OnVictory;
    }


    IEnumerator OnDeath(float deathTime)
    {
        if (isAlive == false) yield break;
        //transform.DOScale(new Vector3(0, 0, 0), deathTime);
        isAlive = false;
        anim.PlayDeath();
        yield return new WaitForSeconds(2);
        EVENTS.InvokePlayerDeath(playerID);
        gameObject.SetActive(false);
    }

    void OnVictory(EventArgs e) 
    {
        EVENTS.InvokeOnCallCamera(this.gameObject);
    }

    private void OnDisable()
    {
        isAlive = false;
        EVENTS.OnVictory -= OnVictory;

    }

    public void OnDetectionWith(IDetect detect)
    {
        //if (invincible == false)
        //{
        //    StartCoroutine(OnDeath(deathTime));
        //}
    }

    public void OnCollisionWith(ICollisionable collisionable)
    {

    }

    public void Explode()
    {
        if (!invincible && isAlive)
        {
            Movement.DeathPlaying();
            anim.PlayDeath();
            StartCoroutine(OnDeath(deathTime));
            Instantiate(Fx_DeathPlayer,transform.position,Quaternion.identity);
            PlayerCollider.enabled = false;
        }
    }

    public void InvincibilityFor(float duration)
    {
        Debug.Log("Invincibility Récupérer");
        invincibilityTime = duration;
        StartCoroutine(WaitForInvincibilityEnd());
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
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (invincible)
        {
            if (collision.gameObject.GetComponent<Obstacle>())
            {
                Destroy(collision.gameObject);
            }
        }
    }

} // FIN DU SCRIPT
