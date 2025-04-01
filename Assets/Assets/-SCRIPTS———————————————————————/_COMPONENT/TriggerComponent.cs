using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerComponent : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        IDetect obj = other.gameObject.GetComponent<IDetect>();
        if (other.gameObject.GetComponent<IDetect>() is not BombManager) return;
        obj.OnDetectionWith(other.gameObject.GetComponent<IDetect>());
    }
}
