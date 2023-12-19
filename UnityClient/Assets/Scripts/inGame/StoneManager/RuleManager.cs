using System.Collections;
using System.Collections.Generic;
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

        for (int d = 0; d < 4; d++) // 4���� ������ Ȯ��
        {
            string lineCopy = "";
            bool isStart = false; //row�� col�� ��(CheckRule()���� emptyObject�� ����)�� �ξ����°�?
            bool checkUntilThree = false; //���� �� ���� üũ�ϰ� �ٷ� IsCheckMoreThree�� �Ѿ�� ���� ����
            bool checkUntilFour = false;
            int rememberPos = 0;
            int isOneBlackStone = 0; //���� 2 1 3 �� ��츦 �����ϱ� ���� �ڵ�. (�̷� ���� 0, isOneBlackStone Ȱ��ȭ 1, ��Ȱ��ȭ -1)

            for (int dir = -1; dir <= 1; dir += 2)
            {
                string line = "";
                bool isEmptyStart = false;
                bool isEnemyStone = false;
                int emptyCookie = 0; // �� ������ �˻��� �� empty�� �������� �ִ� ��츦 �˻��ϴ� ����(empty �̷� ���� 0, empty Ȱ��ȭ 1, empty ��Ȱ��ȭ -1)
                int emptyCount = 0;
                int AfterThreeEmptyCount = 1;

                for (int i = 0; i < 7; i++)
                {
                    int r = row + i * dir * dx[d];
                    int c = col + i * dir * dy[d];

                    if (r >= 0 && r < m_boardSize && c >= 0 && c < m_boardSize)
                    {
                        // m_CurrentBoardState[r, c]�� �浹�� ��. �ݼ� üũ�� �浹���� ����
                        if (m_currentBoardStateInit.m_CurrentBoardState[r, c] == player)
                        {
                            if (rememberPos < 4) // rememberPos�� 4�� ���� �ټ���° �浹�̹Ƿ� ������ �ϼ��� ����. ���� ������ �ʿ� ����
                            {
                                // m_stonePos �迭�� r, c �� ����
                                stonePos[rememberPos * 2] = r;
                                stonePos[rememberPos * 2 + 1] = c;
                                rememberPos++;
                            }

                            line += "O";

                            //dir�� -1���� 1�� �Ѿ ��, stonePos�迭�� r=row, c=col(������ġ) ���� �ߺ� ����Ǵ� ���� �����ϱ� ���� �ڵ�
                            // ����� ��, stonePos[0] = �ι�° ��ġ, stonePos[1] = ����° ��ġ, stonePos[2] = ������ġ or ����° ��ġ,
                            //stonePos[3] = �ݴ���� �ι�° ��ġ or ������ġ �� ����ȴ�.
                            if (!isStart)
                            {
                                isStart = true;
                                rememberPos--;
                            }

                            //���� 2 1 3 �� ��츦 �����ϱ� ���� �ڵ�
                            if (isOneBlackStone == 1)
                            {
                                line += "O";
                                isOneBlackStone = -1;
                            }

                            // �浹 ���̿� �鵹�� ���ų�, ���̿� �� ������ ���� �� ����
                            if (!isEnemyStone && emptyCookie == 1)
                            {
                                isEmptyStart = BoolConverter(isEmptyStart);
                                emptyCookie = -1;
                            }

                            if (lineCopy == "OOO" && line == "O") //lineCopy�� OOO �� ��, line�� O �̶�� r = row, c = col(���� ��ġ)�̹Ƿ� �н�
                                continue;
                            // +O+OO+����(+�� ��ĭ)���� �߰� + ��ġ�� �浹�� �� ��츦 ����(��������� ->�� ����)
                            if (checkUntilThree && line == "OO")
                                lineCopy += "O";

                        }
                        // m_CurrentBoardState[r, c]�� ��ĭ�� ��
                        else if (m_currentBoardStateInit.m_CurrentBoardState[r, c] == -1)
                        {
                            // checkUntilThree Ȱ��ȭ ����, ���� ù ��°�� �ι�° ��ġ�� Ȯ���� ��, �� �� ��ĭ�� ���.
                            if (checkUntilThree && emptyCount < 2 && AfterThreeEmptyCount <= 2)
                            {
                                if (!checkUntilFour && lineCopy == "")
                                    lineCopy = line;
                                emptyCookie = -1;

                                //AfterThreeEmptyCount == 2 ���� break. dir == 1�϶��� �״�� ������
                                if (!checkUntilFour && AfterThreeEmptyCount == 2 && dir == -1)
                                    break;
                                if (AfterThreeEmptyCount == 1)
                                {
                                    AfterThreeEmptyCount++;
                                    continue;
                                }
                                //���� �� ���� ��
                                if (checkUntilFour && dir == -1)
                                {
                                    lineCopy = line;
                                    break;
                                }
                            }
                            else if (!checkUntilFour && checkUntilThree && dir == -1)
                                break;
                            if (emptyCookie == 0) // line�� O�� �ϳ��� ���� ��(���Ĵ� ��ĭ�̹Ƿ� emptyCount++�� �ϱ� ����), �� �� �̻� ������� ���� ���κп��� ó��
                            {
                                isEmptyStart = BoolConverter(isEmptyStart);
                                emptyCookie = 1;
                            }

                        }
                        // m_CurrentBoardState[r, c]�� �鵹�� ��
                        else
                        {
                            line += "X";  // ���� ���� 'X'
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
                        line += "X";  // ���� �ٱ��� ��ġ�� 'X'�� ó��
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
        for (int stonePosArrIndex = 0; stonePosArrIndex < rememberPos; stonePosArrIndex++) // ���� ��ǥ�� ���ư��� �˻�
        {
            // stonePos�迭�� ��� �ִ� ���� -1(��ǥ���� ����)�̸� �Լ� Ż��
            if (stonePos[stonePosArrIndex * 2] == -1)
                return 0;
            for (int d = 0; d < 4; d++) // 4���� ������ Ȯ��
            {
                if (d == excludeDir) // �����ϴ� ���� ����
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

                                //���� 2 1 3 �� ��츦 �����ϱ� ���� �ڵ�
                                if (isOneBlackStone == 1)
                                {
                                    line += "O";
                                    isOneBlackStone = -1;
                                }

                                // �浹 ���̿� �鵹�� ���ų�, ���̿� �� ������ ���� �� ����
                                if (!isEnemyStone && emptyCookie == 1)
                                {
                                    isEmptyStart = BoolConverter(isEmptyStart);
                                    emptyCookie = -1;
                                }

                                if (lineCopy == "OOO" && line == "O") //lineCopy�� OOO �� ��, line�� O �̶�� r = row, c = col(���� ��ġ)�̹Ƿ� �н�
                                    continue;
                                // +O+OO+����(+�� ��ĭ)���� �߰� + ��ġ�� �浹�� �� ��츦 ����(��������� ->�� ����)
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
                                line += "X";  // ���� ���� 'X'
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
                            line += "X";  // ���� �ٱ��� ��ġ�� 'X'�� ó��
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