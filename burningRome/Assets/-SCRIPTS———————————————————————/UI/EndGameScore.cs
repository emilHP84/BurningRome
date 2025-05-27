using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScore : MonoBehaviour
{
    [Header("Base Sprite")]
    public List<GameObject> Sprites = new List<GameObject>();
    public List<TextMeshProUGUI> text = new List<TextMeshProUGUI>();

    [Header("Victory Sprite")]
    public List<Sprite> victorySprites = new List<Sprite>();
    [Header("Death Sprite")]
    public List<Sprite> deathSprites = new List<Sprite>();



    private void OnEnable()
    {
        EVENTS.OnVictory += SetWinner;
        EVENTS.OnEndGame += DisplayScores;
    }

    private void OnDisable()
    {
        EVENTS.OnVictory -= SetWinner;
        EVENTS.OnEndGame -= DisplayScores;
    }


    void SetWinner(int winnerID)
    {
        DisplayScores();
        Sprites[winnerID].GetComponent<Image>().sprite = victorySprites[winnerID];
    }

    void DisplayLosers()
    {
        for (int i = 0; i < GAMEPLAY.access.TotalPlayers; i++)
        {
            Sprites[i].SetActive(true);
            Sprites[i].GetComponent<Image>().sprite = deathSprites[i];
        }
        for (int i = 3; i >= GAMEPLAY.access.TotalPlayers; i--)
        {
            Sprites[i].SetActive(false);
        }
    }

    void DisplayScores()
    {
        for (int i = 0; i < GAMEPLAY.access.TotalPlayers; i++)
        {
            text[i].text = "" + GAMEPLAY.access.GetPlayerScore(i);
            text[i].gameObject.SetActive(true);
        }
        for (int i = 3; i >= GAMEPLAY.access.TotalPlayers; i--)
        {
            text[i].gameObject.SetActive(false);
        }
        DisplayLosers();
    }

} // FIN DU SCRIPT
