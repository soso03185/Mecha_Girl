using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static DataManager;
using static Define;

public class MonsterScript : MonoBehaviour, IMonster
{
    float m_elapsedTime = 0;
    [SerializeField]
    public MonsterState monsterState = MonsterState.spawn;

    MonsterState lastState;

    public MonsterName monsterName;

    //joohong
    private GameObject m_canvas;

    public GameObject hud;

    //joohong
    [HideInInspector] public GameObject m_HpBar;

    [HideInInspector] public GameObject damageText;

    public Player player;
    Animator anim;

    public int monsterID;

    public MonsterManager manager;

    public DataManager.MonsterInfo monsterInfo;
    //public MonsterDataSO monsterInfo;

    public BattleAnalysisSystem m_battleAnalysisSystem;

    public double MaxHealth { get; set; } = 100;
    public double HealthPerSecond { get; set; } = 10;
    public double Defense { get; set; } = 10;
    public float DefensePercentage { get; set; } = 0;
    public double Damage { get; set; } = 0;
    public float DamagePercentage { get; set; } = 0;
    public float CriticalChance { get; set; } = 25;
    public double CriticalMultiplier { get; set; } = 150;
    public float MoveSpeed { get; set; } = 5;
    public float MoveSpeedPercentage { get; set; } = 100;
    public float AttackSpeed { get; set; }
    public float AttackSpeedPercentage
    {
        get
        {
            return _attackSpeedPercentage;
        }
        set
        {
            _attackSpeedPercentage = value;
            if (_attackSpeedPercentage < -100)
            {
                _attackSpeedPercentage = -100;
            }
        }
    }

    [SerializeField]
    private Attribute attribute;

    public Attribute Attribute
    {
        get { return attribute; }
        set { attribute = value; }
    }

    public GameObject m_WaterAuraPrefab;
    public GameObject m_MagneticAuraPrefab;
    public GameObject m_LightningAuraPrefab;

    [HideInInspector]
    public GameObject m_WaterAura;
    [HideInInspector]
    public GameObject m_MagneticAura;
    [HideInInspector]
    public GameObject m_LightningAura;

    public float AdditionalDamage { get; set; } = 100;
    public float ContinuousAdditionalDamage { get; set; } = 0;

    public bool m_CanDead = false;

    [SerializeField]
    private double _health;
    private float _attackSpeedPercentage = 100;
    public double Health
    {
        get => _health;
        set
        {
            _health = value;
            if (_health > MaxHealth)
            {
                _health = MaxHealth;
            }

            if (_health <= 0)
            {
                ChangeState(MonsterState.dead);
                SpawnManager.spawnInstance.currentMonsterCount--;
                SpawnManager.spawnInstance.killedMonsterCount++;

              //  Debug.Log(SpawnManager.spawnInstance.currentMonsterCount);

                if (Managers.Stage.stageInfo.isStageWave)
                {
                    if (SpawnManager.spawnInstance.killedMonsterCount == Managers.Stage.spawnList[0].monsterCount)
                    {
                        Managers.Stage.SpawnNextWave = true;
                    }

                    if (Managers.Stage.stageInfo.isBossStage)
                    {
                        if (SpawnManager.spawnInstance.killedMonsterCount == Managers.Stage.stageInfo.maxMonsterCount - Managers.Stage.stageInfo.bossMonsterCount)
                        {
                            Managers.Stage.SpawnBoss = true;
                        }
                    }
                }

                Managers.Monsters.DeleteMonster(this);
                //transform.localScale = new Vector3(0, 0, 0);

                if (m_HpBar != null)
                {
                    m_HpBar.GetComponent<MonsterHpUI>().DeleteTarget();
                    Managers.Pool.GetPool("Slider").ReturnObject(m_HpBar);
                }
            }
        }
    }

    /// <JaeHyeon>   
    public List<TestMaterialController> m_TestMaterialControllers = new List<TestMaterialController>();
    float m_BurnSpeed = 0.8f;
    public bool m_isHit = false;

    public float knockbackDist;
    Vector3 m_originScale;
    bool m_isScalingUp = true;
    float m_ScaleTimer = 0.0f;
    public float m_ScaleValue = 1.5f;

    public bool m_isRimLight = false;
    /// </JaeHyeon>

    float currentTime = 0f;
    public Dictionary<KeyValuePair<Skill, string>, BuffDebuff> BuffDebuff { get; set; }
    public Dictionary<string, ContinuousDamage> ContinuousDamage { get; set; }
    public Dictionary<string, AbnormalStatus> AbnormalStatus { get; set; }
    public List<Barrier> Barrier { get; set; }
    public List<KeyValuePair<Skill, string>> BuffDebuffStrings { get; set; }
    public List<string> ContinuousDamageStrings { get; set; }
    public List<Barrier> BarrierOnDestroy { get; set; }
    public List<string> AbnormalStatusStrings { get; set; }

    public bool isDamaged = false;

    bool isStunned = false;

    // 활성화 될 때마다 실행됨
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        ResetMonster();

        // spawn shader value reset
        foreach (var child in m_TestMaterialControllers)
        {
            child.ResetValue();
        }

        StartCoroutine("CoSpawn");
    }

    public void Init()
    {
        player = GameManager.Instance.player.GetComponent<Player>();
        BuffDebuff = new Dictionary<KeyValuePair<Skill, string>, BuffDebuff>();
        ContinuousDamage = new Dictionary<string, ContinuousDamage>();
        AbnormalStatus = new Dictionary<string, AbnormalStatus>();
        Barrier = new List<Barrier>();
        BuffDebuffStrings = new List<KeyValuePair<Skill, string>>();
        ContinuousDamageStrings = new List<string>();
        BarrierOnDestroy = new List<Barrier>();
        AbnormalStatusStrings = new List<string>();


        //joohong
        m_canvas = GameObject.Find("HpCanvas");

        m_HpBar = Managers.Pool.GetPool("Slider").GetGameObject("UI/Slider");
        m_HpBar.transform.position = new Vector3(10000, 10000, 10000);
        m_HpBar.GetComponent<MonsterHpUI>().SetMonster(this);

        if (m_HpBar == null)
        {
            Debug.Log("문제발생");
        }
        //m_HpBar.gameObject.SetActive(false);



        // RimLight Shader Start
        foreach (var child in m_TestMaterialControllers)
        {
            if (Attribute == Attribute.Electricity)
            {
                child.RimOn();
                child.RimColor(Color.yellow);
            }
            else if (Attribute == Attribute.Water)
            {
                child.RimOn();
                child.RimColor(Color.cyan);
            }
            else if (Attribute == Attribute.Magnetic)
            {
                child.RimOn();
                child.RimColor(Color.magenta);
            }
            //else if (Attribute == Attribute.Void)
            //{
            //    child.RimOn();
            //    child.RimWhiteColor();          
            //}
            else
            {
                child.RimOff();
            }
        }

    }

    private void Start()
    {
        m_originScale = this.transform.localScale;

        if (this.gameObject.tag != "BossMonster")
            transform.localScale = new Vector3(0, 0, 0);

        monsterInfo = Managers.Data.GetMonserInfoByName(monsterName.ToString());

        MaxHealth = monsterInfo.Hp;
        Health = MaxHealth;
        Damage = monsterInfo.Atk;
        Defense = monsterInfo.Defense;
        AttackSpeed = monsterInfo.AttackSpeed;

        //float randomSpeed = Random.Range(monsterInfo.MoveSpeed - 0.5f, monsterInfo.MoveSpeed);
        //
        //if(randomSpeed <= 0)
        //{
        //    randomSpeed = 1;
        //}
        //monsterInfo.MoveSpeed = randomSpeed;

        m_battleAnalysisSystem = GameObject.Find("BattleAnalysisSystem").GetComponent<BattleAnalysisSystem>();


        m_WaterAura = Instantiate(m_WaterAuraPrefab, this.transform);
        m_WaterAura.SetActive(false);
        m_MagneticAura = Instantiate(m_MagneticAuraPrefab, this.transform);
        m_MagneticAura.SetActive(false);
        m_LightningAura = Instantiate(m_LightningAuraPrefab, this.transform);
        m_LightningAura.SetActive(false);

        if (gameObject.tag == "BossMonster")
        {
            BaseUIManager.instance.m_BossHpBarObj.SetActive(true);
            BossMonster_HpBar.Instance.m_bossMonster = this;
        }
    }

    public void OnDisable()
    {
        //if (m_HpBar != null)
        //{
        //    m_HpBar.SetActive(false);
        //}

    }

    void Update()
    {
        if(gameObject.CompareTag("BossMonster"))
        {
            if(m_elapsedTime <= 0.5)
            {
                transform.position = new Vector3(transform.position.x, 10 - m_elapsedTime * m_elapsedTime * 40, transform.position.z);
                m_elapsedTime += Time.deltaTime;
            }
            else
            {
                transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            }
        }
        if (Health >= 0)
        {
            foreach (var effects in BuffDebuff)
            {
                effects.Value.Update();
            }
            foreach (var effects in ContinuousDamage)
            {
                effects.Value.Update();
            }
            foreach (var effects in AbnormalStatus)
            {
                effects.Value.Update();
            }
            foreach (var effects in Barrier)
            {
                effects.Update();
            }
        }

        foreach (var name in BuffDebuffStrings)
        {
            BuffDebuff.Remove(name);
        }
        foreach (var name in ContinuousDamageStrings)
        {
            ContinuousDamage.Remove(name);
        }
        foreach (var name in AbnormalStatusStrings)
        {
            AbnormalStatus.Remove(name);
        }
        foreach (var name in BarrierOnDestroy)
        {
            Barrier.Remove(name);
        }

        BuffDebuffStrings.Clear();
        ContinuousDamageStrings.Clear();
        AbnormalStatusStrings.Clear();
        BarrierOnDestroy.Clear();

        if(monsterState != MonsterState.dead)
        {
            if(AbnormalStatus.ContainsKey("Flooding"))
            {
                m_WaterAura.gameObject.transform.localScale = new Vector3(1, 1, 1);
                m_WaterAura.SetActive(true);
            }
            else if(!AbnormalStatus.ContainsKey("Flooding"))
            {
                m_WaterAura.SetActive(false);
            }

            if(ContinuousDamage.ContainsKey("ElectricShock"))
            {
                m_LightningAura.gameObject.transform.localScale = new Vector3(1, 1, 1);
                m_LightningAura.SetActive(true);
            }
            else if(!ContinuousDamage.ContainsKey("ElectricShock"))
            {
                m_LightningAura.SetActive(false);
            }

            if(ContinuousDamage.ContainsKey("Tinnitus"))
            {
                m_MagneticAura.gameObject.transform.localScale = new Vector3(1, 1, 1);
                m_MagneticAura.SetActive(true);
            }
            else if(!ContinuousDamage.ContainsKey("Tinnitus"))
            {
                m_MagneticAura.SetActive(false);
            }
        }
        else if(monsterState == MonsterState.dead) 
        {
            m_WaterAura.SetActive(false);
            m_LightningAura.SetActive(false);
            m_MagneticAura.SetActive(false);
        }

        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        UpdateHitShader();

        switch (monsterState)
        {
            case MonsterState.spawn:
                UpdateBase();
                UpdateSpawn();
                break;
            case MonsterState.move:
                UpdateBase();
                UpdateMove();
                break;
            case MonsterState.attack:
                UpdateBase();
                UpdateAttack();
                break;
            case MonsterState.hit:
                UpdateBase();
                UpdateHit();
                break;
            case MonsterState.stun:
                UpdateBase();
                UpdateStun();
                break;
            case MonsterState.dead:
                UpdateDead();
                break;
        }
        anim.SetFloat("AttackSpeed", AttackSpeed * (1 + AttackSpeedPercentage / 100));
    }

    public void UpdateHitShader()
    {
        if (m_isHit)
        {
            if (m_TestMaterialControllers.Count > 0)
            {
                foreach (var child in m_TestMaterialControllers)
                {
                    child.HitUpdate();
                }

                var lastElement = m_TestMaterialControllers[m_TestMaterialControllers.Count - 1];

                //HitScale(lastElement.m_HitDuration * 0.5f);

                if (lastElement.m_HitTimer >= lastElement.m_HitDuration)
                {
                    m_isHit = false;
                    m_isScalingUp = true;

                    foreach (var child in m_TestMaterialControllers)
                    {
                        child.m_HitTimer = 0.0f;
                    }
                }
            }
            else
            m_isHit = false;
        }
    }

    void UpdateBase()
    {
        if (AbnormalStatus.ContainsKey("Stun") && monsterState != MonsterState.stun)
        {
            isStunned = true;
            anim.speed = 0f;
            lastState = monsterState;
            ChangeState(MonsterState.stun);
        }
        else if (!AbnormalStatus.ContainsKey("Stun") && isStunned)
        {
            anim.speed = 1;
            ChangeState(lastState);
            isStunned = false;
        }


        if(this.gameObject.tag != "BossMonster")
        {
            if (transform.localScale == new Vector3(0, 0, 0))
            {
                currentTime = 0f;
            }
            currentTime += Time.deltaTime;

            transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), m_originScale, currentTime);
        }

    }

    void HitScale(float durationTime)
    {
        Vector3 targetScale = Vector3.one * m_ScaleValue;
        m_ScaleTimer += Time.deltaTime;
        if (m_isScalingUp)
        {
            // 현재 스케일을 목표 스케일로 Lerp
            transform.localScale = Vector3.Lerp(m_originScale, targetScale, m_ScaleTimer / durationTime);

            // 목표 스케일에 도달하면
            if (m_ScaleTimer >= durationTime)
            {
                // 방향을 반대로 설정
                m_isScalingUp = false;
                m_ScaleTimer = 0f;
            }
        }
        else
        {
            // 현재 스케일을 원래 스케일로 Lerp
            transform.localScale = Vector3.Lerp(targetScale, m_originScale, m_ScaleTimer / durationTime);

            // 원래 스케일에 도달하면
            if (m_ScaleTimer >= durationTime)
            {
                // 방향을 반대로 설정 (루프할 경우 필요)
                m_ScaleTimer = 0f;
            }
        }
    }

    void UpdateSpawn()
    {
        LookAtPlayer();

    }

    void UpdateMove()
    {
        LookAtPlayer();
        this.gameObject.GetComponent<Rigidbody>().mass = 1;
        FollowPlayer();
        if (GetDistance(player.gameObject.transform.position, transform.position) < monsterInfo.AttackRange)
        {
            ChangeState(MonsterState.attack);
        }
    }

    public void FollowPlayer()
    {
        float fixedPercent = Mathf.Clamp(MoveSpeedPercentage, 50f, 200f);
        transform.position += LookAtPlayer() * monsterInfo.MoveSpeed * fixedPercent / 100 * Time.deltaTime;
    }

    void UpdateAttack()
    {
        LookAtPlayer();

        this.gameObject.GetComponent<Rigidbody>().mass = 10000f;

        if (GetDistance(player.gameObject.transform.position, transform.position) > monsterInfo.AttackRange)
        {
            anim.SetBool("isAttack", false);
            ChangeState(MonsterState.move);
        }
    }

    void UpdateHit()
    {
        KnockBack();
    }

    void UpdateStun()
    {
        //anim.speed = 0f;
    }

    void UpdateDead()
    {
        anim.speed = 1.0f;

        if (m_CanDead)
            StartCoroutine("CoDead");

        //Managers.Monsters.DeleteMonster(this.gameObject);
        this.GetComponent<CapsuleCollider>().enabled = false;
    }

    void ChangeState(MonsterState state)
    {
        monsterState = state;

        switch (monsterState)
        {
            case MonsterState.attack:
                anim.SetBool("isAttack", true);
                break;
            case MonsterState.hit:
                anim.SetBool("isHit", true);
                anim.SetBool("isAttack", false);
                break;
            case MonsterState.dead:
                anim.SetBool("isAttack", false);
                anim.SetBool("isDead", true);
                break;
        }
    }

    Vector3 LookAtPlayer()
    {
        if(player == null)
        {
           player = GameManager.Instance.player.GetComponent<Player>();
        }

        Vector3 followDirection = (player.gameObject.transform.position - transform.position).normalized;

        followDirection.y = 0f;

        transform.rotation = Quaternion.LookRotation(followDirection);

        return followDirection;
    }

    float GetDistance(Vector3 pos1, Vector3 pos2)
    {
        return Vector3.Distance(pos1, pos2);
    }

    public void KnockBack()
    {
        if (monsterState == MonsterState.hit)
        {
            Vector3 knockbackPos = transform.position + -LookAtPlayer() * knockbackDist;

            transform.position = Vector3.Lerp(transform.position, knockbackPos, 5 * Time.deltaTime * 3f);
        }
    }


    public double IsDamaged(Skill skill, double skillCoefficient, float additionalDamage, float additionalCriticalChance = 0f, double additionalCiriticalMultiplier = 0, float durabilityNegation = 0f)
    {
        if (Health <= 0)
            return default;

        if (m_battleAnalysisSystem == null)
        {
            m_battleAnalysisSystem = GameObject.Find("BattleAnalysisSystem").GetComponent<BattleAnalysisSystem>();
        }

        if (monsterState == MonsterState.stun)
        {
            isStunned = true;
        }
        else if (this.gameObject.tag != "BossMonster")
            ChangeState(MonsterState.hit);

        m_isHit = true;
        isDamaged = true;

        foreach (var child in m_TestMaterialControllers)
        {
            child.HitReset();
        }

        string damageTextColor = "DamageText";
        double finalDamage;
        float reactionAdditionalDamage = 100f;

        if (AbnormalStatus.ContainsKey("Stun") && skill.m_attribute != Attribute.Void)
        {
            reactionAdditionalDamage *= (player.ElementAmp + skill.m_elementAmp) / 100;
        }

        if(AbnormalStatus.ContainsKey("Flooding") && skill.m_attribute == Attribute.Electricity)
        {
            reactionAdditionalDamage *= (player.ElementAmp + skill.m_elementAmp) / 100;
            damageTextColor = "YellowDamageText";
            GameManager.Instance.synergyParticles[0].SetActive(true);
        }
        else if(ContinuousDamage.ContainsKey("ElectricShock") && skill.m_attribute == Attribute.Magnetic)
        {
            reactionAdditionalDamage *= (player.ElementAmp + skill.m_elementAmp) / 100;
            damageTextColor = "PurpleDamageText";
            GameManager.Instance.synergyParticles[2].SetActive(true);
        }
        else if(ContinuousDamage.ContainsKey("Tinnitus") && skill.m_attribute == Attribute.Water)
        {
            reactionAdditionalDamage *= (player.ElementAmp + skill.m_elementAmp) / 100;
            damageTextColor = "BlueDamageText";
            GameManager.Instance.synergyParticles[1].SetActive(true);
        }

        bool isCritical = false;
        if(Random.Range(0f, 100f) <= player.CriticalChance)
        {
            isCritical = true;
        }

        if (isCritical)
        {
            finalDamage = ((player.CriticalMultiplier + additionalCiriticalMultiplier) / 100f) * (player.Damage * player.DamageAmplification / 100) * (1 + player.DamagePercentage / 100) * Random.Range(1f, 1.5f) * (skillCoefficient / 100) * ((AdditionalDamage + additionalDamage) / 100) * (reactionAdditionalDamage / 100) - monsterInfo.Defense * (1 + DefensePercentage / 100) * (100 - durabilityNegation) / 100;
        }
        else
        {
            finalDamage = (player.Damage * player.DamageAmplification / 100) * (1 + player.DamagePercentage / 100) * Random.Range(1f, 1.5f) * (skillCoefficient / 100) * ((AdditionalDamage + additionalDamage) / 100) * (reactionAdditionalDamage / 100) - monsterInfo.Defense * (1 + DefensePercentage / 100) * (100 - durabilityNegation) / 100;
        }

        if (finalDamage <= 0)
            finalDamage = 0;


            if (Health <= finalDamage)
            {
                m_battleAnalysisSystem.KillCountUpdate();
                if (player.AbnormalStatus.ContainsKey("rage"))
                {
                    (player.AbnormalStatus["Rage"] as Rage).Activate();
                }
                else if (player.AbnormalStatus.ContainsKey("Frenzy"))
                {
                    (player.AbnormalStatus["Frenzy"] as Frenzy).Activate();
                }
            }

            Health -= finalDamage;

          //  damageText = Managers.Pool.GetPool(damageTextColor).GetGameObject("UI/" + damageTextColor, hud.transform.position);
         //   damageText.GetComponent<DamageText>().ShowDamageText(hud, finalDamage);
            StartCoroutine(CoDelayDamageFont(damageTextColor, finalDamage, isCritical));

        if (m_HpBar != null)
        {
            //if (!m_HpBar.activeSelf)
            //{
            //    m_HpBar.gameObject.SetActive(true);
            //}
            m_HpBar.GetComponent<MonsterHpUI>().ResetValue();
            m_HpBar.GetComponent<MonsterHpUI>().Dmg();
        }


        PlayParticleOnMonster(skill);

        m_battleAnalysisSystem.DamageDataUpdate(skill, finalDamage);

        return Health;
    }

    public double IsContinuosDamaged(Skill skill, double skillCoefficient)
    {
        if (Health <= 0)
            return default;

        if (m_battleAnalysisSystem == null)
        {
            m_battleAnalysisSystem = GameObject.Find("BattleAnalysisSystem").GetComponent<BattleAnalysisSystem>();
        }

        m_isHit = true;
        isDamaged = true;

        foreach (var child in m_TestMaterialControllers)
        {
            child.HitReset();
        }

        string damageTextColor = "DamageText";
        double finalDamage;
        float reactionAdditionalDamage = 100f;

        if (AbnormalStatus.ContainsKey("Stun") && skill.m_attribute != Attribute.Void)
        {
            reactionAdditionalDamage *= (player.ElementAmp + skill.m_elementAmp) / 100;
        }

        if (AbnormalStatus.ContainsKey("Flooding") && skill.m_attribute == Attribute.Electricity)
        {
            reactionAdditionalDamage *= (player.ElementAmp + skill.m_elementAmp) / 100;
            damageTextColor = "YellowDamageText";
            GameManager.Instance.synergyParticles[0].SetActive(true);
        }
        else if (ContinuousDamage.ContainsKey("ElectricShock") && skill.m_attribute == Attribute.Magnetic)
        {
            reactionAdditionalDamage *= (player.ElementAmp + skill.m_elementAmp) / 100;
            damageTextColor = "PurpleDamageText";
            GameManager.Instance.synergyParticles[2].SetActive(true);
        }
        else if (ContinuousDamage.ContainsKey("Tinnitus") && skill.m_attribute == Attribute.Water)
        {
            reactionAdditionalDamage *= (player.ElementAmp + skill.m_elementAmp) / 100;
            damageTextColor = "BlueDamageText";
            GameManager.Instance.synergyParticles[1].SetActive(true);
        }

        finalDamage = (player.Damage * player.DamageAmplification / 100) * (1 + player.DamagePercentage / 100) * Random.Range(1f, 1.5f) * (skillCoefficient / 100) * (AdditionalDamage  / 100) * (reactionAdditionalDamage / 100) - monsterInfo.Defense * (1 + DefensePercentage / 100) / 100;

        if (finalDamage <= 0)
            finalDamage = 0;

        if (Health <= finalDamage)
        {
            m_battleAnalysisSystem.KillCountUpdate();
            if (player.AbnormalStatus.ContainsKey("rage"))
            {
                (player.AbnormalStatus["Rage"] as Rage).Activate();
            }
            else if (player.AbnormalStatus.ContainsKey("Frenzy"))
            {
                (player.AbnormalStatus["Frenzy"] as Frenzy).Activate();
            }
        }

        Health -= finalDamage;
        StartCoroutine(CoDelayDamageFont(damageTextColor, finalDamage));

        if (m_HpBar != null)
        {
            //if (!m_HpBar.activeSelf)
            //{
            //    m_HpBar.gameObject.SetActive(true);
            //}
            m_HpBar.GetComponent<MonsterHpUI>().ResetValue();
            m_HpBar.GetComponent<MonsterHpUI>().Dmg();
        }

        m_battleAnalysisSystem.DamageDataUpdate(skill, finalDamage);

        return Health;
    }

    public double IsTrueDamaged(Skill skill, double damage)
    {
        if (m_battleAnalysisSystem == null)
        {
            m_battleAnalysisSystem = GameObject.Find("BattleAnalysisSystem").GetComponent<BattleAnalysisSystem>();
        }

        m_isHit = true;
        isDamaged = true;

        foreach (var child in m_TestMaterialControllers)
        {
            child.HitReset();
        }

        double finalDamage;
        finalDamage = damage * (AdditionalDamage / 100);

        if (finalDamage <= 0)
            finalDamage = 0;

        if (Health > 0)
        {
            if (Health <= finalDamage)
            {
                m_battleAnalysisSystem.KillCountUpdate();
                if (player.AbnormalStatus.ContainsKey("rage"))
                {
                    (player.AbnormalStatus["Rage"] as Rage).Activate();
                }
                else if (player.AbnormalStatus.ContainsKey("Frenzy"))
                {
                    (player.AbnormalStatus["Frenzy"] as Frenzy).Activate();
                }
            }

            Health -= finalDamage;
            StartCoroutine(CoDelayDamageFont("DamageText", finalDamage));
        }

        if (m_HpBar != null)
        {
            //if (!m_HpBar.activeSelf)
            //{
            //    m_HpBar.gameObject.SetActive(true);
            //}
            m_HpBar.GetComponent<MonsterHpUI>().ResetValue();
            m_HpBar.GetComponent<MonsterHpUI>().Dmg();
        }

        m_battleAnalysisSystem.DamageDataUpdate(skill, finalDamage);

        return Health;
    }

    void ResetMonster()
    {
        DefensePercentage = 0;
        DamagePercentage = 0;
        MoveSpeedPercentage = 0;
        AttackSpeedPercentage = 0;
        ChangeState(MonsterState.spawn);
        anim.SetBool("isDead", false);
        Health = MaxHealth;
        this.GetComponent<CapsuleCollider>().enabled = true;
    }

    void AnimEventSpawn() // Spawn End Timing
    {
        //anim.SetTrigger("isSpawn");
        int a = Random.Range(0, 2);

        float randomStartTime = Random.Range(0f, 1f);

        anim.Play("Walk", 0, randomStartTime);
        ChangeState(MonsterState.move);

        Managers.Monsters.AddMonster(this);

        //foreach (var child in m_TestMaterialControllers)
        //{
        //    child.ShadowSetActive(true);
        //}
    }

    void AnimEventAttack()
    {
        if (GetDistance(player.GetComponent<Transform>().position, transform.position) < monsterInfo.AttackRange)
        {
            double damage = Damage * (1 + DamagePercentage / 100) - player.Defense * (1 + player.DefensePercentage / 100);
            foreach (var bar in player.Barrier)
            {
                if (damage >= bar.m_remainingBarrier)
                {
                    damage -= bar.m_remainingBarrier;
                    bar.m_remainingBarrier = 0;
                }
                else
                {
                    bar.m_remainingBarrier -= damage;
                    damage = 0;
                    break;
                }
            }

            if (m_battleAnalysisSystem == null)
            {
                m_battleAnalysisSystem = GameObject.Find("BattleAnalysisSystem").GetComponent<BattleAnalysisSystem>();
            }

            player.Health -= damage;
            m_battleAnalysisSystem.DamagedUpdate(damage);

            player.GetComponent<Player>().playerHpBar.GetComponent<PlayerHpUI>().ResetValue();
        }
    }

    // 피격 판정 후 상태 처리 이벤트 함수
    void AnimEventHit()
    {
        anim.SetBool("isHit", false);
        if (monsterState != MonsterState.dead)
        {
            if (GetDistance(player.gameObject.transform.position, transform.position) < monsterInfo.AttackRange)
            {
                ChangeState(MonsterState.attack);
            }
            else
            {
                ChangeState(MonsterState.move);
            }
        }
        isDamaged = false;
    }

    // 몬스터 삭제 이벤트 함수
    void AnimEventDead()
    {
        Managers.Stage.deadMonsterCount++;
        EventManager.MonsterKilled();

        player.playerGold += monsterInfo.Gold;
        
        // MedKit Create
        int randomKit = Random.Range(0, 10);
        if (randomKit <= 1)
        {
            GameObject MedKit = Managers.Pool.GetPool("MedKit").GetGameObject("MedKit");
            MedKit.transform.position = this.transform.position;

            TutorialManager.MedKitStart(0);
        }

        m_CanDead = true;
    }

    // Spawn 연출
    IEnumerator CoSpawn()
    {
        float startTime = Time.time;

        // 2초 동안 실행
        while (Time.time - startTime < 2.0f)
        {
            foreach (var child in m_TestMaterialControllers)
            {
                child.SpawnUpdate(m_BurnSpeed * Time.deltaTime);
            }

            // 다음 프레임까지 대기
            yield return null;
        }

        foreach (var child in m_TestMaterialControllers)
        {
            child.BurnSetActive(true);
        }

        yield break;
    }

    IEnumerator CoDelayDamageFont(string _damageTextColor, double _finalDamage, bool isCritical = false)
    {
        yield return new WaitForSeconds(0.1f);

        if (TutorialManager.m_SynergyIndex <= 0 && _damageTextColor != "DamageText" ) 
        {
            yield return new WaitForSeconds(0.35f);
            TutorialManager.SynergyTextStart(0);
        }

        if(_finalDamage == 0)
        {
            damageText = Managers.Pool.GetPool("DamageText").GetGameObject("UI/DamageText", hud.transform.position);

            damageText.GetComponent<DamageText>().ShowDamageText(hud, "Block");
        }
        else
        {
            damageText = Managers.Pool.GetPool(_damageTextColor).GetGameObject("UI/" + _damageTextColor, hud.transform.position);
            if (isCritical)
            {
                damageText.transform.GetChild(0).gameObject.SetActive(true);
                damageText.GetComponent<DamageText>().transform.localScale *= 2.0f;
            }
            else
                damageText.transform.GetChild(0).gameObject.SetActive(false);
            damageText.GetComponent<DamageText>().ShowDamageText(hud, Utility.ToCurrencyString(_finalDamage).ToString());
        }
        yield break;
    }

    // Dead 연출
    IEnumerator CoDead()
    {
        m_CanDead = false;
        yield return new WaitForSeconds(1.0f);

        EventManager.MonsterKilled(); // Quest


        if (SpawnManager.spawnInstance.killedMonsterCount >= Managers.Stage.stageInfo.maxMonsterCount && GameTimer.Instance.m_TimeRemaining > 0 && SpawnManager.spawnInstance.isAllSpawned)
        {
            Managers.Pool.GetPool(this.gameObject.name).ReturnObject(this.gameObject);
            GameManager.Instance.canvasSetting.contentsCanvas[4].GetComponent<PopUpList>().StageClearedUI.SetActive(true);
            GameManager.Instance.canvasSetting.contentsCanvas[4].GetComponent<PopUpList>().StageClearedUI.GetComponent<StageFailedUI>().ShowAndBounce(Screen.height * 0.5f, 1.0f);
            GameManager.Instance.isGameStart = false;

            Invoke("StageClear", 3f);
            SpawnManager.spawnInstance.isAllSpawned = false;
            Debug.Log("몬스터 다 죽임");
        }

        float endTime = Time.time;
        // 2초 동안 실행
        while (Time.time - endTime < 2.0f)
        {
            foreach (var child in m_TestMaterialControllers)
            {
                child.DeadUpdate(m_BurnSpeed * Time.deltaTime);
            }

            // 다음 프레임까지 대기
            yield return null;
        }

        Managers.Pool.GetPool(this.gameObject.name).ReturnObject(this.gameObject);

        //if (SpawnManager.spawnInstance.killedMonsterCount >= Managers.Stage.stageInfo.maxMonsterCount && GameTimer.Instance.m_TimeRemaining > 0 && SpawnManager.spawnInstance.isAllSpawned)
        //{
        //    StageManager.CheckStageCleared();
        //    SpawnManager.spawnInstance.isAllSpawned = false;
        //    Debug.Log("몬스터 다 죽임");
        //}

        yield break;
    }

    public void StageClear()
    {
        GameManager.Instance.canvasSetting.contentsCanvas[4].GetComponent<PopUpList>().StageClearedUI.SetActive(false);
        StageManager.CheckStageCleared();
    }

    public void PlayParticleOnMonster(Skill skill)
    {
        if (skill.m_attribute == Attribute.Water)
        {
            GameObject go = Managers.Pool.GetPool("WaterHit").GetGameObject("Particle/WaterHit");
            go.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z);
            go.GetComponent<ParticleSystem>().Play();
        }
        else if (skill.m_attribute == Attribute.Idle)
        {
            GameObject go = Managers.Pool.GetPool("Electro hit").GetGameObject("Particle/Electro hit");
            go.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z);
            go.GetComponent<ParticleSystem>().Play();
        }
        else if(skill.m_attribute == Attribute.Magnetic)
        {
            GameObject go = Managers.Pool.GetPool("Magnetic").GetGameObject("Particle/Magnetic");
            go.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z);
            go.GetComponent<ParticleSystem>().Play();
        }
        else if(skill.m_attribute == Attribute.Electricity)
        {
            GameObject go = Managers.Pool.GetPool("LightningHit").GetGameObject("Particle/LightningHit");
            go.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z);
            go.GetComponent<ParticleSystem>().Play();
        }
    }
}