using UnityEngine;

public class CameraStartSetPivots : MonoBehaviour
{
    Transform hPivot, vPivot, camSnaper;

    void Awake()
    {
        hPivot = transform.GetChild(0);
        vPivot = hPivot.GetChild(0);
        camSnaper = vPivot.GetChild(0);
        transform.rotation = camSnaper.rotation;
        hPivot.localEulerAngles = Vector3.up * transform.localEulerAngles.y;
        vPivot.localEulerAngles = Vector3.right * transform.localEulerAngles.x;
        transform.localEulerAngles = camSnaper.localEulerAngles = Vector3.zero;
        Destroy(this);
    }

} // SCRIPT END
