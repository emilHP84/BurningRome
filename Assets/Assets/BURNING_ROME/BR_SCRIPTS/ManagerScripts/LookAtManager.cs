using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LookAtManager : MonoBehaviour
{
    private Transform cam;

    private void Awake()
    {
        cam = Camera.main.transform;
        Vector3 camrot = Vector3.right * cam.eulerAngles.x;
        transform.localEulerAngles = camrot;
    }
}
