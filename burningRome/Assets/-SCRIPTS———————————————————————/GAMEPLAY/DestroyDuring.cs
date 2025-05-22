using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDuring : MonoBehaviour
{
    public float time;
    public void Start()
    {
        Destroy(this.gameObject, time);
    }
}
