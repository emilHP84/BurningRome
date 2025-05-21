using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpriteSpawnDG : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(Vector3.one * 1.15f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine) ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
