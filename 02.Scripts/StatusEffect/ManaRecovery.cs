using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaRecovery : BuffDebuff
{
    float m_elapsedTime;
    public ManaRecovery(Skill skill, ICreature target, float duration, float percentage)
     : base(skill, "ManaRecovery", target, duration, percentage)
    {
        effectPath = "Particle/ManaBuff";
        isEffect = true;
    }

    protected override BuffDebuff Clone(Skill skill, ICreature target, float duration, float percentage)
    {
        return new ManaRecovery(skill, target, duration, percentage);
    }

    public override void Update()
    {
        base.Update();
        m_elapsedTime += Time.deltaTime;
        if (m_elapsedTime >= 1)
        {
            if (m_target != null)
            {
                (m_target as IPlayerable).Mana += m_percentage;
            }
            m_elapsedTime = 0;
        }
    }
}
