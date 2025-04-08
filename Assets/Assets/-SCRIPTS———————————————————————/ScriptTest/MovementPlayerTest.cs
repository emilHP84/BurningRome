using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Rewired.ComponentControls.Effects.RotateAroundAxis;
using static UnityEngine.Rendering.DebugUI;

public class MovementPlayerTest : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    private bool KeyPressed = false; // Par défaut, aucune touche n'est active
    private KeyCode activeKey = KeyCode.None; // Stocke la touche actuellement pressée
    private Vector2 moveInput = Vector2.zero;

    private void Start()
    {
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

        // Si une touche est active, continuer le déplacement
        if (KeyPressed)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        // Si la touche active est relâchée, réinitialiser KeyPressed
        if (KeyPressed && Input.GetKeyUp(activeKey))
        {
            KeyPressed = false;
            activeKey = KeyCode.None;
        }

        if (moveInput.magnitude >= 0.1f)
        {
            Vector3 move = new Vector3(moveInput.x, 0f, moveInput.y);
            transform.Translate(move * speed * Time.deltaTime);
        }
    }

    // Fonction pour démarrer le déplacement


    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log("OnMove reçu : " + moveInput);
    }




    void StartMoving(KeyCode key, Vector3 dir)
    {
        activeKey = key; // Mémoriser la touche enfoncée
        direction = dir; // Mettre à jour la direction
        KeyPressed = true; // Empêcher d'autres touches d'agir
    }
}

