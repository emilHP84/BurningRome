using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivePowerUpScript : MonoBehaviour, IExplodable
{
    public void Explode()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        var input = other.GetComponentInParent<PlayerPowerUps>();
        if (input != null)
        {
            input.Perçing();
            Destroy(gameObject);
        }
    }
}
