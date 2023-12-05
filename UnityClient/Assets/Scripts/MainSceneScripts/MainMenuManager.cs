using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public void _GameStart()
    {
        SceneManager.LoadScene("inGame");
    }

    public void _Exit()
    {
        Application.Quit();
    }
}
