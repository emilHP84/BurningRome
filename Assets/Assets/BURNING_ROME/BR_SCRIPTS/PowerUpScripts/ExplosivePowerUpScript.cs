using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosivePowerUpScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var input = other.GetComponent<MovementPlayerTest>();
        if (input != null)
        {
            input.AdesFire();
            Destroy(gameObject);
        }
    }
}
