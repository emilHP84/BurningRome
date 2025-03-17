using UnityEngine;

public class BillboardLookAtCam : MonoBehaviour
{
    Transform _transform => transform;
    Transform cam => Camera.main.transform;

    void Update()
    {
        _transform.LookAt(cam);
    }
} // SCRIPT END
