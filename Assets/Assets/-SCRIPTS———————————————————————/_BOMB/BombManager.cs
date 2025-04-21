using Rewired;
using System.Collections;
using Unity.Jobs;
using UnityEngine;

public class BombManager : MonoBehaviour, ICollisionable, IDetect
{
    private Rigidbody rb;
    private SphereCollider sphereCollider;

    [SerializeField][Range(0, 6)] float m_delayBetweenExplose;
    [SerializeField][Range(0, 3)] float m_delayExplose;
    [SerializeField] GameObject m_ExplosionPatern;
    private int explosionRange = 1;
    public Transform RangeUp;
    public Transform RangeDown;
    public Transform RangeLeft;
    public Transform RangeRight;

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
            StartCoroutine(Explose());
            HasExplose = true;
            time = 0;
        }
    }

    IEnumerator Explose()
    {
        // Récupérer les cubes
        Transform up = RangeUp;
        Transform down = RangeDown;
        Transform left = RangeLeft;
        Transform right = RangeRight;

        // Les déplacer selon la portée
        up.localPosition = Vector3.forward * explosionRange;
        down.localPosition = Vector3.back * explosionRange;
        left.localPosition = Vector3.left * explosionRange;
        right.localPosition = Vector3.right * explosionRange;

        // Activer l'explosion
        m_ExplosionPatern.SetActive(true);

        // Attendre la fin de l'explosion
        yield return new WaitForSeconds(m_delayExplose);
        Destroy(this.gameObject);

    }

    // detecte les différent objet en collision ( nécésite un rigidbody )
    public void OnCollisionWith(ICollisionable collisionable)
    {
        if (collisionable is Ground)
        {
            rb.isKinematic = true;
        }
    }

    public void OnDetectionWith(IDetect detect)
    {
        StartCoroutine(Explose());
    }

    public void SetExplosionRange(int range)
    {
        explosionRange = range;
    }


}
