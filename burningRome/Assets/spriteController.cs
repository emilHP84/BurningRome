using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(transform.localScale * 1.2f, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
