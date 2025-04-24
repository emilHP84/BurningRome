using System.Collections.Generic;
using UnityEngine;

namespace testScript
{
    [RequireComponent(typeof(DropComponent))]
    public class TestInputController : MonoBehaviour
    {
        [SerializeField] private int bombStock = 1; // Peut être augmenté par power-up
        private List<float> bombCooldowns = new(); // Stocke les timestamps de recharge
        [SerializeField] private int explosionRange = 1; // portée initiale
        public int ExplosionRange => explosionRange; // accès en lecture seule
        private DropComponent dropComponent;
        public GameObject dropGameObject;
        void Start()
        {
            dropComponent = GetComponent<DropComponent>();
        }


        void Update()
        {
            DropBomb();
            bombStock = Mathf.Clamp(bombStock, 1, 100);
        }
        void DropBomb()
        {
            if (Input.GetKeyDown(KeyCode.Space) && CanPlaceBomb())
            {
                GameObject bomb = dropComponent.DroppingObject
               (
                    dropGameObject,new Vector3(Mathf.RoundToInt(transform.position.x), 3, Mathf.RoundToInt(transform.position.z)),transform.rotation,null
               );

                bomb.GetComponent<BombManager>().SetExplosionRange(explosionRange);


                PlaceBomb(); //  Déclenche le cooldown
            }
        }

        private bool CanPlaceBomb()
        {
            // Supprimer les bombes dont le cooldown est terminé
            bombCooldowns.RemoveAll(t => Time.time >= t);

            // Si on a encore du stock utilisable, on peut poser une bombe
            return bombCooldowns.Count < bombStock;
        }

        private void PlaceBomb()
        {
            // 1. Instancier ta bombe ici (à ta manière actuelle)

            // 2. Ajouter un timer de 5 secondes dans le cooldown
            bombCooldowns.Add(Time.time + 5f);
        }

        public void AddBombStock(int amount)
        {
            bombStock += amount;
            Debug.Log("+ 1 Bombe dans le stock !");
        }

        public void AddExplosionRange(int amount)
        {
            explosionRange += amount;
            Debug.Log("+ 1 de range ");
        }

        public void FakeBomb(int amount)
        {
            bombStock -= amount;
        }

    }
}