using UnityEngine;

public class CollsionComponent : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        ICollisionable obj = GetComponent<ICollisionable>();
        if (collision.gameObject.GetComponent<ICollisionable>() is null) return;
        obj.OnCollisionWith(collision.gameObject.GetComponent<ICollisionable>());
    }
}
