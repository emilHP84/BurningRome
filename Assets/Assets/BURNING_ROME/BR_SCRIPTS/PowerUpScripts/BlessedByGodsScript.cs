using System.Collections;
using System.Collections.Generic;
using System.Threading;
using testScript;
using UnityEngine;

public class BlessedByGodsScript : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        var input = other.GetComponent<TestInputController>();
        if (input != null)
        {
            input.AddInvicibility(true);
            Destroy(gameObject);
        }
    }
}
