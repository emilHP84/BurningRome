using Rewired;
using UnityEngine;

public class BombManager : MonoBehaviour, ICollisionable
{
    private Rigidbody rb;
    private SphereCollider sphereCollider;


    
    void Start()
    {
        SetComponent();
    }
    void SetComponent()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = rb.GetComponent<SphereCollider>();
    }
    
    void Update()
    {
        
    }


    // detecte les diff�rent objet en collision ( n�c�site un rigidbody 
    public void OnCollisionWith(ICollisionable collisionable)
    {
        if(collisionable is Ground)
        {
            rb.isKinematic = true;
        }
    }
}
