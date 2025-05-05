using UnityEngine;

public class EnableRigidbodyOnGameplayOnly : MonoBehaviour
{
    [SerializeField] bool disableOnStart = true;
    Rigidbody[] rbs => GetComponentsInChildren<Rigidbody>();

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
        foreach (Rigidbody body in rbs) body.isKinematic = !wanted;
    }

} // SCRIPT END
