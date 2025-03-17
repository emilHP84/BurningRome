using UnityEngine;

public class TimeScaleManager
{
    public static float GameSpeed {get{return gameSpeed;} set{gameSpeed = value; SetTimeScale();}}
    public static float FreezeFrameSpeed {get{return freezeFrameSpeed;} set{freezeFrameSpeed = value; SetTimeScale();}}
    public static float PauseSpeed {get{return pauseSpeed;} set{pauseSpeed = value; SetTimeScale();}}
    static float gameSpeed=1f;
    static float freezeFrameSpeed=1f;
    static float pauseSpeed = 1f;
    public static float GameTimeScale{get{return gameSpeed*pauseSpeed;}}


    static void SetTimeScale()
    {
        Time.timeScale = GameTimeScale*freezeFrameSpeed;
        EVENTS.InvokeTimeScaleChange(GameTimeScale);
    }
} // SCRIPT END
