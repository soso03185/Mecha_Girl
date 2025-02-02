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
    // �⺻ ��ų
    [SerializeField]
    [Tooltip("��������ȸ�� �ð�")]
    float m_manaDuration;

    [SerializeField]
    [Tooltip("���� 1 ���� ȸ����")]
    float m_basicManaRecovery;
    [SerializeField]
    [Tooltip("���� �� ���� ȸ�� ������")]
    float m_manaRecoveryPerLevel;

    [SerializeField]
    [Tooltip("�������� �ð�")]
    float m_buffDuration;
    [SerializeField]
    [Tooltip("�Ʊ� ����(%)")]
    float m_buffAmount;
    [SerializeField]
    [Tooltip("���� ����(%)")]
    float m_debuffAmount;

    [SerializeField]
    [Tooltip("�⺻ Ʈ��������1 �÷��̾� �����")]
    float m_basicTripod1PlayerDamage;
    [SerializeField]
    [Tooltip("Ʈ�������� 1_1 �÷��̾� �����")]
    float m_tripod1_1PlayerDamage;

    [SerializeField]
    [Tooltip("�⺻ Ʈ��������1 �Ϲ� ���� �����")]
    float m_basicTripod1NormalMonsterDamage;
    [SerializeField]
    [Tooltip("Ʈ��������1_1 �Ϲ� ���� �����")]
    float m_tripod1_1NormalMonsterDamage;

    [SerializeField]
    [Tooltip("�⺻ Ʈ��������1 �÷��̾� ����")]
    float m_basicTripod1PlayerDefense;
    [SerializeField]
    [Tooltip("Ʈ��������1_2 �÷��̾� ����")]
    float m_tripod1_2PlayerDefense;

    [SerializeField]
    [Tooltip("�⺻ Ʈ��������1 �������� ����")]
    float m_basicTripod1BossMonsterDefense;
    [SerializeField]
    [Tooltip("Ʈ��������1_2 �������� ����")]
    float m_tripod1_2BossMonsterDefense;

    [SerializeField]
    [Tooltip("�⺻ Ʈ��������2 �Ϲݸ��� ����")]
    float m_basicTripod2NormalMonsterDefnese;
    [SerializeField]
    [Tooltip("Ʈ��������2_1 �Ϲ� ���� ����")]
    float m_tripod2_1NormalMonsterDefense;

    [SerializeField]
    [Tooltip("�⺻ Ʈ��������2 �������� ����")]
    float m_basicTripod2BossMonsterDefense;
    [SerializeField]
    [Tooltip("Ʈ��������2_2 ���� ���� ����")]
    float m_tripod2_2BossMonsterDefense;

    [SerializeField]
    [Tooltip("�⺻ Ʈ��������2 �÷��̾� �����")]
    float m_basicTripod2PlayerDamage;
    [SerializeField]
    [Tooltip("Ʈ�������� 2_3 �÷��̾� �����")]
    float m_tripod2_3PlayerDamage;

    [SerializeField]
    [Tooltip("Ʈ��������3_1 ġ��Ÿ Ȯ��")]
    float m_tripod3_1CriticalChance;
    [SerializeField]
    [Tooltip("Ʈ��������3_1 ġ��Ÿ ����")]
    float m_tripod3_1CriticalMultiplier;

    [SerializeField]
    [Tooltip("Ʈ��������3 ���� ����")]
    float m_basicTripod3MonsterDefense;
    [SerializeField]
    [Tooltip("Ʈ��������3_3 ���� ����")]
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
        m_skillExplanation = "5�ʰ� ���� <color=blue>" + m_manaRecovery + "(+" + m_manaRecoveryPerLevel + ")</color>�� ������ ȸ���մϴ�. 10�ʰ� �÷��̾��� ���ݷ°� ������ <color=green>30%</color>�����ϰ�, ���� ���ݷ°� ������ <color=red>30%</color>�����մϴ�.";
    }
}
