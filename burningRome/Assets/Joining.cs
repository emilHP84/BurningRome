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
        EVENTS.OnBattleStart += UnDisplay;
    }

    private void OnDisable()
    {
        EVENTS.OnBattleStart -= UnDisplay;
    }

    public void UnDisplay()
    {
        transform.DOKill(); // security to avoid multiple dotween
        transform.DOScale(0, 0.5f).From(startScale).SetEase(Ease.InCirc).OnComplete(DisableObject);
    }

    void DisableObject()
    {
        gameObject.SetActive(false); // hide
        transform.localScale = startScale; // reset scale to max
    }

  
}
