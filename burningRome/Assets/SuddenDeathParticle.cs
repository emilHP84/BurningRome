using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
            particle.Play();
        }
    }
}
