using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    [SerializeField]SO_Playlist playlist;

    void OnEnable()
    {
        SetDesiredPlaylist();
        EVENTS.OnInitialization += SetDesiredPlaylist;
    }

    private void OnDisable()
    {
        EVENTS.OnInitialization -= SetDesiredPlaylist;
    }

    private void SetDesiredPlaylist()
    {
        if (MUSIC.PLAYER) MUSIC.PLAYER.SetPlaylist(playlist);
    }


} // SCRIPT END
