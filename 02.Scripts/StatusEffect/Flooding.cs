using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flooding : AbnormalStatus
{
    GameObject particle = null;

    Transform particleParent;
    public Flooding(Skill skill, ICreature target, float duration)
         : base(skill, "Flooding", target, duration)
    {
    }

    protected override AbnormalStatus Clone(Skill skill, ICreature target, float duration)
    {
        return new Flooding(skill, target, duration);
    }

    public override void ApplyEffect(Skill skill, ICreature target, float duration)
    {
        if ((target as MonsterScript).Attribute == Attribute.Water)
        {
            var immuneText = Managers.Pool.GetPool("ImmuneText").GetGameObject("UI/ImmuneText", new Vector3((target as MonsterScript).hud.transform.position.x, (target as MonsterScript).hud.transform.position.y - 1, (target as MonsterScript).hud.transform.position.z));
            immuneText.GetComponent<DamageText>().ShowImmuneText((target as MonsterScript).hud);
            TutorialManager.SynergyImmuneStart(0);

            return;
        }

        base.ApplyEffect(skill, target, duration);

        //particle = Managers.Pool.GetPool("Water aura").GetGameObject("Particle/Water aura", (target as MonsterScript).transform.position);
        //particleParent = particle.transform.parent;
        //particle.transform.parent = (target as MonsterScript).transform;
    }

    protected override void ActivateEffect()
    {
        if (m_target != null)
        {
            m_target.MoveSpeedPercentage -= 50f;
            m_target.AttackSpeedPercentage -= 50f;
        }
    }

    protected override void RemoveEffect()
    {
        if (m_target != null)
        {
            m_target.MoveSpeedPercentage += 50f;
            m_target.AttackSpeedPercentage += 50f;
        }
        base.RemoveEffect();

       //particle.transform.parent = particleParent;
       //Managers.Pool.GetPool("Water aura").ReturnObject(particle);
    }
}
