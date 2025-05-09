using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScore : MonoBehaviour
{
    [Header("base Srpite")]
    public List<GameObject> Sprites = new List<GameObject>();

    [Header("victory Sprite")]
    public List<GameObject> victorySprites = new List<GameObject>();

    [Header("Death Sprite")]
    public List<GameObject> deathSprites = new List<GameObject>();

    private void OnEnable()
    {
        EVENTS.OnPlayerDeath += SetDeathSprite;
    }

    private void OnDisable()
    {
        EVENTS.OnPlayerDeath -= SetDeathSprite;
    }

    void Start()
    {
        int spriteCount = 0;
        foreach (GameObject obj in Sprites)
        {
            SetVictorySprite(spriteCount);
            spriteCount++;
        }
    }

    void SetDeathSprite(int playerID)
    {
        Sprites[playerID].GetComponent<Image>().sprite = deathSprites[playerID].GetComponent<Image>().sprite;
    }

    void SetVictorySprite(int playerID)
    {
        Sprites[playerID].GetComponent<Image>().sprite = victorySprites[playerID].GetComponent<Image>().sprite;
    }
}
