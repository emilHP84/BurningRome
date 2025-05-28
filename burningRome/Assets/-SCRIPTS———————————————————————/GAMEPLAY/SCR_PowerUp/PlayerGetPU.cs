using UnityEngine;

public class PlayerGetPU : MonoBehaviour
{
    [SerializeField] GameObject vfx;


    private void OnTriggerEnter(Collider other)
    {
        AllPowerUpManager input = other.GetComponent<AllPowerUpManager>();
        if(input != null )
        {
            Instantiate(vfx, transform.position, Quaternion.identity);
        }
    }
}
