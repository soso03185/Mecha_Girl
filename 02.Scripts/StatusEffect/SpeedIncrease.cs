using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedIncrease : BuffDebuff
{
    public SpeedIncrease(Skill skill, ICreature target, float duration, float percentage)
        : base(skill, "SpeedReduction", target, duration, percentage)
    {
    }

    protected override BuffDebuff Clone(Skill skill, ICreature target, float duration, float percentage)
    {
        return new SpeedIncrease(skill, target, duration, percentage);
    }

    protected override void ActivateEffect()
    {
        if (m_target != null)
        {
            m_target.MoveSpeedPercentage += m_percentage;
        }
    }

    protected override void RemoveEffect()
    {
        if (m_target != null)
        {
            m_target.MoveSpeedPercentage -= m_percentage;
        }
        base.RemoveEffect();
    }
}
