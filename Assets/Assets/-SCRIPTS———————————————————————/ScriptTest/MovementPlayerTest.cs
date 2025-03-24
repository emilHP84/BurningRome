using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayerTest : MonoBehaviour
{
    public float Speed;
    private Vector3 direction;
    private bool KeyPressed = false; // Par défaut, aucune touche n'est active
    private KeyCode activeKey = KeyCode.None; // Stocke la touche actuellement pressée

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
            transform.Translate(direction * Speed * Time.deltaTime);
        }

        // Si la touche active est relâchée, réinitialiser KeyPressed
        if (KeyPressed && Input.GetKeyUp(activeKey))
        {
            KeyPressed = false;
            activeKey = KeyCode.None;
        }
    }

    // Fonction pour démarrer le déplacement
    void StartMoving(KeyCode key, Vector3 dir)
    {
        activeKey = key; // Mémoriser la touche enfoncée
        direction = dir; // Mettre à jour la direction
        KeyPressed = true; // Empêcher d'autres touches d'agir
    }
}

