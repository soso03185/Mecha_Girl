using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIncrease : BuffDebuff
{
    public DamageIncrease(Skill skill, ICreature target, float duration, float percentage)
      : base(skill, "DamageIncrease", target, duration, percentage)
    {
    }

    protected override BuffDebuff Clone(Skill skill, ICreature target, float duration, float percentage)
    {
        return new DamageIncrease(skill, target, duration, percentage);
    }

    protected override void ActivateEffect()
    {
        if (m_target != null)
        {
            m_target.DamagePercentage += m_percentage;
        }
    }

    protected override void RemoveEffect()
    {
        if (m_target != null)
        {
            m_target.DamagePercentage -= m_percentage;
        }
        base.RemoveEffect();
    }
}
