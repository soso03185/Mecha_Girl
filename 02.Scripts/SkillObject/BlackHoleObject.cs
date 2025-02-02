using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class BlackHoleObject : MonoBehaviour
{
    public Skill m_skill;

    public float m_statusEffectDuration;
    public float m_duration;
    public float m_initRadius;
    public float m_radiusIncrease;

    int m_firstSlot;
    int m_secondSlot;
    int m_thirdSlot;

    public float m_additionalDamage;

    float m_elapsedTime;
    float m_elapsedTime2;
    public bool isEnabled = false;

    public float normalMonsterDebuff;
    public float bossMonsterDebuff;

    GameObject particle;

    bool m_tinnitusActivate = true;

    private void Start()
    {
        transform.localScale = new Vector3(m_initRadius * 2, 0, m_initRadius * 2);
        //m_skill.m_particle.gameObject.transform.localScale = new Vector3(1, 1, 1);
        if (m_secondSlot == 3)
        {
            m_skill.m_statusEffectManager.m_buffdebuff["ManaRecovery"].ApplyEffect(m_skill, GameManager.Instance.player.GetComponent<Player>(), m_statusEffectDuration, 3);
        }
    }

    private void Update()
    {
        if(m_tinnitusActivate)
        {
            m_tinnitusActivate = false;
        }
        m_elapsedTime += Time.deltaTime;
        m_elapsedTime2 += Time.deltaTime;
        isEnabled = true;

        if (m_elapsedTime <= 4 / m_radiusIncrease)
        {
            if (isEnabled)
            {
                if (m_skill.m_skillParticlePrefab != null && particle == null)
                {
                    particle = Instantiate(m_skill.m_skillParticlePrefab);
                    particle.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
                    particle.gameObject.transform.localScale = new Vector3(0.3f * m_initRadius, 0, 0.3f * m_initRadius);
                }
            }

            isEnabled = false;

            particle.gameObject.transform.localScale += new Vector3(Time.deltaTime * 0.05f * m_radiusIncrease, 0, Time.deltaTime * 0.05f * m_radiusIncrease);
            transform.localScale += new Vector3(Time.deltaTime * m_radiusIncrease, 0, Time.deltaTime * m_radiusIncrease);
        }

        if (m_elapsedTime >= m_duration)
        {
            Destroy(particle);
            Destroy(gameObject);
            particle = null;
        }

        if (m_elapsedTime >= m_duration)
        {
            Destroy(particle);
            Destroy(gameObject);
            particle = null;
        }

        if (m_elapsedTime2 >= 1)
        {
            m_elapsedTime2 -= 1;
            m_tinnitusActivate = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Monster") || other.CompareTag("BossMonster"))
        {
            MonsterScript monster = other.gameObject.GetComponent<MonsterScript>();
            if (m_thirdSlot == 2)
            {
                if (other.CompareTag("Monster"))
                {
                    m_skill.m_statusEffectManager.m_buffdebuff["DamageIncrease"].ApplyEffect(m_skill, monster, 1, -normalMonsterDebuff);
                    m_skill.m_statusEffectManager.m_buffdebuff["DefenseIncrease"].ApplyEffect(m_skill, monster, 1, -normalMonsterDebuff);
                }
                else
                {
                    m_skill.m_statusEffectManager.m_buffdebuff["DamageIncrease"].ApplyEffect(m_skill, monster, 1, -bossMonsterDebuff);
                    m_skill.m_statusEffectManager.m_buffdebuff["DefenseIncrease"].ApplyEffect(m_skill, monster, 1, -bossMonsterDebuff);
                }
            }
            else if (m_thirdSlot == 3)
            {
                m_skill.m_statusEffectManager.m_buffdebuff["ContinuousAdditionalDamage"].ApplyEffect(m_skill, monster, 5, 50);
            }

            if (m_tinnitusActivate)
            {
                m_skill.m_statusEffectManager.m_continuousDamage["Tinnitus"].ApplyEffect(m_skill, monster, m_statusEffectDuration, 0f);
            }
            m_skill.m_statusEffectManager.m_buffdebuff["AdditionalDamage"].ApplyEffect(m_skill, monster, 1, m_additionalDamage);
        }
    }

    public void SetTripod(int tripodStep, int num)
    {
        switch (tripodStep)
        {
            case 1:
                {
                    m_firstSlot = num;
                    break;
                }
            case 2:
                {
                    m_secondSlot = num;
                    break;
                }
            case 3:
                {
                    m_thirdSlot = num;
                    break;
                }
        }
    }
}
