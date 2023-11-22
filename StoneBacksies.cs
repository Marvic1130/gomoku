using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 물림수 관련 클래스
public class StoneBacksies : MonoBehaviour
{
    CurrentBoardStateInit m_currentBoardStateInit;

    // Util폴더에 있는 Dequeue Script, 사용자 생성 클래스
    // LinkedList<T>로 생성되어 있어서 기능이 제한된 LinkedList라 생각하면 됨
    Dequeue<int> m_row = new Dequeue<int>();
    Dequeue<int> m_col = new Dequeue<int>();
    Dequeue<GameObject> m_Stoneobj = new Dequeue<GameObject>();

    //물림수를 저장하는 함수;
    public void SetBacksies(GameObject obj, int row, int col)
    {
        m_row.PushBack(row);
        m_col.PushBack(col);
        m_Stoneobj.PushBack(obj);
    }

    //Backsies 버튼을 누르면 실행되는 함수
    public void BacksiesButtonDown()
    {
        //생성된 바둑돌이 없거나 흑돌 한 개만 있을 때 Backsies 제한
        if (m_Stoneobj.Count != 0 && m_Stoneobj.Count != 1)
        {
            int row = m_row.PopBack();
            int col = m_col.PopBack();

            m_currentBoardStateInit.m_CurrentBoardState[row, col] = -1;

            Destroy(m_Stoneobj.PopBack());

            Debug.Log("수를 물렸습니다.");
        }
        else
        {
            Debug.Log("수를 물릴 수 없습니다.");
        }
    }

    //Restart 버튼을 누르면 실행되는 함수
    public void RestartButtonDown()
    {
        // Dequeue에 저장되어 있는 데이터들 전부 삭제
        m_row.Clear();
        m_col.Clear();
        m_Stoneobj.Clear();
    }

    void Start()
    {
        m_currentBoardStateInit = FindAnyObjectByType<CurrentBoardStateInit>();
    }
}
