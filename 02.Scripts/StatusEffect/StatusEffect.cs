using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect 
{
    public Skill m_skill;
    public string m_effectName;
    public ICreature m_target;
    protected float m_duration;

    protected string effectPath;
    protected bool isEffect = false;
    protected GameObject effect;

    protected virtual void ActivateEffect()
    {

    }


    protected void PlayEffect()
    {
        if (effect == null && isEffect)
        {
            if (m_target is Player)
            {
                effect = Managers.Resource.Instantiate(effectPath, (m_target as Player).gameObject.transform);
            }
            else
            {
                effect = Managers.Resource.Instantiate(effectPath, (m_target as MonsterScript).gameObject.transform);
            }
        }
        else if (effect != null)
            effect.SetActive(true);
    }
}
