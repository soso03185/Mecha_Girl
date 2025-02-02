using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GambleChanceUI : MonoBehaviour 
{
    public GambleSystem m_gambleSystem;
    private TextMeshProUGUI m_text;

    private int m_currentLevel;
    public int m_index;

    void Start()
    {
        m_text = gameObject.GetComponent<TextMeshProUGUI>();
        m_text.text = $"{m_gambleSystem.m_gambleChance[m_currentLevel][m_index - 1]}%";
    }

    public void LevelUp()
    {
        if (m_currentLevel != m_gambleSystem.m_gambleChance.Count - 1)
        {
            m_currentLevel++;
            ChangeText();
        }
    }

    public void LevelDown()
    {
        if (m_currentLevel != 0)
        {
            m_currentLevel--;
            ChangeText();
        }
    }
    public void ChangeText()
    {
        if (m_index == 0)
        {
            m_text.text = $"스킬 뽑기 레벨 : {m_currentLevel + 1}";
        }
        else
        {
            m_text.text = $"{m_gambleSystem.m_gambleChance[m_currentLevel][m_index - 1]}%";
        }
    }
}
