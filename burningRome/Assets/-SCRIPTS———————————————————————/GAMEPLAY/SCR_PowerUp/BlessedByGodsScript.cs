using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BlessedByGodsScript : MonoBehaviour, IExplodable
{

    // Update is called once per frame
    void Update()
    {

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
        Destroy(gameObject);
    }
}
