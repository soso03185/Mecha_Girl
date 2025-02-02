using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordSmash : Skill
{
    float m_range;
    float m_normalDamage;
    float m_bossDamage;
    float m_tripod1AdditionalDamage;

    // 기본 스킬
    [SerializeField]
    [Tooltip("전기 약화 지속시간(초)")]
    float m_vulnerabilityDuration;
    [SerializeField]
    [Tooltip("전기 약화 퍼센트")]
    float m_percentage;

    [SerializeField]
    [Tooltip("침수 지속 시간")]
    float m_floodingDuration;

    // 트라이포드 1단계
    [SerializeField]
    [Tooltip("기본추가피해")]
    float m_basicAdditionalDamage;
    [SerializeField]
    [Tooltip("트라이포드1_1추가피해")]
    float m_tripod1_1AdditionalDamage;

    [SerializeField]
    [Tooltip("트라이포드1_2 공격력 증가분")]
    float m_damageIncreasePercent;
    [SerializeField]
    [Tooltip("트라이포드1_2 공격력 버프 지속 시간")]
    float m_damageIncreaseDuration;

    [SerializeField]
    [Tooltip("트라이포드1_3 치명타 확률 증가분")]
    float m_criticalChanceIncreasePercent;
    [SerializeField]
    [Tooltip("트라이포드1_3 치명타 확률 버프 지속 시간")]
    float m_criticalChanceIncreaseDuration;

    // 트라이포드 2단계
    [SerializeField]
    [Tooltip("기본 침수대상 추가피해")]
    float m_basicElementAmp;
    [SerializeField]
    [Tooltip("트라이포드2-1 침수대상 추가피해")]
    float m_tripod2_1ElementAmp;

    [SerializeField]
    [Tooltip("트라이포드2_2 공격력 감소 시간")]
    float m_tripod2_2DamageDecreaseDuration;
    [SerializeField]
    [Tooltip("트라이포드2_2 공격력 감소량")]
    float m_tripod2_2DamageDecreasePercent;

    [SerializeField]
    [Tooltip("기본 마나 코스트")]
    float m_basicManaCost;
    [SerializeField]
    [Tooltip("트라이포드 2_3 마나 소모량")]
    float m_tripod2_3ManaCost;

    // 트라이포드 3단계
    [SerializeField]
    [Tooltip("기본 스킬 범위")]
    float m_basicRange;
    [SerializeField]
    [Tooltip("트라이포드 3-1 스킬 범위")]
    float m_tripod3_1Range;

    [SerializeField]
    [Tooltip("기본 일반 몬스터 대상 대미지(%)")]
    float m_basicNormalDamage;
    [SerializeField]
    [Tooltip("트라이포드3-2 일반 몬스터 대상 대미지(%)")]
    float m_tripod3_2NormalDamage;
    [SerializeField]
    [Tooltip("트라이포드3-3 일반 몬스터 대상 대미지(%)")]
    float m_tripod3_3NormalDamage;

    [SerializeField]
    [Tooltip("기본 보스몬스터 대상 대미지(%)")]
    float m_basicBossDamage;
    [SerializeField]
    [Tooltip("트라이포드3-2 보스몬스터 대상 대미지(%)")]
    float m_tripod3_2BossDamage;
    [SerializeField]
    [Tooltip("트라이포드3-3 보스몬스터 대상 대미지(%)")]
    float m_tripod3_3BossDamage;

    public GameObject range;

    protected override void Awake()
    {
        base.Awake();
        SetSkillExplanation();
    }
    public override void Init()
    {
        m_player = GameManager.Instance.player;
        if (m_skillParticlePrefab != null)
        {
            for (int i = 0; i < particleCount; i++)
            {
                GameObject obj = Instantiate(m_skillParticlePrefab, this.transform);
                obj.SetActive(false);
                m_particles.Add(obj.GetComponent<EffectManager>());
                m_particle = m_particles[i];
            }
        }        

        if (m_skillParticlePrefab2 != null)
        {
            GameObject obj = Instantiate(m_skillParticlePrefab2, m_player.transform);
            obj.SetActive(false);
            m_particle2 = obj.GetComponent<EffectManager>();
        }
        m_statusEffectManager = GameObject.Find("StatusEffectManager").GetComponent<StatusEffectManager>();
    }

    public override void Activate()
    {
        List<MonsterScript> monsterInRange = Managers.Monsters.GetMonsterInRange(transform.position, m_range);
        //Instantiate(range, transform.position, transform.rotation).transform.localScale = new Vector3(m_range * 2, 0.1f, m_range * 2);
        m_particle2.transform.localScale = new Vector3(m_range, m_range, m_range);
        SoundManager.Instance.PlaySFX("Water Impact 02");        foreach (MonsterScript monster in monsterInRange)
        {
            if (monster != null)
            {
                if (monster.gameObject.CompareTag("Monster"))
                {
                    monster.IsDamaged(this, m_finalSkillCoefficient * m_normalDamage / 100, m_tripod1AdditionalDamage);
                }
                else if (monster.gameObject.CompareTag("BossMonster"))
                {
                    monster.IsDamaged(this, m_finalSkillCoefficient * m_bossDamage / 100, m_tripod1AdditionalDamage);
                }
                m_statusEffectManager.m_abnormalStatus["Flooding"].ApplyEffect(this, monster, m_floodingDuration);
                if(m_tripod.secondSlot == 2)
                {
                    m_statusEffectManager.m_buffdebuff["DamageIncrease"].ApplyEffect(this, monster, m_tripod2_2DamageDecreaseDuration, m_tripod2_2DamageDecreasePercent);
                }               
            }
        }
        if(m_tripod.firstSlot == 2)
        {
            m_statusEffectManager.m_buffdebuff["DamageIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), m_damageIncreaseDuration, m_damageIncreasePercent);
        }
        if(m_tripod.firstSlot == 3)
        {
            m_statusEffectManager.m_buffdebuff["CriticalChanceIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), m_criticalChanceIncreaseDuration, m_criticalChanceIncreasePercent);
        }
    }
    public override void Update()
    {
        transform.position = m_player.transform.position;
    }

    public override GameObject GetTarget()
    {
        return Managers.Monsters.GetNearestMonster(m_player.transform).gameObject;
    }

    public override void SetTripod(int tripodStep, int num)
    {
        base.SetTripod(tripodStep, num);
        if (tripodStep == 1)
        {
            if (num == 1)
            {
                m_tripod1AdditionalDamage = m_tripod1_1AdditionalDamage;
            }
            else
            {
                m_tripod1AdditionalDamage = 0;
            }
        }

        if (tripodStep == 2)
        {
            if (num == 1)
            {
                m_elementAmp = m_tripod2_1ElementAmp;
                m_manaCost = m_basicManaCost;
            }
            else if (num == 3)
            {
                m_elementAmp = m_basicElementAmp;
                m_manaCost = m_tripod2_3ManaCost;
            }
            else
            {
                m_elementAmp = m_basicElementAmp;
                m_manaCost = m_basicManaCost;
            }
        }

        if (tripodStep == 3)
        {
            if (num == 1)
            {
                m_range = m_tripod3_1Range;
                m_normalDamage = m_basicNormalDamage;
                m_bossDamage = m_basicBossDamage;
            }
            else if(num == 2)
            {
                m_range = m_basicRange;
                m_normalDamage = m_tripod3_2NormalDamage;
                m_bossDamage = m_tripod3_2BossDamage;
            }
            else if(num == 3)
            {
                m_range = m_basicRange;
                m_normalDamage = m_tripod3_3NormalDamage;
                m_bossDamage = m_tripod3_3BossDamage;
            }
            else
            {
                m_range = m_basicRange;
                m_normalDamage = m_basicNormalDamage;
                m_bossDamage = m_basicBossDamage;
            }
        }
        m_finalSkillCoefficient = m_skillCoefficient + m_tripod1AdditionalDamage;
    }

    public override void SkillLevelUp()
    {
        base.SkillLevelUp();
        m_finalSkillCoefficient = m_skillCoefficient + m_tripod1AdditionalDamage;
    }

    public override void ResetSkill()
    {
        base.ResetSkill();
        m_finalSkillCoefficient = m_skillCoefficient + m_tripod1AdditionalDamage;
    }

    public override void SetSkillExplanation()
    {
        m_skillExplanation = "적에게 <color=red>" + m_finalSkillCoefficient + "%</color><color=blue>(+" + m_coefficientIncreasePerLevel + "%)</color>의 피해를 주고, " + m_floodingDuration + "초간 <color=blue>침수</color>효과를 부여합니다. 일반몬스터에게 " + m_normalDamage / 100 + "배, 보스몬스터에게 " + m_bossDamage / 100 + "배의 피해를 줍니다.";

    }
}

