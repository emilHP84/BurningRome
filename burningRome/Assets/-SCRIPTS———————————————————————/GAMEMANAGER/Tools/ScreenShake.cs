using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    static Transform cam;
    static float shake;
    static Vector3 startposition;


    void Start()
    {
        cam = this.transform;
        shake = 0;
        startposition = cam.localPosition;
    }

    static void Reset()
    {
        cam.localPosition = startposition;
        shake = 0;
    }

    public static void AddShake(float amount)
    {
        if (amount<shake) return;
        shake += amount;
    }


    void LateUpdate()
    {
        if (shake>0)
        {
            cam.localPosition = Random.insideUnitSphere * shake;
            shake -= shake/100f + Time.unscaledDeltaTime;
            if (shake <=0) Reset();
        }
    }
}
