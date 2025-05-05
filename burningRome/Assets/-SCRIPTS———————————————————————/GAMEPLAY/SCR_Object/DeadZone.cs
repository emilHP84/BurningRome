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
            player.Explode();
        }
    }

    void DestroyBloc(GameObject obj) 
    {
        Destroy(obj);
    }
}
