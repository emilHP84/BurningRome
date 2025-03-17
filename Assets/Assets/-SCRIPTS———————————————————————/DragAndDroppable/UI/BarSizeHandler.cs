using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BarHandler))]
public class BarSizeHandler : MonoBehaviour
{
    BarHandler bar => GetComponent<BarHandler>();
    [SerializeField] Vector2 barSize = new Vector2(1.6f,0.4f);
    [Range(0,20f)][SerializeField] float borderSize = 5f;

    Image background => bar.childRt.transform.Find("Background").GetComponent<Image>();
    RectTransform backgroundRt => background.GetComponent<RectTransform>();
    float ratio;




    void SetCanvasSize()
    {
        bar.childRt.sizeDelta = barSize;
        ratio = bar.childRt.rect.width / bar.childRt.rect.height;
        backgroundRt.anchorMin = new Vector2(0.01f*borderSize,0.01f*borderSize*ratio);
        backgroundRt.anchorMax = new Vector2(1f-0.01f*borderSize,1f-0.01f*borderSize*ratio);
        backgroundRt.anchoredPosition = Vector2.zero;
    }

    void OnValidate()
    {
        if (gameObject.activeSelf==false) return;
        SetCanvasSize();
    }



}
