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

        cam.rotation = transform.rotation;
        Vector3 camEuler = cam.eulerAngles;
        camEuler.x = transform.eulerAngles.x;
        cam.eulerAngles = camEuler;
    }
}
