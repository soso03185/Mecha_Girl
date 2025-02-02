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

    // �⺻ ��ų
    [SerializeField]
    [Tooltip("�⺻ �߰����� �ۼ�Ʈ")]
    float m_basicAdditionalDamage;
    [SerializeField]
    [Tooltip("���� �� �߰����� �ۼ�Ʈ ������")]
    float m_AdditionalDamageIncreasePerLevel;
    [SerializeField]
    [Tooltip("�⺻ ���� �Ҹ�")]
    float m_basicManaCost;

    // Ʈ��������1
    [SerializeField]
    [Tooltip("Ʈ��������1_1 ���� ���ӽð�")]
    float m_tripod1_1BuffDuration;
    [SerializeField]
    [Tooltip("Ʈ��������1_2 ���� ���ӽð�")]
    float m_tripod1_2BuffDuration;
    [SerializeField]
    [Tooltip("Ʈ��������1_3 ���� ���ӽð�")]
    float m_tripod1_3BuffDuration;

    [SerializeField]
    [Tooltip("Ʈ��������1_1 ���� �ۼ�Ʈ")]
    float m_tripod1_1BuffPercentage;
    [SerializeField]
    [Tooltip("Ʈ��������1_2 ���� �ۼ�Ʈ")]
    float m_tripod1_2BuffPercentage;
    [SerializeField]
    [Tooltip("Ʈ��������1_3 ���� �ۼ�Ʈ")]
    float m_tripod1_3BuffPercentage;

    // Ʈ��������2
    [SerializeField]
    [Tooltip("�⺻ �����̻� ���ӽð�")]
    float m_basicStatusEffectDuration;
    [SerializeField]
    [Tooltip("Ʈ��������2_1 �����̻� ���ӽð�")]
    float m_tripod2_1StatusEffectDuration;

    [SerializeField]
    [Tooltip("Ʈ��������2 �⺻ �߰� ���� �Ҹ�")]
    float m_tripod2BasicAdditionalManaCost;
    [SerializeField]
    [Tooltip("Ʈ��������2_2 �����Ҹ�")]
    float m_tripod2_2AdditionalManaCost;

    [SerializeField]
    [Tooltip("�⺻ ���� ���� ����")]
    float m_basicElementAmp;
    [SerializeField]
    [Tooltip("Ʈ��������2_3 ���� ���� ����")]
    float m_tripod2_3ElementAmp;

    // Ʈ��������3
    [SerializeField]
    [Tooltip("�⺻ ��Ȧ ���ӽð�")]
    float m_basicDuration;
    [SerializeField]
    [Tooltip("Ʈ��������3_1��Ȧ ���ӽð�")]
    float m_tripod3_1Duration;

    [SerializeField]
    [Tooltip("Ʈ��������3_2 �Ϲݸ��� �������")]
    float m_tripod3_2NormalDebuffPercent;
    [SerializeField]
    [Tooltip("Ʈ��������3_2 �������� �������")]
    float m_tripod3_2BossDebuffPercent;

    [SerializeField]
    [Tooltip("Ʈ��������3 �⺻ �߰� ���� �Ҹ�")]
    float m_tripod3BasicAdditionalManaCost;
    [SerializeField]
    [Tooltip("Ʈ��������3_3 �߰� ���� �Ҹ�")]
    float m_tripod3_3AdditionalManaCost;

    [SerializeField]
    [Tooltip("�⺻ �ʴ� ���� ������")]
    float m_basicRadiusIncrease;
    [SerializeField]
    [Tooltip("Ʈ��������3_3 ���� ������")]
    float m_tripod3_3RadiusIncrease;

    [SerializeField]
    [Tooltip("�⺻ �ʱ� ����")]
    float m_basicInitRadius;
    [SerializeField]
    [Tooltip("Ʈ��������3_3 �ʱ� ����")]
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
        m_skillExplanation = "����� ���� ��ġ�� ���� Ŀ���� ��Ȧ�� ��ȯ�մϴ�.(���ӽð� " + m_blackHoleObjectPrepab.m_duration + "��) ��Ȧ�� <color=purple>�̸�</color>ȿ���� �ο��ϰ� �޴� ���ظ� <color=red>" + m_blackHoleObjectPrepab.m_additionalDamage + "%</color><color=blue>(+" + m_AdditionalDamageIncreasePerLevel + "%)</color> ������ŵ�ϴ�.";
    }
}
