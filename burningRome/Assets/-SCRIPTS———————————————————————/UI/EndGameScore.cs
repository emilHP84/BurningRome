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

    [Header("Death Sprite")]
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
        foreach (GameObject sprite in Sprites) 
        {
            int spriteCount = 0;
            sprite.SetActive(false);
            spriteCount++;

            
        }
    }

    void SetDeathSprite(int playerID)
    {
        Debug.Log("hahaya");
        Sprites[playerID].GetComponent<Image>().sprite = deathSprites[playerID];
        //Points[playerID]-=1;
        text[playerID].text = Points[playerID].ToString();
    }

    void SetVictorySprite()
    {
        int spriteCount = 0;
        for(int i = 0; i < GAMEPLAY.access.TotalPlayers; i++)
        {
            Sprites[spriteCount].SetActive(true);
            Sprites[spriteCount].GetComponent<Image>().sprite = victorySprites[spriteCount];
            spriteCount++;

            Points[spriteCount] += 1;
            text[spriteCount].text = Points[spriteCount].ToString();
        }
    }
}
