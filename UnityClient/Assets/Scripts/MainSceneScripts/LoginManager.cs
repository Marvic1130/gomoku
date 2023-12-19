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

        // 간단한 로그인 검증 (예: 하드코딩된 사용자명과 비밀번호)
        if (username == "user" && password == "password")
        {
            // 로그인 성공
            errorMessageText.text = "로그인 성공!";
        }
        else
        {
            // 로그인 실패
            errorMessageText.text = "아이디 또는 비밀번호가 올바르지 않습니다.";
        }
    }
}
