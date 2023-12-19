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
        // 서버 URL
        string serverUrl = ServerURL;
        Debug.Log(serverUrl + endpoint);
        // HTTP 요청 생성
        UnityWebRequest request = new UnityWebRequest(serverUrl + endpoint, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        if (endpoint == "/game/login")
        {
            Debug.Log("핼로");
            request.SetRequestHeader("Authorization", "Bearer YOUR_ACCESS_TOKEN");
            request.SetRequestHeader("RefreshToken", "Bearer YOUR_REFRESH_TOKEN");
        }
        else if(endpoint == "/game/end")
        {
            Debug.Log("할로");
            string AccessToken = GameObject.Find("DontDestroyData").GetComponent<DontDestroyData>().m_AccessToken;
            //string RefreshToken = GameObject.Find("DontDestroyData").GetComponent<DontDestroyData>().m_RefreshToken;

            request.SetRequestHeader("Authorization", AccessToken);
            //request.SetRequestHeader("RefreshToken", RefreshToken);
        }

        // 요청 보내기
        yield return request.SendWebRequest();

        // 요청 완료 후 처리
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("JSON 데이터 전송 성공");
            Debug.Log("서버 응답: " + request.downloadHandler.text);

            if (endpoint == "/game/login")
            {
                AccessTOKEN = request.GetResponseHeader("Authorization");
                RefreshTOKEN = request.GetResponseHeader("RefreshToken");

                GameObject.Find("DontDestroyData").GetComponent<DontDestroyData>().m_AccessToken = AccessTOKEN;
                GameObject.Find("DontDestroyData").GetComponent<DontDestroyData>().m_RefreshToken = AccessTOKEN;
            }

            // 성공 콜백 호출
            onSuccess?.Invoke();
        }
        else
        {
            Debug.LogError("서버 응답 코드: " + request.responseCode);
            Debug.LogError("JSON 데이터 전송 실패: " + request.error);

            // 실패 콜백 호출
            onFailure?.Invoke();
        }
        yield break;
    }
}
