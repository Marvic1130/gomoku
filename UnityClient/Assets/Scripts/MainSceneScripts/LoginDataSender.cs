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
    // �α��� ���� �� ������ �޼���
    void OnSuccessCallback()
    {
        Debug.Log("�����ݹ� ȣ��");
        m_loginOrRegisterCanvas.SetActive(false);

        Debug.Log(GameObject.Find("DontDestroyData").gameObject.GetComponent<DontDestroyData>().m_AccessToken);
        Debug.Log(GameObject.Find("DontDestroyData").gameObject.GetComponent<DontDestroyData>().m_RefreshToken);
    }

    // �α��� ���� �� ������ �޼���
    void OnFailureCallback()
    {
        Debug.Log("�����ݹ� ȣ��");
        m_ErrorMessageText.text = "���̵� �Ǵ� ��й�ȣ�� ���� �ʽ��ϴ�.";
    }
}
