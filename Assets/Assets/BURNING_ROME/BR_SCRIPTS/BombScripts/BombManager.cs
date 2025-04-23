using Rewired;
using System.Collections;
using Unity.Jobs;
using UnityEngine;

public class BombManager : MonoBehaviour, ICollisionable
{
    private Rigidbody rb;
    private SphereCollider sphereCollider;

    [SerializeField][Range(0, 6)] float m_delayBetweenExplose;
    [SerializeField][Range(0, 3)] float m_delayExplose;
    [SerializeField] GameObject m_ExplosionPatern;
    [SerializeField] private int explosionRange = 1;


    private float time;
    private bool HasExplose;

    void Start()
    {
        SetComponent();
        time = 0;
    }
    void SetComponent()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = rb.GetComponent<SphereCollider>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (HasExplose == false && time >= m_delayBetweenExplose)
        {
            Explose();
            HasExplose = true;
            time = 0;
        }
        else if(HasExplose &&  time >= m_delayExplose)
        {
            // Attendre la fin de l'explosion
            Destroy(this.gameObject);
        }
    }

    void Explose()
    {
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
        Physics.Raycast(transform.position,Vector3.forward,out hits[0], explosionRange);
        Physics.Raycast(transform.position, Vector3.right, out hits[1], explosionRange);
        Physics.Raycast(transform.position, Vector3.back, out hits[2], explosionRange);
        Physics.Raycast(transform.position, Vector3.left, out hits[3], explosionRange);

        foreach (var hit in hits)
        {
            
            hit.transform?.GetComponent<IExplodable>()?.Explode();
        }

    }

    // detecte les différent objet en collision ( nécésite un rigidbody )
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
    }

    public void SetExplosionRange(int range)
    {
        explosionRange = range;
    }
}
