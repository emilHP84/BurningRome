using UnityEngine;

public class BombUpScript : MonoBehaviour, IExplodable
{
    [SerializeField] GameObject fxDestroyed, fxPickedUp;
    float time;
    bool invulnerability;
    void Start()
    {
        invulnerability = true;
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1.5) 
        {
            invulnerability = false;
        }
    }
    public void Explode()
    {
        if (!invulnerability)
        {
            if (fxDestroyed) Instantiate(fxDestroyed, transform.position, transform.rotation);
            Destroy(gameObject);        //  Et on détruit le power-up
        }
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
