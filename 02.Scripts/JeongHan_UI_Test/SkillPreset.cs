using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPreset : MonoBehaviour
{
    public int presetNum;
    public string presetName;

    public List<ActionSlot> myPreset = new List<ActionSlot>();

    public int FindSkillCountByName(string _skillName)
    {
        int m_EmptySkillCount = 0;

        foreach (var child in myPreset)
        {
            if (child.skillData.m_skillName == _skillName)
                m_EmptySkillCount++;
        }
        return m_EmptySkillCount;
    }
}
