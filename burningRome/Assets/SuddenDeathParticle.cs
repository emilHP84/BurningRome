using DG.Tweening;
using UnityEngine;

public class SuddenDeathParticle : MonoBehaviour
{
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
        foreach(ParticleSystem particle in GetComponentsInChildren<ParticleSystem>())
        {
            particle.transform.DOScale(Vector3.one,3f).From(0).SetEase(Ease.OutBack);
            particle.Play();
        }
    }
}
