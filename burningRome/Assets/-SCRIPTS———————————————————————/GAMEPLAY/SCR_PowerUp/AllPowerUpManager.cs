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
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 2f).SetEase(Ease.OutElastic);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
