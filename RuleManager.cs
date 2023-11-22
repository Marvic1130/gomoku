using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// 게임룰 관련 클래스

public class RuleManager : MonoBehaviour
{
    [SerializeField] BoardManager m_boardManager;
    CurrentBoardStateInit m_currentBoardStateInit;


    // 오목 보드의 크기 = 15 (row 및 col)
    // start에서 m_currentBoardStateInit.m_BoardSize; 초기화
    int m_boardSize;

    // 주어진 위치에서 4개의 돌이 연속된 패턴을 검사.
    static int[] dx = { 0, 1, 1, 1 };
    static int[] dy = { 1, 1, 0, -1 };

    public int CheckRule(int player, int row, int col)
    {
        if (player == 1 && IsStoneThree(player, row, col))
        {
            return 1;
        }
        //if (player == 1 && IsStoneFour(player, row, col))
        //{
        //    Debug.Log("OOO0");
        //    return 2;
        //}
        //if (player == 1 && IsStoneOver(player, row, col))
        //{
        //    Debug.Log("0OOO00");
        //    return 3;
        //}
        return 0;
    }
    bool BoolConverter(bool isBool)
    {
        if (isBool == true)
            return false;
        else
            return true;
    }

    bool IsStoneThree(int player, int row, int col)
    {
        int[] stonePos = { -1, -1, -1, -1, -1, -1,-1,-1,-1,-1};

        for (int d = 0; d < 4; d++) // 4개의 방향을 확인
        {
            int rememberPos = 0;
            bool isStart = false; //row와 col에 돌(CheckRule()에는 emptyObject가 들어옴)이 두어졌는가?
            bool isOneBlackStone = false; //돌이 2 1 3 인 경우를 검출하기 위한 코드

            for (int dir = -1; dir <= 1; dir += 2)
            {
                string line = "";
                bool isEmptyStart = false;
                bool isEnemyStone = false;
                bool checkUntilThree = false; //돌을 세 개만 체크하고 바로 IsCheckMoreThree로 넘어가는 것을 방지
                int emptyCookie = 0; // 한 라인을 검사할 때 empty가 퐁당퐁당 있는 경우를 검사하는 변수(empty 이력 없음 0, empty 활성화 1, empty 비활성화 -1)
                int emptyCount = 0;

                for (int i = 0; i < 6; i++)
                {
                    int r = row + i * dir * dx[d];
                    int c = col + i * dir * dy[d];

                    if (r >= 0 && r < m_boardSize && c >= 0 && c < m_boardSize)
                    {
                        if (m_currentBoardStateInit.m_CurrentBoardState[r, c] == player)
                        {
                            // m_stonePos 배열에 r, c 값 저장
                            stonePos[rememberPos * 2] = r;
                            stonePos[rememberPos * 2 + 1] = c;
                            rememberPos++;

                            line += "O";  // 플레이어의 돌은 'O'

                            if (!isEnemyStone && emptyCookie == 1) // 흑돌 사이에 백돌이 없거나, 사이에 빈 공간이 있을 때 실행
                            {
                                isEmptyStart = BoolConverter(isEmptyStart);
                                emptyCookie = -1;
                            }

                            if (!isStart)
                                isStart = true;
                            else
                            {
                                if (isOneBlackStone)//돌이 2 1 3 인 경우를 검출하기 위한 코드
                                    line += "O";
                            }

                        }
                        else if (m_currentBoardStateInit.m_CurrentBoardState[r, c] == -1)
                        {
                            if (checkUntilThree && emptyCookie != -1) // checkUntilThree로 네 번째 위치를 확인할 때, 네 번째 위치가 빈칸인 경우
                            {
                                emptyCookie = -1;
                                continue;
                            }
                            if (emptyCookie == 0) // line에 O가 하나만 있을 때(이후는 빈칸이므로 emptyCount++을 하기 위함), 두 개 이상 들어있을 경우는 윗부분에서 처리
                            {
                                isEmptyStart = BoolConverter(isEmptyStart);
                                emptyCookie = 1;
                            }
                        }
                        else
                        {
                            line += "X";  // 상대방 돌은 'X'
                            isEnemyStone = true;

                            if (emptyCookie == 0)
                            {
                                isEmptyStart = BoolConverter(isEmptyStart);
                                emptyCookie = 1;
                            }
                        }
                    }
                    else
                    {
                        line += "X";  // 보드 바깥의 위치는 'X'로 처리
                        isEnemyStone = true;
                    }
                    if (isEmptyStart && emptyCookie == 1)
                        emptyCount++;
                    if (!isOneBlackStone && line == "OO" && emptyCount >= 4)
                        isOneBlackStone = true;
                    if (line.Contains("OOO") && line != "OOOX" && line != "XOOO" && emptyCount < 2)
                    {
                        if (!checkUntilThree)
                            checkUntilThree = true;
                        else
                        {
                            Debug.Log("OOO");
                            return IsCheckMoreThree(player, d, rememberPos, stonePos);
                        }
                    }
                }
            }
        }
        return false;
    }
    public bool IsCheckMoreThree(int player, int excludeDir, int rememberPos, int[] stonePos)
    {
        for (int stonePosArrIndex = 0; stonePosArrIndex < rememberPos; stonePosArrIndex++) // 이전 좌표로 돌아가서 검사
        {
            for (int d = 0; d < 4; d++) // 4개의 방향을 확인
            {
                if (d == excludeDir) // 진행하던 방향 제외
                    continue;

                bool isStart = false;
                bool isOneBlackStone = false;

                for (int dir = -1; dir <= 1; dir += 2)
                {
                    string line = "";
                    bool isEmptyStart = false;
                    bool isEnemyStone = false;
                    bool checkUntilThree = false;
                    int emptyCookie = 0;
                    int emptyCount = 0;

                    for (int i = 0; i < 6; i++)
                    {

                        int r = stonePos[stonePosArrIndex * 2] + i * dir * dx[d];
                        int c = stonePos[stonePosArrIndex * 2 + 1] + i * dir * dy[d];

                        if (r >= 0 && r < m_boardSize && c >= 0 && c < m_boardSize)
                        {
                            if (m_currentBoardStateInit.m_CurrentBoardState[r, c] == player)
                            {
                                line += "O";

                                if (!isEnemyStone && emptyCookie == 1)
                                {
                                    isEmptyStart = BoolConverter(isEmptyStart);
                                    emptyCookie = -1;
                                }

                                // m_stonePos 배열에 저장된 두 번째 세 번째 좌표의 데이터에는 흑돌이 저장되어 있음
                                if (!isStart)
                                    isStart = true;
                                if (isStart && isOneBlackStone)
                                {
                                    line += "O";
                                    isOneBlackStone = false;
                                }

                            }
                            else if (m_currentBoardStateInit.m_CurrentBoardState[r, c] == -1)
                            {
                                //if (checkUntilThree)
                                //    emptyCookie = -1;
                                if (checkUntilThree && emptyCookie != -1)
                                {
                                    emptyCookie = -1;
                                    continue;
                                }

                                if (emptyCookie == 0)
                                {
                                    isEmptyStart = BoolConverter(isEmptyStart);
                                    emptyCookie = 1;
                                }
                            }
                            else
                            {
                                line += "X";  // 상대방 돌은 'X'
                                isEnemyStone = true;

                                if (emptyCookie == 0)
                                {
                                    isEmptyStart = BoolConverter(isEmptyStart);
                                    emptyCookie = 1;
                                }

                            }
                        }
                        else
                        {
                            line += "X";  // 보드 바깥의 위치는 'X'로 처리
                            isEnemyStone = true;
                        }
                        if (isEmptyStart == true && emptyCookie == 1)
                            emptyCount++;
                        if (!isOneBlackStone && line == "OO" && emptyCount >= 4)
                            isOneBlackStone = true;

                        if (line == "OOO" && line != "OOOX" && line != "XOOO" && emptyCount < 2)
                        {
                            if (!checkUntilThree)
                                checkUntilThree = true;
                            else
                            {
                                Debug.Log("OOO");
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
    public bool CheckWin(int player, int row, int col)
    {
        for (int d = 0; d < 4; d++) // 4개의 방향을 확인
        {
            int count = 1; // 현재 위치에서부터 count 시작
            for (int dir = -1; dir <= 1; dir += 2)
            {
                for (int i = 1; i <= 4; i++) // 현재 위치에서 다섯 개의 돌 확인
                {
                    int r = row + i * dir * dx[d];
                    int c = col + i * dir * dy[d];
                    //Debug.Log("r: " + row + " c: " + col);

                    if (r >= 0 && r < m_boardSize && c >= 0 && c < m_boardSize && m_currentBoardStateInit.m_CurrentBoardState[r, c] == player)
                    {
                        count++;
                    }
                    else
                        break;
                }

                if (count >= 5)
                {
                    return true; // 다섯 개의 돌이 연속으로 놓이면 true 반환
                }
            }
        }

        return false; // 다섯 개의 돌이 연속으로 놓이지 않으면 false 반환
    }

    void Start()
    {
        m_currentBoardStateInit = FindObjectOfType<CurrentBoardStateInit>();

        m_boardSize = m_boardManager.m_BoardSize;
    }
}
