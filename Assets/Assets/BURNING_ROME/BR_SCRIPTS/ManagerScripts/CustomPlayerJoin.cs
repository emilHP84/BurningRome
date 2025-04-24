using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using InputSystem = UnityEngine.InputSystem;

public class CustomPlayerJoin : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    private int playerIndex = 0;
    private List<Gamepad> usedGamepads = new List<Gamepad>();
    public Transform[] spawnPoints;

    private DropComponent dropComponent;
    public GameObject dropGameObject;
    void Start()
    {
        dropComponent = GetComponent<DropComponent>();

        
        InstantiateEntity();
    }

    void InstantiateEntity()
    {
        for(int i = 0; i < 4; i++) 
        {
            Debug.Log("ertyi");
            GameObject obj = Instantiate(playerPrefabs[i], spawnPoints[i].position,Quaternion.identity,this.gameObject.transform);
            obj.SetActive(false);
            playerPrefabs[i] = obj;
        }
    }

    void Update()
    {
        //Emilien : il semblerais que celà fonctionne pas comme voulu avec le victory system que j'ai fait 

        foreach (Gamepad gamepad in Gamepad.all)
        {
            if (!usedGamepads.Contains(gamepad))
            {

                if (gamepad.buttonSouth.wasPressedThisFrame)
                {
                    if (playerIndex < playerPrefabs.Length)
                    {
                        InputDevice device = gamepad;
                        //PlayerInputManager.instance.playerPrefab = playerPrefabs[playerIndex];
                        var player = playerPrefabs[playerIndex];//PlayerInputManager.instance.JoinPlayer(-1, -1, "Gamepad", device);
                        if (player != null)
                        {
                            //player.transform.position = spawnPoints[playerIndex].position;
                            // 🔜 On va ajouter la ligne ici
                            usedGamepads.Add(gamepad);
                            playerIndex++;
                            EVENTS.OnPlayerConnectEventHandler?.Invoke(this, playerIndex);
                        }
                    }
                }
            }

            if (gamepad.buttonEast.wasPressedThisFrame)
            {
                Debug.Log("testatonaton");
                dropComponent.DroppingObject
                    (
                        dropGameObject,
                        new Vector3(transform.position.x, 3, transform.position.z),
                        transform.rotation,
                        null
                    );
            }

        }
    }
}
