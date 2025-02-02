using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillExplanationPanel : MonoBehaviour
{
    public void OnEnable()
    {
        Time.timeScale = 0.0000001f;
    }

    public void OnDisable()
    {
        Time.timeScale = Managers.Data.m_timeScale;
    }
}
