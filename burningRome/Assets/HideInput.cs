using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInput : MonoBehaviour
{
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        EVENTS.OnBattleStart += Show;
        sprite.enabled = false ;
    }
    public void Hide()
    {
        if(sprite != null)
        {
            sprite.enabled = false;
        }
       
    }

    public void Show ()
    {
        sprite.enabled = true;
        transform.DOScale(Vector3.one * 1.15f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
