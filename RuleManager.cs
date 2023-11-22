using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// ���ӷ� ���� Ŭ����

public class RuleManager : MonoBehaviour
{
    [SerializeField] BoardManager m_boardManager;
    CurrentBoardStateInit m_currentBoardStateInit;


    // ���� ������ ũ�� = 15 (row �� col)
    // start���� m_currentBoardStateInit.m_BoardSize; �ʱ�ȭ
    int m_boardSize;

    // �־��� ��ġ���� 4���� ���� ���ӵ� ������ �˻�.
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

        for (int d = 0; d < 4; d++) // 4���� ������ Ȯ��
        {
            int rememberPos = 0;
            bool isStart = false; //row�� col�� ��(CheckRule()���� emptyObject�� ����)�� �ξ����°�?
            bool isOneBlackStone = false; //���� 2 1 3 �� ��츦 �����ϱ� ���� �ڵ�

            for (int dir = -1; dir <= 1; dir += 2)
            {
                string line = "";
                bool isEmptyStart = false;
                bool isEnemyStone = false;
                bool checkUntilThree = false; //���� �� ���� üũ�ϰ� �ٷ� IsCheckMoreThree�� �Ѿ�� ���� ����
                int emptyCookie = 0; // �� ������ �˻��� �� empty�� �������� �ִ� ��츦 �˻��ϴ� ����(empty �̷� ���� 0, empty Ȱ��ȭ 1, empty ��Ȱ��ȭ -1)
                int emptyCount = 0;

                for (int i = 0; i < 6; i++)
                {
                    int r = row + i * dir * dx[d];
                    int c = col + i * dir * dy[d];

                    if (r >= 0 && r < m_boardSize && c >= 0 && c < m_boardSize)
                    {
                        if (m_currentBoardStateInit.m_CurrentBoardState[r, c] == player)
                        {
                            // m_stonePos �迭�� r, c �� ����
                            stonePos[rememberPos * 2] = r;
                            stonePos[rememberPos * 2 + 1] = c;
                            rememberPos++;

                            line += "O";  // �÷��̾��� ���� 'O'

                            if (!isEnemyStone && emptyCookie == 1) // �浹 ���̿� �鵹�� ���ų�, ���̿� �� ������ ���� �� ����
                            {
                                isEmptyStart = BoolConverter(isEmptyStart);
                                emptyCookie = -1;
                            }

                            if (!isStart)
                                isStart = true;
                            else
                            {
                                if (isOneBlackStone)//���� 2 1 3 �� ��츦 �����ϱ� ���� �ڵ�
                                    line += "O";
                            }

                        }
                        else if (m_currentBoardStateInit.m_CurrentBoardState[r, c] == -1)
                        {
                            if (checkUntilThree && emptyCookie != -1) // checkUntilThree�� �� ��° ��ġ�� Ȯ���� ��, �� ��° ��ġ�� ��ĭ�� ���
                            {
                                emptyCookie = -1;
                                continue;
                            }
                            if (emptyCookie == 0) // line�� O�� �ϳ��� ���� ��(���Ĵ� ��ĭ�̹Ƿ� emptyCount++�� �ϱ� ����), �� �� �̻� ������� ���� ���κп��� ó��
                            {
                                isEmptyStart = BoolConverter(isEmptyStart);
                                emptyCookie = 1;
                            }
                        }
                        else
                        {
                            line += "X";  // ���� ���� 'X'
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
                        line += "X";  // ���� �ٱ��� ��ġ�� 'X'�� ó��
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
        for (int stonePosArrIndex = 0; stonePosArrIndex < rememberPos; stonePosArrIndex++) // ���� ��ǥ�� ���ư��� �˻�
        {
            for (int d = 0; d < 4; d++) // 4���� ������ Ȯ��
            {
                if (d == excludeDir) // �����ϴ� ���� ����
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

                                // m_stonePos �迭�� ����� �� ��° �� ��° ��ǥ�� �����Ϳ��� �浹�� ����Ǿ� ����
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
                                line += "X";  // ���� ���� 'X'
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
                            line += "X";  // ���� �ٱ��� ��ġ�� 'X'�� ó��
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
        for (int d = 0; d < 4; d++) // 4���� ������ Ȯ��
        {
            int count = 1; // ���� ��ġ�������� count ����
            for (int dir = -1; dir <= 1; dir += 2)
            {
                for (int i = 1; i <= 4; i++) // ���� ��ġ���� �ټ� ���� �� Ȯ��
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
                    return true; // �ټ� ���� ���� �������� ���̸� true ��ȯ
                }
            }
        }

        return false; // �ټ� ���� ���� �������� ������ ������ false ��ȯ
    }

    void Start()
    {
        m_currentBoardStateInit = FindObjectOfType<CurrentBoardStateInit>();

        m_boardSize = m_boardManager.m_BoardSize;
    }
}
