#if UNITY_EDITOR
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoRenameButton : MonoBehaviour
{
    [SerializeField] string nameExtension = " Button Graphics";
    Text t=>GetComponentInChildren<Text>();
    TextMeshProUGUI tmpro =>GetComponentInChildren<TextMeshProUGUI>();


    void OnValidate()
    {
        gameObject.name = tmpro==null ? t.text+nameExtension : tmpro.text + nameExtension;
    }
} // SCRIPT END
#endif