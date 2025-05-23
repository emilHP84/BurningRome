using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightInterpolator : MonoBehaviour
{
    [Header("Cible")]
    [SerializeField] private Light targetLight;

    [Header("Vitesse d'interpolation")]
    [SerializeField] private float transitionSpeed = 100f;
    bool alreadyInterpolated = false;

    private Light sourceLight;
    float time;

    private void Awake()
    {
        sourceLight = GetComponent<Light>();

    }

    private void OnEnable()
    {
        StopAllCoroutines();
        EVENTS.OnSuddenDeathStart += MortSubite;
    }

    private void OnDestroy()
    {
        EVENTS.OnSuddenDeathStart -= MortSubite;
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    void MortSubite()
    {
        if (alreadyInterpolated) return;
        alreadyInterpolated = true;
        StartCoroutine(MortSubiteRoutine());
    }

    IEnumerator MortSubiteRoutine()
    {
        if (targetLight == null || sourceLight == null)
            yield break;

        while (sourceLight.color != targetLight.color)
        {
            // Interpolation de la couleur
            sourceLight.color = Color.Lerp(sourceLight.color, targetLight.color, Time.deltaTime * transitionSpeed);

            // Interpolation de l'intensité
            sourceLight.intensity = Mathf.Lerp(sourceLight.intensity, targetLight.intensity, Time.deltaTime * transitionSpeed);

            // Interpolation de la rotation (direction de la lumière)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetLight.transform.rotation, Time.deltaTime * transitionSpeed);
            yield return null;
        }


    }
}
