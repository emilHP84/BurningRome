// ℹ️ HANDLES DOTWEEN ANIMATIONS AND SOUNDS FOR UNITY BUTTONS

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;


#if UNITY_EDITOR
using TMPro;
#endif

[RequireComponent(typeof(Button))]
public class ButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [Range(0.8f,2f)][SerializeField] float hoverScale = 1.2f;
    [SerializeField] Ease scaleEasing = Ease.InOutQuad;
    [Range(0f,1f)][SerializeField] float scaleDuration = 0.2f;
    [SerializeField] AudioClipExtended buttonHover, buttonExit, buttonClick;
    [SerializeField]GameObject fxButtonClick;
    Transform _transform => transform;
    Button button => GetComponent<Button>();
    [SerializeField] bool pulseEffect = true;
    [SerializeField] bool unselectAfterClicked = false;

    #if UNITY_EDITOR
    TextMeshProUGUI tmpro => GetComponentInChildren<TextMeshProUGUI>();
    Text t => GetComponentInChildren<Text>();
    string buttonName;
    void Start(){buttonName = tmpro==null ? t.text : tmpro.text;}
    #endif
    bool clickEffect = false;
    bool selected = true;

    void SelectButtonEffect()
    {
        selected = true;
        EVENTS.InvokeUIElementSelected(button);
        if (buttonHover.clip) PlaySound(buttonHover.clip,buttonHover.volume);
        _transform.DOScale(hoverScale,scaleDuration).SetEase(scaleEasing).SetUpdate(true).OnComplete(SelectedPulseEffect);
    }

    void UnselectButtonEffect()
    {
        selected = false;
        EVENTS.InvokeUIElementUnselect(button);
        EventSystemUnselectThis();
        StartCoroutine(WaitForUnselectEffect());
    }

    IEnumerator WaitForUnselectEffect()
    {
        while (clickEffect) yield return null;
        _transform.DOKill();
        _transform.DOScale(1f,scaleDuration).SetEase(scaleEasing).SetUpdate(true);
    }

    void ClickButtonEffect()
    {
        clickEffect = true;
        if (buttonClick.clip) PlaySound(buttonClick.clip,buttonClick.volume);
        if (fxButtonClick) Instantiate(fxButtonClick,transform.position, transform.rotation);
        Vector3 currentScale =  _transform.localScale;
        _transform.DOKill();
        transform.localScale = currentScale;
        _transform.DOPunchScale(-Vector3.one*0.5f,scaleDuration,3,1f).SetUpdate(true).OnComplete(ClickEffectDone);
    }

    void ClickEffectDone()
    {
        clickEffect = false;
    }



    void SelectedPulseEffect()
    {
        if (button.interactable==false || pulseEffect==false) return;
        _transform.DOScale(hoverScale+0.05f,0.2f).SetEase(Ease.InOutCubic).SetUpdate(true).SetLoops(-1,LoopType.Yoyo);
    }




    void OnEnable()
    {
        clickEffect = false;
        _transform.DOKill();
        ResetScale();
        if (button.onClick.GetPersistentEventCount()<1) button.interactable=false;
    }

    void OnDisable()
    {
        _transform.DOKill();
        ResetScale();
    }

    void EventSystemUnselectThis()
    {
        if (EventSystem.current.currentSelectedGameObject==gameObject &&EventSystem.current.alreadySelecting==false) EventSystem.current.SetSelectedGameObject(null);
    }


	
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable==false) return;
        #if UNITY_EDITOR
        Debug.Log("👆Hovering "+buttonName);
        #endif
        SelectButtonEffect();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (button.interactable==false) return;
        #if UNITY_EDITOR
        Debug.Log("👇✅Clicked on "+buttonName);
        #endif
        if (unselectAfterClicked) EventSystemUnselectThis();
        ClickButtonEffect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (button.interactable==false) return;
        if (buttonExit.clip) PlaySound(buttonExit.clip,buttonExit.volume);
        #if UNITY_EDITOR
        Debug.Log("✊Leave "+buttonName);
        #endif
        UnselectButtonEffect();
    }

    void ResetScale()
    {
        _transform.DOKill();
        _transform.localScale = Vector3.one;
    }


    void PlaySound(AudioClip clip)
    {
        PlaySound(clip, 1f);
    }

    void PlaySound (AudioClip clip, float volume)
    {
        MENU.SCRIPT.Audio.PlayOneShot(clip,volume);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (selected==false) SelectButtonEffect();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (selected) UnselectButtonEffect();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        if (button.interactable==false) return;
        if (unselectAfterClicked) EventSystemUnselectThis();
        ClickButtonEffect();
    }




} // FIN DU SCRIPT

