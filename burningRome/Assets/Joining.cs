using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Joining : MonoBehaviour
{
    Vector3 startScale;
    [SerializeField] Button startBattleButton;
    [SerializeField] GameObject pressButtonText;

    void Awake()
    {
        startScale = transform.localScale;
        startBattleButton.gameObject.SetActive(false);
        pressButtonText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        EVENTS.OnJoiningStart += Display;
        EVENTS.OnJoiningDone += UnDisplay;
        EVENTS.OnPlayerSpawn += CheckPlayerQuantity;
        if (GAMEPLAY.access.CurrentState == GameplayState.joining) Display();
    }

    private void OnDisable()
    {
        EVENTS.OnJoiningStart -= Display;
        EVENTS.OnJoiningDone -= UnDisplay;
        EVENTS.OnPlayerSpawn -= CheckPlayerQuantity;
    }

    public void UnDisplay()
    {
        transform.DOKill(); // security to avoid multiple dotween
        transform.DOScale(0, 0.5f).From(startScale).SetEase(Ease.InCirc).OnComplete(DisableObject);
    }

    void Display()
    {
        startBattleButton.gameObject.SetActive(false);
        pressButtonText.gameObject.SetActive(true);
        transform.DOKill(); // security to avoid multiple dotween
        transform.DOScale(startScale, 0.5f).From(0).SetEase(Ease.OutCirc);
    }


    void DisableObject()
    {
        gameObject.SetActive(false); // hide
        transform.localScale = startScale; // reset scale to max
    }

    void CheckPlayerQuantity(int playerID)
    {
        if (GAMEPLAY.access.TotalPlayers > 0)
        {
            startBattleButton.gameObject.SetActive(true);
            pressButtonText.gameObject.SetActive(false);
        }
    }

  
}
