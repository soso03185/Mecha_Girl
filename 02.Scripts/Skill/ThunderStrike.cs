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

    // �⺻ ��ų
    [SerializeField]
    [Tooltip("���� ����")]
    float m_thunderRange;
    [SerializeField]
    [Tooltip("���� ���ӽð�")]
    float m_shockDuration;
    [SerializeField]
    [Tooltip("���� ���ӽð�")]
    float m_stunDuration;

    // Ʈ�������� 2�ܰ�
    [SerializeField]
    [Tooltip("�⺻ ���� �Ҹ�")]
    float m_basicManaCost;
    [SerializeField]
    [Tooltip("Ʈ��������2_1 ���� �Ҹ�")]
    float m_tripod2_1ManaCost;

    [SerializeField]
    [Tooltip("Ʈ��������2�� �߰� ��ų���")]
    float m_tripod2BasicAdditionalCoefficient;
    [SerializeField]
    [Tooltip("Ʈ��������2_2 �߰� ��ų ���")]
    float m_tripod2_2AdditionalCoefficient;

    [SerializeField]
    [Tooltip("�⺻ ���� �̻� ����")]
    float m_tripod2BasicElementAmp;
    [SerializeField]
    [Tooltip("Ʈ��������2_3 ���� �̻� ����")]
    float m_tripod2_3ElementAmp;

    // Ʈ�������� 3�ܰ�
    [SerializeField]
    [Tooltip("�⺻ ���� �������� Ƚ��")]
    int m_basicThunderCount;
    [SerializeField]
    [Tooltip("Ʈ�������� 3_1 ���� �������� Ƚ��")]
    int m_tripod3_1ThunderCount;

    [SerializeField]
    [Tooltip("�⺻ 1ȸ�� �������� ���� ����")]
    int m_basicThunderNumber;
    [SerializeField]
    [Tooltip("Ʈ��������3_3 ���� �������� ����")]
    int m_tripod3_3ThunderNumber;
    [SerializeField]
    [Tooltip("Ʈ��������3_2 ���� �������� ����")]
    int m_tripod3_2ThunderNumber;

    [SerializeField]
    [Tooltip("Ʈ��������3 �⺻ �߰������")]
    float m_tripod3BasicAdditionalDamage;
    [SerializeField]
    [Tooltip("Ʈ��������3_2 �߰������")]
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
        m_skillExplanation = "���Ϳ��� " + m_thunderNumber + "���� ���ڸ� ������ ���� ���� ���Ϳ���<color=red>" + m_finalSkillCoefficient + "%</color><color=blue>(+" + m_coefficientIncreasePerLevel + "%)</color>�� ���ظ� �ݴϴ�. " + m_shockDuration + "�ʰ� <color=yellow>����</color>, " + m_stunDuration + "�ʰ� <color=white>����</color>�� �ο��˴ϴ�.";
    }
}
