using UnityEngine;
using Rewired;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] int playerID = 0;
    Player player;
    bool canMove = false;
    CharacterController character=>GetComponent<CharacterController>();
    [SerializeField] float speed = 5f;

    void OnEnable()
    {
        player = ReInput.players.GetPlayer(playerID);
        EVENTS.OnGameplay += Activate;
        EVENTS.OnGameplayExit += Disable;
        if (GAME.MANAGER.CurrentState==State.gameplay) Activate();
    }

    void OnDisable()
    {
        EVENTS.OnGameplay -= Activate;
        EVENTS.OnGameplayExit -= Disable;
    }


    void Update()
    {
        if (canMove && GAMEPLAY.access.PlayerControl) Movement();
    }

    void Activate()
    {
        Debug.Log("Activate Player "+playerID);
        canMove = true;
    }

    void Disable()
    {
        canMove = false;
    }

    public void Movement()
    {
        Vector3 movement = Vector3.zero;
        movement.x = player.GetAxisRaw("MoveHorizontal");
        movement.z = player.GetAxisRaw("MoveVertical");
        if (movement.sqrMagnitude>1f) movement.Normalize();
        movement*=Time.deltaTime*speed;
        movement.y = -0.1f;
        character.Move(movement);
    }

} // FIN DU SCRIPT
