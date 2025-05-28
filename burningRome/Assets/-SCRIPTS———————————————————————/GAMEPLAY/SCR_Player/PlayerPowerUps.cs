using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    PlayerBombing bombSystem => GetComponent<PlayerBombing>();
    PlayerManager manager => GetComponent<PlayerManager>();
    [SerializeField] GameObject positivePickup, negativePickup, oneTimePickup;

    public void BombUp(int amount)
    {
        bombSystem.ChangeMaxBomb(bombSystem.MaxBomb + amount);
        if (positivePickup) Instantiate(positivePickup, transform.position, Quaternion.identity);
    }

    public void RangeUp(int amount)
    {
        bombSystem.ChangeRange(bombSystem.ExplosionRange + amount);
        if (positivePickup) Instantiate(positivePickup, transform.position, Quaternion.identity);
    }

    public void BombDown(int amount)
    {
        bombSystem.ChangeMaxBomb(bombSystem.MaxBomb - amount);
        Debug.Log("MAXBOMB = " + bombSystem.MaxBomb);
        if (positivePickup) Instantiate(negativePickup, transform.position, Quaternion.identity);
    }

    public void RedButton()
    {
        bombSystem.AddManualDetonation();
        if (positivePickup) Instantiate(oneTimePickup, transform.position, Quaternion.identity);
    }

    public void Invincibility()
    {
        manager.InvincibilityFor(10f);
        if (positivePickup) Instantiate(positivePickup, transform.position, Quaternion.identity);
    }

    public void HadesFire()
    {
        bombSystem.NextBombIsHadesFire();
        if (positivePickup) Instantiate(oneTimePickup, transform.position, Quaternion.identity);
    }

    public void Piercing()
    {
        bombSystem.BombPercing();
        if (positivePickup) Instantiate(oneTimePickup, transform.position, Quaternion.identity);
    }



} // FIN DU SCRIPT
