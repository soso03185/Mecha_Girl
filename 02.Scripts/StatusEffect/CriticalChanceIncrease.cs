using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalChanceIncrease : BuffDebuff
{
    public CriticalChanceIncrease(Skill skill, ICreature target, float duration, float percentage)
       : base(skill, "CriticalChanceIncrease", target, duration, percentage)
    {
    }

    protected override BuffDebuff Clone(Skill skill, ICreature target, float duration, float percentage)
    {
        return new CriticalChanceIncrease(skill, target, duration, percentage);
    }

    protected override void ActivateEffect()
    {
        if (m_target != null)
        {
            m_target.CriticalChance += m_percentage;
        }
    }

    protected override void RemoveEffect()
    {
        if (m_target != null)
        {
            m_target.CriticalChance -= m_percentage;
        }
        base.RemoveEffect();
    }
}
