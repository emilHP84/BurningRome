using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AllPowerUpManager : MonoBehaviour
{
    [SerializeField] private float scaleUp = 1.2f;  // Facteur d'agrandissement
    [SerializeField] private float scaleDown = 0.9f; // Facteur de rétrécissement
    [SerializeField] private float duration = 0.2f;  // Durée d'une étape
    public GameObject vfx;
    Transform Sprite => transform.Find("sprite");
    private void Awake()
    {
        Instantiate(vfx,transform.localPosition,Quaternion.identity);
        PlayBounce();
    }

    public void PlayBounce()
    {
        //Sequence bounceSequence = DOTween.Sequence();

        //bounceSequence.Append(transform.DOScale(Vector3.one * scaleUp, duration).SetEase(Ease.OutQuad).SetUpdate(true))
        //              .Append(transform.DOScale(Vector3.one * scaleDown, duration).SetEase(Ease.InOutQuad).SetUpdate(true))
        //              .Append(transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBounce).SetUpdate(true));

        //bounceSequence.Play();
        Sprite.localScale = Vector3.zero;
        Sprite.DOScale(Vector3.one, 2f).SetEase(Ease.OutElastic).OnComplete(BounceLoop);
    }

    void BounceLoop()
    {
        transform.DOPunchScale(Vector3.one * 0.2f, 1f, 4, 1).SetLoops(-1, LoopType.Restart);
        //Sprite.DOLocalRotate(Vector3.up * 360f, 0.5f, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart).SetEase(Ease.InOutBack);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }

}
