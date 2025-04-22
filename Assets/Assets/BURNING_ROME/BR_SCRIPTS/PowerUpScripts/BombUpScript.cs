using System.Collections;
using System.Collections.Generic;
using testScript;
using UnityEngine;

public class BombUpScript : MonoBehaviour, IDetect
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BombUp()
    {

    }

    public void OnDetectionWith(IDetect detect)
    {
        if (detect is PlayerManager)
        {
            BombUp();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var input = other.GetComponent<TestInputController>();
        if (input != null)
        {
            input.AddBombStock(1); // On ajoute +1 bombe
            Destroy(gameObject);   // On détruit le power-up
        }
    }   
}
