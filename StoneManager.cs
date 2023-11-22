using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Stone Manager Object ���� Ŭ����

public class StoneManager : MonoBehaviour
{
    [SerializeField] GameObject m_instantedStone; //StonePrefab ������Ʈ�� ���� �θ� ������Ʈ
    [SerializeField] GameObject m_blackStonePrefabs;
    [SerializeField] GameObject m_whiteStonePrefabs;
    [SerializeField] GameObject m_emptyObject;

    [SerializeField] GameObject m_UI;
    [SerializeField] GameObject m_whiteWinUIPrefabs;
    [SerializeField] GameObject m_BlackWinUIPrefabs;

    CurrentBoardStateInit m_currentBoardStateInit;
    RuleManager m_ruleManager;
    public RaycastHit hit;

    //m_player 1(true)�� ����, 1(true)�� �浹, 0(false)�� �鵹 & player ����
    bool m_player = true;
    static bool m_isBlackStone = true;
    static bool m_isWhiteStone = false;

    // CheckRule()�� ���� 33�ݼ��� ������ �Ǿ����� & ù���� �ξ����ִ����� �˷��ִ� ����
    bool m_isStoneThree = false;
    bool m_isFirstMove = false;

    //�ٸ� ��ũ��Ʈ���� ����� ���� ������Ƽ
    public bool m_IsPlayer { get { return m_player; } set { m_player = value; } }
    public bool m_IsFirstMove { get { return m_isFirstMove; } set { m_isFirstMove = value; } }

    void Start()
    {
        m_currentBoardStateInit = FindObjectOfType<CurrentBoardStateInit>();
        m_ruleManager = FindObjectOfType<RuleManager>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                //UI ������ ���콺 �̺�Ʈ ����
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    // Ư�� ��ġ ���� ������Ʈ �ߺ� ���� ����
                    if (hit.collider.gameObject.tag == "Stone")
                    {
                        Debug.Log("�� �ڸ����� �ٵϵ��� �ֽ��ϴ�.");
                    }
                    else if (hit.collider.gameObject.tag != "Stone")
                    {
                        //m_player�� true(1)�� �� �浹, false(0)�� �� �鵹
                        int player = m_player ? 1 : 0;

                        // obj�� emptyObject�� �Ҵ��Ͽ� ��ġ���� Ȯ�� ��, GetMatrixNum�� ������ ����. GetMatrixNum���� ������ �����ͷ� CheckRule�� ������ �� switch�� �ȿ��� emptyObject�� �ı� �� StoneObject�� ����
                        var obj = Instantiate(m_emptyObject, hit.collider.bounds.center, Quaternion.identity);
                        obj.transform.SetParent(m_instantedStone.transform);
                        m_currentBoardStateInit.GetMatrixNum(hit.point);

                        if (m_currentBoardStateInit.m_Row != 7 && m_currentBoardStateInit.m_Col != 7 && m_isFirstMove == false)
                        {
                            Debug.Log("������� ����� �����Դϴ�. �浹�� ������ ���߾Ӻ��� �����ؾ� �մϴ�.");
                        }
                        else if(m_currentBoardStateInit.m_Row == 7 && m_currentBoardStateInit.m_Col == 7 && m_isFirstMove == false)
                        {
                            Destroy(obj);
                            m_isFirstMove = true;
                        }

                        if(m_isFirstMove)
                        {
                            m_currentBoardStateInit.TempBSInit(true);

                            int CheckRule = m_ruleManager.CheckRule(player, m_currentBoardStateInit.m_Row, m_currentBoardStateInit.m_Col);

                            switch (CheckRule)
                            {
                                case 1:
                                    Debug.Log("33�Դϴ�. �� �ڸ��� �� �� �����ϴ�.");
                                    Destroy(obj);
                                    m_currentBoardStateInit.TempBSInit(false);
                                    m_isStoneThree = true;
                                    break;

                                //case 4:
                                //    Debug.Log("����Դϴ�. �� �ڸ��� �� �� �����ϴ�.");
                                //    break;

                                default:
                                    // empty ������ ����
                                    Destroy(obj);
                                    m_currentBoardStateInit.TempBSInit(false);
                                    m_isStoneThree = false;

                                    // Stone ������ �Է�
                                    obj = Instantiate(m_player ? m_blackStonePrefabs : m_whiteStonePrefabs, hit.collider.bounds.center, Quaternion.identity);
                                    obj.transform.SetParent(m_instantedStone.transform);
                                    m_currentBoardStateInit.UpdateBoardState(obj, hit, m_player);
                                    break;
                            }

                            // Black/White Win ���â ����
                            if (m_ruleManager.CheckWin(player, m_currentBoardStateInit.m_Row, m_currentBoardStateInit.m_Col))
                            {
                                var UIobj = Instantiate(m_player ? m_BlackWinUIPrefabs : m_whiteWinUIPrefabs);
                                UIobj.transform.SetParent(m_UI.transform);
                                UIobj.transform.position = new Vector3(889, 1278, 0);
                                UIobj.GetComponent<RectTransform>().sizeDelta = new Vector3(2000, 4000);
                            }

                            if (!m_isStoneThree && m_isFirstMove)
                            {
                                m_player = m_player ? m_isWhiteStone : m_isBlackStone;
                                m_isStoneThree = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
