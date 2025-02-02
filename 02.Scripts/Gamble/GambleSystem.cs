using System.Collections.Generic;
using UnityEngine;

public class GambleSystem : MonoBehaviour
{
    //public SkillManager m_skillManager = Managers.Skill;
    public GambleResultUI m_gambleResultUi;
    public SkillList m_skillList;
    private readonly RandomItemPicker<Skill> m_skillGamble = new RandomItemPicker<Skill>();
    public List<List<float>> m_gambleChance = new List<List<float>>()
    {
        new List<float>() {80f, 15f, 5f, 0f},
        new List<float>() {70f, 20f, 9f, 1f},
        new List<float>() {50f, 40f, 8f, 2f},
        new List<float>() {40f, 35f, 20f, 5f}
    };

    public int m_gambleLevel;
    private int m_prevGamble;
    private bool m_autoGamble;
    private float m_elapsedTime;
    void Start()
    {
        m_skillList = GameObject.FindGameObjectWithTag("SkillList").GetComponent<SkillList>();    
        List<KeyValuePair<Skill, float>> skillList = new List<KeyValuePair<Skill, float>>();
        foreach (var skill in m_skillList.skillList)
        {
            KeyValuePair<Skill, float> skillPair = new KeyValuePair<Skill, float>(skill, 1);
            skillList.Add(skillPair);
        }
        m_skillGamble.AddDictionaryWithList(skillList);
    }

    void Update()
    {
        m_elapsedTime += Time.deltaTime;
        if (m_autoGamble && m_elapsedTime >= 1f)
        {
            switch (m_prevGamble)
            {
                case 1:
                    SkillGamble();
                    break;
                case 10:
                    SkillGamble10();
                    break;
                case 30:
                    SkillGamble30();
                    break;
            }

            m_elapsedTime = 0f;
        }
    }

    public void SkillGamble()
    {
        List<Skill> skills = new List<Skill>();
        skills.Add(m_skillGamble.GetRandomItem());
        m_gambleResultUi.ShowResult(skills);
        m_prevGamble = 1;
    }

    public void SkillGamble10()
    {
        List<Skill> skills = new List<Skill>();
        for (int i = 0; i < 10; i++)
        {
            Skill skill = m_skillGamble.GetRandomItem();
            //Debug.Log(skill.m_skillName);
            skills.Add(skill);
        }
        m_gambleResultUi.ShowResult(skills);
        m_prevGamble = 10;
    }

    public void SkillGamble30()
    {
        List<Skill> skills = new List<Skill>();
        for (int i = 0; i < 30; i++)
        {
            Skill skill = m_skillGamble.GetRandomItem();
            Debug.Log(skill.m_skillName);
            skills.Add(skill);
        }
        m_gambleResultUi.ShowResult(skills);
        m_prevGamble = 30;
    }

    public void AutoGambleOnOff(bool isOn)
    {
        m_autoGamble = isOn;
    }
}
