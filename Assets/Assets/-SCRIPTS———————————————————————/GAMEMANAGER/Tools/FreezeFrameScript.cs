using UnityEngine;

public class FreezeFrameScript : MonoBehaviour
{
    [Range(0,0.1f)][SerializeField] float frozenTimeScale = 0.05f;
    static float freezeTime = 0;
    static bool frozen = false;

    void Awake()
    {
        freezeTime = 0;
    }

    void OnEnable()
    {
        EVENTS.OnInitialization += DisableFreeze;
    }

    void OnDisable()
    {
        EVENTS.OnInitialization -= DisableFreeze;
    }



    public static void AddMilliseconds(int howMany)
    {
        if (freezeTime<howMany) freezeTime = howMany*0.001f;
    }

    void LateUpdate()
    {
        if (GAME.MANAGER.CurrentState!=State.gameplay) return;
        Freeze(freezeTime>0);
        if (frozen) freezeTime -= Time.unscaledDeltaTime;
    }

    void Freeze(bool wanted)
    {
        if (frozen!=wanted)
        {
            if (wanted) EnableFreeze(); else DisableFreeze();
        }
    }

    void EnableFreeze()
    {
        frozen = true;
        TimeScaleManager.FreezeFrameSpeed = frozenTimeScale;
    }

    void DisableFreeze()
    {
        freezeTime = 0;
        frozen = false;
        TimeScaleManager.FreezeFrameSpeed = 1f;
    }

} // SCRIPT END
