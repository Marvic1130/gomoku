using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GridVertexPrefab
public class StoneOnMouse : MonoBehaviour
{
    public GameObject m_onMousePrefab;

    GameObject generatedAStone;

    private void OnMouseEnter()
    {
            generatedAStone = Instantiate(m_onMousePrefab, transform.position, Quaternion.identity);
    }

    private void OnMouseExit()
    {
        if (generatedAStone != null)
        {
            Destroy(generatedAStone);
        }
    }
}
