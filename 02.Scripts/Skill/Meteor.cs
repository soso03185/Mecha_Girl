using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class Meteor : Skill
{
    int m_meteorReps;
    int m_minCount;
    int m_maxCount;
    float m_meteorRange;
    float m_tripod1ManaCost;
    float m_tripod2ManaCost;
    float m_additionalCoefficient;

    // �⺻ ��ų
    [SerializeField]
    [Tooltip("���׿� �������� �ð� ����(��)")]
    float m_time;

    [SerializeField]
    [Tooltip("�̸� ���¿� ���� ���� �� ��� �߰� ����(%)")]
    float m_additionalDamage;
    [SerializeField]
    [Tooltip("����� ���ӽð�")]
    float m_duration;
    [SerializeField]
    [Tooltip("�̼Ӱ��� �ۼ�Ʈ")]
    float m_speedDecrease;
    [SerializeField]
    [Tooltip("���� �ۼ�Ʈ")]
    float m_defenseDecrease;

    [SerializeField]
    [Tooltip("�⺻ ���� �Ҹ�")]
    float m_basicManaCost;
    // Ʈ�������� 1�ܰ�
    [SerializeField]
    [Tooltip("Ʈ��������1 �߰� ���� �Ҹ�")]
    float m_tripod1BasicManaCost;
    [SerializeField]
    [Tooltip("Ʈ��������1_1 �߰� ���� �Ҹ�")]
    float m_tripod1_1ManaCost;

    [SerializeField]
    [Tooltip("�⺻ �߰� ��ų ���")]
    float m_basicAdditionalCoefficient;
    [SerializeField]
    [Tooltip("Ʈ��������1_2 �߰� ��ų ���")]
    float m_tripod1_2AdditionalCoefficient;

    [SerializeField]
    [Tooltip("�⺻ ���� ���� ����")]
    float m_basicElementAmp;
    [SerializeField]
    [Tooltip("Ʈ��������1_3 ���� ���� ����")]
    float m_tripod1_3ElementAmp;

    // Ʈ�������� 2�ܰ�
    [SerializeField]
    [Tooltip("�⺻ ���׿� �������� Ƚ��")]
    int m_basicReps;
    [SerializeField]
    [Tooltip("Ʈ��������2_1 ���׿� �������� Ƚ��")]
    int m_tripod2_1Reps;

    [SerializeField]
    [Tooltip("Ʈ��������2 �߰� ���� ��뷮")]
    float m_tripod2BasicManaCost;
    [SerializeField]
    [Tooltip("Ʈ��������2_1 �߰� ���� ��뷮")]
    float m_tripod2_1AdditionalManaCost;
    [SerializeField]
    [Tooltip("Ʈ��������2_3 �߰� ���� ��뷮")]
    float m_tripod2_3AdditionalManaCost;


    [SerializeField]
    [Tooltip("�⺻ �ѹ��� �����ϴ� �ּ� ����")]
    int m_basicMinCount;
    [SerializeField]
    [Tooltip("Ʈ��������2_2 ���׿� �ּ� ����")]
    int m_tripod2_2MinCount;
    [SerializeField]
    [Tooltip("Ʈ��������2_3 ���׿� �ּ� ����")]
    int m_tripod2_3MinCount;

    [SerializeField]
    [Tooltip("�⺻ �ѹ��� �����ϴ� �ִ� ����")]
    int m_basicMaxCount;
    [SerializeField]
    [Tooltip("Ʈ��������2_2 ���׿� �ִ� ����")]
    int m_tripod2_2MaxCount;
    [SerializeField]
    [Tooltip("Ʈ��������2_3 ���׿� �ִ� ����")]
    int m_tripod2_3MaxCount;

    [SerializeField]
    [Tooltip("�⺻ ���׿� ����� ����")]
    float m_basicMeteorRange;
    [SerializeField]
    [Tooltip("Ʈ��������2_2 ���׿� ����� ����")]
    float m_tripod2_2MeteorRange;

    public GameObject meteor;

    protected override void Awake()
    {
        base.Awake();
        SetSkillExplanation();
    }
    public override void Activate()
    {
        StartCoroutine("Meteorite");
    }

    IEnumerator Meteorite()
    {
        Camera cam = Camera.main;
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        for (int i = 0; i < m_meteorReps; i++)
        {
            yield return new WaitForSeconds(m_time);

            int randomCount = Random.Range(m_minCount, m_maxCount + 1);
            List<MonsterScript> monsterList = new List<MonsterScript>();
            if (m_tripod.thirdSlot == 1)
            {
                monsterList = Managers.Monsters.GetAllMonsters().Where(monster => monster.BuffDebuff.Count + monster.AbnormalStatus.Count + monster.ContinuousDamage.Count > 0).ToList();

                if (monsterList.Count > randomCount)
                {
                    monsterList = monsterList.Take(randomCount).ToList();
                }
            }
            else if (m_tripod.thirdSlot == 2)
            {
                monsterList = Managers.Monsters.GetAllMonsters().Where(monster => monster.CompareTag("BossMonster")).ToList();
                if (monsterList.Count > randomCount)
                {
                    monsterList = monsterList.Take(randomCount).ToList();
                }
                else if (monsterList.Count < randomCount)
                {
                    if (monsterList.Count != 0)
                    {
                        int tmp = randomCount - monsterList.Count;
                        for (int j = 0; j < tmp; j++)
                        {
                            monsterList.Add(monsterList[Random.Range(0, monsterList.Count)]);
                        }
                    }
                }
            }
            else if (m_tripod.thirdSlot == 3)
            {
                if (Managers.Monsters.GetMonsterPopulation() < randomCount)
                {
                    monsterList = Managers.Monsters.GetAllMonsters();
                }
                else
                {
                    monsterList = Managers.Monsters.GetAllMonsters().OrderByDescending(monster => monster.Health / monster.MaxHealth).Take(randomCount).ToList();
                }
            }

            foreach (var monster in monsterList)
            {
                GameObject go = Instantiate(m_skillParticlePrefab);
                go.transform.position = monster.transform.position;
                Destroy(go, 5f);

                StartCoroutine(Damage(monster.transform.position));
            }
            for (int j = 0; j < randomCount - monsterList.Count; j++)
            {
                float randomX = Random.Range(0, Screen.width);
                float randomY = Random.Range(0, Screen.height);

                Vector3 randomScreenPoint = new Vector3(randomX, randomY, 0);

                Ray ray = cam.ScreenPointToRay(randomScreenPoint);

                float distance;
                if (groundPlane.Raycast(ray, out distance))
                {
                    Vector3 targetPoint = ray.GetPoint(distance);
                    // To ���� : ���⿡�ٰ� ����Ʈ �����Ű�� �ɵ�

                    GameObject go = Instantiate(m_skillParticlePrefab);
                    go.transform.position = targetPoint;
                    go.transform.GetChild(0).localScale = new Vector3(m_meteorRange / m_basicMeteorRange, m_meteorRange / m_basicMeteorRange, m_meteorRange / m_basicMeteorRange);
                    Destroy(go, 5f);
                    //foreach (var par in m_particles)
                    //{
                    //    if (!par.gameObject.activeSelf)
                    //    {
                    //        par.gameObject.SetActive(true);
                    //        par.gameObject.transform.position = worldPoint;
                    //        par.PlayEffects();
                    //        Invoke("DisableEffect", 5.0f);
                    //        break;
                    //    }
                    //}
                    //m_particles[i*j].gameObject.SetActive(true);
                    //m_particles[i*j].gameObject.transform.localPosition = worldPoint;
                    //m_particles[i * j].PlayEffects();
                    StartCoroutine(Damage(targetPoint));
                }
            }
        }
    }

    IEnumerator Damage(Vector3 pos)
    {
        yield return new WaitForSeconds(1.6f); // To ���� : ����� ���׿� �������� �ӵ� ���� �ٲ��ּ���
        //m_player.GetComponent<Player>().AnimEventShake(0f, 0.5f, 0.2f, 0.5f);
        List<MonsterScript> monsters = Managers.Monsters.GetMonsterInRange(pos, m_meteorRange);

        PlaySound("meteor");
        //Instantiate(meteor, pos, Quaternion.identity);
        //
        //meteor.transform.localScale = new Vector3(m_meteorRange, m_meteorRange, m_meteorRange);
        if (monsters == null)
            yield break;

        foreach (MonsterScript monster in monsters)
        {
            monster.IsDamaged(this, m_finalSkillCoefficient, 0);

            m_statusEffectManager.m_buffdebuff["SpeedIncrease"].ApplyEffect(this, monster, m_duration, -m_speedDecrease);
            m_statusEffectManager.m_buffdebuff["DefenseIncrease"].ApplyEffect(this, monster, m_duration, -m_defenseDecrease);
        }
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
                m_tripod1ManaCost = m_tripod1_1ManaCost;
                m_additionalCoefficient = m_basicAdditionalCoefficient;
                m_elementAmp = m_basicElementAmp;
            }
            else if (num == 2)
            {
                m_tripod1ManaCost = m_tripod1BasicManaCost;
                m_additionalCoefficient = m_tripod1_2AdditionalCoefficient;
                m_elementAmp = m_basicElementAmp;
            }
            else if(num == 3)
            {
                m_tripod1ManaCost = m_tripod1BasicManaCost;
                m_additionalCoefficient = m_basicAdditionalCoefficient;
                m_elementAmp = m_tripod1_3ElementAmp;
            }
            else
            {
                m_tripod1ManaCost = m_tripod1BasicManaCost;
                m_additionalCoefficient = m_basicAdditionalCoefficient;
                m_elementAmp = m_basicElementAmp;
            }
        }
        else if (tripodStep == 2)
        {
            if (num == 1)
            {
                m_tripod2ManaCost = m_tripod2_1AdditionalManaCost;
                m_meteorReps = m_tripod2_1Reps;
                m_meteorRange = m_basicMeteorRange;
                m_minCount = m_basicMinCount;
                m_maxCount = m_basicMaxCount;
            }
            else if (num == 2)
            {
                m_tripod2ManaCost = m_tripod2BasicManaCost;
                m_meteorReps = m_basicReps;
                m_meteorRange = m_tripod2_2MeteorRange;
                m_minCount = m_tripod2_2MinCount;
                m_maxCount = m_tripod2_2MaxCount;
            }
            else if (num == 3)
            {
                m_tripod2ManaCost = m_tripod2_3AdditionalManaCost;
                m_meteorReps = m_basicReps;
                m_meteorRange = m_basicMeteorRange;
                m_minCount = m_tripod2_3MinCount;
                m_maxCount = m_tripod2_3MaxCount;
            }
            else
            {
                m_tripod2ManaCost = m_tripod2BasicManaCost;
                m_meteorReps = m_basicReps;
                m_meteorRange = m_basicMeteorRange;
                m_minCount = m_basicMinCount;
                m_maxCount = m_basicMaxCount;
            }
        }
        m_manaCost = m_basicManaCost + m_tripod1ManaCost + m_tripod2ManaCost;
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
        m_skillExplanation = "���Ϳ��� ������(" + m_minCount + "~" + m_maxCount + "��)�� ��� ����߷� ���� ���� ���Ϳ��� <color=red>" + m_finalSkillCoefficient + "%</color><color=blue>(+" + m_coefficientIncreasePerLevel + "%)</color>�� ���ظ� �ݴϴ�. ��� ���ظ� ���� ������ 10�ʰ� �̵��ӵ��� <color=red>30%</color>, ������ <color=red>50%</color> �����մϴ�.";
    }
}
