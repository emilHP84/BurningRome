using System.Collections;
using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    [SerializeField]SO_Playlist playlist;
    [SerializeField] GameObject mainMenu; //

    void OnEnable()
    {
        //SetDesiredPlaylist();
        EVENTS.OnInitialization += TrySetDesiredPlaylist;
    }

    private void OnDisable()
    {
        EVENTS.OnInitialization -= TrySetDesiredPlaylist;
    }

    //private void SetDesiredPlaylist()
    //{
    //    if (mainMenu.activeSelf)
    //    {
    //        if (MUSIC.PLAYER) MUSIC.PLAYER.SetPlaylist(playlist);
    //    }
    //}


    private void TrySetDesiredPlaylist()
    {
        StartCoroutine(DelayedCheck());
    }

    IEnumerator DelayedCheck()
    {
        yield return null; // attend 1 frame
        if (mainMenu != null && mainMenu.activeInHierarchy)
        {
            if (MUSIC.PLAYER) MUSIC.PLAYER.SetPlaylist(playlist);
        }
    }

} // SCRIPT END
