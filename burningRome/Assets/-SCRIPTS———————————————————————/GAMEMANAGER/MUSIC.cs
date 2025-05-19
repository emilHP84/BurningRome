using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MUSIC : MonoBehaviour
{
    private bool m_hasFocus = true;
    
    
    public void SetPlaylist(SO_Playlist desired)
    {
        SetPlaylist(desired,false);
    }

    public void SetPlaylist(SO_Playlist desired, bool forceStart)
    {
        if (desired==null || desired.playlist.tracks.Length<1)
        {
            if (currentPlaylist!=null) FadeOutToSilence(); 
        }
        else
        {
            if (forceStart==false && desired.playlist==currentPlaylist) return;
            currentPlaylistIndex = 0;
            SwitchPlaylist(desired);
        }
    }

    public void Play()
    {
        if (players[current].clip !=null)
        {
            players[current].Play();
            StartCoroutine(PlayAndCheckTrackEnd());
        }
    }

    public void Stop()
    {
        players[current].Stop();
        paused = true;
        Debug.Log("🎸MUSIC STOP");
    }

    public void Pause()
    {
        if (players[current].clip !=null)
        {
            players[current].Pause();
            paused = true;
        }
    }

    public void NextSong()
    {
        TrackEnded();
    }


    public void SetMusicSpeed(float desired)
    {
        speed = desired;
        ApplyFinalMusicPitch();
    }















    public static MUSIC PLAYER;
    AudioSource[] players;
    int current = 0;
    Coroutine fadeInRoutine, fadeOutRoutine, playRoutine;
    [Header("MUSIC MIXER")]
    [SerializeField] AudioMixerGroup mixer;
    float speed = 1f;
    Music currentMusic;
    int currentPlaylistIndex = 0;
    bool paused = false;
    [Header("MONITORING ONLY")]
    [SerializeField]Playlist oldPlaylist;
    [SerializeField]Playlist currentPlaylist;

    void Awake()
    {
        if (PLAYER!=null)
        {
            Destroy(this);
            return;
        }
        PLAYER = this;
        AudioSource[] alreadyExisting = GetComponentsInChildren<AudioSource>();
        for (int i=0;i<alreadyExisting.Length;i++) Destroy(alreadyExisting[i]);
        CreateSource("MUSIC PLAYER 1");
        CreateSource("MUSIC PLAYER 2");
        players = GetComponentsInChildren<AudioSource>();
    }

    void CreateSource(string sourceName)
    {
        Transform source = new GameObject("Music Source").transform;
        source.name = sourceName;
        source.parent = transform;
        source.localPosition = source.localEulerAngles = Vector3.zero;
        source.localScale = Vector3.one;
        AudioSource a = source.gameObject.AddComponent<AudioSource>();
        a.rolloffMode = AudioRolloffMode.Linear;
        a.maxDistance = 11f;
        a.minDistance = 10f;
        a.loop = false;
        a.outputAudioMixerGroup = mixer;
        a.spatialBlend = 0;
        a.dopplerLevel = 0;
        a.playOnAwake = false;
    }

    void OnEnable()
    {
        EVENTS.OnTimeScaleChange += CheckIfMusicFollowTimeScale;
    }

    void OnDisable()
    {
        EVENTS.OnTimeScaleChange -= CheckIfMusicFollowTimeScale;
    }

    void PlayMusic(Music desired)
    {
        if (desired.clip != players[current].clip) StartTrack(desired);
    }


    void StartTrack(Music desired)
    {
        if (players[current].clip == desired.clip && players[current].isPlaying)
        {
            Debug.Log("Le morceau est déjà en cours de lecture, pas besoin de le redémarrer.");
            return;
        }

        Debug.Log("Démarrage du morceau : " + desired.clip.name);
        if (desired.clip != null)
        {
            players[current].loop = false;
            players[current].clip = desired.clip;
            players[current].volume = desired.volume;
            players[current].Play();
            if (playRoutine != null) StopCoroutine(playRoutine);
            playRoutine = StartCoroutine(PlayAndCheckTrackEnd());
            ApplyFinalMusicPitch();
            currentMusic = desired;
            paused = false;
        }
    }


    void SwitchPlaylist(SO_Playlist newPlaylist)
    {
        if (newPlaylist==null || newPlaylist.playlist.tracks.Length<1)
        {
            FadeOutToSilence();
            return;
        }
        Debug.Log("🎵 PLAYLIST "+newPlaylist.name);
        Playlist tempPlist = newPlaylist.playlist;
        if (tempPlist.randomizeTracks)
        {
            for (int t = 0; t < tempPlist.tracks.Length; t++)
            {
                Music tmp = tempPlist.tracks[t];
                int r = Random.Range(t,  tempPlist.tracks.Length);
                tempPlist.tracks[t] = tempPlist.tracks[r];
                tempPlist.tracks[r] = tmp;
            }
        }

        if (currentPlaylist!=null) oldPlaylist = currentPlaylist; // saved previous playlist in oldplaylist
        currentPlaylist = tempPlist; // new current playlist


        if (fadeInRoutine!=null) StopCoroutine(fadeInRoutine);
        if (fadeOutRoutine!=null) StopCoroutine(fadeOutRoutine);
        current = current<1 ? 1:0; // Swapping the AudioSources to make the cross fade effect
        StartTrack(currentPlaylist.tracks[currentPlaylistIndex]);
        players[current].volume = 0;
        if (currentPlaylist!=null) fadeInRoutine = StartCoroutine(FadingPlayer(currentPlaylist.fadeInDuration, currentMusic.volume,players[current]));
        if (oldPlaylist!=null) fadeOutRoutine = StartCoroutine(FadingPlayer(oldPlaylist.fadeOutDuration, 0, current==0 ? players[1]:players[0] ));
    }


    IEnumerator FadingPlayer(float duration, float targetVolume, AudioSource player)
    {
        float chrono = 0;
        float step = Mathf.Abs(targetVolume - player.volume);
        
        while (chrono < duration)
        {   
            chrono += Time.unscaledDeltaTime;  
            player.volume = Mathf.MoveTowards(player.volume,targetVolume,step*Time.unscaledDeltaTime/duration);
            yield return null;
        }

        player.volume = Mathf.Clamp(targetVolume,0,1f);
    }


    IEnumerator PlayAndCheckTrackEnd()
    {
        paused = false;
        while (players[current].isPlaying || (Time.timeScale==0 &&currentPlaylist.timeScale!=TimeScale.Unscaled))
        {
            if(paused) yield break;
            yield return null;
        }
        TrackEnded();
    }


    void FadeOutToSilence()
    {
        players[current].loop = true;
        if (fadeOutRoutine!=null) StopCoroutine(fadeOutRoutine);
        fadeOutRoutine = StartCoroutine(FadingPlayer(currentPlaylist.fadeOutDuration, 0, players[current]));
        currentPlaylist = null;
    }

    void TrackEnded()
    {
        if (currentPlaylist == null) 
        {
            Stop();
            return;
        }
        
        if (!m_hasFocus) 
        {
            Debug.Log("La fenêtre n'a pas le focus, annulation de la reprise.");
            return;
        }

        currentPlaylistIndex += 1;
        if (currentPlaylist.tracks.Length > currentPlaylistIndex)
        {
            PlayMusic(currentPlaylist.tracks[currentPlaylistIndex]);
        }
        else if (currentPlaylist.looping)
        {
            Debug.Log("Dernière piste atteinte, redémarrage de la playlist.");
            currentPlaylistIndex = 0;
            StartTrack(currentPlaylist.tracks[currentPlaylistIndex]);
        }
        else
        {
            currentPlaylistIndex = 0;
            Stop();
        }
    }

    void CheckIfMusicFollowTimeScale(float timeScale)
    {
        if (currentMusic!=null) ApplyFinalMusicPitch();
    }

    void ApplyFinalMusicPitch()
    {
        players[current].pitch = 1f;
        if (currentPlaylist.timeScale==TimeScale.GameSpeed) players[current].pitch = TimeScaleManager.GameTimeScale;
        else if (currentPlaylist.timeScale==TimeScale.UnityTimeScale) players[current].pitch = Time.timeScale;
    }

    int OtherPlayer()
    {
        return current==0 ? 1 : 0;
    }
    
    void OnApplicationFocus(bool focus)
    {
        m_hasFocus = focus;
        if (!focus)
        {
            Debug.Log("Fenêtre du jeu a perdu le focus.");
            Pause(); // Mettre en pause la musique
        }
        else
        {
            Debug.Log("Fenêtre du jeu a repris le focus.");
            Resume(); // Reprendre la musique
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        m_hasFocus = !pauseStatus;
        if (pauseStatus)
        {
            Debug.Log("Jeu mis en pause.");
            Pause(); // Mettre en pause la musique
        }
        else
        {
            Debug.Log("Jeu repris.");
            Resume(); // Reprendre la musique
        }
    }

    public void Resume()
    {
        if (players[current].clip != null && paused)
        {
            players[current].UnPause();
            paused = false;
            Debug.Log("🎸MUSIC RESUME");
        }
    }


} // FIN DU SCRIPT

[System.Serializable]
public class Music
{
    [HideInInspector] public string clipTitle;
    public AudioClip clip;
    [Range(0,1f)]public float volume = 1f;

    public Music()
    {
        volume = 1f;
    }
}
