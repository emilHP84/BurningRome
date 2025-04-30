using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System.Linq;
using UnityEngine.UI;

public class CustomPlayerJoin : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;
    public int ActivePlayers { get { return activePlayers; } }
    int activePlayers = 0;
    Controller[] activeControllers = new Controller[4];
    bool isListening = false;

    void OnAwake()
    {
        EVENTS.OnGameStart += GameStartCreatePlayers;
    }

    private void OnDestroy()
    {
        EVENTS.OnGameStart -= GameStartCreatePlayers;
    }
    private void Start()
    {
        ResetAllControllers();
        GetComponent<Button>().interactable = false;
        ReInput.players.GetPlayer(0).controllers.AddController(ControllerType.Mouse, 0, true);
    }

    void ResetAllControllers()
    {
        activePlayers = 0;
        activeControllers = new Controller[4];
        foreach (Player player in ReInput.players.GetPlayers())
        {
            player.controllers.ClearAllControllers();
            player.isPlaying = false;
        }
    }

    private void Update()
    {
        if (GAME.MANAGER.CurrentState == State.menu && activeControllers[0] == null) // Tant qu'on est dans le menu et que le premier joueur n'a pas de controller
        {
            GetFirstPlayer(); // On attend un premier input pour lui attribuer un controller
        }
       
        if (isListening) ListenPlayers(); // Les inputs provoquent l'assignation des nouveaux controllers aux 3 joueurs restants jusqu'à ce que la battle démarre
        
    }

    void GetFirstPlayer()
    {
        if (ReInput.controllers.GetAnyButton())
        {
            Controller lastActive = ReInput.controllers.GetLastActiveController();
            if (lastActive.type == ControllerType.Mouse) return;
            AssignControllerToPlayer(0, lastActive);
            isListening = true;
        }
    }

    void AssignControllerToPlayer(int playerID, Controller controller)
    {
        activePlayers++;
        ReInput.players.GetPlayer(playerID).controllers.AddController(controller, true);
        activeControllers[playerID] = controller;
        Debug.Log("🎮" + controller.name + " to Player" + playerID);
    }

    void ListenPlayers()
    {
        if (ReInput.controllers.GetAnyButton())
        {
            Controller lastActive = ReInput.controllers.GetLastActiveController();
            if (!activeControllers.Contains(lastActive) && lastActive.type!=ControllerType.Mouse)
            {
                for (int i = 1; i < 4; i++)
                {
                    if (activeControllers[i] == null)
                    {
                        GetComponent<Button>().interactable = true;
                        AssignControllerToPlayer(i, lastActive);
                        ReInput.players.GetPlayer(i).isPlaying = true;
                        if (i == 3) isListening = false;
                        break;
                    }
                }
            }
        }
    }


    void GameStartCreatePlayers()
    {
        for (int i = 0; i < activePlayers; i++)
        {
            GameObject obj = Instantiate(playerPrefabs[i], spawnPoints[i].position, Quaternion.identity, this.gameObject.transform);
            obj.SetActive(false);
            playerPrefabs[i] = obj;
        }
    }
} // FIN DU SCRIPT










   /* public GameObject[] playerPrefabs;
    private int playerIndex = 0;
    private List<Gamepad> usedGamepads = new List<Gamepad>();
    public Transform[] spawnPoints;

    private DropComponent dropComponent;
    public GameObject dropGameObject;
    void Start()
    {
        
        InstantiateEntity();
    }

    void InstantiateEntity()
    {
        for(int i = 0; i < 4; i++) 
        {
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
        }
    }*/

