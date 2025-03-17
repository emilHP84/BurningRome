using System.Collections;
using UnityEngine;

public class CursorTrail : MonoBehaviour
{
    TrailRenderer trail => GetComponentInChildren<TrailRenderer>();

    void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(ClearTrailNextFrame());
    }

    IEnumerator ClearTrailNextFrame()
    {
        yield return null;
        trail.Clear();
    }


    void Awake()
    {
        EVENTS.OnGamePause += DisableTrail;
        EVENTS.OnGamePauseExit += EnableTrail;
    }

    void OnDestroy()
    {
        EVENTS.OnGamePause -= DisableTrail;
        EVENTS.OnGamePauseExit -= EnableTrail;
    }

    void EnableTrail()
    {
        trail.enabled = true;
        trail.Clear();
    }

    void DisableTrail()
    {
        trail.enabled = false;
        trail.Clear();
    }


} // SCRIPT END
