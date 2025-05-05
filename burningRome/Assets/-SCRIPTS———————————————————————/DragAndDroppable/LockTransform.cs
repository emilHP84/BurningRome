using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
[ExecuteInEditMode]
public class LockTransform : MonoBehaviour
{
    public bool lockPosition = true;
    public bool lockRotation = true;
    public bool lockScale = true;
    public bool lockParenting = true;

    Vector3 lockedPos = Vector3.zero;
    Quaternion lockedRot = Quaternion.identity;
    Vector3 lockedScale = Vector3.one;
    Transform lockedParent;
    int siblingIndex;
    RectTransform rt;
    Vector2 lockedOffsetMin,lockedOffsetMax = Vector2.zero;
    AspectRatioFitter aspectFitter;

    void OnEnable()
    {
        if (EditorApplication.isPlaying)
        {
            this.enabled = false;
            return;
        }
        GetLockedData();
        rt = GetComponent<RectTransform>();
        aspectFitter = GetComponent<AspectRatioFitter>();
    }


    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnPlayModeStart()
    {
        foreach (LockTransform lt in FindObjectsByType<LockTransform>(FindObjectsInactive.Include,FindObjectsSortMode.None)) lt.enabled = false;
    }


    void GetLockedData()
    {
        lockedPos = transform.localPosition;
        lockedRot = transform.localRotation;
        lockedScale = transform.localScale;
        lockedParent = transform.parent;
        siblingIndex = transform.GetSiblingIndex();
        if (rt!=null)
        {
            lockedOffsetMax = rt.offsetMax;
            lockedOffsetMin = rt.offsetMin;
        }
    }


    void Update()
    {   
        if (rt==null)
        {
            if (lockPosition) transform.localPosition = lockedPos;
            else lockedPos = transform.localPosition;
        }
        else if (aspectFitter==null)
        {
            if (lockPosition)
            {
                rt.offsetMax = lockedOffsetMax;
                rt.offsetMin = lockedOffsetMin;
            }
            else
            {
                lockedOffsetMax= rt.offsetMax;
                lockedOffsetMin=rt.offsetMin;
            }
        }

        if (lockRotation) transform.localRotation = lockedRot;
        else lockedRot = transform.localRotation;

        if (lockScale) transform.localScale = lockedScale;
        else lockedScale = transform.localScale;

        if (lockParenting)
        {
            if(transform.parent!=lockedParent)
            {
                transform.SetParent(lockedParent,false);
                transform.SetSiblingIndex(siblingIndex);
            }
        }
        else
        {
            lockedParent = transform.parent;
            siblingIndex = transform.GetSiblingIndex();
        }
    }

}
#endif
