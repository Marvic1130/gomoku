using System.Collections;
using System.Collections.Generic;
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
        if (player == 1)
        {
            int RuleCheck = IsStoneOverThree(player, row, col);

            if (RuleCheck == 1)
            {
                return 1;
            }
            else if (RuleCheck == 2)
            {
                return 2;
            }
            else if (RuleCheck == 3)
            {
                return 3;
            }
        }
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

    int IsStoneOverThree(int player, int row, int col)
    {
        int[] stonePos = { -1, -1, -1, -1, -1, -1, -1, -1};

        for (int d = 0; d < 4; d++) // 4개의 방향을 확인
        {
            string lineCopy = "";
            bool isStart = false; //row와 col에 돌(CheckRule()에는 emptyObject가 들어옴)이 두어졌는가?
            bool checkUntilThree = false; //돌을 세 개만 체크하고 바로 IsCheckMoreThree로 넘어가는 것을 방지
            bool checkUntilFour = false;
            int rememberPos = 0;
            int isOneBlackStone = 0; //돌이 2 1 3 인 경우를 검출하기 위한 코드. (이력 없음 0, isOneBlackStone 활성화 1, 비활성화 -1)

            for (int dir = -1; dir <= 1; dir += 2)
            {
                string line = "";
                bool isEmptyStart = false;
                bool isEnemyStone = false;
                int emptyCookie = 0; // 한 라인을 검사할 때 empty가 퐁당퐁당 있는 경우를 검사하는 변수(empty 이력 없음 0, empty 활성화 1, empty 비활성화 -1)
                int emptyCount = 0;
                int AfterThreeEmptyCount = 1;

                for (int i = 0; i < 7; i++)
                {
                    int r = row + i * dir * dx[d];
                    int c = col + i * dir * dy[d];

                    if (r >= 0 && r < m_boardSize && c >= 0 && c < m_boardSize)
                    {
                        // m_CurrentBoardState[r, c]가 흑돌일 때. 금수 체크는 흑돌에만 있음
                        if (m_currentBoardStateInit.m_CurrentBoardState[r, c] == player)
                        {
                            if (rememberPos < 4) // rememberPos가 4일 때는 다섯번째 흑돌이므로 오목이 완성된 상태. 따라서 저장할 필요 없다
                            {
                                // m_stonePos 배열에 r, c 값 저장
                                stonePos[rememberPos * 2] = r;
                                stonePos[rememberPos * 2 + 1] = c;
                                rememberPos++;
                            }

                            line += "O";

                            //dir이 -1에서 1로 넘어갈 때, stonePos배열에 r=row, c=col(현재위치) 값이 중복 저장되는 것을 방지하기 위한 코드
                            // 저장될 때, stonePos[0] = 두번째 위치, stonePos[1] = 세번째 위치, stonePos[2] = 현재위치 or 세번째 위치,
                            //stonePos[3] = 반대방향 두번째 위치 or 현재위치 가 저장된다.
                            if (!isStart)
                            {
                                isStart = true;
                                rememberPos--;
                            }

                            //돌이 2 1 3 인 경우를 검출하기 위한 코드
                            if (isOneBlackStone == 1)
                            {
                                line += "O";
                                isOneBlackStone = -1;
                            }

                            // 흑돌 사이에 백돌이 없거나, 사이에 빈 공간이 있을 때 실행
                            if (!isEnemyStone && emptyCookie == 1)
                            {
                                isEmptyStart = BoolConverter(isEmptyStart);
                                emptyCookie = -1;
                            }

                            if (lineCopy == "OOO" && line == "O") //lineCopy가 OOO 일 때, line이 O 이라면 r = row, c = col(현재 위치)이므로 패스
                                continue;
                            // +O+OO+형태(+는 빈칸)에서 중간 + 위치에 흑돌을 둔 경우를 검출(진행방향은 ->로 가정)
                            if (checkUntilThree && line == "OO")
                                lineCopy += "O";

                        }
                        // m_CurrentBoardState[r, c]가 빈칸일 때
                        else if (m_currentBoardStateInit.m_CurrentBoardState[r, c] == -1)
                        {
                            // checkUntilThree 활성화 이후, 다음 첫 번째와 두번째 위치를 확인할 때, 둘 다 빈칸인 경우.
                            if (checkUntilThree && emptyCount < 2 && AfterThreeEmptyCount <= 2)
                            {
                                if (!checkUntilFour && lineCopy == "")
                                    lineCopy = line;
                                emptyCookie = -1;

                                //AfterThreeEmptyCount == 2 에서 break. dir == 1일때는 그대로 내려감
                                if (!checkUntilFour && AfterThreeEmptyCount == 2 && dir == -1)
                                    break;
                                if (AfterThreeEmptyCount == 1)
                                {
                                    AfterThreeEmptyCount++;
                                    continue;
                                }
                                //돌이 네 개일 때
                                if (checkUntilFour && dir == -1)
                                {
                                    lineCopy = line;
                                    break;
                                }
                            }
                            else if (!checkUntilFour && checkUntilThree && dir == -1)
                                break;
                            if (emptyCookie == 0) // line에 O가 하나만 있을 때(이후는 빈칸이므로 emptyCount++을 하기 위함), 두 개 이상 들어있을 경우는 윗부분에서 처리
                            {
                                isEmptyStart = BoolConverter(isEmptyStart);
                                emptyCookie = 1;
                            }

                        }
                        // m_CurrentBoardState[r, c]가 백돌일 때
                        else
                        {
                            line += "X";  // 상대방 돌은 'X'
                            isEnemyStone = true;

                            if (lineCopy == "OOO" || lineCopy == "OOOO")
                            {
                                lineCopy += "X";
                            }

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
                    if (isOneBlackStone == 0 && line == "OO" && emptyCount >= 4)
                        isOneBlackStone = 1;
                    if(emptyCount <= 1 && line.Contains("X"))
                    {
                        break;
                    }
                    if (line == "OOO" || lineCopy == "OOO")
                    {
                        if (emptyCount < 2)
                        {
                            if (!checkUntilThree)
                                checkUntilThree = true;
                            else
                            {
                                int RuleCheck = IsCheckMoreOverThree(player, d, rememberPos, stonePos);
                                if (RuleCheck == 1)
                                    return 1;
                                else if (RuleCheck == 2)
                                    return 2;
                                else
                                    return 0;
                            }
                        }
                    }
                    if (line == "OOOO" || lineCopy == "OOOO")
                    {
                        if (emptyCount < 2)
                        {
                            if (!checkUntilFour)
                            {
                                //lineCopy = line;
                                checkUntilFour = true;
                            }
                            else
                            {
                                //Debug.Log("OOOO");
                                int RuleCheck = IsCheckMoreOverThree(player, d, rememberPos, stonePos);
                                if (RuleCheck == 1)
                                    return 2;
                                else if (RuleCheck == 2)
                                    return 3;
                                else
                                    return 0;
                            }
                        }
                    }

                }
            }
        }
        return 0;
    }

    public int IsCheckMoreOverThree(int player, int excludeDir, int rememberPos, int[] stonePos)
    {
        for (int stonePosArrIndex = 0; stonePosArrIndex < rememberPos; stonePosArrIndex++) // 이전 좌표로 돌아가서 검사
        {
            // stonePos배열에 들어 있는 값이 -1(좌표정보 없음)이면 함수 탈출
            if (stonePos[stonePosArrIndex * 2] == -1)
                return 0;
            for (int d = 0; d < 4; d++) // 4개의 방향을 확인
            {
                if (d == excludeDir) // 진행하던 방향 제외
                    continue;

                string lineCopy = "";
                bool checkUntilThree = false;
                bool checkUntilFour = false;
                int isOneBlackStone = 0;

                for (int dir = -1; dir <= 1; dir += 2)
                {
                    string line = "";
                    bool isEmptyStart = false;
                    bool isEnemyStone = false;
                    int emptyCookie = 0;
                    int emptyCount = 0;
                    int AfterThreeEmptyCount = 1;

                    for (int i = 0; i < 7; i++)
                    {

                        int r = stonePos[stonePosArrIndex * 2] + i * dir * dx[d];
                        int c = stonePos[stonePosArrIndex * 2 + 1] + i * dir * dy[d];

                        if (r >= 0 && r < m_boardSize && c >= 0 && c < m_boardSize)
                        {
                            if (m_currentBoardStateInit.m_CurrentBoardState[r, c] == player)
                            {
                                line += "O";

                                //돌이 2 1 3 인 경우를 검출하기 위한 코드
                                if (isOneBlackStone == 1)
                                {
                                    line += "O";
                                    isOneBlackStone = -1;
                                }

                                // 흑돌 사이에 백돌이 없거나, 사이에 빈 공간이 있을 때 실행
                                if (!isEnemyStone && emptyCookie == 1)
                                {
                                    isEmptyStart = BoolConverter(isEmptyStart);
                                    emptyCookie = -1;
                                }

                                if (lineCopy == "OOO" && line == "O") //lineCopy가 OOO 일 때, line이 O 이라면 r = row, c = col(현재 위치)이므로 패스
                                    continue;
                                // +O+OO+형태(+는 빈칸)에서 중간 + 위치에 흑돌을 둔 경우를 검출(진행방향은 ->로 가정)
                                if (checkUntilThree && line == "OO")
                                    lineCopy += "O";
                            }
                            else if (m_currentBoardStateInit.m_CurrentBoardState[r, c] == -1)
                            {
                                if (checkUntilThree && emptyCount < 2 && AfterThreeEmptyCount <= 2)
                                {
                                    if (!checkUntilFour && lineCopy == "")
                                        lineCopy = line;
                                    emptyCookie = -1;

                                    if (!checkUntilFour && AfterThreeEmptyCount == 2 && dir == -1)
                                        break;
                                    else if (AfterThreeEmptyCount == 1)
                                    {
                                        AfterThreeEmptyCount++;
                                        continue;
                                    }
                                    if (checkUntilFour && dir == -1)
                                    {
                                        lineCopy = line;
                                        break;
                                    }
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

                                if (lineCopy == "OOO" || lineCopy == "OOOO")
                                {
                                    lineCopy += "X";
                                }

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
                        if (isOneBlackStone == 0 && line == "OO" && emptyCount >= 4)
                            isOneBlackStone = 1;
                        if (emptyCount <= 1 && line.Contains("X"))
                        {
                            //Debug.Log("Cutting");
                            break;
                        }
                        if (line == "OOO" || lineCopy == "OOO")
                        {
                            if (emptyCount < 2)
                            {
                                if (!checkUntilThree)
                                    checkUntilThree = true;
                                else
                                {
                                    //Debug.Log("OOO");
                                        return 1;
                                }
                            }
                        }
                        if (line == "OOOO" || lineCopy == "OOOO")
                        {
                            if (emptyCount < 2)
                            {
                                if (!checkUntilFour)
                                {
                                    //lineCopy = line;
                                    checkUntilFour = true;
                                }
                                else
                                {
                                    //Debug.Log("OOOO");
                                    return 2;
                                }
                            }
                        }
                    }
                }
            }
        }
        return 0;
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