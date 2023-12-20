using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

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

    [SerializeField] GameObject m_goBoard;

    CurrentBoardStateInit m_currentBoardStateInit;
    RuleManager m_ruleManager;
    KeyboardInputManager m_keyboardIM;
    ToAIDataSender m_toAIDataSender;
    public RaycastHit hit;

    //m_player 1(true)�� ����, 1(true)�� �浹, 0(false)�� �鵹 & player ����
    bool m_player = true;
    static bool m_isBlackStone = true;
    static bool m_isWhiteStone = false;

    // CheckRule()�� ���� 33�ݼ��� ������ �Ǿ����� & ù���� �ξ����ִ����� �˷��ִ� ���� & �¸��ߴ����� �˷��ִ� ����
    bool m_isStoneThree = false;
    bool m_isFirstMove = false;
    bool m_isWin = false;

    //AI ��� ���� AI�� �� ��ġ
    int m_AIrow, m_AIcol;

    //�ٸ� ��ũ��Ʈ���� ����� ���� ������Ƽ
    public bool m_IsPlayer { get { return m_player; } set { m_player = value; } }
    public bool m_IsFirstMove { get { return m_isFirstMove; } set { m_isFirstMove = value; } }
    public bool m_IsWin { get { return m_isWin; } set { m_isWin = value; } }

    void JsonInit(int player)
    {

        string jsonData;
        GameEndJson gameEndJson = new GameEndJson();
        GameDataSender data = new GameDataSender();

        if (player == 1)
            gameEndJson.outcome = "Win";
        else
            gameEndJson.outcome = "Lose";

        jsonData = JsonUtility.ToJson(gameEndJson);
        data.Data("/game/end", jsonData, "Server");

        Debug.Log(jsonData);
        StartCoroutine(data.SendJsonData(OnSuccessCallback, OnFailureCallback));
    }
    void OnSuccessCallback()
    {
        Debug.Log("������ ���� ����");
    }

    // �α��� ���� �� ������ �޼���
    void OnFailureCallback()
    {
        Debug.Log("������ ���� ����");
    }
    void CreateWinUI(int player)
    {
        var UIobj = Instantiate(m_player ? m_BlackWinUIPrefabs : m_whiteWinUIPrefabs);
        UIobj.transform.SetParent(m_UI.transform);
        UIobj.transform.position = new Vector3(-3.35f, 6.65f, -1);
        UIobj.GetComponent<RectTransform>().sizeDelta = new Vector2(Camera.main.pixelWidth/16, Camera.main.pixelHeight/4);
        Image imageComponent = UIobj.GetComponent<Image>();

        if (imageComponent != null)
        {
            // �̹��� ũ�� ������ ���� preserveAspect �Ӽ� ���
            imageComponent.preserveAspect = true;
        }

        m_UI.transform.Find("Splashback").gameObject.SetActive(true);
        m_isWin = true;

        JsonInit(player);
    }

    //�ð� ���� �Լ�
    IEnumerator DelayedFunction(int frameCount)
    {
        Debug.Log("Before Delay");

        for (int i = 0; i < frameCount; i++)
        {
            yield return null; // �� ������ ���
        }
        Debug.Log("After 1 second Delay");

        yield break;
    }


    void Start()
    {
        SetLanguage();
        Screen.SetResolution(1920, 980, false);
        m_currentBoardStateInit = FindObjectOfType<CurrentBoardStateInit>();
        m_ruleManager = FindObjectOfType<RuleManager>();
        m_keyboardIM = FindObjectOfType<KeyboardInputManager>();
        m_toAIDataSender = FindObjectOfType<ToAIDataSender>();

        //m_toAIDataSender._Main();
    }
    void SetLanguage()
    {
        var languageCode = GameObject.Find("DontDestroyData").GetComponent<DontDestroyData>().m_Langauge;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(languageCode);

        var Menubar = m_UI.transform.Find("Menubar");

        // ������ ��� ������ ���߾� ��ư�� �ݴ�� �����ϱ� (en > �ѱ���, ko > English)
        if (languageCode == "en")
        {
            Menubar.transform.Find("Language_ko").gameObject.SetActive(true);
            Menubar.transform.Find("Language_en").gameObject.SetActive(false);


        }
        else if (languageCode == "ko")
        {
            Menubar.transform.Find("Language_ko").gameObject.SetActive(false);
            Menubar.transform.Find("Language_en").gameObject.SetActive(true);
        }
    }
    void Update()
    {
        //Ű���� �Է� Ȯ��
        m_keyboardIM.KeyboardInput();

        //if (m_player)
        //{
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
                            //Debug.Log("�� �ڸ����� �ٵϵ��� �ֽ��ϴ�.");
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
                            else if (m_currentBoardStateInit.m_Row == 7 && m_currentBoardStateInit.m_Col == 7 && m_isFirstMove == false)
                            {
                                Destroy(obj);
                                m_isFirstMove = true;
                            }

                            if (m_isFirstMove)
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

                                    case 2:
                                        Debug.Log("34�Դϴ�. �� �ڸ��� �� �� �����ϴ�.");
                                        Destroy(obj);
                                        m_currentBoardStateInit.TempBSInit(false);
                                        m_isStoneThree = true;
                                        break;
                                    case 3:
                                        Debug.Log("44�Դϴ�. �� �ڸ��� �� �� �����ϴ�.");
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
                                    CreateWinUI(player);
                                }

                                // AI ���
                                //if (m_player && !m_isWin)
                                //{
                                //    m_toAIDataSender.InitializeClient(out m_AIrow, out m_AIcol);
                                //    Debug.Log(m_AIrow + ", " + m_AIcol);
                                //}

                                if (!m_isStoneThree && m_isFirstMove)
                                {
                                    m_player = m_player ? m_isWhiteStone : m_isBlackStone;
                                }
                            }
                        }
                    }
                }
            }
        //}
        //AI(���)�� ��
        //    else
        //    {
        //        if (!m_isWin)
        //        {
        //            try
        //            {
        //                var obj = Instantiate(m_whiteStonePrefabs, new Vector3((m_AIrow - 1) * 0.86f, (m_AIcol - 1) * 0.86f, 0), Quaternion.identity);
        //                obj.transform.SetParent(m_instantedStone.transform);
        //                m_currentBoardStateInit.UpdateBoardStateAI(obj, m_player, m_AIrow, m_AIcol);

        //                m_player = m_player ? m_isWhiteStone : m_isBlackStone;
        //            }
        //            catch
        //            {
        //                Debug.Log("����");

        //                m_player = m_player ? m_isWhiteStone : m_isBlackStone;
        //            }
        //        }
        //    }
    }
}
