using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

[Serializable]
public class GameDataSender : MonoBehaviour
{
    public string jsonString;
    string endpoint;
    string baseURL;
    string jsonData;
    string AccessTOKEN;
    string RefreshTOKEN;

    string ServerURL = "https://jihy30n.shop";

    public void Data(string endpoint, string jsonString, string baseURL)
    {
        this.endpoint = endpoint;
        this.jsonString = jsonString;
        this.baseURL = ServerURL;
    }
    public string ObjectToJson(object obj)
    {
        this.jsonData = JsonUtility.ToJson(obj);
        return JsonUtility.ToJson(obj);
    }
    public T JsonToObject<T>(string _jsonData)
    {
        return JsonUtility.FromJson<T>(_jsonData);
    }
    public IEnumerator SendJsonData(Action onSuccess, Action onFailure)
    {
        // ���� URL
        string serverUrl = ServerURL;
        Debug.Log(serverUrl + endpoint);
        // HTTP ��û ����
        UnityWebRequest request = new UnityWebRequest(serverUrl + endpoint, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        if (endpoint == "/game/login")
        {
            Debug.Log("�۷�");
            request.SetRequestHeader("Authorization", "Bearer YOUR_ACCESS_TOKEN");
            request.SetRequestHeader("RefreshToken", "Bearer YOUR_REFRESH_TOKEN");
        }
        else if(endpoint == "/game/end")
        {
            Debug.Log("�ҷ�");
            string AccessToken = GameObject.Find("DontDestroyData").GetComponent<DontDestroyData>().m_AccessToken;
            //string RefreshToken = GameObject.Find("DontDestroyData").GetComponent<DontDestroyData>().m_RefreshToken;

            request.SetRequestHeader("Authorization", AccessToken);
            //request.SetRequestHeader("RefreshToken", RefreshToken);
        }

        // ��û ������
        yield return request.SendWebRequest();

        // ��û �Ϸ� �� ó��
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("JSON ������ ���� ����");
            Debug.Log("���� ����: " + request.downloadHandler.text);

            if (endpoint == "/game/login")
            {
                AccessTOKEN = request.GetResponseHeader("Authorization");
                RefreshTOKEN = request.GetResponseHeader("RefreshToken");

                GameObject.Find("DontDestroyData").GetComponent<DontDestroyData>().m_AccessToken = AccessTOKEN;
                GameObject.Find("DontDestroyData").GetComponent<DontDestroyData>().m_RefreshToken = AccessTOKEN;
            }

            // ���� �ݹ� ȣ��
            onSuccess?.Invoke();
        }
        else
        {
            Debug.LogError("���� ���� �ڵ�: " + request.responseCode);
            Debug.LogError("JSON ������ ���� ����: " + request.error);

            // ���� �ݹ� ȣ��
            onFailure?.Invoke();
        }
        yield break;
    }
}
