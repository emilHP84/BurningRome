using UnityEngine;
using UnityEngine.UI;

public class CursorCustom : MonoBehaviour
{
    public static CursorCustom script;
    [SerializeField] Transform neutralCursor,interactCursor;

    RectTransform rt => transform.GetChild(0).GetComponent<RectTransform>();
    RectTransform canvasRT => GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    Camera uiCam=> GetComponentInParent<Camera>();
    Vector2 mousePos;

    void OnEnable()
    {
        EVENTS.OnUIElementSelected += SetCursorInteract;
        EVENTS.OnUIElementUnselect += SetCursorNeutral;
        SetCursorNeutral(null);
    }

    void OnDisable()
    {
        EVENTS.OnUIElementSelected -= SetCursorInteract;
        EVENTS.OnUIElementUnselect -= SetCursorNeutral;
    }

    void Awake()
    {
        script = this;
        SetCursorNeutral(null);
    }


    public void SetCursorNeutral(Selectable unselected)
    {
        HideAllCursors();
        neutralCursor.gameObject.SetActive(true);
    }

    public void SetCursorInteract(Selectable element)
    {
        HideAllCursors();
        interactCursor.gameObject.SetActive(true);
    }

    void HideAllCursors()
    {
        neutralCursor.gameObject.SetActive(false);
        interactCursor.gameObject.SetActive(false);
    }

    void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT,Input.mousePosition,uiCam,out mousePos );
        rt.anchoredPosition=mousePos;
    }
} // SCRIPT END
