using UnityEngine;

public class CollsionComponent : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        ICollisionable obj = GetComponent<ICollisionable>();
        if (collision.gameObject.GetComponent<ICollisionable>() is null) return;
        obj.OnCollisionWith(collision.gameObject.GetComponent<ICollisionable>());
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
