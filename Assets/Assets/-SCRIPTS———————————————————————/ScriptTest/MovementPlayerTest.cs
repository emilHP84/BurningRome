using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementPlayerTest : MonoBehaviour
{
    private DropComponent dropComponent;

    public float speed;
    private Vector3 direction;
    private bool KeyPressed = false; // Par défaut, aucune touche n'est active
    public bool isBlocked = false;
    private KeyCode activeKey = KeyCode.None; // Stocke la touche actuellement pressée
    private Vector2 moveInput = Vector2.zero;

    [SerializeField] private int bombStock = 1; // Peut être augmenté par power-up
    private List<float> bombCooldowns = new(); // Stocke les timestamps de recharge
    [SerializeField] private int explosionRange = 1; // portée initiale
    public int ExplosionRange => explosionRange;

    public GameObject dropGameObject;
    private GameObject actualBomb;

    private void Start()
    {
        dropComponent = GetComponent<DropComponent>();
        moveInput = Vector2.zero;
    }
    void Update()
    {
        // Vérifier si une touche est enfoncée et qu'aucune autre touche n'est active
        if (!KeyPressed)
        {
            if (Input.GetKey(KeyCode.D))
            {
                StartMoving(KeyCode.D, Vector3.right);
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                StartMoving(KeyCode.Q, Vector3.left);
            }
            else if (Input.GetKey(KeyCode.Z))
            {
                StartMoving(KeyCode.Z, Vector3.forward);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                StartMoving(KeyCode.S, Vector3.back);
            }
        }


        if (KeyPressed && Input.GetKeyUp(activeKey))
        {
            KeyPressed = false;
            activeKey = KeyCode.None;
            isBlocked = false;
        }

        if (moveInput.magnitude >= 0.1f)
        {
            Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
            transform.Translate(move * speed * Time.deltaTime);
        }

    }

    private void FixedUpdate()
    {

        if (KeyPressed)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Debug.Log($"position : {rb.position}");
            Debug.Log($"direction : {direction}");
            Debug.Log($"nouvelle position : {rb.position + direction * speed * Time.fixedDeltaTime}");
            rb.MovePosition( rb.position + direction * speed * Time.fixedDeltaTime);
        }
    }

    #region PLAYER CONTROLLER
    //-----------------------------------------------------------------------//
    //-PLAYER-SYSTEM-CONTROLLER----------------------------------------------//
    //-----------------------------------------------------------------------//

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        //Debug.Log("OnMove reçu : " + moveInput);
    }

    void StartMoving(KeyCode key, Vector3 dir)
    {
        activeKey = key; // Mémoriser la touche enfoncée
        direction = dir; // Mettre à jour la direction
        KeyPressed = true; // Empêcher d'autres touches d'agir
    }

    public void PlayerBlocked()
    {
        isBlocked = true;
    }
    //-----------------------------------------------------------------------//
    #endregion


    #region BOMBE CONTROLLER
    //-----------------------------------------------------------------------//
    //-BOMB-SYSTEM-CONTROLLER------------------------------------------------//
    //-----------------------------------------------------------------------//

    public void OnDropBomb(InputValue value)
    {

        bombStock = Mathf.Clamp(bombStock, 1, 100);

        if (value.isPressed && CanPlaceBomb())
        {
            GameObject bomb = dropComponent.DroppingObject
               (
                    dropGameObject, new Vector3(Mathf.RoundToInt(transform.position.x), 3, Mathf.RoundToInt(transform.position.z)), transform.rotation, null
               );

            bomb.GetComponent<BombManager>().SetExplosionRange(explosionRange);


            PlaceBomb();
        }
    }

    private bool CanPlaceBomb()
    {
        bombCooldowns.RemoveAll(t => Time.time >= t);
        return bombCooldowns.Count < bombStock;
    }

    private void PlaceBomb()
    {
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

    public void AdesFire()
    {
        dropGameObject.GetComponent<BombManager>().DelayExplose = 5;
        dropGameObject.GetComponent<BombManager>().ChangeBombStateToAdes(true);
    }

    public void FakeBomb(int amount)
    {
        bombStock -= amount;
    }
    //-----------------------------------------------------------------------//
    #endregion
}

