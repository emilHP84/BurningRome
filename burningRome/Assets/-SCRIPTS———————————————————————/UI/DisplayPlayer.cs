using UnityEngine;

public class DisplayPlayer : MonoBehaviour
{
    [SerializeField] int targetPlayerID; // ID du joueur pour cet affichage
    [SerializeField] GameObject BlackSprite;
    [SerializeField] GameObject ReadySprite;

    private void OnEnable()
    {
        EVENTS.OnPlayerSpawn += DisplaySprite;
        ShowBlackSprite();
    }

    private void OnDisable()
    {
        EVENTS.OnPlayerSpawn -= DisplaySprite;
    }

    void ShowBlackSprite()
    {
        BlackSprite.SetActive(true);
        ReadySprite.SetActive(false);
    }


    public void DisplaySprite(int playerID)
    {
        if (playerID != targetPlayerID) return;
        BlackSprite.SetActive(false);
        ReadySprite.SetActive(true);
    }
}
