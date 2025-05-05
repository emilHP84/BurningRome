using UnityEngine;
using DG.Tweening;

public class UIAnimationZoom : UIAnimBaseClass, IUIAnimable
{
    Vector3 startScale;


    void Awake()
    {
        startScale = transform.localScale;
    }

    void OnEnable()
    {
        if (base.onEnable==UIAnimStartState.Play) StartTween();
        else Reset();
    }

    public void StartTween()
    {
        StartTween(base.playType,base.duration,base.delay,base.ignoreTimeScale);
    }


    public void StartTween(Progress playType,float duration,float delay,bool ignoreTimeScale)
    {
        Reset();
        transform.localScale = playType==Progress.In ? Vector3.zero : startScale*100f;
        base.tween = transform.DOScale(startScale, duration).SetEase(easing).SetDelay(delay).SetUpdate(ignoreTimeScale); 
        base.tween.Play();
        if (sound.clip) base.PlaySound(sound.clip,sound.volume);
    }



    public void Reset()
    {
        base.StopTween();
        if (base.onEnable==UIAnimStartState.Hide) transform.localScale = Vector3.zero;
        if (base.onEnable==UIAnimStartState.Show) transform.localScale = startScale;
    }



} // SCRIPT END
