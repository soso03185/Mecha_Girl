using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalMultiplierIncrease : BuffDebuff
{  
    public CriticalMultiplierIncrease(Skill skill, ICreature target, float duration, float percentage)
       : base(skill, "CriticalMultiplierIncrease", target, duration, percentage)
    {
    }

    protected override BuffDebuff Clone(Skill skill, ICreature target, float duration, float percentage)
    {
        return new CriticalMultiplierIncrease(skill, target, duration, percentage);
    }

    protected override void ActivateEffect()
    {
        if (m_target != null)
        {
            m_target.CriticalMultiplier += m_percentage;
        }
    }

    protected override void RemoveEffect()
    {
        if (m_target != null)
        {
            m_target.CriticalMultiplier -= m_percentage;
        }
        base.RemoveEffect();
    }
}
