using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInput : MonoBehaviour
{

    private void Awake()
    {
        EVENTS.OnBattleStart += Show;
        gameObject.SetActive(false);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show ()
    {
        gameObject.SetActive(true);
        transform.DOScale(Vector3.one * 1.15f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
