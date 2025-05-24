using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideVFX : MonoBehaviour
{
    GAMEPLAY gameplay => FindAnyObjectByType<GAMEPLAY>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameplay.ActivePlayers < 2)
        {
          HideVFX[] vfx = FindObjectsOfType<HideVFX>();

            foreach(HideVFX hide in vfx)
            {
                Destroy(hide.gameObject);
            }
        }
    }
}
