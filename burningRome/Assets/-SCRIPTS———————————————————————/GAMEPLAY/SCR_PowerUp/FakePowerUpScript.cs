using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePowerUpScript : MonoBehaviour, IExplodable
{

    float time;
    bool invulnerability;
    void Start()
    {
        invulnerability = true;
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1.5)
        {
            invulnerability = false;
        }
    }

    public void Explode()
    {
        if (!invulnerability)
        {
            Destroy(gameObject);        //  Et on détruit le power-up
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var input = other.GetComponent<PlayerPowerUps>();
        if (input != null)
        {
            input.BombDown(-1);
            Destroy(gameObject);
        }
    }
}
