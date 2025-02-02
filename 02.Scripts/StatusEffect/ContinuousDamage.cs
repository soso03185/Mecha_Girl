using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousDamage : StatusEffect
{
    protected float m_additionalDamage;
    public ContinuousDamage(Skill skill, string effectName, ICreature target, float duration, float additionalDamage)
    {
        m_skill = skill;
        m_target = target;
        m_effectName = effectName;
        m_duration = duration;
        m_additionalDamage = additionalDamage;
    }

    public virtual void ApplyEffect(Skill skill, ICreature target, float duration, float additionalDamage)
    {
        if (target.ContinuousDamage.ContainsKey(m_effectName))
        {
            if (target.ContinuousDamage[m_effectName].m_duration < duration)
            {
                target.ContinuousDamage[(m_effectName)].m_duration = duration;
            }
        }
        else
        {
            ContinuousDamage newContinuousDamage = Clone(skill, target, duration, additionalDamage);
            target.ContinuousDamage.Add(m_effectName, newContinuousDamage);
            newContinuousDamage.ActivateEffect();
            newContinuousDamage.PlayEffect();
        }
    }
    protected virtual void ActivateEffect()
    {

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
        if (m_target != null && m_target.ContinuousDamage.ContainsKey(m_effectName))
        {
            m_target.ContinuousDamageStrings.Add(m_effectName);
        }

        if (effect != null)
        {
            effect.SetActive(false);
        }
    }
    protected virtual ContinuousDamage Clone(Skill skill, ICreature target, float duration, float additionalDamage)
    {
        return new ContinuousDamage(skill, "", target, duration, additionalDamage);
    }
}
