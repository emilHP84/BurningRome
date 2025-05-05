using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombUpScript : MonoBehaviour, IExplodable
{
    public void Explode()
    {
        Destroy(gameObject);        //  Et on détruit le power-up
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        var input = other.GetComponent<PlayerPowerUps>();
        if (input != null)
        {
            input.BombUp(1); // On ajoute +1 bombe
            Destroy(gameObject);   // On détruit le power-up
        }
    }   
}
