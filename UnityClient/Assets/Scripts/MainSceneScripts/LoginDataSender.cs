using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginDataSender : MonoBehaviour
{
    [SerializeField] GameObject m_loginOrRegisterCanvas;
    [SerializeField] Text m_ErrorMessageText;
    public string BaseURL;

    public void Login(string identifier, string password)
    {
        LoginJson loginJson = new LoginJson();

        loginJson.userEmail = identifier;
        loginJson.password = password;

        string jsonString = JsonUtility.ToJson(loginJson);

        PostAuthRequest("/game/login", jsonString);

    }
    public virtual void PostAuthRequest(string endpoint, string jsonString)
    {
        //if (!BaseURL.EndsWith("/"))
        //{
        //    BaseURL += "/";
        //}
        GameDataSender data = new GameDataSender();

        data.Data(endpoint, jsonString, "Server");

        StartCoroutine(data.SendJsonData(OnSuccessCallback, OnFailureCallback));
    }
    // 로그인 성공 시 실행할 메서드
    void OnSuccessCallback()
    {
        Debug.Log("성공콜백 호출");
        m_loginOrRegisterCanvas.SetActive(false);

        Debug.Log(GameObject.Find("DontDestroyData").gameObject.GetComponent<DontDestroyData>().m_AccessToken);
        Debug.Log(GameObject.Find("DontDestroyData").gameObject.GetComponent<DontDestroyData>().m_RefreshToken);
    }

    // 로그인 실패 시 실행할 메서드
    void OnFailureCallback()
    {
        Debug.Log("실패콜백 호출");
        m_ErrorMessageText.text = "아이디 또는 비밀번호가 맞지 않습니다.";
    }
}
