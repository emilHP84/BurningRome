using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScore : MonoBehaviour
{
    [Header("Base Sprite")]
    public List<Image> Halo = new List<Image>();
    public List<Image> Sprites = new List<Image>();
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
        Sprites[winnerID].GetComponentInChildren<Image>().sprite = victorySprites[winnerID];
        if (Halo.Count>winnerID) Halo[winnerID].gameObject.SetActive(true);
    }

    void DisplayLosers()
    {

        foreach(Image image in Halo)
        {
            image.gameObject.SetActive(false);
        }
        for (int i = 0; i < GAMEPLAY.access.TotalPlayers; i++)
        {
            Sprites[i].enabled = true;
            Sprites[i].GetComponentInChildren<Image>().sprite = deathSprites[i];
        }
        for (int i = 3; i >= GAMEPLAY.access.TotalPlayers; i--)
        {
            Sprites[i].enabled = false;
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
