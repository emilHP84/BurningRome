using System;
using UnityEngine;
using UnityEngine.UI;

public class EVENTS
{
    static void LogEventInConsole(string eventName) // Custom message to display in Unity Console when event is invoked
    {
        Debug.Log("ℹ️ "+eventName);
    } 

    public static event Action OnInitialization;
    public static void InvokeInitialization() {LogEventInConsole("Initialization"); OnInitialization?.Invoke();}

    public static event Action<int> OnSceneLoaded;
    public static void InvokeSceneLoaded(int buildIndex) {LogEventInConsole("🏞SceneLoaded"); OnSceneLoaded?.Invoke(buildIndex);}    

    public static event Action OnGameStart;
    public static void InvokeGameStart() {LogEventInConsole("▶️GameStart"); OnGameStart?.Invoke();}

    public static event Action OnAfterGameStart;

    public static void InvokeAfterGameStart() {LogEventInConsole("▶️GameStart"); OnAfterGameStart?.Invoke(); }

    public static event Action OnGameOver;
    public static void InvokeGameOver() {LogEventInConsole("💀GameOver"); OnGameOver?.Invoke();}

    public static event Action OnGamePause;
    public static void InvokeGamePause() {LogEventInConsole("GamePause"); OnGamePause?.Invoke();}

    public static event Action OnGamePauseExit;
    public static void InvokeGamePauseExit() {LogEventInConsole("❌GamePauseExit"); OnGamePauseExit?.Invoke();}

    public static event Action OnGamePauseReboot;
    public static void InvokeGamePauseReboot() {LogEventInConsole("♻️GamePauseReboot"); OnGamePauseReboot?.Invoke();}

    public static event Action OnGameResume;
    public static void InvokeGameResume() {LogEventInConsole("⏯️GameResume"); OnGameResume?.Invoke();}

    public static event Action OnMenu;
    public static void InvokeMenu() {LogEventInConsole("Menu"); OnMenu?.Invoke();}

    public static event Action OnMenuExit;
    public static void InvokeMenuExit() {LogEventInConsole("❌MenuExit"); OnMenuExit?.Invoke();}

    public static event Action OnMenuReboot;
    public static void InvokeMenuReboot() {LogEventInConsole("♻️MenuReboot"); OnMenuReboot?.Invoke();}

    public static event Action OnGameplay;
    public static void InvokeGameplay() {LogEventInConsole("Gameplay"); OnGameplay?.Invoke();}

    public static event Action OnGameplayExit;
    public static void InvokeGameplayExit() {LogEventInConsole("❌GameplayExit"); OnGameplayExit?.Invoke();}

    public static event Action OnGameplayReboot;
    public static void InvokeGameplayReboot() {LogEventInConsole("♻️GameplayReboot"); OnGameplayReboot?.Invoke();}

    public static event Action OnWaiting;
    public static void InvokeWaiting() {LogEventInConsole("Waiting"); OnWaiting?.Invoke();}

    public static event Action OnWaitingExit;
    public static void InvokeWaitingExit() {LogEventInConsole("❌WaitingExit"); OnWaitingExit?.Invoke();}
       
    public static event Action OnWaitingReboot;
    public static void InvokeWaitingReboot() {LogEventInConsole("♻️WaitingReboot"); OnWaitingReboot?.Invoke();}

    public static event Action OnMouse;
    public static void InvokeMouse() {LogEventInConsole("🖱️Mouse"); OnMouse?.Invoke();}
    
    public static event Action OnController;
    public static void InvokeController() {LogEventInConsole("🎮Controller"); OnController?.Invoke();}

    public static event Action OnScreenResChange;
    public static void InvokScreenResChange() {LogEventInConsole("ScreenResChange 🖥️"); OnScreenResChange?.Invoke();}

    public static event Action<float> OnGameRatioChange;
    public static void InvokGameRatioChange(float newRatio) {LogEventInConsole("GameRatioChange "+newRatio); OnGameRatioChange?.Invoke(newRatio);}

    public static event Action<float> OnMouseSensitivityChange;
    public static void InvokeMouseSensitivityChange(float desired) {/*Debug.Log("🖱️MouseSensitivityChange "+desired); */OnMouseSensitivityChange?.Invoke(desired);}

    public static event Action OnMouseCursorLocked;
    public static void InvokeMouseCursorLocked() {Debug.Log("🖱️🔒MouseCursorLocked"); OnMouseCursorLocked?.Invoke();}

    public static event Action OnMouseCursorUnlocked;
    public static void InvokeMouseCursorUnlocked() {Debug.Log("🖱️🔓MouseCursorUnlocked"); OnMouseCursorUnlocked?.Invoke();}

    public static event Action<Selectable> OnUIElementSelected;
    public static void InvokeUIElementSelected(Selectable element) {LogEventInConsole("UIElementSelected "+element.name); OnUIElementSelected?.Invoke(element);}   

    public static event Action<Selectable> OnUIElementUnselect;
    public static void InvokeUIElementUnselect(Selectable element) {LogEventInConsole("UIElementUnselect "+element.name); OnUIElementUnselect?.Invoke(element);}

    public static event Action<float> OnTimeScaleChange;
    public static void InvokeTimeScaleChange(float newScale) {Debug.Log("⏱️TimeScaleChange "+newScale); OnTimeScaleChange?.Invoke(newScale);}

    // <-- Add new game events here

    public static event Action DestroyAllBombs;
    public static void InvokeDestroyAllBombs() { LogEventInConsole("All BOMB DESTROYED");  DestroyAllBombs?.Invoke();}

    public static event Action OnBattleStart;
    public static void InvokeBattleStart() {LogEventInConsole("🏁Battle Start"); OnBattleStart?.Invoke();}

    public static event Action OnSuddenDeathStart;
    public static void InvokeSuddenDeathStart() {LogEventInConsole("⌚Sudden Death!"); OnSuddenDeathStart?.Invoke();}

    public static event Action OnJoiningStart;
    public static void InvokeJoiningStart() {LogEventInConsole("Entering JOINING"); OnJoiningStart?.Invoke();}

    public static event Action OnJoiningDone;
    public static void InvokeJoiningDone() {LogEventInConsole("Finished JOINING"); OnJoiningDone?.Invoke();}

    public static event Action<int> OnPlayerDeath;
    public static void InvokePlayerDeath(int deadID) {LogEventInConsole("☠️Player Death! #"+deadID); OnPlayerDeath?.Invoke(deadID);}

    public static event Action<int> OnPlayerSpawn;
    public static void InvokePlayerSpawn(int ID) { LogEventInConsole("Player Spawn! #" + ID); OnPlayerSpawn?.Invoke(ID); }

    public static event Action<EventArgs> OnEndGame;
    public static void InvokeEndGame() { LogEventInConsole("☠️it's End Game! #"); OnEndGame?.Invoke(new System.EventArgs()); }

    public static event Action<EventArgs> OnVictory;
    public static void InvokeOnVictory() { LogEventInConsole("a player has win! #"); OnVictory?.Invoke(new System.EventArgs()); }

    public static event Action<GameObject,EventArgs> OnCallCamera;
    public static void InvokeOnCallCamera(GameObject obj) { LogEventInConsole("a player has win! #"); OnCallCamera?.Invoke(obj,new System.EventArgs()); }


} // SCRIPT END
