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
        // ���� �� �ػ� ���� �Լ� ȣ��
        AdjustUIRatio();

    }

    void Update()
    {
        // ����� �ػ󵵰� ����Ǹ� UI ���� ����
        if (Screen.width != m_canvasScaler.referenceResolution.x || Screen.height != m_canvasScaler.referenceResolution.y)
        {
            AdjustUIRatio();
        }
    }

    void AdjustUIRatio()
    {
        // ���� ����� �ػ� ��������
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // UI ���� ���� ����
        // ��: ���� ������ �׻� �����ϰ� ���� ������ ����
        //m_canvasScaler.referenceResolution = new Vector3(screenWidth / 2.0f + m_mainCamera.WorldToScreenPoint(m_board.transform.position.x), screenWidth / 2.0f);
    }
}
