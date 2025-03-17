using UnityEngine;
using Rewired;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    [Range(0,3)][SerializeField] int playerID = 0; // Rewired plugin
    public bool CanRun{get{return canRun;}set{canRun=value;}}
    bool canRun=false;
    public bool CanFall{get{return canFall;}set{canFall=value;}}
    bool canFall=false;
    [Range(0,100f)][SerializeField] float maxSpeed = 5f;
    Vector2 inputs;
    Vector3 movement, movementRelativeToCam;
    CharacterController character => GetComponent<CharacterController>();
    PlayerLook lookScript => GetComponent<PlayerLook>();
    Player player; // Rewired plugin


    void Awake()
    {
        EVENTS.OnGameplay += EnableMoveSet;
        EVENTS.OnGameplayExit += DisableMoveSet;
    }

    void OnDestroy()
    {
        EVENTS.OnGameplay -= EnableMoveSet;
        EVENTS.OnGameplayExit -= DisableMoveSet;
    }

    void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
        if (GAME.MANAGER.CurrentState==State.gameplay) EnableMoveSet();
    }

    void EnableMoveSet()
    {
        CanRun = CanFall = true;
    }

    void DisableMoveSet()
    {
        CanRun = CanFall = false;
    }



    void Update()
    {
        HorizontalMovement();
        VerticalMovement();
        ApplyMovement();
    }

    void GetInputs()
    {
        inputs.x = player.GetAxis("MoveHorizontal");
        inputs.y = player.GetAxis("MoveVertical");
        if (inputs.sqrMagnitude>1f) inputs.Normalize(); // avoir diagonals bigger than 1 (pythagoras)
    }

    void VerticalMovement()
    {
        movement.y= CanFall ? -60f : 0; // very simple fall with constant speed
    }

    void HorizontalMovement()
    {
        GetInputs();
        movement.x = CanRun ? inputs.x * maxSpeed : 0;
        movement.z = CanRun ? inputs.y * maxSpeed : 0;
    }

    void ApplyMovement()
    {
        if (lookScript!=null) MovementRelativeToCamera();
        character.Move(movement * Time.deltaTime);
    }

    void MovementRelativeToCamera()
    {
        movementRelativeToCam = lookScript.HorizontalPivot.right * movement.x; // Horizontal (left-right) movement relative to camera
        movementRelativeToCam += lookScript.HorizontalPivot.forward * movement.z; // Longitudinal (forward-backward) movement relative to camera
        movementRelativeToCam += transform.up * movement.y; // Vertical movement relative to character
        movement = movementRelativeToCam;
    }


} // SCRIPT END
