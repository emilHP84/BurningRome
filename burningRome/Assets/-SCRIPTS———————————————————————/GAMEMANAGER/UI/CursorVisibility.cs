using UnityEngine;
using Rewired;

public class CursorVisibility : MonoBehaviour
{
    [SerializeField] bool VisibleDuringGameplay = false;
    [SerializeField] bool VisibleDuringMenu = true;
    [SerializeField] bool KeyboardAsGamepadController = true;
    [SerializeField] bool ConfinedWhenVisible = false;
    bool usingMouse;
    [SerializeField] GameObject customCursor;

    void OnApplicationFocus(bool focus)
    {
        if (focus) SwitchControlMode();
    }

    void OnEnable()
    {
        EVENTS.OnInitialization += CursorInvisibleLocked;
        EVENTS.OnMenu += OnMenu;
        EVENTS.OnGameplay += OnGameplay;
        EVENTS.OnGamePause += OnMenu;
        EVENTS.OnWaiting += CursorInvisibleLocked;
    }

    void OnDisable()
    {
        EVENTS.OnInitialization -= CursorInvisibleLocked;
        EVENTS.OnMenu -= OnMenu;
        EVENTS.OnGameplay -= OnGameplay;
        EVENTS.OnGamePause -= OnMenu;
        EVENTS.OnWaiting -= CursorInvisibleLocked;
    }

    void SwitchControlMode()
    {
        switch (GAME.MANAGER.CurrentState)
        {
            case State.gameplay: OnGameplay(); break;
            case State.menu: OnMenu(); break;
            case State.paused: OnMenu(); break;
            case State.waiting: CursorInvisibleLocked(); break;
        }
    }

    void OnGameplay()
    {
        if (!usingMouse || VisibleDuringGameplay==false) CursorInvisibleLocked();
        else CursorVisible();
    }

    void OnMenu()
    {
        if (!usingMouse || VisibleDuringMenu==false) CursorInvisible();
        else CursorVisible();
    }

    void CursorVisible()
    {
        if (customCursor!=null)
        {
            customCursor.SetActive(true);
            Cursor.visible = false;
        }
        else Cursor.visible = true;
        
        Cursor.lockState = ConfinedWhenVisible ? CursorLockMode.Confined : CursorLockMode.None;
        EVENTS.InvokeMouseCursorUnlocked();
    }

    void CursorInvisible()
    {
        if (customCursor!=null) customCursor.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void CursorInvisibleLocked()
    {
        CursorInvisible();
        Cursor.lockState = CursorLockMode.Locked;
        EVENTS.InvokeMouseCursorLocked();
    }




    void Update()
    {
        if (MouseIsUsed() != usingMouse) // CHANGE IN MOUSE USAGE
        {
            if (usingMouse) // Was using mouse, but touched keyboard or controller
            {
                if (!KeyboardAsGamepadController && ReInput.controllers.GetLastActiveControllerType()==ControllerType.Keyboard) return; // if keyboard has been touched and keyboard is not seen as a controller for this game, then stay in mouse mode
                usingMouse = false;
                SwitchControlMode();
                EVENTS.InvokeController();
            }
            else // Was not using mouse, but now is
            {
                usingMouse = true;
                SwitchControlMode();
                EVENTS.InvokeMouse();
            }
        }
    }


    bool MouseIsUsed()
    {
        return ReInput.controllers.GetLastActiveControllerType()==ControllerType.Mouse;
    }


} // SCRIPT END
