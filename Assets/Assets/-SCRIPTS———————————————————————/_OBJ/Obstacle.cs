using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IBreakable, IDetect
{
    public void Break()
    {
        Destroy(gameObject);
    }

    public void OnDetectionWith(IDetect detect)
    {
            Destroy(gameObject);
    }
}
