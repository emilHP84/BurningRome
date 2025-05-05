using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FlammeHadesScript : MonoBehaviour,IExplodable
{
    private void OnTriggerEnter(Collider other)
    {
        var input = other.GetComponent<PlayerPowerUps>();
        if (input != null)
        {
            input.HadesFire();
            Destroy(gameObject);
        }
    }

    public void Explode()
    {
        Destroy(gameObject);
    }
}

