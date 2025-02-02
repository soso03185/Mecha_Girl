using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : AbnormalStatus
{
    public Stun(Skill skill, ICreature target, float duration)
      : base(skill, "Stun", target, duration)
    {
    }

    protected override AbnormalStatus Clone(Skill skill, ICreature target, float duration)
    {
        return new Stun(skill, target, duration);
    }
}
