using Rewired;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScore : MonoBehaviour
{
    [Header("base Srpite")]
    public List<GameObject> Sprites = new List<GameObject>();
    public List<TextMeshProUGUI> text = new List<TextMeshProUGUI>();

    [Header("victory Sprite")]
    public List<Sprite> victorySprites = new List<Sprite>();
    [Header("Death Sprite")]
    public List<Sprite> deathSprites = new List<Sprite>();

    [Header("Points")]
    public List<int> Points = new List<int>();

    private void OnEnable()
    {
        EVENTS.OnPlayerDeath += SetDeathSprite;
        EVENTS.OnBattleStart += SetVictorySprite;
        EVENTS.OnGameStart += ResetSprite;
    }

    private void OnDisable()
    {
        EVENTS.OnPlayerDeath -= SetDeathSprite;
        EVENTS.OnBattleStart -= SetVictorySprite;
        EVENTS.OnGameStart -= ResetSprite;
    }

    void ResetSprite()
    {
        int spriteCount = 0;
        foreach (GameObject sprite in Sprites) 
        {
            sprite.SetActive(false);
            spriteCount++;
        }
    }

    void SetDeathSprite(int playerID)
    {
        Sprites[playerID].GetComponent<Image>().sprite = deathSprites[playerID];
        RemovePoints(playerID);
    }

    void SetVictorySprite()
    {
        int spriteCount = 0;
        for (int i = 0; i < GAMEPLAY.access.TotalPlayers; i++)
        {
            Sprites[spriteCount].SetActive(true);
            Sprites[spriteCount].GetComponent<Image>().sprite = victorySprites[spriteCount];
            SetPoints(spriteCount);
            spriteCount++;
        }
    }

    void SetPoints(int number)
    {
        Points[number] += 1;
        text[number].text = Points[number].ToString();
    }

    void RemovePoints(int number)
    {
        number = Mathf.Clamp(number, 0, Points.Count - 1); // Vérifie que l'index est valide

        if (Points[number] > 0) // Vérifie que la valeur est supérieure à 0 avant de décrémenter
        {
            Points[number] -= 1;
        }
        text[number].text = Points[number].ToString();
    }
}
