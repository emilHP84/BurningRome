using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class BarHandler : MonoBehaviour
{
    public bool Visible{get{return visible;}set{childRt.gameObject.SetActive(value);}}

    public float FillAmount
    {
        get
        {
            return mayoValue;
        }
        set
        {
            clampedValue  = Mathf.Clamp(value, 0,1f);
            if (clampedValue!=mayoValue)
            {
                SetFillGradient(clampedValue);
                if (autoFlash && clampedValue<mayoValue && flashChrono>0.25f) StartFlash();
                mayoValue = clampedValue;
                if (blinkWhenEmpty)
                {
                    if (clampedValue<=0)
                    {
                        diffChrono = 1000f;
                        blinking = true;
                    }
                    else blinking = false;
                }
            }
        }
    }

    public float CatchupAmount
    {
        get{return ketchupValue;}
    }

    public void Flash()
    {
        StartFlash();
    }

    bool BackgroundBlink
    {
        get{return blinking;}
        set
        {
            if (blinking && value==false) ShowBlinkColor(false);
            if (blinking==false && value) ShowBlinkColor(true);
            blinking = value; 
        }
    }

    public void InstantCatchUp()
    {
        ketchupValue = FillAmount;
        UpdateFillAmount();
    }




    // -------------------------------------------------------------------------------------

    [Header("REMPLISSAGE")]
    [Range(0,1f)][SerializeField] float testFill = 0.5f;
    [Range(0,1f)][SerializeField] float testCatchup = 0.65f;
    [Header("COULEURS")]
    [SerializeField]public Gradient fillColor = new Gradient()
    {
        colorKeys = new GradientColorKey[1]{new GradientColorKey(Color.green, 0)},
        alphaKeys = new GradientAlphaKey[1] {new GradientAlphaKey(1f, 0)}
    };
    [SerializeField] Color catchupColor = Color.red;
    [SerializeField] Color backgroundColor = new Color(0.05f,0.15f,0);
    [SerializeField] Color borderColor = Color.black;
    [Header("FLASH")]
    [SerializeField] bool autoFlash = true;
    [Range(0.01f,0.5f)][SerializeField] float flashDuration = 0.2f;
    [SerializeField] Color flashColor = new Color(1f,1f,1f,0.7f);
    [Range(0f,50f)][SerializeField] float flashScalePercentIncrease = 10f;
    [Header("CLIGNOTEMENT")]
    [SerializeField] bool blinkWhenEmpty = true;
    [SerializeField] Color blinkColor = Color.red * 0.5f;
    [Header("RATTRAPAGE")]
    [Range(0.1f,10f)][SerializeField] float catchupSpeed = 1f;
    [Range(0f,5f)][SerializeField] float catchupDelay = 1f;
    bool blinking = false;

    bool visible = true;
    float mayoValue = 1f;
    float clampedValue;
    float ketchupValue;
    int flashing = 0;
    float flashChrono = 0;
    float diffChrono = 0;
    float ratio;
    Vector3 punchedScale;
    static float unscaledDeltaTime;
    Transform scalable => transform.GetChild(0);
    [HideInInspector][SerializeField]public RectTransform childRt => transform.GetChild(0).GetComponent<RectTransform>();
    Image background => childRt.transform.Find("Background")==null ? childRt.transform.Find("Mask/Background").GetComponent<Image>() : childRt.transform.Find("Background").GetComponent<Image>();
    Image[] borders => childRt.transform.Find("Borders").GetComponentsInChildren<Image>();
    Image fill => background.transform.Find("Mayo").GetComponent<Image>();
    Image catchup => background.transform.Find("Ketchup").GetComponent<Image>();




    void OnValidate()
    {
        if (gameObject.activeSelf==false) return;
        SetFill();
        SetColors();
        visible = gameObject.activeSelf;
    }

    void SetFill()
    {
        fill.fillAmount = testFill;
        catchup.fillAmount = testCatchup;
    }

    void SetColors()
    {
        background.color = backgroundColor;
        FillAmount = fill.fillAmount;
        SetFillGradient(fill.fillAmount);
        catchup.color = catchupColor;
        for (int i=0;i<borders.Length;i++) borders[i].color = borderColor;
    }

    void SetFillGradient(float value)
    {
        fill.color = fillColor.Evaluate(value);
    }


    void Start()
    {
        ratio = childRt.rect.width/childRt.rect.height;
        punchedScale = new Vector3(0.01f*flashScalePercentIncrease,0.01f*flashScalePercentIncrease*ratio,0f);
        FillAmount = ketchupValue = 1f;
        FlashIsReady();
        InstantCatchUp();
    }


    void OnEnable()
    {
        //canvasRt = transform.GetComponentInChildren<Canvas>().GetComponent<RectTransform>();
        ResetFlashing();
    }

    void FlashIsReady()
    {
        flashChrono = 1000f;
    }


    void StartFlash()
    {
        if (enabled) StartCoroutine(Flashing());
    }


    IEnumerator Flashing()
    {
        ResetFlashing();
        flashing++;
        flashChrono = 0;
        fill.color = flashColor;
        for (int i=0;i<borders.Length;i++) borders[i].color = flashColor;
        scalable.DOPunchScale(punchedScale,flashDuration,1,0).SetUpdate(true);
        yield return new WaitForSecondsRealtime(flashDuration*0.5f);
        if (flashing>1)
        {
            flashing--;
            yield break;
        }
        SetFillGradient(fill.fillAmount);
        for (int i=0;i<borders.Length;i++) borders[i].color = borderColor;
        yield return new WaitForSecondsRealtime(flashDuration*0.5f);
        ResetFlashing();
        flashing--;
    }

    void ResetFlashing()
    {
        scalable.DOKill();
        scalable.localScale = Vector3.one;
        SetFillGradient(fill.fillAmount);
        for (int i=0;i<borders.Length;i++) borders[i].color = borderColor;
        transform.GetChild(0).localScale = Vector3.one;
    }


    void Update()
    {
        unscaledDeltaTime = Time.unscaledDeltaTime;
        if (FillAmount!=ketchupValue)
        {
            diffChrono += unscaledDeltaTime;
            if (diffChrono>catchupDelay) ketchupValue = Mathf.MoveTowards(ketchupValue,FillAmount, (0.95f*Mathf.Abs(ketchupValue-FillAmount)+0.05f)*catchupSpeed*unscaledDeltaTime);
            UpdateFillAmount();
        }
        else diffChrono = 0;
        flashChrono += unscaledDeltaTime;


        if (blinking) BlinkSystem();
    }

    float blinkChrono = 0;


    void BlinkSystem()
    {
        blinkChrono += Time.unscaledDeltaTime;
        if (blinkChrono>0.1f) ShowBlinkColor(background.color == blinkColor ? false : true);
    }

    void ShowBlinkColor(bool wanted)
    {
        blinkChrono = 0;
        background.color = wanted ? blinkColor : backgroundColor;
    }


    void UpdateFillAmount()
    {
        if (fill!=null) fill.fillAmount = FillAmount;
        if (catchup!=null) catchup.fillAmount = ketchupValue;
    }



} // FIN DU SCRIPT
