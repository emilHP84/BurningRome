using UnityEngine;

public class BombUpScript : MonoBehaviour, IExplodable
{
    [SerializeField] GameObject fxDestroyed, fxPickedUp;

    public void Explode()
    {
        if (fxDestroyed) Instantiate(fxDestroyed,transform.position, transform.rotation);
        Destroy(gameObject);        //  Et on détruit le power-up
    }


    private void OnTriggerEnter(Collider other)
    {
        var input = other.GetComponent<PlayerPowerUps>();
        if (input != null)
        {
            if (fxPickedUp) Instantiate(fxPickedUp,transform.position, transform.rotation);
            input.BombUp(1); // On ajoute +1 bombe
            Destroy(gameObject);   // On d�truit le power-up
        }
    }   
}
