using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputManager : MonoBehaviour
{
    [SerializeField] GameObject m_UI; //inGame Scene¿« Canvas(UI)

    StoneBacksies m_stoneBacksies;
    public void KeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            var obj = m_UI.transform.Find("Menubar").gameObject;
            var isActive = obj.activeSelf;
            obj.SetActive(isActive? false : true);
        }
        if(Input.GetKeyUp(KeyCode.U))
        {
            m_stoneBacksies.BacksiesButtonDown();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        m_stoneBacksies = FindObjectOfType<StoneBacksies>();
    }
}