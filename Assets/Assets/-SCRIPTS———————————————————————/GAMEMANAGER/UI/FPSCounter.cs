using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FPSCounter : MonoBehaviour
{
    Text t=> GetComponent<Text>();
    [SerializeField]string previousText = "FPS ";


    void OnEnable()
    {
        t.text = "FPS "+((int)(1f/Time.unscaledDeltaTime)).ToString();
        StartCoroutine(DisplayFPS());
    }


    IEnumerator DisplayFPS()
    {
        while(enabled)
        {
            float timer = 1f;
            int frames = 0;
            while (timer>0)
            {
                float unscaledDeltaTime = Time.unscaledDeltaTime;
                timer -= unscaledDeltaTime;
                frames += 1;
                yield return null;
            }
            t.text = previousText+frames.ToString();
        }
    }
} // SCRIPT END
