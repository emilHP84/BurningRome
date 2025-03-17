using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    [SerializeField] float pushForce = 100f;
    
    void OnControllerColliderHit(ControllerColliderHit hit) // Works with the CharacterController component
    {
        hit.collider.GetComponentInParent<iPushable>()?.Push(hit.point,(hit.collider.transform.position-transform.position).normalized*pushForce);
    }

    // void OnTriggerEnter(Collider col) // Activate this to use the isTrigger collider for push
    // {
    //     col.GetComponentInParent<iPushable>()?.Push(transform.position,(col.transform.position-transform.position).normalized*pushForce);
    // }

} // SCRIPT END
