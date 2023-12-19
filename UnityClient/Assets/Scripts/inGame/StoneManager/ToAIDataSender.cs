using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

class ToAIDataSender : MonoBehaviour
{
    CurrentBoardStateInit m_currentBoardStateInit;

    private TcpClient tcpClient;
    private NetworkStream clientStream;

    GetPortNumJson data;

    public void _Main()
    {
        // 요청을 보낼 URL
        string url = "http://192.168.0.14:5001/start";

        // HttpWebRequest 생성
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

        // GET 요청 설정
        request.Method = "GET";

        try
        {
            // 응답 받기
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // 응답 스트림 읽기
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string responseData = reader.ReadToEnd();
                        Debug.Log("Response Data: " + responseData);

                        data = JsonUtility.FromJson<GetPortNumJson>(responseData);

                    }
                }
            }
        }
        catch (WebException ex)
        {
            // 예외 처리
            Debug.Log($"Error: {ex.Message}");
        }

    }
    //public void _InitializeClient(out int AIcol, out int AIrow)
    public void InitializeClient(out int AIrow, out int AIcol)
    {
        Debug.Log(data.port);
        byte[] receiveBuffer = new byte[4];

        try
        {
            if (tcpClient == null)
            {
                tcpClient = new TcpClient(data.host, data.port);
                clientStream = tcpClient.GetStream();
            }
            // 서버로 전송할 데이터
            string message = m_currentBoardStateInit.m_MatrixData;

            Debug.Log(message);
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            //// 메시지를 서버로 보냄
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();

            // NetworkStream을 통해 데이터를 읽어옴
            int bytesRead = clientStream.Read(receiveBuffer, 0, receiveBuffer.Length);

            // 받아온 데이터를 문자열로 변환
            string receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);

            Decoding(receivedData, out AIrow, out AIcol);

            // 받아온 데이터 출력
            //Debug.Log("Received Data: " + receivedData);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");

            // 예외 발생 시 초기값 할당
            AIrow = 0;
            AIcol = 0;
        }
    }
    void Decoding(string data, out int m_row, out int m_col)
    {
        // 데이터를 ','를 기준으로 분할
        string[] parts = data.Split(',');

        if (parts.Length == 2)
        {
            // 첫 번째 부분을 문자로 변환하여 'a'를 뺀 값으로 m_row 설정
            char rowChar = parts[0][0];
            m_row = char.ToUpper(rowChar) - 64;

            // 두 번째 부분을 정수로 변환하여 m_col 설정
            if (int.TryParse(parts[1], out int col))
            {
                m_col = col;
            }
            else
            {
                // 변환 실패 시 예외 처리 또는 기본값 설정
                m_col = 0;
                Debug.LogError("Failed to parse m_col from string");
            }
        }
        else
        {
            // 부분의 개수가 예상과 다를 경우 예외 처리 또는 기본값 설정
            m_row = 0;
            m_col = 0;
            Debug.LogError("Unexpected number of parts in the string");
        }
    }

    private void Start()
    {
        m_currentBoardStateInit = FindObjectOfType<CurrentBoardStateInit>();
    }
}