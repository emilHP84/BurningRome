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
        BombManager[] allManagers = FindObjectsOfType<BombManager>();

        foreach (BombManager bm in allManagers)
        {
            if (bm.gameObject.name.Contains("Bomb")) // ou == "NomExact"
            {
                Debug.Log("BombManager trouvé sur : " + bm.gameObject.name);
              
                Destroy(bm.gameObject);
            }
        }
    }
}
