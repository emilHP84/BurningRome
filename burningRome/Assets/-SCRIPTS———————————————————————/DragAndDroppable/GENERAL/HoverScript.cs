using UnityEngine;
using DG.Tweening;
using GD.MinMaxSlider;

public class HoverScript : MonoBehaviour
{
    [SerializeField][MinMaxSlider(0.01f,100f)]Vector2 distance = new Vector2(10f,25f);
    [SerializeField][MinMaxSlider(0.01f,10f)]Vector2 duration = new Vector2(0.5f,0.5f);
    [SerializeField] Ease easing = Ease.InOutCubic;
    [SerializeField] bool ignoreTimeScale = false;
    Tween tween;
    RectTransform rt => GetComponent<RectTransform>();

    void OnEnable()
    {
        StartTween(Random.Range(distance.x,distance.y), Random.Range(duration.x,duration.y), easing, ignoreTimeScale);
    }

    void OnDisable()
    {
        StopTween();
    }

    public void StartTween(float distance, float duration, Ease easing, bool ignoreTimeScale)
    {
        Reset();
        tween = transform.DOLocalMoveY(transform.localPosition.y+distance,duration).SetEase(easing).SetLoops(-1,LoopType.Yoyo).SetUpdate(ignoreTimeScale);
        tween.Play();
    }

    public void StopTween()
    {
        tween?.Kill();
    }

    public void Reset()
    {
        StopTween();
        transform.localPosition = Vector3.zero;
        if (rt) rt.offsetMax = rt.offsetMin = Vector2.zero;
    }

} // SCRIPT END
