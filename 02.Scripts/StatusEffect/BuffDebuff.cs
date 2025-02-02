using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BuffDebuff : StatusEffect
{
    protected float m_percentage;

    public BuffDebuff(Skill skill, string effectName, ICreature target, float duration, float percentage)
    {
        m_skill = skill;
        m_target = target;
        m_effectName = effectName;
        m_duration = duration;
        m_percentage = percentage;
    }

    public virtual void ApplyEffect(Skill skill, ICreature target, float duration, float percentage)
    {
        var key = new KeyValuePair<Skill, string>(skill, m_effectName);
        if(target.BuffDebuff.ContainsKey(key))
        {
            if (target.BuffDebuff[key].m_duration < duration)
            {
                target.BuffDebuff[(key)].m_duration = duration;
            }
        }
        else
        {
            BuffDebuff newBuffDebuff = Clone(skill, target, duration, percentage);
            target.BuffDebuff.Add(key, newBuffDebuff);
            newBuffDebuff.ActivateEffect();
            newBuffDebuff.PlayEffect();
        }
    }

    public virtual void Update()
    {
        m_duration -= Time.deltaTime;
        if (m_duration <= 0)
        {
            RemoveEffect();
        }
    }

    protected virtual void RemoveEffect()
    {
        var key = new KeyValuePair<Skill, string>(m_skill, m_effectName);
        if (m_target != null && m_target.BuffDebuff.ContainsKey(key))
        {
            m_target.BuffDebuffStrings.Add(key);
        }

        if (effect != null)
        {
            effect.SetActive(false);
        }
    }
    protected virtual BuffDebuff Clone(Skill skill, ICreature target, float duration, float percentage)
    {
        return new BuffDebuff(skill, "", target, duration, percentage);
    }

}
