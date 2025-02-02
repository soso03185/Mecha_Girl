using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousAdditionalDamage : BuffDebuff
{
    public ContinuousAdditionalDamage(Skill skill, ICreature target, float duration, float percentage)
     : base(skill, "ContinuousAdditionalDamage", target, duration, percentage)
    {
    }

    protected override BuffDebuff Clone(Skill skill, ICreature target, float duration, float percentage)
    {
        return new ContinuousAdditionalDamage(skill, target, duration, percentage);
    }

    protected override void ActivateEffect()
    {
        if (m_target != null)
        {
            (m_target as MonsterScript).ContinuousAdditionalDamage += m_percentage;
        }
    }

    protected override void RemoveEffect()
    {
        if (m_target != null)
        {
            (m_target as MonsterScript).ContinuousAdditionalDamage -= m_percentage;
        }
        base.RemoveEffect();
    }
}
