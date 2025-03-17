using UnityEngine;

public class MusicStop : MonoBehaviour
{
    void OnEnable()
    {
        MUSIC.PLAYER.Stop();
    }
} // SCRIPT END
