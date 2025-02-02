using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Barrier : StatusEffect
{
    public float m_maxBarrier;
    public double m_remainingBarrier;
    float m_elapsedTime;
    public Barrier(Skill skill, string effectName, ICreature target, float duration, float barrierAmount)
    {
        m_skill = skill;
        m_target = target;
        m_effectName = effectName;
        m_duration = duration;
        m_maxBarrier = barrierAmount;
        m_remainingBarrier = barrierAmount;
    }

    public virtual void ApplyEffect(Skill skill, ICreature target, float duration, float barrierAmount)
    {
        var key = skill;
        int index = target.Barrier.FindIndex(barrier => barrier.m_effectName == m_effectName);
        if (index != -1)
        {
            if (target.Barrier[index].m_duration < duration)
            {
                target.Barrier[(index)].m_duration = duration;
                target.Barrier[index].m_remainingBarrier = barrierAmount;
            }
        }
        else
        {
            Barrier newBarrier = Clone(skill, target, duration, barrierAmount);
            target.Barrier.Add(newBarrier);
            newBarrier.ActivateEffect();
            newBarrier.PlayEffect();
        }
    }

    public virtual void Update()
    {
        if(m_remainingBarrier <= 0)
        {
            RemoveEffect();
        }

        m_duration -= Time.deltaTime;
        if (m_duration <= 0)
        {
            RemoveEffect();
        }
    }

    protected virtual void RemoveEffect()
    {
        int index = m_target.Barrier.FindIndex(x => x.m_effectName == m_effectName);
        if (m_target != null && index != -1)
        {
            m_target.BarrierOnDestroy.Add(m_target.Barrier[index]);
        }

        if (effect != null)
        {
            effect.SetActive(false);
        }
    }

    protected virtual  Barrier Clone(Skill skill, ICreature target, float duration, float percentage)
    {
        return new Barrier(skill, "", target, duration, percentage);
    }

}

