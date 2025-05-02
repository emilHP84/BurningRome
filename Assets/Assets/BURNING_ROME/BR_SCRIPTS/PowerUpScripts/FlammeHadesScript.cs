using System.Collections;
using System.Collections.Generic;
using testScript;
using UnityEngine;

public class FlammeHadesScript : MonoBehaviour,IExplodable
{
    private void OnTriggerEnter(Collider other)
    {
        var input = other.GetComponent<PlayerPowerUps>();
        if (input != null)
        {
            input.AdesFire();
            Destroy(gameObject);
        }
    }

    public void Explode()
    {
        Destroy(gameObject);
    }
}

