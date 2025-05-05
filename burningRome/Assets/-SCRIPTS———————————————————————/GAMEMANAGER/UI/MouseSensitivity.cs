using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivity : MonoBehaviour
{
    public static MouseSensitivity access;
    public float Sensitivity{get{return mouseSensitivity;}set{mouseSensitivity = value; UpdateSlider(); SaveAndInvokeEvent();}}
    float mouseSensitivity = 0.1f;
    const float defaultSensitivity = 0.1f;
    [SerializeField] Slider mouseSensitivitySlider;

    void Awake()
    {
        access = this;
    }

    void OnEnable()
    {
        EVENTS.OnGameplay += SaveAndInvokeEvent;
    }

    void OnDisable()
    {
        EVENTS.OnGameplay -= SaveAndInvokeEvent;
    }

    void Start()
    {
        if(mouseSensitivitySlider)
        {
            mouseSensitivitySlider.onValueChanged.AddListener(delegate {GetValueFromSlider(); });
            mouseSensitivitySlider.minValue = 0;
            mouseSensitivitySlider.maxValue = 1f;
        }
        LoadSetting();
    }

    public void ResetSensitivityToDefault()
    {
        Sensitivity = 0.1f;
    }


    void GetValueFromSlider()
    {
        mouseSensitivity = ConvertSliderToFloat(mouseSensitivitySlider.value);
        SaveAndInvokeEvent();
    }



    void SaveAndInvokeEvent()
    {
        SaveSetting();
        EVENTS.InvokeMouseSensitivityChange(mouseSensitivity);
    }


    void LoadSetting()
    {
        if (PlayerPrefs.HasKey("MouseSensitivity"))mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity"); else mouseSensitivity = defaultSensitivity;
        UpdateSlider();
    }


    void SaveSetting()
    {
        PlayerPrefs.SetFloat("MouseSensitivity",mouseSensitivity);
    }

    void UpdateSlider()
    {
        if(mouseSensitivitySlider) mouseSensitivitySlider.value = ConvertFloatToSlider(mouseSensitivity);
    }


    float ConvertSliderToFloat(float sliderValue)
    {
        return 0.01f * Mathf.Pow(100f,Mathf.Clamp(sliderValue,0,1f));
    }

    float ConvertFloatToSlider(float value)
    {
        return (Mathf.Log10(value) + 2f) * 0.5f;
    }


} // SCRIPT END
