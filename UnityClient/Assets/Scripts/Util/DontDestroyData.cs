using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyData : MonoBehaviour
{
    [SerializeField] string m_language;
    [SerializeField] string m_accessToken;
    [SerializeField] string m_refreshToken;

    public string m_Langauge { get {  return m_language; } set { m_language = value; } }
    public string m_AccessToken { get {  return m_accessToken; } set { m_accessToken = value; } }
    public string m_RefreshToken { get {  return m_refreshToken; } set { m_refreshToken = value; } }

    //Singleton
    private static DontDestroyData _instance;
    public static DontDestroyData Instance { get { return _instance; } }

    protected virtual void Awake()
    {

        //Singleton
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
    }
}
}
