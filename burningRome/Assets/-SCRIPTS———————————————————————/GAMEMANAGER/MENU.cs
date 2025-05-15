using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.Properties;
using UnityEngine.UIElements;
using Microsoft.Win32.SafeHandles;

[RequireComponent(typeof(AudioSource))]
public class MENU : MonoBehaviour
{

    //DelegateProperty(apagnan);
    //DragLeaveEvent leaveEvent(SafeHandleZeroOrMinusOneIsInvalid)
    //    apagnan else return ArgumentOutOfRangeException
    //    Destroy(ParticleSystemEmissionType);


    //public AudioSource LEAGUEOFLEGENDS = new AudioSource();




    Player player;
    [SerializeField] GameObject mainCanvas, menusList, inGameList, mainMenu, pauseMenu, settingsMenu, creditsMenu, endMenu; //<-- Reference to all the menus and the in-game UI
    void AllMenus(bool wanted) // <-- Don't forget to add new menus here too
    {
        mainMenu.SetActive(wanted);
        pauseMenu.SetActive(wanted);
        settingsMenu.SetActive(wanted);
        creditsMenu.SetActive(wanted);
        endMenu.SetActive(wanted);
    }



    public static MENU SCRIPT;
    void Awake()
    {
        if (SCRIPT != null)
        {
            Debug.Log("⚠️ ERROR! MULTIPLE MENU SCRIPTS IN SCENE!");
            Destroy(this);
            return;
        }
        SCRIPT = this;
        player = ReInput.players.GetSystemPlayer();
    }
    GraphicRaycaster gRaycaster => mainCanvas.gameObject.GetComponent<GraphicRaycaster>();
    public AudioSource Audio { get { return source; } }
    AudioSource source => GetComponent<AudioSource>();
    List<GameObject> menuHistory = new List<GameObject>();
    Coroutine inputRoutine;

    void OnEnable()
    {
        EVENTS.OnInitialization += ShowMainMenuFromStart;
        EVENTS.OnGameplay += HidePauseMenu;
        EVENTS.OnGamePause += ShowPauseMenu;
        EVENTS.OnGameResume += HidePauseMenu;
        EVENTS.OnMouse += OnMouse;
        EVENTS.OnController += OnController;
        EVENTS.OnMenu += EnableUIInputs;
        EVENTS.OnGamePause += EnableUIInputs;
        EVENTS.OnMenuExit += DisableUIInputs;
        EVENTS.OnGameResume += DisableUIInputs;
        EVENTS.OnEndGame += ShowEndMenu;
    }

    void OnDisable()
    {
        EVENTS.OnInitialization -= ShowMainMenuFromStart;
        EVENTS.OnGameplay -= HidePauseMenu;
        EVENTS.OnGamePause -= ShowPauseMenu;
        EVENTS.OnGameResume -= HidePauseMenu;
        EVENTS.OnMouse -= OnMouse;
        EVENTS.OnController -= OnController;
        EVENTS.OnMenu -= EnableUIInputs;
        EVENTS.OnGamePause -= EnableUIInputs;
        EVENTS.OnMenuExit -= DisableUIInputs;
        EVENTS.OnGameResume -= DisableUIInputs;
        EVENTS.OnEndGame -= ShowEndMenu;
    }

    void Start()
    {
        FollowGameCamera(); // important for UI sounds (the AudioListener is on the game camera)
    }


    IEnumerator ListenInput()
    {
        while (GAME.MANAGER.CurrentState == State.menu || GAME.MANAGER.CurrentState == State.paused)
        {
            if (player.GetButtonDown("UICancel")) Back();
            CheckNewControllers();
            if (PlayerPressDirections() && NoActiveButtonSelected() && menuHistory.Count > 0) SetFirstSelectedIn(menuHistory[0]);
            yield return null;
        }
    }

    List<Controller> systemControllers = new List<Controller>();

    void CheckNewControllers()
    {
        Controller lastActive = ReInput.controllers.GetLastActiveController();
        if (!player.controllers.ContainsController(lastActive)) player.controllers.AddController(lastActive, false);
    }

    void OnMouse()
    {
        gRaycaster.enabled = true;
        Unselect();
    }

    void OnController()
    {
        gRaycaster.enabled = false;
        Unselect();
    }

    void Unselect()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }



    bool PlayerPressDirections()
    {
        bool value = (player.GetAxis("UIVertical") != 0 || player.GetAxis("UIHorizontal") != 0);
        return value;
    }

    bool NoActiveButtonSelected()
    {
        bool value = (EventSystem.current.currentSelectedGameObject == null);
        return value;
    }



    void FollowGameCamera()
    {
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = Camera.main.transform.parent;
        source.weight = 1f;
        GetComponent<ParentConstraint>().AddSource(source);
    }


    void EnableUIInputs()
    {
        Unselect();
        gRaycaster.enabled = true; // enable mouse clicks;
        EventSystem.current.sendNavigationEvents = true; // enable UI navigation from controller and keyboard
        inputRoutine = StartCoroutine(ListenInput());
    }

    void DisableUIInputs()
    {
        Unselect();
        gRaycaster.enabled = false; // disable mouse clicks;
        EventSystem.current.sendNavigationEvents = false; // disable UI navigation from controller and keyboard
        if (inputRoutine != null) StopCoroutine(inputRoutine);
    }



    public void ShowMainMenuFromStart()
    {
        Black.screen.Show();
        MenusList(true);
        EnableAllMenus();
        HideAllMenus();
        menuHistory.Insert(0, mainMenu);
        StartCoroutine(TransitionToMenu(TransitionType.Iris, mainMenu, 0, 1f, 2f));
        CancelInvoke();
        Invoke("DisableMainMenuAnimations", 2f);
    }

    void DisableMainMenuAnimations()
    {
        foreach (UIAnimBaseClass animable in mainMenu.GetComponentsInChildren<UIAnimBaseClass>()) animable.ChangeEnableMode(UIAnimStartState.Show);
    }


    public void BackToMainMenu()
    {
        EVENTS.InvokeGameOver();
        MenusList(false);
        ClearMenuHistory();
        menuHistory.Insert(0, mainMenu);
        StartCoroutine(TransitionToMenu(TransitionType.Iris, mainMenu, 1f, 1f, 2f));
    }



    public void NewGame()
    {
        StartCoroutine(NewGameRoutine());
    }

    IEnumerator NewGameRoutine()
    {
        MenusList(true);
        SceneLoader.access.LoadScene(SceneLoader.access.CurrentScene, 2f, 1f, 1f, false, 0);
        while (SceneLoader.access.IsLoading) yield return null;
        MenusList(false);
        EVENTS.InvokeGameStart();
    }


    public void LoadNewScene(int desired)
    {
        StartCoroutine(LoadSceneThenHideMenus(desired));
    }

    IEnumerator LoadSceneThenHideMenus(int desired)
    {
        SceneLoader.access.LoadScene(0, 2f, 1f, 1f, false, 0);
        while (SceneLoader.access.IsLoading) yield return null;
        HideAllMenus();
        MenusList(false);
    }



    void MenusList(bool wanted)
    {
        menusList.SetActive(wanted);
        inGameList.SetActive(!wanted);
    }

    void MainCanvas(bool wanted)
    {
        mainCanvas.SetActive(wanted);
    }



    // SETTINGS —————————————————————————————————————————————————————————————————
    public void ShowSettingsFromPause()
    {
        Unselect();
        ClearMenuHistory();
        menuHistory.Add(settingsMenu);
        menuHistory.Add(pauseMenu);
        HideAllMenus();
        MenusList(true);
        settingsMenu.SetActive(true);
    }

    public void ShowSettingsFromMainMenu()
    {
        ShowNextMenu(settingsMenu, 0.5f, 0, 0.5f);
    }
    // —————————————————————————————————————————————————————————————————————————



    // CREDITS —————————————————————————————————————————————————————————————————
    public void ShowCreditsFromMainMenu()
    {
        ShowNextMenu(creditsMenu, 0.5f, 0, 0.5f);
    }

    public void ShowCredits() // à la fin du jeu ?
    {

    }
    // —————————————————————————————————————————————————————————————————————————




    public void Back()
    {
        Debug.Log("BACK");
        Unselect();
        if (GAME.MANAGER.CurrentState == State.paused)
        {
            if (menuHistory.Count < 2)
            {
                MenusList(false);
                GAME.MANAGER.Resume();
            }

            else
            {
                if (menuHistory[1] == pauseMenu) MenusList(false);
                menuHistory[1].SetActive(true);
                menuHistory[0].SetActive(false);
                menuHistory.RemoveAt(0);
            }
        }
        else if (GAME.MANAGER.CurrentState == State.menu) ShowPreviousMenu(0.5f, 0, 0.5f);
    }




    void ShowPauseMenu()
    {
        MenusList(false);
        pauseMenu.SetActive(true);
        menuHistory.Insert(0, pauseMenu);
    }

    void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
        MenusList(false);
    }

    void HideAllMenus()
    {
        AllMenus(false);
    }

    void EnableAllMenus()
    {
        AllMenus(true);
    }



    // void ShowMenuInstantNoHistory(GameObject desiredMenu)
    // {
    //     ClearMenuHistory();
    //     ShowNextMenu(desiredMenu,0,0,0);
    // }

    void ShowPreviousMenu(float fadeInDuration, float blackDuration, float fadeOutDuration)
    {
        if (menuHistory.Count < 2) return;
        GameObject desiredMenu = menuHistory[1];
        Debug.Log("Previous menu > " + desiredMenu.name + "    From " + menuHistory[0].name);
        menuHistory.RemoveAt(0);
        StartCoroutine(TransitionToMenu(TransitionType.Fade, desiredMenu, fadeInDuration, blackDuration, fadeOutDuration));
    }



    void ShowNextMenu(GameObject desiredMenu, float fadeInDuration, float blackDuration, float fadeOutDuration)
    {
        menuHistory.Insert(0, desiredMenu);
        StartCoroutine(TransitionToMenu(TransitionType.Fade, desiredMenu, fadeInDuration, blackDuration, fadeOutDuration));
    }



    public void ClearMenuHistory()
    {
        menuHistory.Clear();
    }




    IEnumerator TransitionToMenu(TransitionType transition, GameObject desiredMenu, float fadeInDuration, float blackDuration, float fadeOutDuration)
    {
        Unselect();
        Black.screen.OpenIris();
        Black.screen.Hide();
        GAME.MANAGER.SwitchTo(State.waiting);
        switch (transition)
        {
            case TransitionType.Fade: Black.screen.FadeIn(fadeInDuration); break;
            case TransitionType.Iris: Black.screen.IrisIn(fadeInDuration); break;
            case TransitionType.None: break;
        }
        while (Black.screen.IsWorking) yield return null;
        HideAllMenus();
        desiredMenu.SetActive(true);
        MenusList(true);
        yield return new WaitForSecondsRealtime(blackDuration);

        switch (transition)
        {
            case TransitionType.Fade: Black.screen.FadeOut(fadeOutDuration); break;
            case TransitionType.Iris: Black.screen.IrisOut(fadeOutDuration); break;
            case TransitionType.None: break;
        }
        while (Black.screen.Progression < 0.8f) yield return null;
        GAME.MANAGER.SwitchTo(State.menu);
        while (Black.screen.IsWorking) yield return null;
    }


    void SetFirstSelectedIn(GameObject menu)
    {
        Debug.Log("Trying to select something in " + menu.name);
        Selectable[] selectables = menu.GetComponentsInChildren<Selectable>(false);
        for (int i = 0; i < selectables.Length; i++)
        {
            if (selectables[i].gameObject.activeInHierarchy && selectables[i].IsInteractable())
            {
                EventSystem.current.SetSelectedGameObject(selectables[i].gameObject);
                Debug.Log(EventSystem.current.currentSelectedGameObject.name + " sélectionné");
                return;
            }
        }
    }





    // QUIT GAME FUNCTION ———————————————————————————————————————————————————————————————————
    Coroutine quitRoutine;
    public void QuitApp()
    {
        if (quitRoutine == null) quitRoutine = StartCoroutine(QuittingRoutine());
    }

    IEnumerator QuittingRoutine()
    {
        GAME.MANAGER.SwitchTo(State.waiting);
        Black.screen.IrisIn(1f);
        yield return new WaitForSecondsRealtime(1.2f);
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_WEBGL
        Application.ExternalEval("document.location.reload(true);");
#endif
    }
    // ——————————————————————————————————————————————————————————————————————————————————————

    void ShowEndMenu(EventArgs e)
    {
        StartCoroutine(ShowiEndGame());
    }

    IEnumerator ShowiEndGame()
    {
        Black.screen.IrisIn(1f);
        yield return new WaitForSeconds(2f);
        menusList.SetActive(true);
        mainMenu.SetActive(false);
        endMenu.SetActive(true);
        GAME.MANAGER.SwitchTo(State.menu);
        SetFirstSelectedIn(endMenu);
        Black.screen.IrisOut(1f);
    }
} // SCRIPT END


