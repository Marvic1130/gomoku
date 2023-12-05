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
        // 서버 URL
        //string serverUrl = "https://heneinbackapi.shop/normal-get";
        string serverUrl = "https://heneinbackapi.shop/header-test-get";
        //string serverUrl = "https://heneinbackapi.shop/dto-post";

        // HTTP 요청 생성
        UnityWebRequest request = new UnityWebRequest(serverUrl, "GET");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer YOUR_ACCESS_TOKEN");

        // 요청 보내기
        yield return request.SendWebRequest();

        // 요청 완료 후 처리
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("JSON 데이터 전송 성공");
            Debug.Log("서버 응답: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("서버 응답 코드: " + request.responseCode);
            Debug.LogError("JSON 데이터 전송 실패: " + request.error);
        }
    }
}
