using UnityEngine;
using DG.Tweening;

public class WiggleScript : MonoBehaviour
{
    [Range(0,90f)] [SerializeField] float angle = 15f;
    [Range(0.02f,5f)] [SerializeField] float duration = 0.5f;
    [SerializeField] Ease easing = Ease.InOutCubic;
    public enum StartDirection{Left, Right, Random};
    [SerializeField] StartDirection firstDirection = StartDirection.Right;
    [SerializeField] bool ignoreTimeScale = false;
    Tween tween;
    Vector3 startRot => transform.localEulerAngles;


    void OnEnable()
    {
        StartTween(angle,duration,easing,firstDirection, ignoreTimeScale);
    }

    void OnDisable()
    {
        StopTween();
    }

    public void StartTween(float angle, float duration, Ease easing, StartDirection firstDirection, bool ignoreTimeScale)
    {
        Reset();
        transform.localEulerAngles = Vector3.back * angle;
        Vector3 targetAngle = Vector3.forward*2f*angle;
        if (firstDirection==StartDirection.Left || (firstDirection==StartDirection.Random && Random.value > 0.5f))
        {
            transform.localEulerAngles *= -1f;
            targetAngle *= -1f;
        }

        tween = transform.DOLocalRotate(targetAngle, duration).SetLoops(-1, LoopType.Yoyo).SetEase(easing).SetUpdate(ignoreTimeScale);
        tween.Play();
    }

    public void StopTween()
    {
        tween?.Kill();
    }

    public void Reset()
    {
        StopTween();
        transform.localEulerAngles = Vector3.zero;;
    }


} // SCRIPT END
