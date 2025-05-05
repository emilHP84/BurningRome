// ℹ️ A TOOL TO HANDLE MULTIPLE TEXT LANGUAGES. JUST PUT IT NEAR THE GAME MANAGER

using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static SystemLanguage currentLang, defaultLang;

    void Start()
    {
        currentLang = Application.systemLanguage; // get the computer current language
        ChangeLanguage(currentLang);
    }

    public static void ChangeLanguage(SystemLanguage Desired) // finds all texts currently visible and change them
    {
        currentLang = Desired;
        foreach (LangText texteachanger in FindObjectsByType<LangText>(FindObjectsSortMode.None))
        {
           texteachanger.SetLanguage();
        }
    }

    public void ButtonChangeFR()
    {
        ChangeLanguage(SystemLanguage.French);
    }

    public void ButtonChangeEN()
    {
        ChangeLanguage(SystemLanguage.English);
    }


} // SCRIPT END
