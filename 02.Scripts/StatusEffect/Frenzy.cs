using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frenzy : AbnormalStatus
{
    float m_accumulatedDamageIncrease = 0f;
    public Frenzy(Skill skill, ICreature target, float duration)
       : base(skill, "Frenzy", target, duration)
    {
    }

    protected override AbnormalStatus Clone(Skill skill, ICreature target, float duration)
    {
        return new Frenzy(skill, target, duration);
    }

    public void Activate()
    {
        m_target.DamagePercentage += 20;
        m_accumulatedDamageIncrease += 20;
    }

    protected override void RemoveEffect()
    {
        base.RemoveEffect();
        m_target.DamagePercentage -= m_accumulatedDamageIncrease;
    }
}
