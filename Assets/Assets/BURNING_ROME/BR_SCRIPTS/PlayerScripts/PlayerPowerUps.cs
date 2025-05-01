using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{
    PlayerBombing bombSystem => GetComponent<PlayerBombing>();
    PlayerManager manager => GetComponent<PlayerManager>();

        public void BombUp(int amount)
        {
            bombSystem.ChangeMaxBomb(bombSystem.MaxBomb+amount);
        }

        public void RangeUp(int amount)
        {
            bombSystem.ChangeRange(bombSystem.ExplosionRange+amount);
        }

        public void BombDown(int amount)
        {
            bombSystem.ChangeMaxBomb(bombSystem.MaxBomb-amount);
        }

        public void RedButton()
        {
            bombSystem.AddManualDetonation();
        }

        public void Invincibility()
        {
            manager.InvincibilityFor(10f);
        }



} // FIN DU SCRIPT
