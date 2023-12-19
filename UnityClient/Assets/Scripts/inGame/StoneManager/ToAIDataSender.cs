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
        // ��û�� ���� URL
        string url = "http://192.168.0.14:5001/start";

        // HttpWebRequest ����
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

        // GET ��û ����
        request.Method = "GET";

        try
        {
            // ���� �ޱ�
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // ���� ��Ʈ�� �б�
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
            // ���� ó��
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
            // ������ ������ ������
            string message = m_currentBoardStateInit.m_MatrixData;

            Debug.Log(message);
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            //// �޽����� ������ ����
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();

            // NetworkStream�� ���� �����͸� �о��
            int bytesRead = clientStream.Read(receiveBuffer, 0, receiveBuffer.Length);

            // �޾ƿ� �����͸� ���ڿ��� ��ȯ
            string receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);

            Decoding(receivedData, out AIrow, out AIcol);

            // �޾ƿ� ������ ���
            //Debug.Log("Received Data: " + receivedData);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");

            // ���� �߻� �� �ʱⰪ �Ҵ�
            AIrow = 0;
            AIcol = 0;
        }
    }
    void Decoding(string data, out int m_row, out int m_col)
    {
        // �����͸� ','�� �������� ����
        string[] parts = data.Split(',');

        if (parts.Length == 2)
        {
            // ù ��° �κ��� ���ڷ� ��ȯ�Ͽ� 'a'�� �� ������ m_row ����
            char rowChar = parts[0][0];
            m_row = char.ToUpper(rowChar) - 64;

            // �� ��° �κ��� ������ ��ȯ�Ͽ� m_col ����
            if (int.TryParse(parts[1], out int col))
            {
                m_col = col;
            }
            else
            {
                // ��ȯ ���� �� ���� ó�� �Ǵ� �⺻�� ����
                m_col = 0;
                Debug.LogError("Failed to parse m_col from string");
            }
        }
        else
        {
            // �κ��� ������ ����� �ٸ� ��� ���� ó�� �Ǵ� �⺻�� ����
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