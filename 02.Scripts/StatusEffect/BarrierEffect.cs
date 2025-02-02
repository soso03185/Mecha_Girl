using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierEffect : Barrier
{
    float m_elapsedTime;
    public BarrierEffect(Skill skill, ICreature target, float duration, float barrierAmount)
        : base(skill, "BarrierEffect", target, duration, barrierAmount)
    {
        m_maxBarrier = barrierAmount;
        m_remainingBarrier = m_maxBarrier;
    }

    protected override Barrier Clone(Skill skill, ICreature target, float duration, float barrierAmount)
    {
        return new BarrierEffect(skill, target, duration, barrierAmount);
    }

    public override void Update()
    {
        base.Update();

        if(m_remainingBarrier <= 0)
        {
            RemoveEffect();
        }

        m_elapsedTime += Time.deltaTime;
        if (m_elapsedTime >= 1)
        {
            (m_target as Player).Mana += 5;
            m_elapsedTime = 0;
        }
    }
    protected override void RemoveEffect()
    {
        List<MonsterScript> target = Managers.Monsters.GetMonsterInRange((m_target as Player).gameObject.transform.position, 3);
        foreach (MonsterScript monster in target)
        {
            monster.IsDamaged(m_skill, (m_maxBarrier - m_remainingBarrier) / target.Count, 0f);
        }
        base.RemoveEffect();
    }
}
