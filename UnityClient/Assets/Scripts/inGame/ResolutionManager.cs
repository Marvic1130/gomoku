using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField]
    Camera m_mainCamera;
    [SerializeField]
    GameObject m_board;
    [SerializeField]
    CanvasScaler m_canvasScaler;

    void Start()
    {
        // 시작 시 해상도 변경 함수 호출
        AdjustUIRatio();

    }

    void Update()
    {
        // 사용자 해상도가 변경되면 UI 비율 조절
        if (Screen.width != m_canvasScaler.referenceResolution.x || Screen.height != m_canvasScaler.referenceResolution.y)
        {
            AdjustUIRatio();
        }
    }

    void AdjustUIRatio()
    {
        // 현재 사용자 해상도 가져오기
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // UI 비율 조절 로직
        // 예: 가로 비율을 항상 유지하고 세로 비율을 조절
        //m_canvasScaler.referenceResolution = new Vector3(screenWidth / 2.0f + m_mainCamera.WorldToScreenPoint(m_board.transform.position.x), screenWidth / 2.0f);
    }
}
