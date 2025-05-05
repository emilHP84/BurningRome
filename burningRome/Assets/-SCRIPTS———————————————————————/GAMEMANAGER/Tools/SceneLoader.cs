using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader access;
    public bool IsLoading{get{return loading;}}
    bool loading = false;
    public int CurrentScene{get{return loadedScene;}}
    int loadedScene;
    List<int> additiveScenes;

    void Awake()
    {
        if (access!=null)
        {
            Debug.Log("⚠️ ERROR! MULTIPLE SCENE LOADERS IN SCENE!");
            Destroy(gameObject);
            return;
        }
        access = this;
        loadedScene = SceneManager.GetActiveScene().buildIndex;
        additiveScenes = new List<int>();
    }


    public void LoadScene(string sceneName)
    {
        LoadScene(GetScene(sceneName));
    }

    public void LoadScene(int index)
    {
        LoadScene(index,1f,1f,1f,false,0);
    }

    public void LoadSceneAfterDelay(int index, float delay)
    {
        LoadScene(index,1f,1f,1f,false,delay);
    }

    public void LoadSceneAfterDelay(string sceneName, float delay)
    {
        LoadSceneAfterDelay(GetScene(sceneName), delay);
    }

    public void LoadSceneInstant(string sceneName)
    {
        LoadSceneInstant(GetScene(sceneName));
    }

    public void LoadSceneInstant(int index)
    {
        LoadScene(index,0,0,0,false,0);
    }

    public void LoadSceneInstantAdditive(string sceneName)
    {
        LoadSceneInstantAdditive(GetScene(sceneName));
    }

    public void LoadSceneInstantAdditive(int index)
    {
        LoadScene(index,0,0,0,true,0);
    }

    public void LoadScene(int index, float fadeIn, float minWait, float fadeOut, bool additive, float delay)
    {
        StartCoroutine(LoadingScene(index, fadeIn, minWait, fadeOut, additive, delay));
    }




    int GetScene(string sceneName)
    {
        return SceneManager.GetSceneByName(sceneName).buildIndex;
    }


    IEnumerator LoadingScene(int index, float fadeIn, float minWait, float fadeOut, bool additive, float delay)
    {
        if (loading) { Debug.Log("Scene already loading!"); yield break; }
        loading = true;
        // Start transition
        GAME.MANAGER.SwitchTo(State.waiting);
        if (delay>0) yield return new WaitForSecondsRealtime(delay);
        Black.screen.Hide();
        if (fadeIn>0) Black.screen.IrisIn(fadeIn);
        while (Black.screen.IsWorking) yield return null;
        float timeBeforeLoad = Time.time;
        // Loading scene...
        var asyncLoadLevel = SceneManager.LoadSceneAsync(index, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);
        if (additive) additiveScenes.Add(index); else loadedScene = index;
        while (!asyncLoadLevel.isDone) // Waiting for scene to be loaded
        {
            Debug.Log("Loading scene "+Mathf.FloorToInt(100*asyncLoadLevel.progress)+"%"); // Display percent loaded
            yield return null;
        }
        // Scene is loaded
        EVENTS.InvokeSceneLoaded(index);
        loading = false;
        float waitTimeRemaining = minWait-(Time.time-timeBeforeLoad);
        if (waitTimeRemaining>0) yield return new WaitForSecondsRealtime(waitTimeRemaining);
        GAME.MANAGER.SwitchTo(State.gameplay);
        if (fadeOut>0) Black.screen.IrisOut(fadeOut);
        while (Black.screen.IsWorking) yield return null;
        Black.screen.Hide();
    }

} // SCRIPT END
