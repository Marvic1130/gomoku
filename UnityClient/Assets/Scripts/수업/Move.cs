using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] GameObject m_obj;
    [SerializeField] float m_speed = 3f;
    [SerializeField] Vector3 m_dir;
    void ActionControl()
    {
        if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            m_dir = Vector3.zero;
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            m_dir = Vector3.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            m_dir = Vector3.right;
        }
        gameObject.transform.position += m_speed * m_dir * Time.deltaTime;
    }

    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        ActionControl();
    }
}
