using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Skill
{
    float m_elapsedTime;

    float m_duration;

    float m_barrierAmount;
    float m_finalBarrierAmount;
    float m_barrierPercent;

    // 기본 스킬
    [SerializeField]
    [Tooltip("레벨 1 보호막양")]
    float m_level1BarrierAmount;

    [SerializeField]
    [Tooltip("레벨 당 보호막 증가량")]
    float m_barrierIncreasePerLevel;

    // 트라이포드1
    [SerializeField]
    [Tooltip("기본 마나 소모량")]
    float m_basicManaCost;
    [SerializeField]
    [Tooltip("트라이포드1_3 마나 소모량")]
    float m_tripod1_3ManaCost;

    [SerializeField]
    [Tooltip("기본 추가 보호막 퍼센트")]
    float m_basicBarrierPercent;
    [SerializeField]
    [Tooltip("트라이포드2_3 추가 보호막 퍼센트")]
    float m_tripod2_3BarrierPercent;


    // 트라이포드3
    [SerializeField]
    [Tooltip("트라이포드3_3 마나 회복량(초당)")]
    float m_tripod3_3ManaRecovery;

    private void Awake()
    {
        m_barrierAmount = m_level1BarrierAmount;
        base.Awake();
        SetSkillExplanation();
    }

    public override void Activate()
    {
        PlaySound("Shield");

        DisableEffect();
        Invoke("PlayShieldEffect", 0.2f);

        m_statusEffectManager.m_barrier["MechanicBarrier"].ApplyEffect(this, m_player.GetComponent<Player>(), 100, m_finalBarrierAmount);
        if (m_tripod.secondSlot == 1)
        {
            m_statusEffectManager.m_buffdebuff["SpeedIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), 100, 30);
            m_statusEffectManager.m_buffdebuff["AttackSpeedIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), 100, 30);

        }
        if (m_tripod.firstSlot == 2)
        {
            m_statusEffectManager.m_buffdebuff["DamageIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), 100, 20);
        }
    }

    public void PlayShieldEffect()
    {
        PlayEffect();
    }

    public override void Update()
    {
        base.Update();

        if (m_particle)
        {
            m_particle.transform.position = new Vector3(m_player.transform.position.x, m_playerBody.position.y, m_player.transform.position.z);
        }
    }

    public void BarrierDestroy(double remainingShield)
    {
        DisableEffect();

        if (m_tripod.thirdSlot == 1)
        {
            m_player.GetComponent<Player>().Health += 0.3 * m_player.GetComponent<Player>().MaxHealth;
        }

        if (m_tripod.firstSlot == 1)
        {
            List<MonsterScript> targetMonster = Managers.Monsters.GetMonsterInRange(transform.position, 3);
            if (targetMonster.Count > 0)
            {
                foreach (MonsterScript monster in targetMonster)
                {
                    if (m_tripod.thirdSlot == 2)
                    {
                        monster.IsTrueDamaged(this, 2 * (m_barrierAmount - remainingShield));
                    }
                    monster.IsTrueDamaged(this, (m_barrierAmount - remainingShield) / targetMonster.Count);
                }
            }
        }

        if (m_tripod.secondSlot == 1)
        {
            m_player.GetComponent<Player>().BuffDebuff.Remove(new KeyValuePair<Skill, string>(this, "SpeedIncrease"));
            m_player.GetComponent<Player>().BuffDebuff.Remove(new KeyValuePair<Skill, string>(this, "AttackSpeedIncrease"));
        }
        else if(m_tripod.secondSlot == 2)
        {
            m_player.GetComponent<Player>().Mana += 20;
        }

        if (m_tripod.firstSlot == 2)
        {
            m_player.GetComponent<Player>().BuffDebuff.Remove(new KeyValuePair<Skill, string>(this, "DamageIncrease"));
        }
    }

    public void BarrierUpdate()
    {
        if (m_tripod.thirdSlot == 3)
        {
            m_elapsedTime += Time.deltaTime;
            if (m_elapsedTime >= 1)
            {
                m_player.GetComponent<Player>().Mana += m_tripod3_3ManaRecovery;
                m_elapsedTime = 0;
            }
        }
    }

    public override void SetTripod(int tripodStep, int num)
    {
        base.SetTripod(tripodStep, num);
        if (tripodStep == 1)
        {
            if (num == 3)
            {
                m_manaCost = m_tripod1_3ManaCost;
            }
            else
            {
                m_manaCost = m_basicManaCost;
            }
        }
        else if (tripodStep == 2)
        {
            if (num == 3)
            {
                m_barrierPercent = m_tripod2_3BarrierPercent;
            }
            else
            {
                m_barrierPercent = m_basicBarrierPercent;
            }
        }
        m_finalBarrierAmount = m_barrierAmount * (1 + m_barrierPercent / 100);
    }

    public override void SkillLevelUp()
    {
        base.SkillLevelUp();
        m_barrierAmount = m_level1BarrierAmount + (m_skillLevel - 1) * m_barrierIncreasePerLevel;
        m_finalBarrierAmount = m_barrierAmount * (1 + m_barrierPercent / 100);
    }

    public override void ResetSkill()
    {
        base.ResetSkill();
        m_barrierAmount = m_level1BarrierAmount;
    }

    public override void SetSkillExplanation()
    {
        m_skillExplanation = "플레이어에게 <color=green>" + m_finalBarrierAmount + "</color><color=blue>(+" + m_barrierIncreasePerLevel + ")</color>의 대미지를 흡수하는 보호막을 부여합니다.";
    }
}
