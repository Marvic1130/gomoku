using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : MonoBehaviour
{
    public GameObject m_gridVertexPrefab;
    public GameObject m_instantedStone;
    public GameObject m_AlphaStonePool;
    public GameObject m_blackStone60A;
    public GameObject m_whiteStone60A;
    public GameObject m_blackStonePrefabs;
    public GameObject m_whiteStonePrefabs;
    public Collider m_boardCollider;

    //isOrder 1(true)로 시작, 1(true)은 흑돌, 0(false)은 백돌,
    //다른 Script에서 isOrder를 사용하기 위한 get
    bool m_isOrder = true;
    static bool m_isBlackStone = true;
    static bool m_isWhiteStone = false;

    //Ray
    Ray ray;
    RaycastHit hit;

    void _ray()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _ray();
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.tag == "Stone")
                {
                    Debug.Log("이 자리에는 바둑돌이 있습니다.");
                }
                else if (hit.collider.gameObject.tag != "Stone")
                {
                    if (m_isOrder) //isOrder가 1일 때 흑돌, 0일 때 백돌
                    {
                        var obj = Instantiate(m_blackStonePrefabs, hit.collider.bounds.center, Quaternion.identity);
                        obj.transform.SetParent(m_instantedStone.transform);
                        m_isOrder = m_isWhiteStone;

                        Vector3 newPosition = obj.transform.position;
                        newPosition.z = -1;
                        obj.transform.position = newPosition;
                    }
                    else
                    {
                        var obj = Instantiate(m_whiteStonePrefabs, hit.collider.bounds.center, Quaternion.identity);
                        obj.transform.SetParent(m_instantedStone.transform);
                        m_isOrder = m_isBlackStone;

                        Vector3 newPosition = obj.transform.position;
                        newPosition.z = -1;
                        obj.transform.position = newPosition;
                    }
                }
            }
        }
    }
}
