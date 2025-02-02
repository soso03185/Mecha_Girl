using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ThunderStrike : Skill
{
    int m_thunderNumber;
    int m_thunderCount;
    float m_tripod2AdditionalCoefficient;
    float m_tripod3AdditionalDamage;

    // 기본 스킬
    [SerializeField]
    [Tooltip("낙뢰 범위")]
    float m_thunderRange;
    [SerializeField]
    [Tooltip("감전 지속시간")]
    float m_shockDuration;
    [SerializeField]
    [Tooltip("경직 지속시간")]
    float m_stunDuration;

    // 트라이포드 2단계
    [SerializeField]
    [Tooltip("기본 마나 소모량")]
    float m_basicManaCost;
    [SerializeField]
    [Tooltip("트라이포드2_1 마나 소모량")]
    float m_tripod2_1ManaCost;

    [SerializeField]
    [Tooltip("트라이포드2번 추가 스킬계수")]
    float m_tripod2BasicAdditionalCoefficient;
    [SerializeField]
    [Tooltip("트라이포드2_2 추가 스킬 계수")]
    float m_tripod2_2AdditionalCoefficient;

    [SerializeField]
    [Tooltip("기본 상태 이상 증폭")]
    float m_tripod2BasicElementAmp;
    [SerializeField]
    [Tooltip("트라이포드2_3 상태 이상 증폭")]
    float m_tripod2_3ElementAmp;

    // 트라이포드 3단계
    [SerializeField]
    [Tooltip("기본 낙뢰 떨어지는 횟수")]
    int m_basicThunderCount;
    [SerializeField]
    [Tooltip("트라이포드 3_1 낙뢰 떨어지는 횟수")]
    int m_tripod3_1ThunderCount;

    [SerializeField]
    [Tooltip("기본 1회당 떨어지는 낙뢰 개수")]
    int m_basicThunderNumber;
    [SerializeField]
    [Tooltip("트라이포드3_3 낙뢰 떨어지는 개수")]
    int m_tripod3_3ThunderNumber;
    [SerializeField]
    [Tooltip("트라이포드3_2 낙뢰 떨어지는 개수")]
    int m_tripod3_2ThunderNumber;

    [SerializeField]
    [Tooltip("트라이포드3 기본 추가대미지")]
    float m_tripod3BasicAdditionalDamage;
    [SerializeField]
    [Tooltip("트라이포드3_2 추가대미지")]
    float m_tripod3_2AdditionalDamage;
    [SerializeField]


    protected override void Awake()
    {
        base.Awake();
        SetSkillExplanation();
    }
    public override void Activate()
    {
        StartCoroutine(Damage());
    }

    public override GameObject GetTarget()
    {
        return Managers.Monsters.GetNearestMonster(m_player.transform).gameObject;
    }

    IEnumerator Damage()
    {
        System.Random random = new System.Random();

        List<MonsterScript> finalTargetMonsters = new List<MonsterScript>();
        List<MonsterScript> randomMonsterInRange = new List<MonsterScript>();
        List<MonsterScript> statusEffectMonsterInRange = new List<MonsterScript>();
        List<MonsterScript> healthyMonsterInRange = new List<MonsterScript>();
        List<MonsterScript> nearMonsterInRange = new List<MonsterScript>();
        List<MonsterScript> resultMonsters = new List<MonsterScript>();

        for (int i = 0; i < m_thunderCount; i++)
        {
            List<MonsterScript> monstersInRange = Managers.Monsters.GetMonsterInRange(transform.position, 10);

            if (monstersInRange.Count <= m_thunderNumber)
            {
                randomMonsterInRange = monstersInRange;
                statusEffectMonsterInRange = monstersInRange;
                healthyMonsterInRange = monstersInRange;
                nearMonsterInRange = monstersInRange;
            }
            else
            {
                randomMonsterInRange = monstersInRange.OrderBy(monster => random.Next()).Take(m_thunderNumber).ToList();

                healthyMonsterInRange = monstersInRange.OrderByDescending(monster => monster.Health / monster.MaxHealth).Take(m_thunderNumber).ToList();

                nearMonsterInRange = monstersInRange.OrderBy(monster => Vector3.Distance(monster.transform.position, transform.position)).Take(m_thunderNumber).ToList();

                statusEffectMonsterInRange = monstersInRange.OrderBy(monster => monster.BuffDebuff.Count + monster.ContinuousDamage.Count + monster.AbnormalStatus.Count).Take(m_thunderNumber).ToList();
            }



            if (m_tripod.firstSlot == 0)
                finalTargetMonsters = randomMonsterInRange;
            else if (m_tripod.firstSlot == 1)
                finalTargetMonsters = statusEffectMonsterInRange;
            else if (m_tripod.firstSlot == 2)
                finalTargetMonsters = healthyMonsterInRange;
            else if (m_tripod.firstSlot == 3)
                finalTargetMonsters = nearMonsterInRange;

            if (finalTargetMonsters != null)
            {
                for (int j = 0; j < finalTargetMonsters.Count; j++)
                {
                    m_particles[j].transform.position = finalTargetMonsters[j].transform.position;
                    m_particles[j].gameObject.SetActive(true);
                    m_particles[j].PlayEffects();

                    PlaySound();
                }
            }

            foreach (MonsterScript monster in finalTargetMonsters)
            {
                resultMonsters.AddRange(Managers.Monsters.GetMonsterInRange(monster.transform.position, m_thunderRange));
            }

            foreach (MonsterScript monster in resultMonsters)
            {
                monster.IsDamaged(this, m_finalSkillCoefficient, m_tripod3AdditionalDamage);

                m_statusEffectManager.m_continuousDamage["ElectricShock"].ApplyEffect(this, monster, m_shockDuration, 0f);
                m_statusEffectManager.m_abnormalStatus["Stun"].ApplyEffect(this, monster, m_stunDuration);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public override void SetTripod(int tripodStep, int num)
    {
        base.SetTripod(tripodStep, num);
        if (tripodStep == 2)
        {
            if(num == 1)
            {
                m_manaCost = m_tripod2_1ManaCost;
                m_tripod2AdditionalCoefficient = m_tripod2BasicAdditionalCoefficient;
                m_elementAmp = m_tripod2BasicElementAmp;
            }
            else if(num == 2)
            {
                m_manaCost = m_basicManaCost;
                m_tripod2AdditionalCoefficient = m_tripod2_2AdditionalCoefficient;
                m_elementAmp = m_tripod2BasicElementAmp;
            }
            else if(num == 3)
            {
                m_manaCost = m_basicManaCost;
                m_tripod2AdditionalCoefficient = m_tripod2BasicAdditionalCoefficient;
                m_elementAmp = m_tripod2_3ElementAmp;
            }
            else
            {
                m_manaCost = m_basicManaCost;
                m_tripod2AdditionalCoefficient = m_tripod2BasicAdditionalCoefficient;
                m_elementAmp = m_tripod2BasicElementAmp;
            }
        }
        if (tripodStep == 3)
        {
            if (num == 1)
            {
                m_thunderCount = m_tripod3_1ThunderCount;
                m_thunderNumber = m_basicThunderNumber;
                m_tripod3AdditionalDamage = m_tripod3BasicAdditionalDamage;
            }
            else if (num == 2)
            {
                m_thunderCount = m_basicThunderCount;
                m_thunderNumber = m_tripod3_2ThunderNumber;
                m_tripod3AdditionalDamage = m_tripod3_2AdditionalDamage;
            }
            else if (num == 3)
            {
                m_thunderCount = m_basicThunderCount;
                m_thunderNumber = m_tripod3_3ThunderNumber;
                m_tripod3AdditionalDamage = m_tripod3BasicAdditionalDamage;
            }
            else
            {
                m_thunderCount = m_basicThunderCount;
                m_thunderNumber = m_basicThunderNumber;
                m_tripod3AdditionalDamage = m_tripod3BasicAdditionalDamage;
            }
        }
        m_finalSkillCoefficient = m_skillCoefficient + m_tripod2AdditionalCoefficient;
    }

    public override void SkillLevelUp()
    {
        base.SkillLevelUp();
        m_finalSkillCoefficient = m_skillCoefficient + m_tripod2AdditionalCoefficient;
    }

    public override void ResetSkill()
    {
        base.ResetSkill();
        m_finalSkillCoefficient = m_skillCoefficient + m_tripod2AdditionalCoefficient;
    }

    public override void SetSkillExplanation()
    {
        m_skillExplanation = "몬스터에게 " + m_thunderNumber + "개의 낙뢰를 일으켜 범위 내의 몬스터에게<color=red>" + m_finalSkillCoefficient + "%</color><color=blue>(+" + m_coefficientIncreasePerLevel + "%)</color>의 피해를 줍니다. " + m_shockDuration + "초간 <color=yellow>감전</color>, " + m_stunDuration + "초간 <color=white>경직</color>이 부여됩니다.";
    }
}
