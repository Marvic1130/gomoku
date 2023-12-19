using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject m_UI;
    [SerializeField] GameObject m_loginForm;
    [SerializeField] DontDestroyData DontDestroyData;


    public void _Login()
    {
        
        //m_loginForm.SetActive(true);
        m_UI.transform.Find("Game Start").gameObject.SetActive(true);
        m_UI.transform.Find("Login").gameObject.SetActive(false);

    }
    public void _GameStart()
    {
        SceneManager.LoadScene("inGame");
    }

    public void _Exit()
    {
        Application.Quit();
    }
    public void _Language()
    {
        var selectedLocale = LocalizationSettings.SelectedLocale;
        string languageCode = selectedLocale.Identifier.Code;

        if (languageCode == "en")
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale("ko");
            languageCode = "ko";
        }
        else if (languageCode == "ko")
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale("en");
            languageCode = "en";
        }

        GameObject.Find("DontDestroyData").GetComponent<DontDestroyData>().m_Langauge = languageCode;

    }
    void SetLanguage()
    {
        var languageCode = GameObject.Find("DontDestroyData").GetComponent<DontDestroyData>().m_Langauge;

        if (languageCode == "")
        {
            languageCode = "en";
        }
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(languageCode);

        if (languageCode == "en")
        {
            m_UI.transform.Find("Language_ko").gameObject.SetActive(true);
            m_UI.transform.Find("Language_en").gameObject.SetActive(false);
        }
        else if (languageCode == "ko")
        {
            m_UI.transform.Find("Language_ko").gameObject.SetActive(false);
            m_UI.transform.Find("Language_en").gameObject.SetActive(true);
        }
    }
    void Start()
    {
        SetLanguage();

        string m_isAccessToken = GameObject.Find("DontDestroyData").GetComponent<DontDestroyData>().m_AccessToken;
        if (m_isAccessToken != "")
        {
            m_UI.transform.Find("Game Start").gameObject.SetActive(true);
            m_UI.transform.Find("Login").gameObject.SetActive(false);
        }
    }
}
