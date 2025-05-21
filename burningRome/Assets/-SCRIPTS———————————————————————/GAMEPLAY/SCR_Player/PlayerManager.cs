using DG.Tweening;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IDetect, ICollisionable, IExplodable
{
    [Header("VFX")]
    [SerializeField] GameObject FeedBack;
    [SerializeField] GameObject Fx_DeathPlayer;
    [SerializeField] Material OnInvincibility;

    [Header("GAME SYSTEM")]
    [SerializeField] private int playerID;
    public Collider PlayerCollider;

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
        FeedBack.SetActive(false);
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
        transform.DOScale(new Vector3(0, 0, 0), deathTime);
        isAlive = false;
        yield return new WaitForSeconds(1);
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
            FeedBack?.SetActive(true);
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Le joueur est entré dans : " + other.name);
        if (invincible)
        {
            if (other.gameObject.GetComponent<Obstacle>())
            {
                other.gameObject.GetComponent<Obstacle>().Explode();
            }
        }
    }

} // FIN DU SCRIPT
