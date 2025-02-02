using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaserGun : Skill
{
    [SerializeField]
    [Tooltip("���ݷ� �ۼ�Ʈ ����(%)")]
    float m_additionalDamagePerDamagePercent;
    [SerializeField]
    [Tooltip("���� ���ӽð�")]
    float StunDuration;
    [SerializeField]
    [Tooltip("��ä�� ����")]
    float m_circularSectorAngle;
    [SerializeField]
    [Tooltip("��ä�� ������")]
    float m_circularSectorRadius;
    [SerializeField]
    [Tooltip("��ų ��� ������(%)")]
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
