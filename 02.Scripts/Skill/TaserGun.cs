using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaserGun : Skill
{
    [SerializeField]
    [Tooltip("공격력 퍼센트 기준(%)")]
    float m_additionalDamagePerDamagePercent;
    [SerializeField]
    [Tooltip("경직 지속시간")]
    float StunDuration;
    [SerializeField]
    [Tooltip("부채꼴 각도")]
    float m_circularSectorAngle;
    [SerializeField]
    [Tooltip("부채꼴 반지름")]
    float m_circularSectorRadius;
    [SerializeField]
    [Tooltip("스킬 계수 증가분(%)")]
    float m_damagePercent;
    public GameObject range;
    public override void Activate()
    {
        Instantiate(range, transform.position, transform.rotation).transform.localScale = new Vector3(m_circularSectorRadius * 2, 0.1f, m_circularSectorRadius * 2);
        List<MonsterScript> target = Managers.Monsters.GetMonsterInCircularSector(transform, m_circularSectorAngle, m_circularSectorRadius);
        foreach (MonsterScript obj in target)
        {
            m_statusEffectManager.m_abnormalStatus["Stun"].ApplyEffect(this, obj, StunDuration);

            obj.IsDamaged(this, m_skillCoefficient + m_player.GetComponent<Player>().DamagePercentage / m_damagePercent * m_additionalDamagePerDamagePercent, 0);
        }
    }

    public override GameObject GetTarget()
    {
        return Managers.Monsters.GetNearestMonster(transform).gameObject;
    }

    public override void SkillLevelUp()
    {
        base.SkillLevelUp();

        if (m_skillLevel == 1 || m_skillLevel == 10)
        {
            m_skillCoefficient += 5f;
        }
        else
            m_skillCoefficient += 2f;

    }
}
