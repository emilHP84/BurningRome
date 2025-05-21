using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightInterpolator : MonoBehaviour
{
    [Header("Cible")]
    [SerializeField] private Light targetLight;

    [Header("Vitesse d'interpolation")]
    [SerializeField] private float transitionSpeed = 1f;

    private Light sourceLight;

    private void Awake()
    {
        sourceLight = GetComponent<Light>();
    }

    private void Update()
    {
        if (targetLight == null || sourceLight == null)
            return;

        // Interpolation de la couleur
        sourceLight.color = Color.Lerp(sourceLight.color, targetLight.color, Time.deltaTime * transitionSpeed);

        // Interpolation de l'intensité
        sourceLight.intensity = Mathf.Lerp(sourceLight.intensity, targetLight.intensity, Time.deltaTime * transitionSpeed);

        // Interpolation de la rotation (direction de la lumière)
        transform.rotation = Quaternion.Slerp(transform.rotation, targetLight.transform.rotation, Time.deltaTime * transitionSpeed);
    }
}
