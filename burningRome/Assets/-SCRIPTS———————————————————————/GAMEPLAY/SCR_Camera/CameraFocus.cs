using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFocus : MonoBehaviour
{
    private void OnEnable()
    {
        EVENTS.OnCallCamera += MoveCamera;
    }
    void MoveCamera(GameObject i,EventArgs e)
    {
        gameObject.transform.DOMove(new Vector3(i.transform.position.x, i.transform.position.y + 4.5f, i.transform.position.z - 1), 3); ;
    }
    private void OnDisable()
    {
        EVENTS.OnCallCamera -= MoveCamera;
    }
}
