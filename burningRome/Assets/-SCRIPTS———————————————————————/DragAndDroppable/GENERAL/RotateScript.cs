using UnityEngine;

public class RotateScript : MonoBehaviour
{
    [Range(-1080f,1080f)]public float speed;
    Vector3 rotation = Vector3.zero;

    void Update()
    {
        rotation.z = speed*Time.deltaTime;
        transform.localEulerAngles += rotation;
    }
}
