using UnityEngine;

public class BombManager : MonoBehaviour, ICollisionable, IExplodable
{
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

    public void Explode()
    {
        Explosion();
    }

    [SerializeField]bool ManualDetonation = false;
    [SerializeField]bool isPiercing = false;
    [SerializeField] GameObject fx_bombExplosion;
    [SerializeField] int explosionRange = 1;
    [SerializeField][Range(0, 10)] float delayBeforeExplosion = 3f;

    bool exploded = false;
    Rigidbody rb => GetComponent<Rigidbody>();
    PlayerBombing owner;
    float time;
    float flameDuration = 1f;

    void Start()
    {
        time = delayBeforeExplosion;
    }

    void Update()
    {
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
        int row = Mathf.RoundToInt(transform.position.x);
        int column = Mathf.RoundToInt(transform.position.z);
        GameGrid.access.BurnCell(column,row,flameDuration,explosionRange,Cardinal.North, isPiercing);
        GameGrid.access.BurnCell(column,row,flameDuration,explosionRange,Cardinal.South, isPiercing);
        GameGrid.access.BurnCell(column,row,flameDuration,explosionRange,Cardinal.East, isPiercing);
        GameGrid.access.BurnCell(column,row,flameDuration,explosionRange,Cardinal.West, isPiercing);
        if (fx_bombExplosion) Instantiate(fx_bombExplosion,transform.position,transform.rotation);
        if (owner) owner.BombExploded(this);
        Destroy(this.gameObject);
    }



    public void OnCollisionWith(ICollisionable collisionable)
    {
        if (collisionable is PlayerManager)
        {
            rb.isKinematic = false;
        }
        else if (collisionable is Ground)
        {
            rb.isKinematic = true;
        }
        if (collisionable is PlayerManager && collisionable is Ground)
        {
            rb.isKinematic = true;
        }
    }

} // SCRIPT END
