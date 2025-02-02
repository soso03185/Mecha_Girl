using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillList : Singleton<SkillList>
{
    public List<GameObject> prefabList = new List<GameObject>();
    public List<Skill> skillList;
    public Skill idleAttack;
    int index = 0;

    public override void Awake()
    {
        base.Awake();

        var obj = FindObjectsOfType<SkillList>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (var skill in prefabList)
        {
            GameObject go = Instantiate(skill, this.transform);
            skillList.Add(go.GetComponent<Skill>());
        }
    }

    public void Init()
    {
        skillList.Sort((a, b) => a.m_skillID.CompareTo(b.m_skillID));

        foreach (var sprite in SpriteManager.instance.skillIdSprites)
        {
            foreach(var skill in skillList)
            {
                if (sprite.Key == skill.m_skillName)
                {
                    skill.m_image = sprite.Value;
                }
            }
        }

        foreach(var skill in skillList)
        {
            skill.Init();
        }
    }

    public Skill GetSkill(int skillId)
    {
        foreach(var skill in skillList)
        {
            if(skill.GetComponent<Skill>().m_skillID == skillId)
            {
                return skill;
            }
        }
        return null;
    }

    public Skill GetSkillByName(string skillName)
    {
        foreach (var skill in skillList)
        {
            if (skill.GetComponent<Skill>().m_skillName == skillName)
            {
                return skill;
            }
        }
        return null;
    }
}
