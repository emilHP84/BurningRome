using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnText : MonoBehaviour
{
    public int PlayerID;
    // Start is called before the first frame update
    void OnEnable()
    {
        EVENTS.OnBattleStart += HideText;
        EVENTS.OnPlayerSpawn += Spawn;
    }

    void OnDisable()
    {
        EVENTS.OnBattleStart -= HideText;
        EVENTS.OnPlayerSpawn -= Spawn;
    }
    // Update is called once per frame
    void Spawn(int ID)
    {
        if (PlayerID == ID)
        {
            HideText();
        }
    }

    void HideText()
    {
        gameObject.SetActive(false) ;

    }
}
