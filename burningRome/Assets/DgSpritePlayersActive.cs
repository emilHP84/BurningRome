using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DgSpritePlayersActive : MonoBehaviour
{
    void Start()
    {
        transform.DOScale(Vector3.one * 1.1f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
