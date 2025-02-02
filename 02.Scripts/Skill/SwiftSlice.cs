using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class SwiftSlice : Skill
{
    int m_attackFrequency;

    // 기본 스킬
    [SerializeField]
    [Tooltip("횟수마다 증가하는 피해량(%)")]
    float m_damageIncrease;
    [SerializeField]
    [Tooltip("공격 사이의 시간 간격(초)")]
    float m_interval;
    [SerializeField]
    [Tooltip("대미지 들어가는 범위")]
    float m_range;

    // 트라이포드3
    [SerializeField]
    [Tooltip("기본 마나 소모량")]
    float m_basicManaCost;
    [SerializeField]
    [Tooltip("트라이포드3_1 마나 소모량")]
    float m_tripod3_1ManaCost;

    [SerializeField]
    [Tooltip("기본 연섬 공격 횟수")]
    int m_basicAttackFrequency;
    [SerializeField]
    [Tooltip("트라이포드3_2연섬 공격 횟수")]
    int m_tripod3_2AttackFrequency;

    [SerializeField]
    [Tooltip("기본 상태 반응 증폭(%)")]
    float m_basicElementAmp;
    [SerializeField]
    [Tooltip("트라이포드3_3 상태 반응 증폭(%)")]
    float m_tripod3_3ElementAmp;



    protected override void Awake()
    {
        base.Awake();
        SetSkillExplanation();
    }

    public override void Activate()
    {
        StartCoroutine(Damage());
        m_player.transform.GetChild(0).gameObject.SetActive(true);
        PlaySecondEffect(2f);
        m_player.GetComponent<Player>().AnimEventZoomOut(m_interval * 4);
        Invoke("DisableHologram", 2f);
    }

    public void DisableHologram()
    {
        m_player.transform.GetChild(0).gameObject.SetActive(false);
    }

    IEnumerator Damage()
    {
        System.Random random = new System.Random();
        for (int i = 0; i < m_attackFrequency; i++)
        {
            MonsterScript target = new MonsterScript();
            List<MonsterScript> monsterInRange = Managers.Monsters.GetMonsterInRange(transform.position, 30);

            if (monsterInRange.Count == 0)
            {
                yield return new WaitForSeconds(m_interval);
                continue;
            }

            if (m_tripod.firstSlot == 0)
            {
                target = monsterInRange.OrderBy(x => random.Next()).ToList()[0];
            }
            else if (m_tripod.firstSlot == 1 || m_tripod.firstSlot == 2)
            {
                if (m_tripod.firstSlot == 1)
                {
                    List<MonsterScript> abnomralMonsters = monsterInRange.Where(x => x.BuffDebuff.Count > 0 || x.AbnormalStatus.Count > 0 || x.ContinuousDamage.Count > 0).ToList().OrderBy(x => random.Next()).ToList();
                    if (abnomralMonsters.Count > 0)
                    {
                        target = abnomralMonsters[0];
                    }
                    else
                    {
                        target = monsterInRange.OrderBy(x => random.Next()).ToList()[0];
                    }
                }
                else
                {
                    List<MonsterScript> bossMonsters = monsterInRange.Where(x => x.CompareTag("BossMonster")).ToList();
                    if (bossMonsters.Count > 0)
                    {
                        target = bossMonsters[0];
                    }
                    else
                    {
                        target = monsterInRange.OrderBy(x => random.Next()).ToList()[0];
                    }
                }

            }
            if (m_tripod.firstSlot == 3)
            {
                target = monsterInRange.OrderByDescending(x => x.Health / x.MaxHealth).ToList()[0];
            }

            Vector3 currentPosition = transform.position;
            Vector3 direction;

            direction = (target.transform.position - currentPosition).normalized;
            m_player.transform.position = target.transform.position + direction * 2;

            List<MonsterScript> monsters = new List<MonsterScript>();
            monsters = Managers.Monsters.GetMonsterInPerpendicularDistance(currentPosition, m_player.transform.position, 1);

            //m_player.GetComponent<Player>().AnimEventShake(0f, 0.2f, 0.2f, 0f);

            foreach (MonsterScript monster in monsters)
            {
                PlaySound("Sword_attack");
                monster.IsDamaged(this, m_finalSkillCoefficient + i * m_damageIncrease, 0);
            }
            yield return new WaitForSeconds(m_interval);
        }

        if (m_tripod.secondSlot == 1)
        {
            m_statusEffectManager.m_buffdebuff["CriticalMultiplierIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), 8, 50);
        }
        else if (m_tripod.secondSlot == 2)
        {
            m_statusEffectManager.m_buffdebuff["DamageIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), 10, 100);
        }
        else if (m_tripod.secondSlot == 3)
        {
            m_statusEffectManager.m_buffdebuff["CriticalChanceIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), 5, 8);
        }
    }

    public override void SetTripod(int tripodStep, int num)
    {
        base.SetTripod(tripodStep, num);
        if (tripodStep == 3)
        {
            if (num == 1)
            {
                m_manaCost = m_tripod3_1ManaCost;
                m_attackFrequency = m_basicAttackFrequency;
                m_elementAmp = m_basicElementAmp;
            }
            else if (num == 2)
            {
                m_manaCost = m_basicManaCost;
                m_attackFrequency = m_tripod3_2AttackFrequency;
                m_elementAmp = m_basicElementAmp;
            }
            else if (num == 3)
            {
                m_manaCost = m_basicManaCost;
                m_attackFrequency = m_basicAttackFrequency;
                m_elementAmp = m_tripod3_3ElementAmp;
            }
            else
            {
                m_manaCost = m_basicManaCost;
                m_attackFrequency = m_basicAttackFrequency;
                m_elementAmp = m_basicElementAmp;
            }
        }
        m_finalSkillCoefficient = m_skillCoefficient;
    }

    public override void SkillLevelUp()
    {
        base.SkillLevelUp();
        m_finalSkillCoefficient = m_skillCoefficient;
    }

    public override void ResetSkill()
    {
        base.ResetSkill();
        m_finalSkillCoefficient = m_skillCoefficient;
    }

    public override void SetSkillExplanation()
    {
        m_skillExplanation = "몬스터에게 최대 " + m_attackFrequency + "회 돌진하여 충돌한 모든 적에게 (<color=red>" + m_finalSkillCoefficient + "%</color><color=blue>(+" + m_coefficientIncreasePerLevel + "%)</color> + <color=red>" + m_damageIncrease + "%</color> x 돌진횟수) 의 피해를 줍니다.";
    }

    public override GameObject GetTarget()
    {
        return Managers.Monsters.GetNearestMonster(transform).gameObject;
    }
}
