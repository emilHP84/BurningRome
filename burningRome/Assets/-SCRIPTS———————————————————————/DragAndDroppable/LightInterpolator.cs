using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Light))]
public class LightInterpolator : MonoBehaviour
{
    [Header("Cible")]
    [SerializeField] private Light targetLight;

    [Header("Vitesse d'interpolation")]
    [SerializeField] private float animDuration = 2f;
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
        //if (alreadyInterpolated) return;
        //alreadyInterpolated = true;
        sourceLight.DOColor(targetLight.color, animDuration).SetEase(Ease.InOutQuad);
        sourceLight.DOIntensity(targetLight.intensity, animDuration);
        transform.DORotateQuaternion(targetLight.transform.rotation,animDuration);
        //StartCoroutine(MortSubiteRoutine());
    }

    IEnumerator MortSubiteRoutine()
    {
        if (targetLight == null || sourceLight == null)
            yield break;

        while (sourceLight.color != targetLight.color)
        {
            // Interpolation de la couleur
            sourceLight.color = Color.Lerp(sourceLight.color, targetLight.color, Time.deltaTime * animDuration);

            // Interpolation de l'intensité
            sourceLight.intensity = Mathf.Lerp(sourceLight.intensity, targetLight.intensity, Time.deltaTime * animDuration);

            // Interpolation de la rotation (direction de la lumière)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetLight.transform.rotation, Time.deltaTime * animDuration);
            yield return null;
        }


    }
}
