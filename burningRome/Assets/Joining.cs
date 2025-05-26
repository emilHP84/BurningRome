using UnityEngine;
using DG.Tweening;

public class Joining : MonoBehaviour
{
    Vector3 startScale;

    void Awake()
    {
        startScale = transform.localScale;
    }

    private void OnEnable()
    {
        EVENTS.OnJoiningStart += Display;
        EVENTS.OnJoiningDone += UnDisplay;
    }

    private void OnDisable()
    {
        EVENTS.OnJoiningStart -= Display;
        EVENTS.OnJoiningDone -= UnDisplay;
    }

    public void UnDisplay()
    {
        transform.DOKill(); // security to avoid multiple dotween
        transform.DOScale(0, 0.5f).From(startScale).SetEase(Ease.InCirc).OnComplete(DisableObject);
    }

    void Display()
    {
        transform.DOKill(); // security to avoid multiple dotween
        transform.DOScale(startScale, 0.5f).From(0).SetEase(Ease.OutCirc);
    }


    void DisableObject()
    {
        gameObject.SetActive(false); // hide
        transform.localScale = startScale; // reset scale to max
    }

  
}
