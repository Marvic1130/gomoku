using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenubarManager : MonoBehaviour
{
    [SerializeField] GameObject m_instantedStone; // Stone Manager ������Ʈ�� ������ �ִ� Instanted Stone ������Ʈ 
    [SerializeField] GameObject m_UI; //inGame Scene�� Canvas(UI)
    [SerializeField] GameObject DontDestroyData;

    public void _Restart()
    {

        for (int i = 0; i < m_instantedStone.transform.childCount; i++)
        {
            // �ڽ� ������Ʈ�� ����
            Destroy(m_instantedStone.transform.GetChild(i).gameObject);
        }

        Transform parentTransform = m_UI.transform;
        Transform lastChild = null;

        //UI�� ������ child�� ����
        foreach (Transform child in parentTransform)
        {
            lastChild = child;
        }
        if (lastChild != null && lastChild.gameObject.tag == "Result")
        {
            Destroy(lastChild.gameObject);
        }

    }

    public void _MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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

    //public void _ShowBoard()
    //{
    //    //winUI Show/Hide
    //    Transform winUITransform = m_UI.transform.Find("Win Black(Clone)");

    //    winUITransform = winUITransform == null ? m_UI.transform.Find("Win White(Clone)") : winUITransform;

    //    if (winUITransform != null)
    //    {
    //        GameObject winUI = winUITransform.gameObject;
    //        CanvasGroup winUIAlpha = winUI.GetComponent<CanvasGroup>();

    //        Debug.Log(winUI.name);

    //        if (winUIAlpha != null)
    //        {
    //            winUIAlpha.alpha = (winUIAlpha.alpha == 0) ? 1 : 0;
    //        }
    //    }
    //}
}
