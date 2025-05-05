// ℹ️ PUT THIS NEAR A TEXT OR TEXTMESHPRO COMPONENT TO ADD MULTIPLE LANGUAGES. NEED LanguageManager IN THE SCENE


using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LangText : MonoBehaviour
{
    public string en, fr; //    <-- 1. Add new language strings here


    TextMeshProUGUI[] tmpros => GetComponentsInChildren<TextMeshProUGUI>();
    Text[] ts => GetComponentsInChildren<Text>();

    void OnEnable()
    {
        SetLanguage();
    }


    public void SetLanguage()
    {
        switch(LanguageManager.currentLang)
        {
            default: SetEnglishByDefault(); break;
            case SystemLanguage.French: TryReplaceString(fr); break;
            //case SystemLanguage.Spanish: TryReplaceString(es); break;
            //case SystemLanguage.Portuguese: TryReplaceString(pt); break;
            //case SystemLanguage.Italian: TryReplaceString(it); break;
            //case SystemLanguage.Greek: TryReplaceString(el); break;
            //case SystemLanguage.German: TryReplaceString(de); break;
            //case SystemLanguage.Polish: TryReplaceString(pl); break;
            //case SystemLanguage.Swedish: TryReplaceString(sv); break;
            //case SystemLanguage.Finnish: TryReplaceString(fi); break;
            //case SystemLanguage.Norwegian: TryReplaceString(no); break;
            //case SystemLanguage.Russian: TryReplaceString(ru); break;
            //case SystemLanguage.Arabic: TryReplaceString(ar); break;
            //case SystemLanguage.Hebrew: TryReplaceString(he); break;
            //case SystemLanguage.Japanese: TryReplaceString(jp); break;
            //case SystemLanguage.Chinese: TryReplaceString(zh); break;
            //case SystemLanguage.Korean: TryReplaceString(ko); break;
            //case SystemLanguage.Thai: TryReplaceString(th); break;
            //                  <-- 2. Add (or uncomment) new language switch here. Done!
        }
    }

    void TryReplaceString(string desired)
    {
        if (desired!=null && desired.Length>0) ReplaceString(desired);
        else SetEnglishByDefault();
    }

    void SetEnglishByDefault()
    {
        ReplaceString(en);
    }

    void ReplaceString(string desired)
    {
        if (desired!=null && desired.Length>0)
        { 
            foreach (TextMeshProUGUI tmpro in tmpros) tmpro.text = desired;
            foreach (Text t in ts) t.text = desired;
        }
    }

#if UNITY_EDITOR
    private void OnValidate() // update the text component when modified in the inspector
    {
        SetLanguage();
    }
#endif

} // SCRIPT END
