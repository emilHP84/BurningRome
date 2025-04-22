using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour, ICollisionable
{
    public void OnCollisionWith(ICollisionable collisionable)
    {
        if (collisionable is Obstacle obj )
        {
            DestroyBloc(obj.gameObject);
        }
        if (collisionable is Indestructible ind)
        {
            DestroyBloc(ind.gameObject);
        }
    }

    void DestroyBloc(GameObject obj) 
    {
        Destroy(obj);
    }
}
