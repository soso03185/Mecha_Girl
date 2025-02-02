using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SkillType
{
    BuffSkill,
    ProjectileSkill,
    HitScanSkill
}

public enum Attribute
{
    Electricity,
    Water,
    Magnetic,
    Void,
    Idle
}
[Serializable]
public struct TripodData
{
    public string tripodName;
    public Sprite tripodSprite;
    public Sprite deactivatedTripodSprite;
    public string tripodExplanation;
}
public abstract class Skill : MonoBehaviour, IGamble
{
    public StatusEffectManager m_statusEffectManager;
    public GameObject m_target;
    protected GameObject m_player;
    public Transform m_playerBody;

    public int m_skillID;
    public string m_skillName;
    public string m_skillKorName;
    public List<string> m_tripodExplanation;
    public List<TripodData> m_tripodDatas;
    [Multiline(5)] public string m_skillExplanation = "메롱메롱";

    // 스킬 정보
    public int m_skillLevel;
    public Sprite m_image;
    public Tripod m_tripod;

    // 스킬 연출 정보
    public int particleCount = 1;

    public GameObject m_skillParticlePrefab;
    public EffectManager m_particle;

    public GameObject m_skillParticlePrefab2;
    public EffectManager m_particle2;

    protected List<EffectManager> m_particles = new List<EffectManager>();
    public string m_skillSound;

    // 스킬 변동 수치
    [Tooltip("스킬 속성")]
    public Attribute m_attribute;

    [Tooltip("사정거리 (1000으로 되어있는 것들은 사정거리 없는 것들")]
    public float m_skillRange = 1000;

    [Tooltip("필요 마나")]
    public float m_manaCost;

    [SerializeField]
    [Tooltip("레벨 1 스킬 계수")]
    protected float m_level1Coefficient;
    [SerializeField]
    [Tooltip("레벨당 스킬 계수 증가량")]
    protected float m_coefficientIncreasePerLevel;

    public float m_skillCoefficient;
    public float m_finalSkillCoefficient;


    [Tooltip("원소 반응 증폭")]
    public float m_elementAmp;
    // 뽑기

    public Define.Rarity Rarity
    {
        get => m_rarity;
        set => m_rarity = value;
    }
    public float Chance
    {
        get => m_chance;
        set => m_chance = value;
    }


    [SerializeField]
    private float m_chance;
    [SerializeField]
    private Define.Rarity m_rarity;

    protected virtual void Awake()
    {
        m_skillCoefficient = m_level1Coefficient;

        SetTripod(1, 0);
        SetTripod(2, 0);
        SetTripod(3, 0);
    }

    public virtual void Init()
    {
        StopAllCoroutines();

        m_statusEffectManager = GameObject.Find("StatusEffectManager").GetComponent<StatusEffectManager>();

        m_player = GameManager.Instance.player;
        m_playerBody = m_player.transform.GetChild(4).GetChild(2);

        if (m_skillParticlePrefab != null)
        {
            m_particles.Clear();
            for (int i = 0; i < particleCount; i++)
            {
                GameObject obj = Instantiate(m_skillParticlePrefab);
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

    }

    public virtual void Update()
    {
        transform.position = m_player.transform.position;
        //transform.rotation = m_player.transform.rotation;

        if (m_particle2)
        {
            m_particle2.transform.position = m_player.transform.position;
        }
    }

    public virtual GameObject GetTarget()
    {
        if (m_player == null)
        {
            m_player = GameManager.Instance.player;
        }
        return m_player;
    }

    public virtual void Activate()
    {

    }

    public void ClearParticleList()
    {

    }

    public virtual void PlayEffect(float stopTime = 0f)
    {
        if (m_particle)
        {
            m_particle.gameObject.SetActive(true);
            m_particle.PlayEffects();
        }

        if (stopTime > 0f)
        {
            Invoke("DisableEffect", stopTime);
        }
    }

    public virtual void PlaySecondEffect(float stopTime = 0f)
    {
        if (m_particle2)
        {
            m_particle2.gameObject.SetActive(true);
            m_particle2.PlayEffects();
        }

        if (stopTime > 0f)
        {
            Invoke("DisableSecondEffect", stopTime);
        }
    }

    public void DisableEffect()
    {
        m_particle.gameObject.SetActive(false);
    }

    public void DisableSecondEffect()
    {
        m_particle2.gameObject.SetActive(false);
    }

    public virtual void PlaySound(string s = "")
    {
        if (s == "meteor")
        {
            SoundManager.Instance.SetSFXVolume(0.2f);
        }
        else if (s != "")
        {
            SoundManager.Instance.PlaySFX(s);
            SoundManager.Instance.SetSFXVolume(0.5f);
        }
        else
        {
            SoundManager.Instance.PlaySFX(m_skillSound);
            SoundManager.Instance.SetSFXVolume(0.5f);
        }
    }

    public virtual void SetTripod(int tripodStep, int num)
    {
        switch (tripodStep)
        {
            case 1:
                {
                    m_tripod.firstSlot = num;
                    break;
                }
            case 2:
                {
                    m_tripod.secondSlot = num;
                    break;
                }
            case 3:
                {
                    m_tripod.thirdSlot = num;
                    break;
                }
        }
    }

    public virtual void SkillLevelUp()
    {
        // 여기에 트라이포드 잠금해제 넣기
        if (m_skillLevel == 3)
        {
            SkillTabManager.Instance.tripodUI.SetTripodLock(m_skillLevel);
        }
        else if (m_skillLevel == 6)
        {
            SkillTabManager.Instance.tripodUI.SetTripodLock(m_skillLevel);
        }
        else if (m_skillLevel == 9)
        {
            SkillTabManager.Instance.tripodUI.SetTripodLock(m_skillLevel);
        }
        m_skillCoefficient = m_level1Coefficient + (m_skillLevel - 1) * m_coefficientIncreasePerLevel;
    }

    public virtual void SkillLevelReset()
    {
        m_skillLevel = 1;
        m_skillCoefficient = m_level1Coefficient;
    }
    public virtual void ResetSkill()
    {
        SetTripod(1, 0);
        SetTripod(2, 0);
        SetTripod(3, 0);
        m_skillCoefficient = m_level1Coefficient + (m_skillLevel - 1) * m_coefficientIncreasePerLevel;
    }

    public virtual void SkillStart()
    {

    }

    public virtual void SetSkillExplanation()
    {


    }
}