using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeUpScript : MonoBehaviour, IExplodable
{
    public void Explode()
    {
        Destroy(gameObject);        //  Et on détruit le power-up
    }

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        PlayerPowerUps input = other.GetComponentInParent<PlayerPowerUps>();
        if (input != null)
        {
            input.RangeUp(1);           //  On augmente la portée
            Destroy(gameObject);        //  Et on détruit le power-up
        }
    }
}
