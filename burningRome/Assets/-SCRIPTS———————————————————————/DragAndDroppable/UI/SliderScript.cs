using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Slider))]
public class SliderScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [Range(0.8f,2f)][SerializeField] float hoverScale = 0.5f;
    [SerializeField] Ease scaleEasing = Ease.InOutQuad;
    [Range(0f,1f)][SerializeField] float scaleDuration = 0.2f;
    Slider slider =>GetComponent<Slider>();
    [SerializeField] AudioClipExtended sliderSelected, sliderMoved, sliderUnselected;
    Transform _transform => transform;
    Transform knob => transform.Find("Handle Slide Area/Handle");
    float startKnobScale=1f;
    [SerializeField] bool pulseEffect = true;



    void Awake()
    {
        startKnobScale = knob.localScale.x;
    }

    void EventSystemUnselectThis()
    {
        if (EventSystem.current.currentSelectedGameObject==gameObject &&EventSystem.current.alreadySelecting==false) EventSystem.current.SetSelectedGameObject(null);
    }


    void SelectSliderKnobEffect()
    {
        EVENTS.InvokeUIElementSelected(slider);
        if (sliderSelected.clip) PlaySound(sliderSelected.clip,sliderSelected.volume);
        knob.DOScale(startKnobScale*hoverScale,scaleDuration).SetEase(scaleEasing).SetUpdate(true).OnComplete(SelectedPulseEffect);
    }

    void UnselectSliderKnobEffect()
    {
        EVENTS.InvokeUIElementUnselect(slider);
        EventSystemUnselectThis();
        if (sliderUnselected.clip) PlaySound(sliderUnselected.clip,sliderUnselected.volume);
        knob.DOKill();
        knob.DOScale(startKnobScale,scaleDuration).SetEase(scaleEasing).SetUpdate(true);
    }

    void MovedSliderKnobEffect()
    {
        if (sliderMoved.clip) PlaySound(sliderMoved.clip,sliderMoved.volume);
    }


    void SelectedPulseEffect()
    {
        if (slider.interactable==false || pulseEffect==false) return;
        knob.DOScale(startKnobScale+hoverScale+0.25f,0.2f).SetEase(Ease.InOutCubic).SetUpdate(true).SetLoops(-1,LoopType.Yoyo);
    }



    public void OnDeselect(BaseEventData eventData)
    {
        UnselectSliderKnobEffect();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectSliderKnobEffect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnselectSliderKnobEffect();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SelectSliderKnobEffect();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        //throw new System.NotImplementedException();
    }


    void PlaySound(AudioClip clip)
    {
        PlaySound(clip, 1f);
    }

    void PlaySound (AudioClip clip, float volume)
    {
        MENU.SCRIPT.Audio.PlayOneShot(clip,volume);
    }

} // SCRIPT END
