using System.Collections;
using System.Collections.Generic;
using testScript;
using UnityEngine;

public class GrosBoutonRougeScript : MonoBehaviour,IExplodable
{
    public void Explode()
    {
        Destroy(gameObject);        //  Et on détruit le power-up
    }

    private void OnTriggerEnter(Collider other)
    {
        var input = other.GetComponent<PlayerPowerUps>();
        if (input != null)
        {
            input.RedButton();
            Destroy(gameObject);
        }
    }
}
