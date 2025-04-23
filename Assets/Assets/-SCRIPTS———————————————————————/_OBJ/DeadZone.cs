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
        if(collisionable is PlayerManager player) 
        {
            Debug.Log("test");
            EVENTS.InvokeOnDeath(this, player.PlayerID);
        }
    }

    void DestroyBloc(GameObject obj) 
    {
        Destroy(obj);
    }
}
