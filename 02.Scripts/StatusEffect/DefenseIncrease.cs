using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseIncrease : BuffDebuff
{
    public DefenseIncrease(Skill skill, ICreature target, float duration, float percentage)
     : base(skill, "DefenseIncrease", target, duration, percentage)
    {
    }

    protected override BuffDebuff Clone(Skill skill, ICreature target, float duration, float percentage)
    {
        return new DefenseIncrease(skill, target, duration, percentage);
    }

    protected override void ActivateEffect()
    {
        if (m_target != null)
        {
            m_target.DefensePercentage += m_percentage;
        }
    }

    protected override void RemoveEffect()
    {
        if (m_target != null)
        {
            m_target.DefensePercentage -= m_percentage;
        }
        base.RemoveEffect();
    }
}
