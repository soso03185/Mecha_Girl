using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbnormalStatus : StatusEffect
{
    public AbnormalStatus(Skill skill, string effectName, ICreature target, float duration)
    {
        m_skill = skill;
        m_target = target;
        m_effectName = effectName;
        m_duration = duration;
    }

    public virtual void ApplyEffect(Skill skill, ICreature target, float duration)
    {
        var key = m_effectName;
        if (target.AbnormalStatus.ContainsKey(key))
        {
            if (target.AbnormalStatus[key].m_duration < duration)
            {
                target.AbnormalStatus[(key)].m_duration = duration;
            }
        }
        else
        {
            AbnormalStatus newAbnormalStatus = Clone(skill, target, duration);
            target.AbnormalStatus.Add(key, newAbnormalStatus);
            newAbnormalStatus.ActivateEffect();
            newAbnormalStatus.PlayEffect();
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
        var key = m_effectName;
        if (m_target != null && m_target.AbnormalStatus.ContainsKey(key))
        {
            m_target.AbnormalStatusStrings.Add(key);
        }

        if (effect != null)
        {
            effect.SetActive(false);
        }
    }

    protected virtual AbnormalStatus Clone(Skill skill, ICreature target, float duration)
    {
        return new AbnormalStatus(skill, "", target, duration);
    }
}
