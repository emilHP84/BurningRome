using UnityEngine;
[CreateAssetMenu(fileName = "Music Playlist", menuName = "ScriptableObjects/Music Playlist Scriptable Object", order = 1)]
public class SO_Playlist : ScriptableObject
{
    public Playlist playlist;

    private void OnValidate()
    {
        if (playlist.tracks != null)
        {
            for (int i = 0; i < playlist.tracks.Length; i++)
            {
                if (playlist.tracks[i] != null && playlist.tracks[i].clip != null)
                {
                    playlist.tracks[i].clipTitle = playlist.tracks[i].clip.name;
                }
            }
        }
    }
} // SCRIPT END

[System.Serializable]
public class Playlist
{
    public bool looping = true;
    public bool randomizeTracks = false;
    public TimeScale timeScale = TimeScale.Unscaled;
    [Range(0,5f)]public float fadeInDuration, fadeOutDuration = 0;
    public Music[] tracks = new Music[1];
}

