using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

//Stone Manager Object 메인 클래스

public class StoneManager : MonoBehaviour
{
    [SerializeField] GameObject m_instantedStone; //StonePrefab 오브젝트를 담을 부모 오브젝트
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

    //m_player 1(true)로 시작, 1(true)은 흑돌, 0(false)은 백돌 & player 정보
    bool m_player = true;
    static bool m_isBlackStone = true;
    static bool m_isWhiteStone = false;

    // CheckRule()을 통해 33금수가 검출이 되었는지 & 첫수가 두어져있는지를 알려주는 변수 & 승리했는지를 알려주는 변수
    bool m_isStoneThree = false;
    bool m_isFirstMove = false;
    bool m_isWin = false;

    //AI 통신 이후 AI가 둔 위치
    int m_AIrow, m_AIcol;

    //다른 스크립트에서 사용할 변수 프로퍼티
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
        Debug.Log("데이터 전송 성공");
    }

    // 로그인 실패 시 실행할 메서드
    void OnFailureCallback()
    {
        Debug.Log("데이터 전송 실패");
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
            // 이미지 크기 조절을 위해 preserveAspect 속성 사용
            imageComponent.preserveAspect = true;
        }

        m_UI.transform.Find("Splashback").gameObject.SetActive(true);
        m_isWin = true;

        JsonInit(player);
    }

    //시간 지연 함수
    IEnumerator DelayedFunction(int frameCount)
    {
        Debug.Log("Before Delay");

        for (int i = 0; i < frameCount; i++)
        {
            yield return null; // 한 프레임 대기
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

        // 설정된 언어 설정에 맞추어 버튼은 반대로 설정하기 (en > 한국어, ko > English)
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
        //키보드 입력 확인
        m_keyboardIM.KeyboardInput();

        //if (m_player)
        //{
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    //UI 위에서 마우스 이벤트 제한
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        // 특정 위치 스톤 오브젝트 중복 생성 제한
                        if (hit.collider.gameObject.tag == "Stone")
                        {
                            //Debug.Log("이 자리에는 바둑돌이 있습니다.");
                        }
                        else if (hit.collider.gameObject.tag != "Stone")
                        {
                            //m_player가 true(1)일 때 흑돌, false(0)일 때 백돌
                            int player = m_player ? 1 : 0;

                            // obj에 emptyObject를 할당하여 위치정보 확인 후, GetMatrixNum에 데이터 저장. GetMatrixNum에서 생성한 데이터로 CheckRule을 진행한 뒤 switch문 안에서 emptyObject를 파괴 후 StoneObject를 생성
                            var obj = Instantiate(m_emptyObject, hit.collider.bounds.center, Quaternion.identity);
                            obj.transform.SetParent(m_instantedStone.transform);
                            m_currentBoardStateInit.GetMatrixNum(hit.point);

                            if (m_currentBoardStateInit.m_Row != 7 && m_currentBoardStateInit.m_Col != 7 && m_isFirstMove == false)
                            {
                                Debug.Log("렌쥬룰이 적용된 게임입니다. 흑돌은 무조건 정중앙부터 시작해야 합니다.");
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
                                        Debug.Log("33입니다. 이 자리에 둘 수 없습니다.");
                                        Destroy(obj);
                                        m_currentBoardStateInit.TempBSInit(false);
                                        m_isStoneThree = true;
                                        break;

                                    case 2:
                                        Debug.Log("34입니다. 이 자리에 둘 수 없습니다.");
                                        Destroy(obj);
                                        m_currentBoardStateInit.TempBSInit(false);
                                        m_isStoneThree = true;
                                        break;
                                    case 3:
                                        Debug.Log("44입니다. 이 자리에 둘 수 없습니다.");
                                        Destroy(obj);
                                        m_currentBoardStateInit.TempBSInit(false);
                                        m_isStoneThree = true;
                                        break;

                                    //case 4:
                                    //    Debug.Log("장목입니다. 이 자리에 둘 수 없습니다.");
                                    //    break;

                                    default:
                                        // empty 데이터 해제
                                        Destroy(obj);
                                        m_currentBoardStateInit.TempBSInit(false);
                                        m_isStoneThree = false;

                                        // Stone 데이터 입력
                                        obj = Instantiate(m_player ? m_blackStonePrefabs : m_whiteStonePrefabs, hit.collider.bounds.center, Quaternion.identity);
                                        obj.transform.SetParent(m_instantedStone.transform);
                                        m_currentBoardStateInit.UpdateBoardState(obj, hit, m_player);
                                        break;
                                }


                                // Black/White Win 결과창 생성
                                if (m_ruleManager.CheckWin(player, m_currentBoardStateInit.m_Row, m_currentBoardStateInit.m_Col))
                                {
                                    CreateWinUI(player);
                                }

                                // AI 통신
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
        //AI(흰색)일 때
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
        //                Debug.Log("오류");

        //                m_player = m_player ? m_isWhiteStone : m_isBlackStone;
        //            }
        //        }
        //    }
    }
}
