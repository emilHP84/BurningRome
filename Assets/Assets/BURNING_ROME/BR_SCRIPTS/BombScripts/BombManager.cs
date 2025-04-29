using Rewired;
using System.Collections;
using Unity.Jobs;
using UnityEngine;

public class BombManager : MonoBehaviour, ICollisionable, IExplodable
{
    private Rigidbody rb;

    [SerializeField][Range(0, 6)] float m_delayBetweenExplose;
    [SerializeField][Range(0, 10)] float m_delayExplose;
    public float DelayExplose
    {
        get { return m_delayExplose; }
        set { m_delayExplose = value; }
    }
    [SerializeField] GameObject m_ExplosionPatern;
    [SerializeField] private int explosionRange = 1;

    public GameObject FlammePrefab;

    private LayerMask ignoreLayer = 3;

    private bool IsRed;
    private bool IsAdesFire;
    private bool IsPercing;

    private float time;

    void Start()
    {
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
                //AudioSource sound = gameObject.GetComponent<AudioSource>();
                //sound.Play();
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

    public void ChangeBombStateToPercing(bool Active)
    {
        IsAdesFire = Active;
    }

    public void Explose()
    {
        if (!IsAdesFire)
        {

        }

        //Emilien: a dégager ( code ci dessous non fonctionnelle ) 
        /*
        // Récupérer les cubes
        Transform up = m_ExplosionPatern.transform.Find("Up");
        Transform down = m_ExplosionPatern.transform.Find("Down");
        Transform left = m_ExplosionPatern.transform.Find("Left");
        Transform right = m_ExplosionPatern.transform.Find("Right");

        // Les déplacer selon la portée
        up.localPosition = Vector3.forward / explosionRange;
        down.localPosition = Vector3.back * explosionRange;
        left.localPosition = Vector3.left * explosionRange;
        right.localPosition = Vector3.right * explosionRange;

        // Activer l'explosion
        m_ExplosionPatern.SetActive(true);
        */

        RaycastHit[] hits = new RaycastHit[4];
        Physics.Raycast(transform.position, Vector3.forward, out hits[0], explosionRange, ignoreLayer);
        Physics.Raycast(transform.position, Vector3.right, out hits[1], explosionRange, ignoreLayer);
        Physics.Raycast(transform.position, Vector3.back, out hits[2], explosionRange, ignoreLayer);
        Physics.Raycast(transform.position, Vector3.left, out hits[3], explosionRange, ignoreLayer);

        foreach (var hit in hits)
        {
            for (int i = 1; i <= hit.distance + 0.5f; i++) 
            {
                Vector3 direction = (hit.point - transform.position).normalized;
                Destroy(Instantiate(FlammePrefab, transform.position + (direction * i), Quaternion.identity),DelayExplose);

            }
        }
    }

    // detecte les différent objet en collision ( nécésite un rigidbody )
    public void OnCollisionWith(ICollisionable collisionable)
    {
        if (collisionable is PlayerManager)
        {
            rb.isKinematic = false;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Bomb"), true);
        }
        else if (collisionable is Ground)
        {
            rb.isKinematic = true;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Bomb"), false);
        }
        if (collisionable is PlayerManager && collisionable is Ground)
        {
            rb.isKinematic = true;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Bomb"), false);
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
