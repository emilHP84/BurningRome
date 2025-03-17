using DG.Tweening;
using UnityEngine;
using GD.MinMaxSlider;

public class PulseScript : MonoBehaviour
{
    Vector3 startScale;
    [SerializeField] [MinMaxSlider(0,2f)]Vector2 delay = new Vector2(0,0);
    [SerializeField] [MinMaxSlider(0,10f)]Vector2 pulseScale = new Vector2(2f,2f);
    [SerializeField] [MinMaxSlider(0.01f,10f)]Vector2 duration = new Vector2(0.5f,0.5f);
    [SerializeField] Ease easing = Ease.InOutCubic;
    [SerializeField] bool looping = true;
    [SerializeField] LoopType loopType = LoopType.Yoyo;
    [SerializeField] bool ignoreTimeScale = false;


    void Awake()
    {
        startScale = transform.localScale;
    }

    void OnEnable()
    {
        transform.localScale = startScale;
        transform.DOKill();
        transform.DOScale( Vector3.one*Random.Range(pulseScale.x,pulseScale.y), Random.Range(duration.x,duration.y))
            .SetUpdate(ignoreTimeScale)
            .SetEase(easing)
            .SetLoops(looping ? -1 : 1, looping ? loopType : LoopType.Yoyo)
            .SetDelay(Random.Range(delay.x,delay.y));
    }

    void OnDisable()
    {
        transform.DOKill();
    }

} // SCRIPT END
