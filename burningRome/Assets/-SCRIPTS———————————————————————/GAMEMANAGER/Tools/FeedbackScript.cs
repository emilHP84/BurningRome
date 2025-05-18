using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackScript : MonoBehaviour
{
    [Header("SCREEN EFFECTS")]
    [Range(0,10f)]public float screenShake;
    [Range(0,2000)]public int freezeMilliseconds;
    public bool freezeBeforeShake;
    AudioSource[] sounds;
    ParticleSystem[] particles;
    bool destroyable;
    int i;

    void Awake()
    {
        sounds = GetComponentsInChildren<AudioSource>();
        particles = GetComponentsInChildren<ParticleSystem>();
        StartCoroutine(WaitForDestruction());
    }




    IEnumerator WaitForDestruction()
    {
        destroyable = false;
        if (freezeMilliseconds>0) FreezeFrameScript.AddMilliseconds(freezeMilliseconds);
        if (freezeBeforeShake) yield return new WaitForSeconds(freezeMilliseconds/15000f);
        if (screenShake>0) ScreenShake.AddShake(screenShake);

        while (destroyable == false)
        {
            yield return null;
            destroyable = true;
            for (i=0; i<sounds.Length;i++)
            {
                if (sounds[i]!=null && sounds[i].isPlaying) destroyable = false;
            }
            for (i=0; i<particles.Length;i++)
            {
                if (particles[i]!=null && particles[i].isPlaying) destroyable = false;
            }
        }

        Destroy(this.gameObject);
    }
} // FIN DU SCRIPT
