using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI.Extensions.Tweens;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class HyperBeam : Skill
{
    float m_crossWidth;
    float m_additionalDamage;
    float m_criticalChance;
    float m_criticalMultiplier;
    float m_defenseNegationRatio;
    float m_damageIncrease = 20;
    [SerializeField]
    [Tooltip("���ڰ� ����")]
    float m_crossLength;

    // Ʈ�������� 1
    [SerializeField]
    [Tooltip("�⺻ ���� �Ҹ�")]
    float m_basicManaCost;
    [SerializeField]
    [Tooltip("Ʈ��������1_1 ���� �Ҹ�")]
    float m_tripod1_1ManaCost;

    [SerializeField]
    [Tooltip("Ʈ��������1_2 ���� ���ӽð�")]
    float m_tripod1_2BuffDuration;
    [SerializeField]
    [Tooltip("Ʈ��������1_2 ���� �ۼ�Ʈ")]
    float m_tripod1_2BuffPercnetage;

    [SerializeField]
    [Tooltip("�⺻ �߰�����")]
    float m_basicAdditionalDamage;
    [SerializeField]
    [Tooltip("Ʈ��������1_3 �߰� ����")]
    float m_tripod1_3AdditionalDamage;

    // Ʈ��������2
    [SerializeField]
    [Tooltip("�⺻ ���� ���� ����")]
    float m_basicElementAmp;
    [SerializeField]
    [Tooltip("Ʈ��������2_1 ���� ���� ����")]
    float m_tripod2_1ElementAmp;

    [SerializeField]
    [Tooltip("�⺻ ���� ���� ��ġ")]
    float m_basicDefenseNegationRatio;
    [SerializeField]
    [Tooltip("Ʈ��������2_3 ���� ���� ��ġ")]
    float m_tripod2_3DefenseNegationRatio;

    // Ʈ��������3
    [SerializeField]
    [Tooltip("�⺻ ���ڰ� ����")]
    float m_basicCrossWidth;
    [SerializeField]
    [Tooltip("Ʈ��������3_1 ���ڰ� ����")]
    float m_tripod3_1CrossWidth;


    public GameObject cross1;
    public GameObject cross2;

    protected override void Awake()
    {
        base.Awake();
        SetSkillExplanation();
    }
    public override void Activate()
    {
        List<MonsterScript> targetMonsters;
        if (m_tripod.secondSlot == 1)
        {
            List<MonsterScript> outlineMonsters = Managers.Monsters.GetMonsterInRange(transform.position, 4);
            List<MonsterScript> inlineMonsters = Managers.Monsters.GetMonsterInRange(transform.position, 4 - m_crossWidth);
            targetMonsters = outlineMonsters.Except(inlineMonsters).ToList();

        }
        else
        {
            Vector3 leftPoint = transform.position - new Vector3(m_crossLength / 2, 0, 0);
            Vector3 rightPoint = transform.position + new Vector3(m_crossLength / 2, 0, 0);
            Vector3 upPoint = transform.position + new Vector3(0, 0, m_crossLength / 2);
            Vector3 downPoint = transform.position - new Vector3(0, 0, m_crossLength / 2);
            List<MonsterScript> horizontalLine = Managers.Monsters.GetMonsterInPerpendicularDistance(leftPoint, rightPoint, m_crossWidth);
            List<MonsterScript> verticalLine = Managers.Monsters.GetMonsterInPerpendicularDistance(upPoint, downPoint, m_crossWidth);
            List<MonsterScript> crossLine = horizontalLine.Union(verticalLine).ToList();
            if (m_tripod.thirdSlot == 3)
            {
                Vector3 leftupPoint = new Vector3(leftPoint.x, 0, upPoint.z);
                Vector3 leftdownPoint = new Vector3(leftPoint.x, 0, downPoint.z);
                Vector3 rightupPoint = new Vector3(rightPoint.x, 0, upPoint.z);
                Vector3 rightdownPoint = new Vector3(rightPoint.x, 0, downPoint.z);
                List<MonsterScript> updownLine = Managers.Monsters.GetMonsterInPerpendicularDistance(leftupPoint, rightdownPoint, m_crossWidth);
                List<MonsterScript> downupLine = Managers.Monsters.GetMonsterInPerpendicularDistance(leftdownPoint, rightupPoint, m_crossWidth);
                List<MonsterScript> xLine = updownLine.Union(downupLine).ToList();

                targetMonsters = xLine.Union(crossLine).ToList();
            }
            else
            {

                targetMonsters = crossLine;
            }
        }

        var a = Instantiate(cross1);
        var b = Instantiate(cross2);
        a.transform.localScale = new Vector3(m_crossWidth, 1f, m_crossLength);
        b.transform.localScale = new Vector3(m_crossWidth, 1f, m_crossLength);
        a.transform.position = gameObject.transform.position;
        b.transform.position = gameObject.transform.position;

        PlaySound("HyperBeam2");
        float angle = 0;

        if (m_tripod.thirdSlot == 3)
        {
            foreach (var particle in m_particles)
            {
                particle.gameObject.transform.position = m_player.transform.position;
                particle.gameObject.transform.rotation = Quaternion.Euler(0, angle, 0);
                particle.gameObject.transform.localScale = new Vector3(m_crossWidth / 2f, 1f, m_crossLength / 30);
                particle.gameObject.SetActive(true);
                particle.PlayEffects();
                angle += 45f;
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                m_particles[i].gameObject.transform.position = m_player.transform.position;
                m_particles[i].gameObject.transform.rotation = Quaternion.Euler(0, angle, 0);
                m_particles[i].gameObject.transform.localScale = new Vector3(m_crossWidth / 2f, 1f, m_crossLength / 30);
                m_particles[i].gameObject.SetActive(true);
                m_particles[i].PlayEffects();
                angle += 90f;
            }
        }

        bool isKill = false;
        foreach (MonsterScript monster in targetMonsters)
        {

            if (monster.IsDamaged(this, m_finalSkillCoefficient, m_additionalDamage, m_criticalChance, m_criticalMultiplier, m_defenseNegationRatio) <= 0)
            {
                isKill = true;
            }

            if (m_tripod.thirdSlot == 2)
            {
                m_statusEffectManager.m_continuousDamage["ElectricShock"].ApplyEffect(this, monster, 5, 0);
            }
            if (m_tripod.firstSlot == 2)
            {
                m_statusEffectManager.m_buffdebuff["CriticalChanceIncrease"].ApplyEffect(this, m_player.GetComponent<Player>(), m_tripod1_2BuffDuration, m_tripod1_2BuffPercnetage);
            }
        }

        if (isKill)
        {
            if (m_tripod.secondSlot == 2)
            {
                m_statusEffectManager.m_abnormalStatus["Frenzy"].ApplyEffect(this, m_player.GetComponent<Player>(), 10);
            }
            else
            {
                m_statusEffectManager.m_abnormalStatus["Rage"].ApplyEffect(this, m_player.GetComponent<Player>(), 10);
            }
        }
    }

    public override void SetTripod(int tripodStep, int num)
    {
        base.SetTripod(tripodStep, num);
        if (tripodStep == 1)
        {
            if (num == 1)
            {
                m_manaCost = m_tripod1_1ManaCost;
                m_additionalDamage = m_basicAdditionalDamage;
            }
            else if (num == 3)
            {
                m_manaCost = m_basicManaCost;
                m_additionalDamage = m_tripod1_3AdditionalDamage;
            }
            else
            {
                m_manaCost = m_basicManaCost;
                m_additionalDamage = m_basicAdditionalDamage;
            }
        }
        else if (tripodStep == 2)
        {
            if (num == 1)
            {
                m_elementAmp = m_tripod2_1ElementAmp;
                m_damageIncrease = 15;
                m_defenseNegationRatio = m_basicDefenseNegationRatio;
            }
            else if(num == 2)
            {
                m_elementAmp = m_basicElementAmp;
                m_damageIncrease = 20;
                m_defenseNegationRatio = m_basicDefenseNegationRatio;
            }
            else if (num == 3)
            {
                m_elementAmp = m_basicElementAmp;
                m_damageIncrease = 15;
                m_defenseNegationRatio = m_tripod2_3DefenseNegationRatio;
            }
            else
            {
                m_elementAmp = m_basicElementAmp;
                m_damageIncrease = 15;
                m_defenseNegationRatio = m_basicDefenseNegationRatio;
            }
        }
        else if (tripodStep == 3)
        {
            if (num == 1)
            {
                m_crossWidth = m_tripod3_1CrossWidth;
            }
            else if (num == 3)
            {
                m_crossWidth = m_basicCrossWidth;
            }
            else
            {
                m_crossWidth = m_basicCrossWidth;
            }
        }
        m_finalSkillCoefficient = m_skillCoefficient + m_additionalDamage;
    }

    public override void SkillLevelUp()
    {
        base.SkillLevelUp();
        m_finalSkillCoefficient = m_skillCoefficient + m_additionalDamage;
    }

    public override GameObject GetTarget()
    {
        return Managers.Monsters.GetNearestMonster(transform).gameObject;
    }

    public override void ResetSkill()
    {
        base.ResetSkill();
        m_finalSkillCoefficient = m_skillCoefficient + m_additionalDamage;
    }

    public override void SetSkillExplanation()
    {
        m_skillExplanation = "���� ����� �������� �߻��� <color=red>" + m_finalSkillCoefficient + "%</color><color=blue>(+" + m_coefficientIncreasePerLevel + "%)</color>�� ���ظ� �ݴϴ�. ���� ������ " + m_defenseNegationRatio + "% �����մϴ�. ��ų ��� �� 10�ʰ� ���� óġ�� ������ <color=green>" + m_damageIncrease + "%</color>�� ���ݷ��� �����մϴ�.";
    }
}
