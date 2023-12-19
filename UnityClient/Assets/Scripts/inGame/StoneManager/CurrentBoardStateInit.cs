using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 가상의 바둑판(배열로 구현된) 최초, 현재 상태 업데이트 클래스

public class CurrentBoardStateInit : MonoBehaviour
{
    [SerializeField] BoardManager m_boardManager;
    StoneBacksies m_stoneBacksies;

    // 보드 사이즈, 한 칸 변의 길이 BoardManager에서 값을 받아 옴.
    int m_boardSize;
    float m_sideLeng;

    //보드의 현재상태를 저장하는 배열변수 & 교차점들에 부여한 행열 번호를 저장할 변수
    public int[,] m_CurrentBoardState { get; set; }

    int m_row, m_col;

    public int m_Row { get { return m_row; } }
    public int m_Col { get { return m_col; } }

    // 바둑판의 현재상태를 저장할 바둑판 배열을 생성하는 함수
    // -1: 비어있음, 0: 백돌, 1: 흑돌
    public void BoardStateArrInit()
    {
        m_CurrentBoardState = new int[m_boardSize, m_boardSize];
        for (int i = 0; i < m_boardSize; i++)
        {
            for (int j = 0; j < m_boardSize; j++)
            {
                m_CurrentBoardState[i, j] = -1;
            }
        }
    }

    ///<summary> RuleCheck()할 때, CurrentBoardStateInit.m_CurrentBoardState 배열에 player 데이터를 임시로 넣는 함수<br></br>true로 데이터를 입력하고, false로 데이터 삭제</summary>
    public void TempBSInit(bool Input)
    {
        if (Input)
            m_CurrentBoardState[m_row, m_col] = 1;
        else
            m_CurrentBoardState[m_row, m_col] = -1;

    }

    ///<summary> CurrentBoardStateInit.m_CurrentBoardState 배열에 player 데이터를 넣는 함수</summary>
    public void UpdateBoardState(GameObject obj, RaycastHit hit, bool player)
    {
        GetMatrixNum(hit.point);
        m_CurrentBoardState[m_row, m_col] = player ? 1 : 0;

        //물림수를 저장
        m_stoneBacksies.SetBacksies(obj, m_row, m_col);

        //GetCurrenBoardStateArr();
    }

    //생성한 바둑돌 오브젝트(바둑판 위에 둔 바둑돌)의 행렬데이터를 구하는 함수
    public void GetMatrixNum(Vector3 StoneWorldPoint)
    {
        m_row = Mathf.RoundToInt(StoneWorldPoint.x / m_sideLeng);
        m_col = Mathf.RoundToInt(StoneWorldPoint.y / m_sideLeng);
    }

    // 바둑판의 현재 상태를 Console창에 띄우는 Debug 함수
    void GetCurrenBoardStateArr()
    {
        string s = null;
        for (int i = 0; i < m_boardSize; i++)
        {
            for (int j = 0; j < m_boardSize; j++)
            {
                s += m_CurrentBoardState[j, 14 - i] + " ";
            }
            s += "\n";
        }
        Debug.Log(s);
    }

    void Start()
    {
        m_stoneBacksies = FindObjectOfType<StoneBacksies>();

        m_boardSize = m_boardManager.m_BoardSize;
        m_sideLeng = m_boardManager.m_SideLeng;

        BoardStateArrInit();

    }
}