using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class UIAnimationFade : UIAnimBaseClass, IUIAnimable
{
    CanvasGroup canvasGroup => GetComponent<CanvasGroup>();

    void OnEnable()
    {
        if (base.onEnable==UIAnimStartState.Play) StartTween(base.playType,base.duration,base.delay,base.ignoreTimeScale);
        else Reset();
    }

    public void StartTween()
    {
        StartTween(playType,base.duration,base.delay,base.ignoreTimeScale);
    }

    public void StartTween(Progress playType,float duration,float delay,bool ignoreTimeScale)
    {
        Reset();
        canvasGroup.alpha = playType==Progress.In ? 0 : 1f;
        tween = canvasGroup.DOFade(playType==Progress.In ? 1f : 0, duration).SetEase(easing).SetUpdate(true).SetDelay(delay).SetUpdate(ignoreTimeScale);
        tween.Play();
        if (sound.clip) PlaySound(sound.clip,sound.volume);
    }


    public void Reset()
    {
        StopTween();
        if (base.onEnable==UIAnimStartState.Hide) canvasGroup.alpha = 0;
        if (base.onEnable==UIAnimStartState.Show) canvasGroup.alpha = 1f;
    }

} // SCRIPT END
