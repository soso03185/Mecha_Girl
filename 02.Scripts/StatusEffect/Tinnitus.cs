using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tinnitus : ContinuousDamage
{
    float m_elapsedTime;
    public Tinnitus(Skill skill, ICreature target, float duration, float additionalDamage)
        : base(skill, "Tinnitus", target, duration, additionalDamage)
    {
    }

    protected override ContinuousDamage Clone(Skill skill, ICreature target, float duration, float additionalDamage)
    {
        return new Tinnitus(skill, target, duration, additionalDamage);
    }

    public override void ApplyEffect(Skill skill, ICreature target, float duration, float additionalDamage)
    {
        if ((target as MonsterScript).Attribute == Attribute.Magnetic)
        {
            var immuneText = Managers.Pool.GetPool("ImmuneText").GetGameObject("UI/ImmuneText", new Vector3((target as MonsterScript).hud.transform.position.x, (target as MonsterScript).hud.transform.position.y - 1, (target as MonsterScript).hud.transform.position.z));
            immuneText.GetComponent<DamageText>().ShowImmuneText((target as MonsterScript).hud);
            TutorialManager.SynergyImmuneStart(0);
            return;
        }
        base.ApplyEffect(skill, target, duration, additionalDamage);
    }

    public override void Update()
    {
        base.Update();
        m_elapsedTime += Time.deltaTime;
        if (m_elapsedTime >= 1)
        {
            if (m_target != null)
            {
                (m_target as IMonster).IsContinuosDamaged(m_skill, 100f);
            }
            m_elapsedTime = 0;
        }
    }
    protected override void RemoveEffect()
    {
        base.RemoveEffect();
    }
}
