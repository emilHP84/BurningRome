using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPowerUpManager : MonoBehaviour
{
    public GameObject vfx;
    private void Awake()
    {
        Instantiate(vfx,transform.localPosition,Quaternion.identity);
    }
}
