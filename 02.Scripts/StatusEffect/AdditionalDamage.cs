using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalDamage : BuffDebuff
{
    public AdditionalDamage(Skill skill, ICreature target, float duration, float percentage)
    : base(skill, "AdditionalDamage", target, duration, percentage)
    {
    }

    protected override BuffDebuff Clone(Skill skill, ICreature target, float duration, float percentage)
    {
        return new AdditionalDamage(skill, target, duration, percentage);
    }

    protected override void ActivateEffect()
    {
        if (m_target != null)
        {
            (m_target as IMonster).AdditionalDamage += m_percentage;
        }
    }

    protected override void RemoveEffect()
    {
        if (m_target != null)
        {
            (m_target as IMonster).AdditionalDamage -= m_percentage;
        }
        base.RemoveEffect();
    }
}
