using System.Collections;
using System.Collections.Generic;
using testScript;
using UnityEngine;

public class RangeUpScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        TestInputController input = other.GetComponent<TestInputController>();
        if (input != null)
        {
            input.AddExplosionRange(1); //  On augmente la port�e
            Destroy(gameObject);        //  Et on d�truit le power-up
        }
    }
}
