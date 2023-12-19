using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    [SerializeField] Text m_errorMessageText;
    [SerializeField] InputField m_idInput;
    [SerializeField] InputField m_passwordInput;
    [SerializeField] Button m_loginSubmitButton;
    [SerializeField] GameObject m_loadingObject;

    public LoginDataSender m_loginDataSender;

    private bool m_isLoading = false;

    string m_errorMessage;
    private void toggleLoading()
    {
        m_isLoading = !m_isLoading;
        m_loadingObject.SetActive(m_isLoading);
    }

    private void handleUnsuccessfulAuthentication(Exception error)
    {
        toggleLoading();
        //HeaderText.text = $"Authentication Error: {error.Message}";
    }

    public void OnLoginSubmit()
    {
        //toggleLoading();

        m_loginDataSender.Login(m_idInput.text, m_passwordInput.text);

        if (m_errorMessage == null)
        {
            m_errorMessageText.gameObject.SetActive(true);
            m_errorMessageText.text = m_errorMessage;
        }
    }
    private void Start()
    {
        m_loadingObject.SetActive(false);
    }
}
