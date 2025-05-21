using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YPositionMover : MonoBehaviour
{
    [Header("Position cible sur l'axe Y")]
    [SerializeField] private float targetY = 1.5f;

    [Header("Vitesse de déplacement")]
    [SerializeField] private float moveSpeed = 2f;

    private void Update()
    {
        Vector3 currentPosition = transform.position;

        // Interpolation seulement sur l'axe Y
        float newY = Mathf.Lerp(currentPosition.y, targetY, Time.deltaTime * moveSpeed);

        transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
    }
}
