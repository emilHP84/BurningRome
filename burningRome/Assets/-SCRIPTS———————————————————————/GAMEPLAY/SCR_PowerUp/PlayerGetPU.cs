using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetPU : MonoBehaviour
{
    [SerializeField] GameObject vfx;
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
        AllPowerUpManager input = other.GetComponent<AllPowerUpManager>();
        if(input != null )
        {
            Instantiate(vfx, transform.position, Quaternion.identity);
        }
    }
}
