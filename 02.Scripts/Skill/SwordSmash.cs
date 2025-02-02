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

    // �⺻ ��ų
    [SerializeField]
    [Tooltip("���� ��ȭ ���ӽð�(��)")]
    float m_vulnerabilityDuration;
    [SerializeField]
    [Tooltip("���� ��ȭ �ۼ�Ʈ")]
    float m_percentage;

    [SerializeField]
    [Tooltip("ħ�� ���� �ð�")]
    float m_floodingDuration;

    // Ʈ�������� 1�ܰ�
    [SerializeField]
    [Tooltip("�⺻�߰�����")]
    float m_basicAdditionalDamage;
    [SerializeField]
    [Tooltip("Ʈ��������1_1�߰�����")]
    float m_tripod1_1AdditionalDamage;

    [SerializeField]
    [Tooltip("Ʈ��������1_2 ���ݷ� ������")]
    float m_damageIncreasePercent;
    [SerializeField]
    [Tooltip("Ʈ��������1_2 ���ݷ� ���� ���� �ð�")]
    float m_damageIncreaseDuration;

    [SerializeField]
    [Tooltip("Ʈ��������1_3 ġ��Ÿ Ȯ�� ������")]
    float m_criticalChanceIncreasePercent;
    [SerializeField]
    [Tooltip("Ʈ��������1_3 ġ��Ÿ Ȯ�� ���� ���� �ð�")]
    float m_criticalChanceIncreaseDuration;

    // Ʈ�������� 2�ܰ�
    [SerializeField]
    [Tooltip("�⺻ ħ����� �߰�����")]
    float m_basicElementAmp;
    [SerializeField]
    [Tooltip("Ʈ��������2-1 ħ����� �߰�����")]
    float m_tripod2_1ElementAmp;

    [SerializeField]
    [Tooltip("Ʈ��������2_2 ���ݷ� ���� �ð�")]
    float m_tripod2_2DamageDecreaseDuration;
    [SerializeField]
    [Tooltip("Ʈ��������2_2 ���ݷ� ���ҷ�")]
    float m_tripod2_2DamageDecreasePercent;

    [SerializeField]
    [Tooltip("�⺻ ���� �ڽ�Ʈ")]
    float m_basicManaCost;
    [SerializeField]
    [Tooltip("Ʈ�������� 2_3 ���� �Ҹ�")]
    float m_tripod2_3ManaCost;

    // Ʈ�������� 3�ܰ�
    [SerializeField]
    [Tooltip("�⺻ ��ų ����")]
    float m_basicRange;
    [SerializeField]
    [Tooltip("Ʈ�������� 3-1 ��ų ����")]
    float m_tripod3_1Range;

    [SerializeField]
    [Tooltip("�⺻ �Ϲ� ���� ��� �����(%)")]
    float m_basicNormalDamage;
    [SerializeField]
    [Tooltip("Ʈ��������3-2 �Ϲ� ���� ��� �����(%)")]
    float m_tripod3_2NormalDamage;
    [SerializeField]
    [Tooltip("Ʈ��������3-3 �Ϲ� ���� ��� �����(%)")]
    float m_tripod3_3NormalDamage;

    [SerializeField]
    [Tooltip("�⺻ �������� ��� �����(%)")]
    float m_basicBossDamage;
    [SerializeField]
    [Tooltip("Ʈ��������3-2 �������� ��� �����(%)")]
    float m_tripod3_2BossDamage;
    [SerializeField]
    [Tooltip("Ʈ��������3-3 �������� ��� �����(%)")]
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
        m_skillExplanation = "������ <color=red>" + m_finalSkillCoefficient + "%</color><color=blue>(+" + m_coefficientIncreasePerLevel + "%)</color>�� ���ظ� �ְ�, " + m_floodingDuration + "�ʰ� <color=blue>ħ��</color>ȿ���� �ο��մϴ�. �Ϲݸ��Ϳ��� " + m_normalDamage / 100 + "��, �������Ϳ��� " + m_bossDamage / 100 + "���� ���ظ� �ݴϴ�.";

    }
}

