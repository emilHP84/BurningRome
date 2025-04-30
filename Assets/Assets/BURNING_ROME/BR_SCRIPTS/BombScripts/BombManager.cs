using Rewired;
using System.Collections;
using Unity.Jobs;
using UnityEngine;

public class BombManager : MonoBehaviour, ICollisionable, IExplodable
{
    private Rigidbody rb;
    private SphereCollider sphereCollider;
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
    private Vector3 InitialPosition;


    private float time;
    private bool HasExplose;

    void Start()
    {
        InitialPosition = transform.position;
        SetComponent();
        time = 0;
        DelayExplose = 1;
    }

    void SetComponent()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = rb.GetComponent<SphereCollider>();
    }

    void Update()
    {
        if (IsRed == false)
        {
            time += Time.deltaTime;
            if (HasExplose == false && time >= m_delayBetweenExplose)
            {
                if (HasExplose) return;              
                Explose();
                HasExplose = true;
                time = 0;
            }
            else if (HasExplose && time >= m_delayExplose)
            {
                // Attendre la fin de l'explosion             
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
            if (HasExplose) return;
            HasExplose = true;
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
        Vector3[] directions = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };
        foreach (var direction in directions)
        {
            // 3. Pour chaque unité de distance (1, 2, 3... jusqu'à explosionRange)
            for (int i = 1; i <= explosionRange; i++)
            {
                // 4. Calculer la nouvelle position
                Vector3 newPosition = transform.position + direction * i;
                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, explosionRange))
                {
                    // Quelque chose est touché
                    // Si c'est un mur
                    if(hit.transform.tag == "Mur")
                    {
                        break;
                    }
                    // Si c'est destructible
                    hit.transform?.GetComponent<IExplodable>()?.Explode();
                    Instantiate(vfx, hit.transform.position, Quaternion.Euler(-90, 0, 0));

                    break; // On arrête la propagation dans cette direction
                }
                else
                {
                    // Rien touché, donc libre : on instancie le VFX
                    Instantiate(vfx, newPosition, Quaternion.Euler(-90, 0, 0));
                }
                
            }
        }
        //RaycastHit[] hits = new RaycastHit[4];
        //Physics.Raycast(transform.position, Vector3.forward, out hits[0], explosionRange);
        //Physics.Raycast(transform.position, Vector3.right, out hits[1], explosionRange);
        //Physics.Raycast(transform.position, Vector3.back, out hits[2], explosionRange);
        //Physics.Raycast(transform.position, Vector3.left, out hits[3], explosionRange);

        //foreach (var hit in hits)
        //{
        //    hit.transform?.GetComponent<IExplodable>()?.Explode(); 
        //}

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
        HasExplose = true;
        time = 0;
    }
}
