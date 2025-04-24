using System.Collections;
using System.Collections.Generic;
using testScript;
using UnityEngine;

public class FakePowerUpScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FakePowerUp()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        var input = other.GetComponent<TestInputController>();
        if (input != null)
        {
            input.FakeBomb(1);
            Destroy(gameObject);
        }
    }
}
