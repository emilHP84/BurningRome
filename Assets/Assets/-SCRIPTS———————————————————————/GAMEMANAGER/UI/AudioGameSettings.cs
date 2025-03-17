using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioGameSettings : MonoBehaviour
{
    public static AudioGameSettings access;
    //[SerializeField][Range(0,1f)]
    float generalVolume, sfxVolume, musicVolume, voiceVolume, uiVolume;
    [SerializeField] Slider generalVolumeSlider, sfxVolumeSlider, uiVolumeSlider,musicVolumeSlider, voiceVolumeSlider;
    [SerializeField] AudioMixer sfxMixer, musicMixer, voiceMixer, uiMixer;
    float sfxVolumeModifier = 1f;
    public float SFXVolumeModifier
    {
        get{return sfxVolumeModifier;}
        set{sfxVolumeModifier = Mathf.Clamp(value,0,1f); ApplySFXVolume();}
    }


    void Awake()
    {
        access = this;
        generalVolumeSlider.onValueChanged.AddListener(delegate {SaveGeneralVolume(); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate {SaveSFXVolume(); });
        uiVolumeSlider.onValueChanged.AddListener(delegate {SaveUIVolume(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate {SaveMusicVolume(); });
        voiceVolumeSlider.onValueChanged.AddListener(delegate {SaveVoiceVolume(); });

        AudioConfiguration audioConfig = AudioSettings.GetConfiguration();
        audioConfig.speakerMode = AudioSettings.driverCapabilities;
        AudioSettings.Reset(audioConfig);
    }

    void Start()
    {
        Debug.Log("Audio Drivers "+AudioSettings.driverCapabilities);
        //AudioSettings.speakerMode = AudioSettings.driverCapabilities;
        LoadSettings();
    }


    void SaveThisVolume(ref float value,Slider slider,string pref)
    {
        value = slider.value;
        PlayerPrefs.SetFloat(pref,value);
        ApplySettings();
    }

    void SaveGeneralVolume()
    {
        SaveThisVolume(ref generalVolume,generalVolumeSlider,"VolumeGeneral");
    }

    void SaveSFXVolume()
    {
        SaveThisVolume(ref sfxVolume,sfxVolumeSlider,"VolumeSFX");
    }

    void SaveUIVolume()
    {
        SaveThisVolume(ref uiVolume,uiVolumeSlider,"VolumeUI");
    }

    void SaveMusicVolume()
    {
        SaveThisVolume(ref musicVolume,musicVolumeSlider,"VolumeMusic");
    }

    void SaveVoiceVolume()
    {
        SaveThisVolume(ref voiceVolume,voiceVolumeSlider,"VolumeVoices");
    }


    void SaveSettings()
    {
        SaveGeneralVolume();
        SaveSFXVolume();
        SaveMusicVolume();
        SaveVoiceVolume();
        SaveUIVolume();
    }

    public void LoadSettings()
    {
        LoadThisSetting("VolumeGeneral", ref generalVolume, 1f);
        LoadThisSetting("VolumeSFX", ref sfxVolume, 0.7f);
        LoadThisSetting("VolumeVoices", ref voiceVolume, 0.7f);
        LoadThisSetting("VolumeMusic", ref musicVolume, 0.7f);
        LoadThisSetting("VolumeUI", ref uiVolume, 0.7f);
        UpdateInterface();
    }


    void LoadThisSetting(string key, ref float  value, float defaultValue)
    {
        if (PlayerPrefs.HasKey(key)) value = PlayerPrefs.GetFloat(key); else value = defaultValue;
    }



    void UpdateInterface()
    {
        generalVolumeSlider.value = Mathf.Clamp(generalVolume, 0, 1f);
        musicVolumeSlider.value = Mathf.Clamp(musicVolume, 0.0001f, 1f);
        sfxVolumeSlider.value = Mathf.Clamp(sfxVolume, 0.0001f, 1f);
        voiceVolumeSlider.value = Mathf.Clamp(voiceVolume, 0.0001f, 1f);
        uiVolumeSlider.value = Mathf.Clamp(uiVolume, 0.0001f, 1f);
        ApplySettings();
    }

    void ApplySettings()
    {
        AudioListener.volume = generalVolume*generalVolume;
        ApplySFXVolume();
        musicMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume)*20f);
    }

    void ApplySFXVolume()
    {
        sfxMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume * sfxVolumeModifier)*20f);
        voiceMixer.SetFloat("VoiceVolume", Mathf.Log10(voiceVolume * sfxVolumeModifier)*20f);
        uiMixer.SetFloat("UIVolume", Mathf.Log10(uiVolume * sfxVolumeModifier)*20f);
    }

    public void ResetVolumeToDefault()
    {
        generalVolume = 1f;
        sfxVolume = 0.7f;
        uiVolume = 0.7f;
        voiceVolume = 0.7f;
        musicVolume = 0.7f;
        UpdateInterface();
        SaveSettings();
    }


} // FIN DU SCRIPT
