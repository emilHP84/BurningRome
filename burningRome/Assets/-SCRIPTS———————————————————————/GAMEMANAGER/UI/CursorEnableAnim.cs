using UnityEngine;
using DG.Tweening;

public class CursorEnableAnim : MonoBehaviour
{
    Vector3 startRot;

    void Awake()
    {
        startRot = transform.localEulerAngles;
    }

    void OnEnable()
    {
        Reset();
        transform.DOLocalRotate(Vector3.up*180f,0.5f,RotateMode.LocalAxisAdd).From(Vector3.zero).SetEase(Ease.OutCubic).SetUpdate(true);
    }

    void OnDisable()
    {
        Reset();
    }

    void Reset()
    {
        transform.DOKill();
        transform.localEulerAngles = startRot;
    }

} // SCRIPT END
