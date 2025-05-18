using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BombManager : MonoBehaviour, ICollisionable, IExplodable
{
    [SerializeField] GameObject FeedBack;
    public void Awake()
    {
        FeedBack.SetActive(false);
        GameObject vfx = Instantiate(fx_BombPlaced,transform.position, Quaternion.identity);
        //vfx.transform.SetParent(transform);
    }
    public void SetDelay(float newDelay)
    {
        time = delayBeforeExplosion = newDelay;
    }

    public void SetExplosionRange(int range)
    {
        explosionRange = range;
    }

    public void SetBombOwner(PlayerBombing player)
    {
        owner = player;
    }

    public void SetManualDetonation(bool Active)
    {
        ManualDetonation = Active;
    }

    public void SetPiercing(bool wanted)
    {
        isPiercing = wanted;
    }

    public void SetFlameDuration(float newDuration)
    {
        flameDuration = newDuration;
    }

    public void SetIsHadesFire(bool nextBombIsHadesFire)
    {
        isHadesFire = nextBombIsHadesFire;
    }

    public void Explode()
    {
        Explosion();
    }
    
    [SerializeField]bool ManualDetonation = false;
    [SerializeField]bool isPiercing = false;
    [SerializeField] GameObject fx_BombPlaced;
    [SerializeField] int explosionRange = 2;
    [SerializeField][Range(0, 10)] float delayBeforeExplosion = 3f;

    bool exploded = false;
    Rigidbody rb => GetComponent<Rigidbody>();
    BoxCollider boxCollider => GetComponent<BoxCollider>();

    PlayerBombing owner;
    float time;
    float flameDuration = 1f;
    private bool isHadesFire;

    void Start()
    {
        time = delayBeforeExplosion;
    }

    void Update()
    {
        //Debug.Log(time);

        if (GAME.MANAGER.CurrentState != State.gameplay) return;
        if (ManualDetonation == false)
        {
            time -= Time.deltaTime;
        }
        if (time < 0) Explosion();
    }


    void Explosion()
    {
        if (exploded) return;
        exploded = true;
        int row = Mathf.RoundToInt(transform.position.z);
        int column = Mathf.RoundToInt(transform.position.x);
        //Debug.Log("Explosion de la case " + column + "," + row);
        GameGrid.access.BurnCell(column,row,flameDuration,explosionRange,Cardinal.North, isPiercing, isHadesFire);
        GameGrid.access.BurnCell(column,row,flameDuration,explosionRange,Cardinal.South, isPiercing, isHadesFire);
        GameGrid.access.BurnCell(column,row,flameDuration,explosionRange,Cardinal.East, isPiercing, isHadesFire);
        GameGrid.access.BurnCell(column,row,flameDuration,explosionRange,Cardinal.West, isPiercing, isHadesFire);
        if (owner) owner.BombExploded(this);
        FeedBack.SetActive(true);
        Destroy(this.gameObject);
    }



    public void OnCollisionWith(ICollisionable collisionable)
    {
        if (collisionable is PlayerManager)
        {
            Debug.Log("TU FAIT CHIER");
            boxCollider.isTrigger = true;
        }

        else if (collisionable is Ground )
        {
            Debug.Log("TU FAIT CHIER PTN");
            boxCollider.isTrigger = false;
        }
    }


} // SCRIPT END
