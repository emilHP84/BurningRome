using UnityEngine;
using Rewired;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] int playerID = 0;
    Player player;
    bool canMove = false;
    CharacterController character=>GetComponent<CharacterController>();
    [SerializeField] float speed = 5f;
    PlayerAnim anims => GetComponent<PlayerAnim>();
    Vector3 lastPos;

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
        if (GAMEPLAY.access.CurrentState==GameplayState.joining) anims.PlaySpawn();
        else if (canMove && GAMEPLAY.access.PlayerControl) Movement();
        else anims.PlayIdle();
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
        WalkAnimations();
    }


    void WalkAnimations()
    {
        Vector3 realMovement = transform.localPosition - lastPos;
        realMovement.y = 0;
        if (realMovement.sqrMagnitude==0)
        {
            if (GAMEPLAY.access.CurrentState==GameplayState.joining) anims.PlaySpawn();
            else anims.PlayIdle();
        }
        else
        {
            if (Mathf.Abs(realMovement.z)>=Mathf.Abs(realMovement.x))
            {
                if (realMovement.z>0) anims.PlayMoveUp(); else anims.PlayMoveDown();
            }
            else
            {
                if (realMovement.x>0) anims.PlayMoveRight(); else anims.PlayMoveLeft();
            }
        }
    }

    void LateUpdate()
    {
        lastPos = transform.localPosition;
    }

} // FIN DU SCRIPT
