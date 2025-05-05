using UnityEngine;

[System.Serializable]
public class AudioClipExtended
{
    public AudioClip clip;
    [Range(0,1f)]public float volume = 1f;
    public bool looping = false;
} // SCRIPT END
