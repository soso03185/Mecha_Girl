using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionLogicManager : Singleton<ActionLogicManager>
{
    public List<Skill> m_actionLogic = new List<Skill>(7);
    public Skill m_idleAttack;
    public List<int> m_skillIdList = new List<int>();

    public SkillList skillData;

    public BattleAnalysisSystem m_battleAnalysisSystem;

    [SerializeField]
    private int m_currentIndex = 0;

    public int CurrentIndex => m_currentIndex;

    public GameObject rotateActionLogic;

    public void NextSkill()
    {
        m_currentIndex = (m_currentIndex + 1) % m_actionLogic.Count;
    }

    public Skill this[int i]
    {
        get => m_actionLogic[i];
        set => m_actionLogic[i] = value;
    }


    void Start()
    {
        skillData = GameObject.FindGameObjectWithTag("SkillList").GetComponent<SkillList>();

        ChangeSkill();
        rotateActionLogic = GameObject.FindGameObjectWithTag("ActionSlot");
        rotateActionLogic.GetComponent<RotateActionLogic>().Init();
    }

    public void ChangeSkill()
    {
        m_actionLogic.Clear();
        foreach (var data in Managers.Data.currentSkillList)
        {
            m_actionLogic.Add(skillData.skillList[data]);
        }
        m_idleAttack = skillData.idleAttack;
        //foreach (var skill in m_actionLogic)
        //{
        //    skill.gameObject.SetActive(false);
        //}

        m_battleAnalysisSystem.Initialize(m_actionLogic, m_idleAttack);
    }

    public Skill GetCurrentSkill()
    {
        return this[m_currentIndex];
    }


    public void Reset()
    {
        StopAllCoroutines();
        m_currentIndex = -1;
    }

    public void ChangeSkillNameList(int index, int skillId)
    {
        m_skillIdList[index] = skillId;
    }
}
