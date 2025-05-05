using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    [SerializeField]SO_Playlist playlist;

    void OnEnable()
    {
        MUSIC.PLAYER.SetPlaylist(playlist);
    }


} // SCRIPT END
