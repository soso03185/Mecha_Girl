using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedIncrease : BuffDebuff
{
    public AttackSpeedIncrease(Skill skill, ICreature target, float duration, float percentage)
         : base(skill, "AttackSpeedIncrease", target, duration, percentage)
    {
    }

    protected override BuffDebuff Clone(Skill skill, ICreature target, float duration, float percentage)
    {
        return new AttackSpeedIncrease(skill, target, duration, percentage);
    }

    protected override void ActivateEffect()
    {
        if (m_target != null)
        {
            m_target.AttackSpeedPercentage += m_percentage;
        }
    }

    protected override void RemoveEffect()
    {
        if (m_target != null)
        {
            m_target.AttackSpeedPercentage -= m_percentage;
        }
        base.RemoveEffect();
    }
}
