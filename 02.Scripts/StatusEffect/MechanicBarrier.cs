using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanicBarrier : Barrier
{
    float m_elapsedTime;
    public MechanicBarrier(Skill skill, ICreature target, float duration, float barrierAmount)
    : base(skill, "MechanicBarrier", target, duration, barrierAmount)
    {
        
    }

    protected override void ActivateEffect()
    {
        
    }

    public override void Update()
    {
        base.Update();
        (m_skill as Shield).BarrierUpdate();
    }

    protected override void RemoveEffect()
    {
        base.RemoveEffect();

        (m_skill as Shield).BarrierDestroy(m_remainingBarrier);
    }
    protected override Barrier Clone(Skill skill, ICreature target, float duration, float barrierAmount)
    {
        return new MechanicBarrier(skill, target, duration, barrierAmount);
    }

}
