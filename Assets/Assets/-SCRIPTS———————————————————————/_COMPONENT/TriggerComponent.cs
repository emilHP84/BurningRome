using UnityEngine;

public class TriggerComponent : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        IDetect detect = other.GetComponent<IDetect>();
        if (detect != null)
        {
            detect.OnDetectionWith(detect);
        }
    }
}
