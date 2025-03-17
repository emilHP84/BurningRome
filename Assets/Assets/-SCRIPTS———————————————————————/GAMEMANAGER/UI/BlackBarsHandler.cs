using UnityEngine;
using UnityEngine.UI;

public class BlackBarsHandler : MonoBehaviour
{
    //float Ratio{get{return targetRatio;}set{targetRatio=value;SetCameraRatio();}}
    //[SerializeField] float targetRatio = 1.77777777f;
    int screenHeight, screenWidth = 1;
    [SerializeField] RectTransform bar_top, bar_bottom, bar_left, bar_right;//, menu_fitter;
    [SerializeField]CanvasScaler[] scalers;
    [SerializeField]ScreenRatio[] possibleRatios;
    //int currentRatioIndex = 2;
    float variance;
    float Variance{get{variance = AspectRatioGlobal.GlobalRatio / ((float)screenWidth/(float)screenHeight); return variance;}}



    void Start()
    {
        SetScreenRatioLimiter(AspectRatioGlobal.GlobalRatio);
        SetCameraRatio();
    }

    public void SetScreenRatioLimiter(float desired)
    {
        desired = Mathf.Clamp(desired,0.1f,10f);
        AspectRatioGlobal.GlobalRatio = desired;
    }


    int ClosestRatioIndex()
    {
        int desiredRatioIndex = 0;
        float trueScreenRatio = ((float)screenWidth/(float)screenHeight);
        float smallestVariance = Mathf.Abs(trueScreenRatio - possibleRatios[0].ratio);
        for (int i=1; i<possibleRatios.Length;i++)
        {
            float newVariance = Mathf.Abs(trueScreenRatio - possibleRatios[i].ratio);
            //Debug.Log("variance = "+newVariance);
            if (newVariance<smallestVariance)
            {
                desiredRatioIndex = i;
                smallestVariance = newVariance;
            }
        }
        return desiredRatioIndex;
    }


    void HideBars()
    {
        bar_right.anchorMin =
        bar_right.anchorMax =
        bar_left.anchorMin =
        bar_left.anchorMax =
        bar_top.anchorMin =
        bar_top.anchorMax =
        bar_bottom.anchorMin =
        bar_bottom.anchorMax = Vector2.zero;
    }

    void Update()
    {
        if (Screen.width!=screenWidth || Screen.height!=screenHeight) SetCameraRatio();
    }



    void SetCameraRatio()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;
        float desiredRatio = possibleRatios[ClosestRatioIndex()].ratio;
        Debug.Log("🖥️ New Screen size : "+Screen.width+"x"+Screen.height+"  ratio = "+Camera.main.aspect+"  variance="+Variance+"  effective ratio "+possibleRatios[ClosestRatioIndex()].ratioName);
        Rect desiredRect;

        if (Variance < 1.0f) // Screen is wider than desired ratio
        {
            desiredRect = new Rect ((1.0f - variance) / 2.0f, 0 , variance, 1.0f);
        }
        else // Screen is taller than desired ratio
        {
            float newVariance = 1.0f/variance;
            desiredRect = new Rect (0f, (1.0f - newVariance) / 2.0f , 1.0f, newVariance);
        }

        if (desiredRatio>1.77777777f) CanvasScalerMatch(1f); else CanvasScalerMatch(0);

        //Camera.main.rect = desiredRect;//⚠️⚠️⚠️⚠️⚠️⚠️⚠️⚠️
        //Camera.main.fieldOfView = 85f + (16f*desiredRect.x-9f*desiredRect.y); 
        //for (int i=0; i<aspectRatioFitter.Length;i++) aspectRatioFitter[i].aspectRatio = targetRatio;
        AspectRatioGlobal.GlobalRatio = desiredRatio;
        EVENTS.InvokScreenResChange();
        SetBlackBarsPosition();
    }

    void CanvasScalerMatch(float desired)
    {
        foreach (CanvasScaler scaler in scalers) scaler.matchWidthOrHeight = desired;
    }

    void SetBlackBarsPosition()
    {
        HideBars();
        //Camera.main.fieldOfView = 2f*AspectRatioGlobal.GlobalRatio+50f;
        if (Variance>1f) // Screen is taller than desired ratio
        {     
            float heightPercentNotUsed = 1f-(1f/variance);
            bar_top.anchorMin = new Vector2(0,1f-(heightPercentNotUsed/2f));
            bar_top.anchorMax = new Vector2(1f,1f);
            bar_bottom.anchorMin = new Vector2(0,0);
            bar_bottom.anchorMax = new Vector2(1f,heightPercentNotUsed/2f);
            //menu_fitter.anchorMin = new Vector2(0, heightPercentNotUsed/2f);
            //menu_fitter.anchorMax = new Vector2(1f, 1f-(heightPercentNotUsed/2f));
        }
        else if (variance<1f) // Screen is wider than desired ratio
        {
            float widthPercentNotUsed = 1f-variance;
            bar_right.anchorMin = new Vector2(1f-(widthPercentNotUsed/2f),0);
            bar_right.anchorMax = new Vector2(1f, 1f);
            bar_left.anchorMin = new Vector2(0,0);
            bar_left.anchorMax = new Vector2(widthPercentNotUsed/2f,1f);
            //menu_fitter.anchorMin = new Vector2(widthPercentNotUsed/2f, 0);
            //menu_fitter.anchorMax = new Vector2(1f-(widthPercentNotUsed/2f), 1f);
        }
    }


} // FIN DU SCRIPT

[System.Serializable]
public struct ScreenRatio
{
    public string ratioName;
    public float ratio;
}
