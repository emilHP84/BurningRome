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
        m_ExplosionPatern.SetActive(true);
        yield return new WaitForSeconds(m_delayExplose);
        Destroy(this.gameObject);
    }

    // detecte les diff�rent objet en collision ( n�c�site un rigidbody )
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
}
