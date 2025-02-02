using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Alchemy : BuffSkill
{
    float m_tripod1PlayerDamage;
    float m_tripod1NormalMonsterDamage;
    float m_tripod1PlayerDefense;
    float m_tripod1BossMonsterDefense;

    float m_tripod2NormalMonsterDefense;
    float m_tripod2BossMonsterDefense;
    float m_tripod2PlayerDamage;

    float m_tripod3MonsterDefense;

    float m_manaRecovery;
    // 기본 스킬
    [SerializeField]
    [Tooltip("마나지속회복 시간")]
    float m_manaDuration;

    [SerializeField]
    [Tooltip("레벨 1 마나 회복양")]
    float m_basicManaRecovery;
    [SerializeField]
    [Tooltip("레벨 당 마나 회복 증가량")]
    float m_manaRecoveryPerLevel;

    [SerializeField]
    [Tooltip("버프지속 시간")]
    float m_buffDuration;
    [SerializeField]
    [Tooltip("아군 버프(%)")]
    float m_buffAmount;
    [SerializeField]
    [Tooltip("적군 버프(%)")]
    float m_debuffAmount;

    [SerializeField]
    [Tooltip("기본 트라이포드1 플레이어 대미지")]
    float m_basicTripod1PlayerDamage;
    [SerializeField]
    [Tooltip("트라이포드 1_1 플레이어 대미지")]
    float m_tripod1_1PlayerDamage;

    [SerializeField]
    [Tooltip("기본 트라이포드1 일반 몬스터 대미지")]
    float m_basicTripod1NormalMonsterDamage;
    [SerializeField]
    [Tooltip("트라이포드1_1 일반 몬스터 대미지")]
    float m_tripod1_1NormalMonsterDamage;

    [SerializeField]
    [Tooltip("기본 트라이포드1 플레이어 방어력")]
    float m_basicTripod1PlayerDefense;
    [SerializeField]
    [Tooltip("트라이포드1_2 플레이어 방어력")]
    float m_tripod1_2PlayerDefense;

    [SerializeField]
    [Tooltip("기본 트라이포드1 보스몬스터 방어력")]
    float m_basicTripod1BossMonsterDefense;
    [SerializeField]
    [Tooltip("트라이포드1_2 보스몬스터 방어력")]
    float m_tripod1_2BossMonsterDefense;

    [SerializeField]
    [Tooltip("기본 트라이포드2 일반몬스터 방어력")]
    float m_basicTripod2NormalMonsterDefnese;
    [SerializeField]
    [Tooltip("트라이포드2_1 일반 몬스터 방어력")]
    float m_tripod2_1NormalMonsterDefense;

    [SerializeField]
    [Tooltip("기본 트라이포드2 보스몬스터 방어력")]
    float m_basicTripod2BossMonsterDefense;
    [SerializeField]
    [Tooltip("트라이포드2_2 보스 몬스터 방어력")]
    float m_tripod2_2BossMonsterDefense;

    [SerializeField]
    [Tooltip("기본 트라이포드2 플레이어 대미지")]
    float m_basicTripod2PlayerDamage;
    [SerializeField]
    [Tooltip("트라이포드 2_3 플레이어 대미지")]
    float m_tripod2_3PlayerDamage;

    [SerializeField]
    [Tooltip("트라이포드3_1 치명타 확률")]
    float m_tripod3_1CriticalChance;
    [SerializeField]
    [Tooltip("트라이포드3_1 치명타 배율")]
    float m_tripod3_1CriticalMultiplier;

    [SerializeField]
    [Tooltip("트라이포드3 몬스터 방어력")]
    float m_basicTripod3MonsterDefense;
    [SerializeField]
    [Tooltip("트라이포드3_3 몬스터 방어력")]
    float m_tripod3_3MonsterDefense;

    protected override void Awake()
    {
        base.Awake();
        m_manaRecovery = m_basicManaRecovery;
        SetSkillExplanation();
    }

    public override void Activate()
    {
        if(m_tripod.firstSlot == 3)
        {
            m_player.GetComponent<Player>().Mana += 20;
        }

        PlaySound("ManaBuff");
        if(m_tripod.thirdSlot == 1)
        {
            m_statusEffectManager.m_buffdebuff["CriticalChanceIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), m_buffDuration, m_tripod3_1CriticalChance);
            m_statusEffectManager.m_buffdebuff["CriticalMultiplierIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), m_buffDuration, m_tripod3_1CriticalMultiplier);
        }
        else if(m_tripod.thirdSlot == 2)
        {
            m_player.GetComponent<Player>().Mana += 20;
        }
        m_statusEffectManager.m_buffdebuff["ManaRecovery"].ApplyEffect(this, m_player.GetComponent<Player>(), m_manaDuration, m_manaRecovery);

        m_statusEffectManager.m_buffdebuff["DamageIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), m_buffDuration, m_buffAmount + m_tripod1PlayerDamage + m_tripod2PlayerDamage);
        m_statusEffectManager.m_buffdebuff["DefenseIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), m_buffDuration, m_buffAmount + m_tripod1PlayerDefense);
        foreach (var monster in Managers.Monsters.GetAllMonsters())
        {
            if (monster.CompareTag("Monster"))
            {
                m_statusEffectManager.m_buffdebuff["DefenseIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), m_buffDuration, -(m_debuffAmount + m_tripod2NormalMonsterDefense + m_tripod3MonsterDefense));
                m_statusEffectManager.m_buffdebuff["DamageIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), m_buffDuration, -(m_debuffAmount + m_tripod1NormalMonsterDamage));
            }
            else
            {
                m_statusEffectManager.m_buffdebuff["DefenseIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), m_buffDuration, -(m_debuffAmount + m_tripod1BossMonsterDefense + m_tripod2BossMonsterDefense + m_tripod3MonsterDefense));
                m_statusEffectManager.m_buffdebuff["DamageIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), m_buffDuration, -m_debuffAmount);
            }
        }
    }

    public override GameObject GetTarget()
    {
        return base.GetTarget();
    }

    public override void SetTripod(int tripodStep, int num)
    {
        base.SetTripod(tripodStep, num);
        if(tripodStep == 1)
        {
            if(num == 1)
            {
                m_tripod1PlayerDamage = m_tripod1_1PlayerDamage;
                m_tripod1NormalMonsterDamage = m_tripod1_1NormalMonsterDamage;
                m_tripod1PlayerDefense = m_basicTripod1PlayerDefense;
                m_tripod1BossMonsterDefense = m_basicTripod1BossMonsterDefense;
            }
            else if(num == 2)
            {
                m_tripod1PlayerDamage = m_basicTripod1PlayerDamage;
                m_tripod1NormalMonsterDamage = m_basicTripod1NormalMonsterDamage;
                m_tripod1PlayerDefense = m_tripod1_2PlayerDefense;
                m_tripod1BossMonsterDefense = m_tripod1_2BossMonsterDefense;
            }
            else
            {
                m_tripod1PlayerDamage = m_basicTripod1PlayerDamage;
                m_tripod1NormalMonsterDamage = m_basicTripod1NormalMonsterDamage;
                m_tripod1PlayerDefense = m_basicTripod1PlayerDefense;
                m_tripod1BossMonsterDefense = m_basicTripod1BossMonsterDefense;
            }
        }
        else if(tripodStep == 2)
        {
            if (num == 1)
            {
                m_tripod2NormalMonsterDefense = m_tripod2_1NormalMonsterDefense;
                m_tripod2BossMonsterDefense = m_basicTripod2BossMonsterDefense;
                m_tripod2PlayerDamage = m_basicTripod2PlayerDamage;
            }
            else if (num == 2)
            {
                m_tripod2NormalMonsterDefense = m_basicTripod2NormalMonsterDefnese;
                m_tripod2BossMonsterDefense = m_tripod2_2BossMonsterDefense;
                m_tripod2PlayerDamage = m_basicTripod2PlayerDamage;
            }
            else if (num == 3)
            {
                m_tripod2NormalMonsterDefense = m_basicTripod2NormalMonsterDefnese;
                m_tripod2BossMonsterDefense = m_basicTripod2BossMonsterDefense;
                m_tripod2PlayerDamage = m_tripod2_3PlayerDamage;
            }
            else
            {
                m_tripod2NormalMonsterDefense = m_basicTripod2NormalMonsterDefnese;
                m_tripod2BossMonsterDefense = m_basicTripod2BossMonsterDefense;
                m_tripod2PlayerDamage = m_basicTripod2PlayerDamage;
            }
        }
        else if(tripodStep == 3)
        {
            if(num == 3)
            {
                m_tripod3MonsterDefense = m_tripod3_3MonsterDefense;
            }
            else
            {
                m_tripod3MonsterDefense = m_basicTripod3MonsterDefense;
            }
        }
    }

    public override void SkillLevelUp()
    {
        base.SkillLevelUp();

        m_manaRecovery = m_basicManaRecovery + (m_skillLevel - 1) * m_manaRecoveryPerLevel;

    }

    public override void ResetSkill()
    {
        base.ResetSkill();

        m_manaRecovery = m_basicManaRecovery;
    }

    public override void SetSkillExplanation()
    {
        m_skillExplanation = "5초간 매초 <color=blue>" + m_manaRecovery + "(+" + m_manaRecoveryPerLevel + ")</color>의 마나를 회복합니다. 10초간 플레이어의 공격력과 방어력이 <color=green>30%</color>증가하고, 적의 공격력과 방어력이 <color=red>30%</color>감소합니다.";
    }
}
