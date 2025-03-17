using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class InitialForce : MonoBehaviour
{
    public Vector3 moveForce;
    public Vector3 rotationForce;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    void OnEnable()
    {
        if (rb==null) return;
        if (moveForce!= Vector3.zero) rb.AddForce(moveForce, ForceMode.VelocityChange);
        if (rotationForce != Vector3.zero) rb.AddTorque(rotationForce, ForceMode.VelocityChange);
    }
}
