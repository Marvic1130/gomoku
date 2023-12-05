using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RayCastUtil : MonoBehaviour
{
    public static bool RaycastFromMouse(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //UI 위에서 마우스 이벤트 제한
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            //hit = new RaycastHit();
            return false;
        }
        return Physics.Raycast(ray, out hit, Mathf.Infinity);
    }
}
