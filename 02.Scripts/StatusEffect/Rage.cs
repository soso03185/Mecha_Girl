using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rage : AbnormalStatus
{
    float m_accumulatedDamageIncrease = 0f;
    public Rage(Skill skill, ICreature target, float duration)
       : base(skill, "Rage", target, duration)
    {
    }

    protected override AbnormalStatus Clone(Skill skill, ICreature target, float duration)
    {
        return new Rage(skill, target, duration);
    }

    public void Activate()
    {
        m_target.DamagePercentage += 15;
        m_accumulatedDamageIncrease += 15;
    }

    protected override void RemoveEffect()
    {
        base.RemoveEffect();
        m_target.DamagePercentage -= m_accumulatedDamageIncrease;
    }
}
