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
        public bool IsInvicible = false;
        private float timerinvicible;
        public bool IsRedButton = false;
        private BombManager bombManager;
        private GameObject bomb1 = null;
        void Start()
        {
            bombManager = GetComponent<BombManager>();
            dropComponent = GetComponent<DropComponent>();
        }


        void Update()
        {
            DropBomb();
            TimerInvicibility();
            bombStock = Mathf.Clamp(bombStock, 1, 100);
        }
        void DropBomb()
        {
            if (IsRedButton == false)
            {
                if (Input.GetKeyDown(KeyCode.Space) && CanPlaceBomb())
                {
                    GameObject bomb = dropComponent.DroppingObject
                   (
                        dropGameObject, new Vector3(Mathf.RoundToInt(transform.position.x), 3, Mathf.RoundToInt(transform.position.z)), transform.rotation, null
                   );

                    bomb.GetComponent<BombManager>().SetExplosionRange(explosionRange);


                    PlaceBomb(); //  Déclenche le cooldown
                }
            }
            else if (IsRedButton == true)
            {
               
                if (Input.GetKeyDown(KeyCode.Space))
                {                    
                    GameObject bomb = dropComponent.DroppingObject

                 (
                        dropGameObject, new Vector3(Mathf.RoundToInt(transform.position.x), 3, Mathf.RoundToInt(transform.position.z)), transform.rotation, null
                   );
                    bomb.GetComponent<BombManager>().ChangeBombState(true);
                    bomb1 = bomb;

                    bomb.GetComponent<BombManager>().SetExplosionRange(explosionRange);
                    
                }
                if (Input.GetKey(KeyCode.A))
                {
                    Debug.Log("Okay");
                    bomb1.GetComponent<BombManager>().Explode();
                    bomb1.GetComponent<BombManager>().ChangeBombState(false);
                    IsRedButton = false;
                    Destroy( bomb1,0.5f );
                }
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
            if (IsRedButton == false)
            {
                bombCooldowns.Add(Time.time + 5f);
            }

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


        public void AddInvicibility(bool Active)
        {
            IsInvicible = Active;
            Debug.Log("Power-up d'invincibilité activé");
        }

        public void Invicibility(GameObject other)
        {
            var destructible = other.GetComponent<IDestructible>();
            if (destructible != null)
            {
                destructible.DestroySelf();
                Debug.Log("Objet détruit !");
            }
        }

        public void TimerInvicibility()
        {
            if (IsInvicible)
            {
                timerinvicible += Time.deltaTime;
                if (timerinvicible >= 10)
                {
                    IsInvicible = false;
                    timerinvicible = 0;
                }
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("collision");
            if (IsInvicible)
            {
                Invicibility(collision.gameObject);
                Debug.Log("collison reçu");
            }
        }

        public void GrosBoutonRouge(bool Active)
        {
            IsRedButton = Active;
        }
    }
}