using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Text errorMessageText;

    public void OnLoginButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        // ������ �α��� ���� (��: �ϵ��ڵ��� ����ڸ�� ��й�ȣ)
        if (username == "user" && password == "password")
        {
            // �α��� ����
            errorMessageText.text = "�α��� ����!";
        }
        else
        {
            // �α��� ����
            errorMessageText.text = "���̵� �Ǵ� ��й�ȣ�� �ùٸ��� �ʽ��ϴ�.";
        }
    }
}
