using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSystemManager : MonoBehaviour
{
    private void OnEnable()
    {
        EVENTS.DestroyAllBombs += DestroyAllBombs;
    }

    private void OnDisable()
    {
        EVENTS.DestroyAllBombs -= DestroyAllBombs;
    }

    private void DestroyAllBombs()
    {
        BombManager[] bombs = FindObjectsOfType<BombManager>();
        Debug.Log(" Suppression de toutes les bombes (" + bombs.Length + ")");
        foreach (BombManager bomb in bombs)
        {
            Destroy(bomb.gameObject);
        }
    }
}
