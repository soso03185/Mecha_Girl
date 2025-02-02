using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSettingCanvas : MonoBehaviour
{
    public TotalPowerUI m_totalPowerUI;    

    private void OnDisable()
    {
        if(m_totalPowerUI != null)
        {
            m_totalPowerUI.gameObject.SetActive(true);
        }
    }
}
