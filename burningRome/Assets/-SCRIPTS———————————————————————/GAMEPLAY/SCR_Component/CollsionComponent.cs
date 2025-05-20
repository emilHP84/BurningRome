using Unity.VisualScripting;
using UnityEngine;

public class CollsionComponent : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        ICollisionable obj = GetCollisionable(gameObject);

        if (obj is null) 
            return;

        ICollisionable other = GetCollisionable(collision.gameObject);
        
        if (other != null && obj != null)
            obj.OnCollisionWith(other); 
    }

    private ICollisionable GetCollisionable(GameObject go)
    {
        if (go is null) 
            return null;

        ICollisionable obj = go.GetComponent<ICollisionable>();

        if (obj is null)
        {
            obj = go.GetComponentInParent<ICollisionable>();

            if (obj is null)
                obj = go.GetComponentInChildren<ICollisionable>();

        }
        return obj;
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerMovement input = other.gameObject.GetComponent<PlayerMovement>();
        if (input != null)
        {
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                col.isTrigger = false;
            }
        }
    }
}
