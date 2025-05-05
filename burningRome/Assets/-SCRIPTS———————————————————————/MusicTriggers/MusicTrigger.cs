using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [SerializeField]SO_Playlist playlist;

    void OnTriggerEnter(Collider col)
    {
        MUSIC.PLAYER.SetPlaylist(playlist);
    }

} // SCRIPT END
