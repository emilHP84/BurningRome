using UnityEngine;

public class TriggerComponent : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.name);
        IDetect obj = other.gameObject.GetComponent<IDetect>();
        if (obj == null) return;
        obj.OnDetectionWith(other.gameObject.GetComponent<IDetect>());
    }
}
