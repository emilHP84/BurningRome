using UnityEngine;
using DG.Tweening;


public class UIAnimBaseClass : MonoBehaviour
{
    [SerializeField] protected UIAnimStartState onEnable = UIAnimStartState.Play;
    [SerializeField] protected Progress playType = Progress.In;
    [Range(0,10f)][SerializeField]protected float delay = 0;
    [Range(0.01f,10f)][SerializeField]protected float duration = 1f;
    [SerializeField]protected Ease easing = Ease.InOutCubic;
    [SerializeField]protected AudioClipExtended sound;


    protected Tween tween;
    [SerializeField] protected bool ignoreTimeScale = false;


    void OnDisable()
    {
        StopTween();
    }


    public void ChangeEnableMode(UIAnimStartState desired)
    {
        onEnable = desired;
    }

    public void StopTween()
    {
        tween?.Kill();
    }


    protected void PlaySound(AudioClip clip, float volume)
    {
        MENU.SCRIPT.Audio.PlayOneShot(clip,volume);
    }


} // SCRIPT END

public interface IUIAnimable
{
    public void StartTween();
    public void StopTween();
    public void Reset();
}

public enum Progress{In,Out}