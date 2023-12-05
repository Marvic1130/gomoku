using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class GameDataSender : MonoBehaviour
{
    public string gameResult;
    string jsonData;

    public void Data(string gameResult)
    {
        this.gameResult = gameResult;
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
    public IEnumerator SendJsonData()
    {
        // ���� URL
        //string serverUrl = "https://heneinbackapi.shop/normal-get";
        string serverUrl = "https://heneinbackapi.shop/header-test-get";
        //string serverUrl = "https://heneinbackapi.shop/dto-post";

        // HTTP ��û ����
        UnityWebRequest request = new UnityWebRequest(serverUrl, "GET");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer YOUR_ACCESS_TOKEN");

        // ��û ������
        yield return request.SendWebRequest();

        // ��û �Ϸ� �� ó��
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("JSON ������ ���� ����");
            Debug.Log("���� ����: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("���� ���� �ڵ�: " + request.responseCode);
            Debug.LogError("JSON ������ ���� ����: " + request.error);
        }
    }
}
