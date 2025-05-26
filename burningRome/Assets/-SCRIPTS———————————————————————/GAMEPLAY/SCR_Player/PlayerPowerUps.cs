using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    PlayerBombing bombSystem => GetComponent<PlayerBombing>();
    PlayerManager manager => GetComponent<PlayerManager>();
    public void BombUp(int amount)
    {
        bombSystem.ChangeMaxBomb(bombSystem.MaxBomb + amount);
        GameObject Go = Instantiate(new GameObject(), transform.position, Quaternion.identity);
        AudioSource aus = gameObject.AddComponent<AudioSource>();
        }

    public void RangeUp(int amount)
    {
        bombSystem.ChangeRange(bombSystem.ExplosionRange + amount);
    }

    public void BombDown(int amount)
    {
        bombSystem.ChangeMaxBomb(bombSystem.MaxBomb - amount);
        Debug.Log("MAXBOMB = "+bombSystem.MaxBomb);
    }

    public void RedButton()
    {
        bombSystem.AddManualDetonation();
    }

    public void Invincibility()
    {
        manager.InvincibilityFor(2f);
    }

    public void HadesFire()
    {

        bombSystem.NextBombIsHadesFire();
    }

    public void Piercing()
    {
        bombSystem.BombPercing();
    }



} // FIN DU SCRIPT
