using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaterFall : Skill
{
    float m_additionalDamage;
    float m_attackRange;
    float m_bossAdditionalDamage;
    float m_additionalCoefficient;

    // �⺻ ��ų
    [SerializeField]
    [Tooltip("������������")]
    float m_monsterRange;
    [SerializeField]
    [Tooltip("���� ���� �ð�")]
    float m_stunDuration;

    // Ʈ��������1
    [SerializeField]
    [Tooltip("Ʈ��������1_3 ���� �ۼ�Ʈ")]
    float m_tripod1_3BuffPercent;

    // Ʈ��������2
    [SerializeField]
    [Tooltip("�⺻ ���� �̻� ����")]
    float m_basicElementAmp;
    [SerializeField]
    [Tooltip("Ʈ��������2_1 ���� �̻� ����")]
    float m_tripod2_1ElementAmp;

    [SerializeField]
    [Tooltip("�⺻ ���� �Ҹ�")]
    float m_basicManaCost;
    [SerializeField]
    [Tooltip("Ʈ��������2_2 ���� �Ҹ�")]
    float m_tripod2_2ManaCost;

    [SerializeField]
    [Tooltip("�⺻ ��ų ��� ������")]
    float m_basicAdditionalCoefficient;
    [SerializeField]
    [Tooltip("Ʈ��������2_3 ��ų ��� ������")]
    float m_tripod2_3AdditionalCoefficient;

    // Ʈ��������3
    [SerializeField]
    [Tooltip("�⺻ ����� ����")]
    float m_basicAttackRange;
    [SerializeField]
    [Tooltip("Ʈ��������3_1 ���� ����")]
    float m_tripod3_1AttackRange;

    [SerializeField]
    [Tooltip("�⺻ ���� ��� �߰� ����(%)")]
    float m_basicBossAdditionalDamage;
    [SerializeField]
    [Tooltip("Ʈ������Ʈ3-2 ���� ��� �߰�����")]
    float m_tripod3_2BossAdditionalDamage;

    [SerializeField]
    [Tooltip("�⺻ �߰� �����")]
    float m_basicAdditionalDamage;
    [SerializeField]
    [Tooltip("Ʈ��������3_3 �߰� �����")]
    float m_tripod3_3AdditionalDamage;

    Vector3 lookRotation;

    public GameObject range;

    System.Random random = new System.Random();

    protected override void Awake()
    {
        base.Awake();
        SetSkillExplanation();
    }
    public override void Activate()
    {
        List<MonsterScript> monsterInRange = Managers.Monsters.GetMonsterInRange(transform.position, m_monsterRange);
        MonsterScript target = null;

        if (m_tripod.firstSlot == 1)
        {
            List<MonsterScript> statusEffectMonsters = monsterInRange.Where(monster => monster.BuffDebuff.Count + monster.ContinuousDamage.Count + monster.AbnormalStatus.Count > 0).ToList();

            if (statusEffectMonsters.Count > 0)
            {
                target = statusEffectMonsters.OrderBy(monster => random.Next()).ToList()[0];
            }
        }
        else if (m_tripod.firstSlot == 2)
        {
            List<MonsterScript> bossMonsters = monsterInRange.Where(monster => monster.CompareTag("BossMonster")).ToList();
            if (bossMonsters.Count > 0)
            {
                target = bossMonsters.OrderBy(monster => random.Next()).ToList()[0];
            }
        }
        if (target == null)
        {
            if (monsterInRange.Count != 0)
            {
                target = monsterInRange.OrderBy(monster => random.Next()).ToList()[0];
            }
        }

        if (target != null)
        {
            Vector3 direction = (target.transform.position - m_player.transform.position).normalized;
            direction.y = 0;
            m_player.transform.position = target.transform.position + direction * 2;
        }

        StartCoroutine(Damage());

        if (m_particle)
        {
            m_particle.transform.position = m_player.transform.position;
            m_particle.transform.localScale = new Vector3(m_attackRange / 3, 1, m_attackRange / 3);
        }

    }

    public override void SkillStart()
    {
        if (m_tripod.firstSlot == 3)
        {
            m_statusEffectManager.m_buffdebuff["CriticalMultiplierIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), 2.5f, m_tripod1_3BuffPercent);
        }
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(0.3f);
        List<MonsterScript> monsterList = Managers.Monsters.GetMonsterInRange(transform.position, m_attackRange);

        m_player.GetComponent<Player>().AnimEventShake(0f, 0.5f, 0.3f, 0f);

        PlaySound("WaterFall2");
        //Instantiate(range, transform.position, transform.rotation).transform.localScale = new Vector3(m_attackRange * 2, 0.1f, m_attackRange * 2);
        if (monsterList != null)
        {
            foreach (var monster in monsterList)
            {
                lookRotation = (monster.transform.position - m_player.transform.position).normalized;

                lookRotation.y = 0f;

                m_player.transform.rotation = Quaternion.LookRotation(lookRotation);

                if (monster.CompareTag("Monster"))
                {
                    monster.IsDamaged(this, m_finalSkillCoefficient, m_additionalDamage);
                }
                else
                {
                    monster.IsDamaged(this, m_finalSkillCoefficient, m_bossAdditionalDamage + m_additionalDamage);
                }
                m_statusEffectManager.m_abnormalStatus["Stun"].ApplyEffect(this, monster, m_stunDuration);

                if (m_tripod.thirdSlot == 3)
                {
                    m_statusEffectManager.m_buffdebuff["ContinuousAdditionalDamage"].ApplyEffect(this, monster, 5, 50);
                }
                else if (m_tripod.thirdSlot == 2 && monsterList.Count >= 3)
                {
                    m_player.GetComponent<Player>().Mana += 50;
                }
            }
        }
    }
    public override GameObject GetTarget()
    {
        if (m_player == null)
        {
            m_player = GameManager.Instance.player;
        }
        m_target = Managers.Monsters.GetNearestMonster(m_player.transform).gameObject;
        return m_target;
    }

    public override void SetTripod(int tripodStep, int num)
    {
        base.SetTripod(tripodStep, num);
        if (tripodStep == 2)
        {
            if(num == 1)
            {
                m_elementAmp = m_tripod2_1ElementAmp;
                m_manaCost = m_basicManaCost;
                m_additionalCoefficient = m_basicAdditionalCoefficient;
            }
            else if (num == 2)
            {
                m_elementAmp = m_basicElementAmp;
                m_manaCost = m_tripod2_2ManaCost;
                m_additionalCoefficient = m_basicAdditionalCoefficient;
            }
            else if (num == 3)
            {
                m_elementAmp = m_basicElementAmp;
                m_manaCost = m_basicManaCost;
                m_additionalCoefficient = m_tripod2_3AdditionalCoefficient;
            }
            else
            {
                m_elementAmp = m_basicElementAmp;
                m_manaCost = m_basicManaCost;
                m_additionalCoefficient = m_basicAdditionalCoefficient;
            }
        }
        if (tripodStep == 3)
        {
            if (num == 1)
            {
                m_attackRange = m_tripod3_1AttackRange;
                m_bossAdditionalDamage = m_basicBossAdditionalDamage;
                m_additionalDamage = m_basicAdditionalDamage;
            }
            else if (num == 2)
            {
                m_attackRange = m_basicAttackRange;
                m_bossAdditionalDamage = m_tripod3_2BossAdditionalDamage;
                m_additionalDamage = m_basicAdditionalDamage;
            }
            else if (num == 3)
            {
                m_attackRange = m_basicAttackRange;
                m_bossAdditionalDamage = m_basicBossAdditionalDamage;
                m_additionalDamage = m_tripod3_3AdditionalDamage;
            }
            else
            {
                m_attackRange = m_basicAttackRange;
                m_bossAdditionalDamage = m_basicBossAdditionalDamage;
                m_additionalDamage = m_basicAdditionalDamage;
            }
        }
        m_finalSkillCoefficient = m_skillCoefficient + m_additionalCoefficient;
    }
    public override void SkillLevelUp()
    {
        base.SkillLevelUp();
        m_finalSkillCoefficient = m_skillCoefficient + m_additionalCoefficient;
    }

    public override void ResetSkill()
    {
        base.ResetSkill();
        m_finalSkillCoefficient = m_skillCoefficient + m_additionalCoefficient;
    }
    public override void SetSkillExplanation()
    {
        m_skillExplanation = "����� ������ �����Ͽ� ���� �����մϴ�. �� �ֺ��� ������ <color=red>" + m_finalSkillCoefficient + "%</color><color=blue>(+" + m_coefficientIncreasePerLevel + "%)</color>�� ���ظ� �ְ� " + m_stunDuration + "�ʰ� ������ �ο��մϴ�. ���ظ� ���� ��� ���� �̵��ӵ��� ���ݼӵ��� 50% ���ҽ�ŵ�ϴ�. �������Դ� +" + m_bossAdditionalDamage + "%�� �߰� ���ظ� �ݴϴ�.";
    }
}
