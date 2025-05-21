using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZPositionMover : MonoBehaviour
{
    [Header("Position cible sur l'axe Z")]
    [SerializeField] private float targetZ = 5f;

    [Header("Vitesse de déplacement")]
    [SerializeField] private float moveSpeed = 2f;

    private void Update()
    {
        Vector3 currentPosition = transform.position;
        float newZ = Mathf.Lerp(currentPosition.z, targetZ, Time.deltaTime * moveSpeed);
        transform.position = new Vector3(currentPosition.x, currentPosition.y, newZ);
    }
}
