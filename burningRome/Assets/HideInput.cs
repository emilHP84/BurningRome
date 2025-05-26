using DG.Tweening;
using UnityEngine;

public class HideInput : MonoBehaviour
{
    private SpriteRenderer sprite => GetComponent<SpriteRenderer>();

    private void Start()
    {
        Hide();
    }

    void OnEnable()
    {
        EVENTS.OnBattleStart += Show;
    }

    void OnDisable()
    {
        EVENTS.OnBattleStart -= Show;
    }
    
    public void Hide()
    {
        if (sprite == null) return;
        sprite.enabled = false;
    }

    public void Show ()
    {
        if (sprite == null) return;
        sprite.enabled = true;
        transform.DOKill();
        transform.DOScale(Vector3.one * 1.15f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
