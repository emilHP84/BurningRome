// ℹ️ BLACK SCREEN SYSTEM FOR TRANSITIONS IN THE UI


using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Black : MonoBehaviour
{
    public float Progression{get{if (IsFading)return fadeProgression; else if (IsIrising)return irisProgression; else return 1f;}}
    float irisProgression, fadeProgression = 0;
    public static Black screen;
    [SerializeField] CanvasGroup blackScreen;
    [SerializeField] Image iris;
    public bool IsWorking{get{return (IsFading||IsIrising);}}
    public bool IsFading{get{return fading;}}
    public bool IsIrising{get{return irising;}}
    bool fading, irising;
    float currentAlphaCutout = 1f;
    float targetValue;
    float countdown = 0;

    void Awake()
    {
        if (screen!=null) Debug.Log("⚠️ ERROR! MULTIPLE BLACK SCREENS IN SCENE");
        screen = this;
        Hide();
        OpenIris();
    }


    public void FadeIn(float duration)
    {
        FadeTo(1f,duration);
    }

    public void FadeOut(float duration)
    {
        FadeTo(0,duration);
    }

    public void IrisIn(float duration)
    {
        if (irising==false) StartCoroutine(IrisRoutine(0f,duration,Ease.InCirc));
    }

    public void IrisOut(float duration)
    {
        if (irising==false) StartCoroutine(IrisRoutine(1f,duration,Ease.OutCirc));
    }

    public void Show()
    {
        fading = false;
        blackScreen.alpha = targetValue = 1f;
        blackScreen.gameObject.SetActive(true);
    }

    public void Hide()
    {
        fading = false;
        blackScreen.alpha = targetValue = 0f;
        blackScreen.gameObject.SetActive(false);
    }

    public void OpenIris()
    {
        irising = false;
        currentAlphaCutout = 1f;
        SetIrisAlphaClip(currentAlphaCutout);
        iris.gameObject.SetActive(false);
    }

    public void CloseIris()
    {
        irising = false;
        currentAlphaCutout = 0f;
        SetIrisAlphaClip(currentAlphaCutout);
        iris.gameObject.SetActive(true);
    }

    void FadeTo(float targetAlpha,float duration)
    {
        gameObject.SetActive(true);
        targetValue = targetAlpha;
        countdown = duration * Mathf.Abs(targetAlpha-blackScreen.alpha);
        if (fading==false) StartCoroutine(FadeRoutine(targetValue,duration));
    }

    IEnumerator FadeRoutine(float targetValue, float duration)
    {
        fadeProgression = 0;
        fading = true;
        blackScreen.gameObject.SetActive(true);
        float chrono = 0;
        while (blackScreen.alpha != targetValue)
        {
            if (fading==false) yield break;
            if (countdown <=0) countdown = 0.001f;
            blackScreen.alpha = Mathf.MoveTowards(blackScreen.alpha, targetValue, Time.unscaledDeltaTime/countdown);
            chrono += Time.unscaledDeltaTime;
            fadeProgression = chrono/duration;
            yield return null;
        }
        if (fading==false) yield break;
        if (blackScreen.alpha == 0) blackScreen.gameObject.SetActive(false);
        fading = false;
        fadeProgression = 1f;
    }

    IEnumerator IrisRoutine(float targetAlpha,float duration, Ease easing)
    {
        irisProgression = 0;
        if (targetAlpha==currentAlphaCutout) yield break;
        irising = true;
        iris.gameObject.SetActive(true);
        irisTween?.Kill();
        if (duration<=0 || currentAlphaCutout==targetAlpha)
        {
            currentAlphaCutout = targetAlpha;
        }
        else
        {
            irisTween = DOTween.To(() => currentAlphaCutout, x => currentAlphaCutout = x, targetAlpha, duration).SetEase(easing).SetUpdate(true);
            irisTween.Play();
            float chrono = 0;
            while (currentAlphaCutout!=targetAlpha)
            {
                chrono += Time.unscaledDeltaTime;
                irisProgression = chrono/duration;
                SetIrisAlphaClip(currentAlphaCutout);
                yield return null;
            }
        }
        SetIrisAlphaClip(currentAlphaCutout);
        if (currentAlphaCutout==1f) iris.gameObject.SetActive(false);
        irising = false;
        irisProgression = 1f;
    }


    void SetIrisAlphaClip(float alphaClip)
    {
        iris.material.SetFloat("_Cutoff", alphaClip);
    }

    Tween irisTween;

} // SCRIPT END
