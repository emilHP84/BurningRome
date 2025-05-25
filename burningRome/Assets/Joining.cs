using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joining : MonoBehaviour
{

    private void OnEnable()
    {
        EVENTS.OnBattleStart += UnDisplay;
    }

    private void OnDisable()
    {
        EVENTS.OnBattleStart -= UnDisplay;
    }

    public void UnDisplay()
    {
        gameObject.SetActive(false);
    }

  
}
