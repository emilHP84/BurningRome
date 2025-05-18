using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithCurve : MonoBehaviour
{
    public AnimationCurve curve;

    private Transform originPosition => this.gameObject.transform;
    private Transform targetPosition;
    [SerializeField]private float delay = 6;

    private float time;
    private float elapsedTime;

    private bool isCalling = false;

    private void OnEnable()
    {
        EVENTS.OnPlayerInstance += IsCalled;
    }

    public void IsCalled(bool isCalled, Transform targetPos, EventArgs e)
    {
        isCalling = isCalled;
        targetPosition = targetPos;
    }

    public void Update()
    {
        if (isCalling == false) return;


        if (time < delay) 
        { 
            time += Time.deltaTime; 
            elapsedTime = time/delay;
            MoveWithAnimationCurve();
        }
        else isCalling = false;

    }

    public void MoveWithAnimationCurve()
    {
        float curveValue = curve.Evaluate(elapsedTime);
        transform.position = Vector3.Lerp(originPosition.position,new Vector3(targetPosition.position.x, targetPosition.position.y + curveValue ,targetPosition.position.z),1 * Time.deltaTime);
    }

    private void OnDisable()
    {
        EVENTS.OnPlayerInstance -= IsCalled;

    }
}
