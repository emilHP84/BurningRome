using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BlessedByGodsScript : MonoBehaviour, IExplodable
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
        if (time >= 2)
        {
            invulnerability = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        var input = other.GetComponent<PlayerPowerUps>();
        if (input != null)
        {
            input.Invincibility();
            Destroy(gameObject);
        }
    }

    public void Explode()
    {
        if (!invulnerability)
        {
            Destroy(gameObject);
        }
    }
}
