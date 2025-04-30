using Rewired;
using System.Collections;
using Unity.Jobs;
using UnityEngine;

public class BombManager : MonoBehaviour, ICollisionable, IExplodable
{
    private Rigidbody rb;
    public GameObject vfx;

    [SerializeField][Range(0, 6)] float m_delayBetweenExplose;
    [SerializeField][Range(0, 10)] float m_delayExplose;
    public float DelayExplose {
        get { return m_delayExplose; }
        set { m_delayExplose = value; }
    }

    [SerializeField] GameObject m_ExplosionPatern;
    [SerializeField] private int explosionRange = 1;

    private bool IsRed;
    private bool IsAdesFire;


    private float time;

    void Start()
    {
        m_delayExplose = 1;
        SetComponent();
        time = 0;
    }

    void SetComponent()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (IsRed == false)
        {
            time += Time.deltaTime;
            if (time >= m_delayBetweenExplose)
            {
                AudioSource sound = gameObject.GetComponent<AudioSource>();
                sound.Play();
                Explose();
                time = 0;
                Destroy(this.gameObject);
            }
        }

        if (IsAdesFire)
        {
            Explose();
        }
    }

    public void ChangeBombState(bool Active)
    {
        IsRed = Active;
    }

    public void ChangeBombStateToAdes(bool Active)
    {
        IsAdesFire = Active;
    }

    public void Explose()
    {
        if (!IsAdesFire)
        {

        }

        int foreachBoucle = 0;

        Vector3[] directions = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
        RaycastHit[] hits = new RaycastHit[4];

        foreach (var direction in directions)
        {
            for (int i = 1; i <= explosionRange; i++)
            {
                Vector3 newPosition = transform.position + direction * i;
                Physics.Raycast(transform.position, direction, out hits[foreachBoucle], explosionRange);
                Debug.DrawRay(transform.position, direction, Color.red, explosionRange);
            }
            foreachBoucle++;
        }

        foreach (var hit in hits)
        {
            for (int i = 1; i <= hit.distance + 0.5f; i++)
            {
                Vector3 direction = (hit.point - transform.position).normalized;
                Destroy(Instantiate(vfx, transform.position + (direction * i), Quaternion.identity), DelayExplose);
                hit.transform?.GetComponent<IExplodable>()?.Explode();
            }
        }
        foreachBoucle = 0;
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

    public void SetExplosionRange(int range)
    {
        explosionRange = range;
    }

    public void Explode()
    {
        Explose();
        time = 0;
    }
}
