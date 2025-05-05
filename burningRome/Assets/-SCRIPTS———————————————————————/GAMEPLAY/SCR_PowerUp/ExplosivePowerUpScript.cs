using UnityEngine;

public class ExplosivePowerUpScript : MonoBehaviour, IExplodable
{
    [SerializeField] GameObject fxDestroyed, fxPickedUp;

    public void Explode()
    {
        if (fxDestroyed) Instantiate(fxDestroyed,transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        var input = other.GetComponentInParent<PlayerPowerUps>();
        if (input != null)
        {
            if (fxPickedUp) Instantiate(fxPickedUp,transform.position, transform.rotation);
            input.Piercing();
            Destroy(gameObject);
        }
    }
}
