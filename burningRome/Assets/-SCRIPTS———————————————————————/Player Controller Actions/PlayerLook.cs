using UnityEngine;
using Rewired;

public class PlayerLook : MonoBehaviour
{
    [Range(0,3)][SerializeField] int playerID = 0;
    [SerializeField] Vector2 lookSpeed = Vector2.one;
    [SerializeField] Vector2 mouseSensitivity = Vector2.one *0.1f;
    [Range(0,90f)][SerializeField]float maxVerticalAngle = 85f;
    public bool CanLook{get{return canLook;}set{canLook=value;}}
    bool canLook = false;
    public Transform HorizontalPivot {get{return horizontalPivot;}}
    public Transform VerticalPivot {get{return verticalPivot;}}
    [SerializeField]Transform horizontalPivot,verticalPivot;
    float vAngle,hAngle = 0;
    Player player;
    float inputX,inputY;
    bool MouseInput
    {
    get{
        if (player.id==0 && (ReInput.controllers.Mouse.GetAxisRaw(0)!=0 || ReInput.controllers.Mouse.GetAxisRaw(1)!=0)) return true;
        else return false;
        }
    }
    HealthScript health => GetComponent<HealthScript>();

    void Awake()
    {
        EVENTS.OnGameplay += EnableLook;
        EVENTS.OnGameplayExit += DisableLook;
        EVENTS.OnMouseSensitivityChange += ChangeMouseSensitivity;
        health.OnSpawn += ResetLook;
    }

    void OnDestroy()
    {
        EVENTS.OnGameplay -= EnableLook;
        EVENTS.OnGameplayExit -= DisableLook;
        EVENTS.OnMouseSensitivityChange -= ChangeMouseSensitivity;
        health.OnSpawn -= ResetLook;
    }



    void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
    }


    void EnableLook()
    {
        GetAnglesFromPivots();
        CanLook = true;
    }

    void DisableLook()
    {
        CanLook = false;
    }

    void ChangeMouseSensitivity(float desired)
    {
        mouseSensitivity = Vector2.one * desired;
    }


    void Update()
    {
        if (CanLook)
        {
            if (horizontalPivot) GetHorizontalAngle();
            if (verticalPivot) GetVerticalAngle();
            ApplyToCamera();
        }
    }

    void GetHorizontalAngle()
    {
        inputX = player.GetAxis("LookHorizontal") * lookSpeed.x;
        if (MouseInput) inputX *= mouseSensitivity.x;
        vAngle +=inputX;
        while (vAngle>360f) vAngle-=360f;
        while (vAngle<-360f) vAngle -= 360f;
    }

    void GetVerticalAngle()
    {
        inputY = -player.GetAxis("LookVertical") * lookSpeed.y;
        if (MouseInput) inputY *= mouseSensitivity.y;
        hAngle += inputY;
        hAngle = Mathf.Clamp(hAngle,-maxVerticalAngle,maxVerticalAngle);
    }

    void ApplyToCamera()
    {
        horizontalPivot.localEulerAngles = Vector3.up * vAngle;
        verticalPivot.localEulerAngles = Vector3.right * hAngle;
    }

    void GetAnglesFromPivots()
    {
        vAngle = horizontalPivot.localEulerAngles.y;
        hAngle = verticalPivot.localEulerAngles.x;
    }

    void ResetLook()
    {
        vAngle = hAngle = 0;
        horizontalPivot.localEulerAngles = verticalPivot.localEulerAngles = Vector3.zero;
    }



} // SCRIPT END
