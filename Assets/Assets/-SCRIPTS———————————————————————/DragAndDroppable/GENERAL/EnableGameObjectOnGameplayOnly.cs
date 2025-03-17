using UnityEngine;

public class EnableGameObjectOnGameplayOnly : MonoBehaviour
{
    [SerializeField] bool disableOnStart = false;
    void Awake()
    {
        EVENTS.OnGameplay += EnableElement;
        EVENTS.OnGameplayExit += DisableElement;
    }

    void OnDestroy()
    {
        EVENTS.OnGameplay -= EnableElement;
        EVENTS.OnGameplayExit -= DisableElement;
    }

    void Start()
    {
        if (disableOnStart) DisableElement();
    }

    void EnableElement()
    {
        ToggleElement(true);
    }

    void DisableElement()
    {
        ToggleElement(false);
    }

    void ToggleElement(bool wanted)
    {
        gameObject.SetActive(wanted);
    }


} // SCRIPT END
