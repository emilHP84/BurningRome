using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ZPositionMover : MonoBehaviour
{
    [Header("Position cible sur l'axe Z")]
    [SerializeField] private float targetZ = 5f;

    [Header("Vitesse de déplacement")]
    [SerializeField] private float moveSpeed = 2f;

    private void OnEnable()
    {
        EVENTS.OnSuddenDeathStart += MortSubite;
    }
    private void OnDisable()
    {
        EVENTS.OnSuddenDeathStart -= MortSubite;
    }

    void MortSubite()
    {
        transform.DOMoveZ(targetZ, moveSpeed).SetSpeedBased();
    }

}
