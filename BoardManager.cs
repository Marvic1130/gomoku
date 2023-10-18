using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject m_gridVertexPrefab; //좌표값만을 담은 GameObject Prefabs, 아래 Board Coordinate 오브젝트에 담아야 함
    public GameObject m_boardCoordinate; // GoBoard 오브젝트의 하위에 있는 Board Coordinate 오브젝트
    GameObject m_gridVertexObj;

    //가로, 세로 크기 & 교차점 개수
    int m_vertical = 15;
    int m_horizontal = 15;

    int m_GridVertexMax;

    //m_boardCoordinate의 기준점 & 좌표이동 & 한 칸 변의 길이
    const double m_cXPos = -2.75f; 
    const double m_cYPos = 2.75f;

    double m_xPos = m_cXPos;
    double m_yPos = m_cYPos;

    double m_sideLeng = 0.3925f;

    //보드의 상하좌우 로컬좌표 & 보드 크기
    Vector3 m_boardUp;
    Vector3 m_boardDown;
    Vector3 m_boardLeft;
    Vector3 m_boardRight;

    // 그냥 i, j
    int i, j;

    void GridVertexMax() // 교차점의 개수 구하는 함수
    {
        m_GridVertexMax = m_vertical * m_horizontal;
    }
    void CreateGridVertex(int count) // 교차점의 좌표값을 담을 오브젝트 생성하는 함수
    {
        for(i = 0; i<m_horizontal; i++)
        {
            for (j = 0; j < m_vertical; j++)
            {
                m_gridVertexObj = Instantiate(m_gridVertexPrefab);
                m_gridVertexObj.transform.SetParent(m_boardCoordinate.transform);
                m_gridVertexObj.transform.position = new Vector3((float)m_xPos, (float)m_yPos, 0);

                m_xPos += m_sideLeng;
            }
            m_xPos = m_cXPos;
            m_yPos -= m_sideLeng;
        }
    }

    //public Vector3 SettoPoint(Vector3 m_mousePos)
    //{
    //    return 
    //}

    void Start()
    {

        GridVertexMax();
        CreateGridVertex(m_GridVertexMax);
    }
}
