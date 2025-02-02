using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHole : Skill
{
    [SerializeField]
    BlackHoleObject m_blackHoleObjectPrepab;

    float m_tripod2AdditionalManaCost;
    float m_tripod3AdditionalManaCost;

    // 기본 스킬
    [SerializeField]
    [Tooltip("기본 추가피해 퍼센트")]
    float m_basicAdditionalDamage;
    [SerializeField]
    [Tooltip("레벨 당 추가피해 퍼센트 증가량")]
    float m_AdditionalDamageIncreasePerLevel;
    [SerializeField]
    [Tooltip("기본 마나 소모량")]
    float m_basicManaCost;

    // 트라이포드1
    [SerializeField]
    [Tooltip("트라이포드1_1 버프 지속시간")]
    float m_tripod1_1BuffDuration;
    [SerializeField]
    [Tooltip("트라이포드1_2 버프 지속시간")]
    float m_tripod1_2BuffDuration;
    [SerializeField]
    [Tooltip("트라이포드1_3 버프 지속시간")]
    float m_tripod1_3BuffDuration;

    [SerializeField]
    [Tooltip("트라이포드1_1 버프 퍼센트")]
    float m_tripod1_1BuffPercentage;
    [SerializeField]
    [Tooltip("트라이포드1_2 버프 퍼센트")]
    float m_tripod1_2BuffPercentage;
    [SerializeField]
    [Tooltip("트라이포드1_3 버프 퍼센트")]
    float m_tripod1_3BuffPercentage;

    // 트라이포드2
    [SerializeField]
    [Tooltip("기본 상태이상 지속시간")]
    float m_basicStatusEffectDuration;
    [SerializeField]
    [Tooltip("트라이포드2_1 상태이상 지속시간")]
    float m_tripod2_1StatusEffectDuration;

    [SerializeField]
    [Tooltip("트라이포드2 기본 추가 마나 소모량")]
    float m_tripod2BasicAdditionalManaCost;
    [SerializeField]
    [Tooltip("트라이포드2_2 마나소모량")]
    float m_tripod2_2AdditionalManaCost;

    [SerializeField]
    [Tooltip("기본 상태 반응 증폭")]
    float m_basicElementAmp;
    [SerializeField]
    [Tooltip("트라이포드2_3 상태 반응 증폭")]
    float m_tripod2_3ElementAmp;

    // 트라이포드3
    [SerializeField]
    [Tooltip("기본 블랙홀 지속시간")]
    float m_basicDuration;
    [SerializeField]
    [Tooltip("트라이포드3_1블랙홀 지속시간")]
    float m_tripod3_1Duration;

    [SerializeField]
    [Tooltip("트라이포드3_2 일반몬스터 디버프양")]
    float m_tripod3_2NormalDebuffPercent;
    [SerializeField]
    [Tooltip("트라이포드3_2 보스몬스터 디버프양")]
    float m_tripod3_2BossDebuffPercent;

    [SerializeField]
    [Tooltip("트라이포드3 기본 추가 마나 소모량")]
    float m_tripod3BasicAdditionalManaCost;
    [SerializeField]
    [Tooltip("트라이포드3_3 추가 마나 소모량")]
    float m_tripod3_3AdditionalManaCost;

    [SerializeField]
    [Tooltip("기본 초당 지름 증가량")]
    float m_basicRadiusIncrease;
    [SerializeField]
    [Tooltip("트라이포드3_3 지름 증가량")]
    float m_tripod3_3RadiusIncrease;

    [SerializeField]
    [Tooltip("기본 초기 지름")]
    float m_basicInitRadius;
    [SerializeField]
    [Tooltip("트라이포드3_3 초기 지름")]
    float m_tripod3_3InitRadius;


    protected override void Awake()
    {
        base.Awake();
        m_blackHoleObjectPrepab.m_additionalDamage = m_basicAdditionalDamage;
        m_blackHoleObjectPrepab.bossMonsterDebuff = m_tripod3_2BossDebuffPercent;
        m_blackHoleObjectPrepab.normalMonsterDebuff = m_tripod3_2NormalDebuffPercent;
        SetSkillExplanation();
    }

    public override void Init()
    {
        foreach (var sprite in SpriteManager.instance.skillIdSprites)
        {
            if (sprite.Key == m_skillName)
            {
                m_image = sprite.Value;
            }
        }

        m_player = GameManager.Instance.player;
        m_playerBody = m_player.transform.GetChild(4).GetChild(2);
        m_statusEffectManager = GameObject.Find("StatusEffectManager").GetComponent<StatusEffectManager>();
    }

    public override void Activate()
    {
        MonsterScript nearestMonster = Managers.Monsters.GetNearestMonster(transform);

        PlaySound("blackhole");

        Instantiate(m_blackHoleObjectPrepab, nearestMonster.transform.position + new Vector3(0, 1, 0), Quaternion.identity).m_skill = this;

        if (m_tripod.firstSlot == 1)
        {
            m_statusEffectManager.m_buffdebuff["CriticalMultiplierIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), m_tripod1_1BuffDuration, m_tripod1_1BuffPercentage);
        }
        if (m_tripod.firstSlot == 2)
        {
            m_statusEffectManager.m_buffdebuff["DamageIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), m_tripod1_2BuffDuration, m_tripod1_2BuffPercentage);
        }
        if (m_tripod.firstSlot == 3)
        {
            m_statusEffectManager.m_buffdebuff["CriticalChanceIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), m_tripod1_3BuffDuration, m_tripod1_3BuffPercentage);
        }
    }

    public override GameObject GetTarget()
    {
        return Managers.Monsters.GetNearestMonster(transform).gameObject;
    }

    public override void SetTripod(int tripodStep, int num)
    {
        base.SetTripod(tripodStep, num);
        m_blackHoleObjectPrepab.SetTripod(tripodStep, num);
        if (tripodStep == 2)
        {
            if (num == 1)
            {
                m_blackHoleObjectPrepab.m_statusEffectDuration = m_tripod2_1StatusEffectDuration;
                m_tripod2AdditionalManaCost = m_tripod2BasicAdditionalManaCost;
                m_elementAmp = m_basicElementAmp;
            }
            else if (num == 2)
            {
                m_blackHoleObjectPrepab.m_statusEffectDuration = m_basicStatusEffectDuration;
                m_tripod2AdditionalManaCost = m_tripod2_2AdditionalManaCost;
                m_elementAmp = m_basicElementAmp;
            }
            else if (num == 3)
            {
                m_blackHoleObjectPrepab.m_statusEffectDuration = m_basicStatusEffectDuration;
                m_tripod2AdditionalManaCost = m_tripod2BasicAdditionalManaCost;
                m_elementAmp = m_tripod2_3ElementAmp;
            }
            else
            {
                m_blackHoleObjectPrepab.m_statusEffectDuration = m_basicStatusEffectDuration;
                m_tripod2AdditionalManaCost = m_tripod2BasicAdditionalManaCost;
                m_elementAmp = m_basicElementAmp;
            }
        }
        if (tripodStep == 3)
        {
            if (num == 1)
            {
                m_blackHoleObjectPrepab.m_duration = m_tripod3_1Duration;
                m_tripod3AdditionalManaCost = m_tripod3BasicAdditionalManaCost;
                m_blackHoleObjectPrepab.m_radiusIncrease = m_basicRadiusIncrease;
                m_blackHoleObjectPrepab.m_initRadius = m_basicInitRadius;
            }
            else if (num == 2)
            {
                m_blackHoleObjectPrepab.m_duration = m_tripod3_1Duration;
                m_tripod3AdditionalManaCost = m_tripod3BasicAdditionalManaCost;
                m_blackHoleObjectPrepab.m_radiusIncrease = m_basicRadiusIncrease;
                m_blackHoleObjectPrepab.m_initRadius = m_basicInitRadius;
            }
            else if (num == 3)
            {
                m_blackHoleObjectPrepab.m_duration = m_basicDuration;
                m_tripod3AdditionalManaCost = m_tripod3_3AdditionalManaCost;
                m_blackHoleObjectPrepab.m_radiusIncrease = m_tripod3_3RadiusIncrease;
                m_blackHoleObjectPrepab.m_initRadius = m_tripod3_3InitRadius;
            }
            else
            {
                m_blackHoleObjectPrepab.m_duration = m_basicDuration;
                m_tripod3AdditionalManaCost = m_tripod3BasicAdditionalManaCost;
                m_blackHoleObjectPrepab.m_radiusIncrease = m_basicRadiusIncrease;
                m_blackHoleObjectPrepab.m_initRadius = m_basicInitRadius;

            }
        }
        m_manaCost = m_basicManaCost + m_tripod2AdditionalManaCost + m_tripod3AdditionalManaCost;
    }

    public override void SkillLevelUp()
    {
        base.SkillLevelUp();
        m_blackHoleObjectPrepab.m_additionalDamage = m_basicAdditionalDamage + (m_skillLevel - 1) * m_AdditionalDamageIncreasePerLevel;

    }

    public override void ResetSkill()
    {
        base.ResetSkill();
        m_blackHoleObjectPrepab.m_additionalDamage = m_basicAdditionalDamage;
    }

    public override void SetSkillExplanation()
    {
        m_skillExplanation = "가까운 적의 위치에 점점 커지는 블랙홀을 소환합니다.(지속시간 " + m_blackHoleObjectPrepab.m_duration + "초) 블랙홀은 <color=purple>이명</color>효과를 부여하고 받는 피해를 <color=red>" + m_blackHoleObjectPrepab.m_additionalDamage + "%</color><color=blue>(+" + m_AdditionalDamageIncreasePerLevel + "%)</color> 증가시킵니다.";
    }
}
