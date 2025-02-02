using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class Player : MonoBehaviour, IPlayerable
{
    public EventHandler playerAppearanceEnd;
    public EventHandler playerJump;
    public EventHandler emptySkillAlert;
    // 애니메이션
    public Animator m_animator;

    private GameObject m_canvas;

    // 행동 트리
    private BTSelectorNode m_root;
    private bool m_isAttacking = false;
    bool m_isPlayerAppearing = false;
    // 스킬 관련 데이터
    public ActionLogicManager m_logicManager;
    private float m_range;
    public GameObject m_target;
    private Skill m_currentSkill;
    private bool m_useSkill;

    public double initDamage = 100;
    public double initHealth = 1000;
    public double initDefense = 10;

    public Skill m_idleAttackSkill;
    // 플레이어 데이터
    [SerializeField] private readonly float m_rotationSpeed = 10.0f;
    public double MaxHealth { get; set; } = 1000;
    public double HealthPerSecond { get; set; } = 10;
    public double Defense { get; set; } = 10;
    public float DefensePercentage { get; set; } = 0;
    public double Damage { get; set; } = 100;
    public float DamagePercentage { get; set; } = 0;
    public float CriticalChance { get; set; } = 25;
    public double CriticalMultiplier { get; set; } = 150;
    public float MoveSpeed { get; set; } = 10;
    public float MoveSpeedPercentage { get; set;} = 0;    
    public float AttackSpeed { get; set; } = 1;
    public float AttackSpeedPercentage
    {
        get
        {
            return _attackSpeedPercentage;
        }
        set
        {
            _attackSpeedPercentage = value;
            if (_attackSpeedPercentage < 0)
            {
                _attackSpeedPercentage = 0;
            }
            SetAttackSpeed();
        }
    }
    public double MaxMana { get; set; } = 100;
    public double DamageAmplification { get; set; } = 100;
    public float ManaPerSecond
    {
        get
        {
            return _manaPerSecond;
        }
        set
        {
            _manaPerSecond = value;
        }
    }
    public double ManaAmplification { get; set; } = 1;
    public float ElementAmp { get; set; } = 100f;

    // 상태효과 변수들
    public Dictionary<KeyValuePair<Skill, string>, BuffDebuff> BuffDebuff { get; set; }
    public Dictionary<string, ContinuousDamage> ContinuousDamage { get; set; }
    public Dictionary<string, AbnormalStatus> AbnormalStatus { get; set; }
    public List<Barrier> Barrier { get; set; }
    public List<KeyValuePair<Skill, string>> BuffDebuffStrings { get; set; }
    public List<string> ContinuousDamageStrings { get; set; }
    public List<Barrier> BarrierOnDestroy { get; set; }
    public List<string> AbnormalStatusStrings { get; set; }

    //
    [SerializeField]
    private double _health;
    [SerializeField]
    private double _mana;
    private float _attackSpeedPercentage = 0f;
    [SerializeField]
    [Tooltip("초당 마나 회복량")]
    float _manaPerSecond;

    public double Health
    {
        get => _health;
        set
        {
            _health = value;

            if (playerHpBar != null)
            {
                playerHpBar.GetComponent<PlayerHpUI>().ResetValue();
            }

            if (_health > MaxHealth)
            {
                _health = MaxHealth;
            }

            if (_health < 0)
            {
                // ToDo :: 테스트를 위해 죽는건 막아놓음.
               // StageManager.CheckStageFailed();
            }
        }
    }
    public CameraMoving camera;

    public ManaGaugeUI manaGauge;

    public double Mana
    {
        get => _mana;
        set
        {
            _mana = value;
            if (_mana > MaxMana)
            {
                _mana = MaxMana;
            }
            else if (_mana < 0)
            {
                _mana = 0;
            }
        }
    }

    //이펙트
    public GameObject m_actionSlot;
    public float playerGold;

    // 타이머
    private float m_elapsedTime;

    // 평타 Slash 이펙트
    public ParticleSystem m_IdleAttackPrefab;
    protected ParticleSystem m_IdleAttackParticle;

    // 뒤집힌 평타 Slash 이펙트
    public ParticleSystem m_IdleAttackPrefabBackward;
    protected ParticleSystem m_IdleAttackParticleBackward;

    // 평타 터지는 이펙트
    public ParticleSystem m_IdleAttackOnMonsterPrefab;
    protected ParticleSystem m_IdleAttackOnMonster;

    // 평타 사운드
    public string m_attackSound;

    public List<ParticleSystem> m_particles;

    public GameObject playerHpBar;

    public bool m_IsPlayerDead = false;


    void Awake()
    {
        MaxHealth = Managers.Data.playerHealth;
        Health = MaxHealth;
        Mana = MaxMana;

        m_animator = gameObject.GetComponent<Animator>();

        GameManager.Instance.PlayerAppearance += new EventHandler(PlayerAppear);
        //m_particleObject = gameObject.transform.GetChild(4).GetChild(2);
    }

    void PlayerAppear(object sender, EventArgs e)
    {
        StartCoroutine(PlayerAppearing());
    }

    IEnumerator PlayerAppearing()
    {
        float elapsedTime = 0;
        yield return new WaitForSeconds(1);
        while (transform.position.y < 0)
        {
            transform.position += new Vector3(0, Time.deltaTime * 32, 0);
            yield return null;
        }
        m_animator.SetBool("PlayerJump", true);
        playerJump(this, EventArgs.Empty);
        while (elapsedTime <= 0.5f)
        {
            transform.position = new Vector3(0, 4 - 16 * (elapsedTime - 0.5f) * (elapsedTime - 0.5f), 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
        while (elapsedTime < 0.75f)
        {
            transform.position = new Vector3(0, 4 - 64 * (elapsedTime - 0.5f) * (elapsedTime - 0.5f), 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(0, 0, 0);
        m_animator.SetBool("PlayerJump", false);

        yield return new WaitForSeconds(1f);
        playerAppearanceEnd(this, EventArgs.Empty);

//        TutorialManager.m_TutorialIndex = 15;
        TutorialManager.TutorialStart(0);
        TutorialManager.TutorialStart(9);
        TutorialManager.TutorialStart(15);
    }

    public void OnDisable()
    {
        GameObject.Destroy(playerHpBar);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_logicManager = GameObject.FindGameObjectWithTag("ActionLogicManager").GetComponent<ActionLogicManager>();
        m_actionSlot = GameObject.FindGameObjectWithTag("ActionSlot");
        m_currentSkill = m_logicManager[m_logicManager.CurrentIndex];
        //m_logicManager[m_logicManager.CurrentIndex].gameObject.SetActive(true);
        m_range = m_currentSkill.m_skillRange;
        m_target = m_currentSkill.m_target;
        camera = GameObject.FindGameObjectWithTag("CameraShake").GetComponent<CameraMoving>();
        camera.player = this.transform;
        BuffDebuff = new Dictionary<KeyValuePair<Skill, string>, BuffDebuff>();
        ContinuousDamage = new Dictionary<string, ContinuousDamage>();
        AbnormalStatus = new Dictionary<string, AbnormalStatus>();
        Barrier = new List<Barrier>();
        m_canvas = GameObject.Find("HpCanvas");
        BuffDebuffStrings = new List<KeyValuePair<Skill, string>>();
        ContinuousDamageStrings = new List<string>();
        BarrierOnDestroy = new List<Barrier>();
        AbnormalStatusStrings = new List<string>();

        Damage = Managers.Data.playerDamage;
        Defense = Managers.Data.playerDefense;
        MaxHealth = Managers.Data.playerHealth;
        Health = MaxHealth;

        if (m_IdleAttackPrefab)
        {
            m_IdleAttackParticle = Instantiate(m_IdleAttackPrefab, this.gameObject.transform);
            m_IdleAttackParticle.gameObject.SetActive(false);
        }

        if (m_IdleAttackPrefabBackward)
        {
            m_IdleAttackParticleBackward = Instantiate(m_IdleAttackPrefabBackward, this.gameObject.transform);
            m_IdleAttackParticleBackward.gameObject.SetActive(false);
        }


        for (int i = 0; i < 20; i++)
        {
            if (m_IdleAttackOnMonsterPrefab)
            {
                m_IdleAttackOnMonster = Instantiate(m_IdleAttackOnMonsterPrefab);
                m_IdleAttackOnMonster.gameObject.SetActive(false);
                m_particles.Add(m_IdleAttackOnMonster);
            }
        }

        //if (playerHpBarPrefab)
        //{
        //    playerHpBar = Instantiate(playerHpBarPrefab, Vector3.zero, Quaternion.identity, m_canvas.transform);
        //}

        m_root = new BTSelectorNode();
        BTSequenceNode playerDeadSequence = new BTSequenceNode();
        BTSequenceNode playerWaitSequence = new BTSequenceNode();
        BTSelectorNode playerActionSelector = new BTSelectorNode();
        BTSequenceNode skillCheckSequence = new BTSequenceNode();
        BTSequenceNode distanceCheckSequence = new BTSequenceNode();
        BTConditionNode isPlayerAttacking = new BTConditionNode(IsPlayerAttacking);
        BTConditionNode isPlayerHpZero = new BTConditionNode(IsPlayerHpZero);
        BTConditionNode isTargetNotExist = new BTConditionNode(IsTargetNotExist);
        BTConditionNode isSkillExist = new BTConditionNode(IsSkillExist);
        BTConditionNode isManaExist = new BTConditionNode(IsManaExist);
        BTConditionNode isMonsterInRange = new BTConditionNode(IsMonsterInRange);
        BTActionNode playerDeadAction = new BTActionNode(PlayerDead);
        BTActionNode playerWaitAction = new BTActionNode(PlayerWait);
        BTActionNode playerMoveAction = new BTActionNode(PlayerMove);
        BTActionNode actionCheck = new BTActionNode(ActionCheck);
        BTActionNode attackAction = new BTActionNode(UseSkill);

        m_root.AddChild(playerDeadSequence);
        m_root.AddChild(isPlayerAttacking);
        m_root.AddChild(playerWaitSequence);
        m_root.AddChild(playerActionSelector);
        m_root.AddChild(playerMoveAction);
        playerDeadSequence.AddChild(isPlayerHpZero);
        playerDeadSequence.AddChild(playerDeadAction);
        playerWaitSequence.AddChild(isTargetNotExist);
        playerActionSelector.AddChild(skillCheckSequence);
        playerActionSelector.AddChild(distanceCheckSequence);
        playerWaitSequence.AddChild(playerWaitAction);
        skillCheckSequence.AddChild(isSkillExist);
        skillCheckSequence.AddChild(isManaExist);
        skillCheckSequence.AddChild(actionCheck);
        distanceCheckSequence.AddChild(isMonsterInRange);
        distanceCheckSequence.AddChild(attackAction);
    }

    // Update is called once per frame
    void Update()
    {

        ManaGaugeUI.Instance.m_Mana = (float)Mana;

        m_elapsedTime += Time.deltaTime;
        if (m_elapsedTime >= 1)
        {
            Health += HealthPerSecond;
            Mana += ManaPerSecond * ManaAmplification;
            m_elapsedTime = 0;
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

        m_root.Evaluate();
    }

    public void KillPlayer()
    {
        Health = -10;
    }
    bool IsPlayerAttacking()
    {
        return m_isAttacking;
    }

    /// <summary>
    /// Camera Anim Event
    /// </summary>
    public void AnimEventShake(float startDelay, float duration, float magnitudePos, float magnitudeRot)
    {
        camera.AnimEventShake(startDelay, duration, magnitudePos, magnitudeRot);
    }

    public void AnimEventZoomIn()
    {
        camera.AnimEventZoomIn();
    }

    public void AnimEventZoomOut(float duration)
    {
        camera.AnimEventZoomOut(duration);
    }


    public void AnimEventSlowMotion()
    {
        SlowMotionClass.instance.DoSlowMotion();
    }

    bool IsPlayerHpZero()
    {
        if (Health <= 0)
        {
            return true;
        }
        return false;
    }

    bool IsTargetNotExist()
    {
        if (Managers.Monsters.GetMonsterPopulation() == 0)
        {
            return true;
        }
        return false;
    }
    bool IsMonsterInRange()
    {

        float distance;
        if (!m_useSkill) // 평타
        {
            m_target = Managers.Monsters.GetNearestMonster(transform).gameObject;
            distance = Vector3.Distance(transform.position, m_target.transform.position);
            if (distance <= 2)
            {
                return true;
            }
        }
        else // 스킬
        {
            //if(m_currentSkill.m_skillType == SkillType.BuffSkill) { return true; }
            m_target = m_currentSkill.GetTarget();
            float skillRange = m_currentSkill.m_skillRange;
            distance = Vector3.Distance(transform.position, m_target.transform.position);
            if (skillRange >= distance)
            {
                return true;
            }
        }

        return false;
    }

    public void PlayAttackEffect()
    {
        if (m_IdleAttackParticle)
        {
            //m_particle.transform.rotation = Quaternion.identity;
            m_IdleAttackParticle.transform.rotation = this.transform.rotation;
            m_IdleAttackParticle.gameObject.SetActive(true);
            m_IdleAttackParticle.Play();
        }
    }

    public void PlayAttackEffectBackward()
    {
        if (m_IdleAttackParticleBackward)
        {
            m_IdleAttackParticleBackward.gameObject.SetActive(true);
            m_IdleAttackParticleBackward.Play();
        }
    }

    public void PlayAttackSound()
    {
        SoundManager.Instance.PlaySFX(m_attackSound);
    }

    public void PlayParticeOnMonster(Vector3 pos)
    {
        foreach (var particle in m_particles)
        {
            if (!particle.gameObject.activeSelf)
            {
                particle.transform.position = pos;
                particle.gameObject.SetActive(true);
                particle.Play();
                return;
            }
        }

        //m_IdleAttackOnMonster.transform.position = pos;
        //m_IdleAttackOnMonster.gameObject.SetActive(true);
        //m_IdleAttackOnMonster.Play();
    }

    bool IsSkillExist()
    {
        if (m_currentSkill != null)
            return true;
        return false;
    }

    bool IsManaExist()
    {
        m_useSkill = false;
        if (m_currentSkill.m_manaCost < Mana)
            return true;
        return false;
    }
    public void PlayEffect(float stopTime = 0f)
    {
        m_currentSkill.PlayEffect(stopTime);
    }

    public void PlaySecondEffect(float stopTime = 0f)
    {
        m_currentSkill.PlaySecondEffect(stopTime);
    }

    public void PlaySound(string s)
    {
        SoundManager.Instance.PlaySFX(s);
    }

    public void SkillEnd()
    {
        m_actionSlot.GetComponent<RotateActionLogic>().RotateImage();
        //m_isAttacking = false;
        m_logicManager.NextSkill();
        if (m_currentSkill is EmptySkill)
        {
            m_animator.SetTrigger("EmptySkillEnd");
        }
        else
        {
            m_animator.SetBool(m_currentSkill.m_skillName, false);
        }
        //m_currentSkill.gameObject.SetActive(false);

        m_currentSkill = m_logicManager[m_logicManager.CurrentIndex];
        m_range = m_currentSkill.m_skillRange;
        m_target = null;

        // 스킬 이미지 돌리는 코드
        //if (!m_actionSlot.GetComponent<RotateActionLogic>().isRotating)
        //{
        //    m_actionSlot.GetComponent<RotateActionLogic>().RotateImage();
        //}
    }

    public void EmptySlotAlert()
    {
        emptySkillAlert(this, EventArgs.Empty);
    }

    public void SkillStart()
    {
        m_currentSkill.SkillStart();
    }

    public void Activate()
    {
        m_currentSkill.Activate();
    }

    public void IdleAttackManaCheck()
    {
        Mana += 5;
        if (Mana > m_currentSkill.m_manaCost)
        {
            //m_isAttacking = false;
            m_animator.SetBool("IdleAttack", false);
        }
    }

    public void IdleAttackEnd()
    {
        //m_isAttacking = false;
        m_animator.SetBool("IdleAttack", false);
    }

    public void IdleAttackHit()
    {
        if (Managers.Monsters.GetMonsterInCircularSector(transform, 180, 3) != null)
        {
            foreach (var monster in Managers.Monsters.GetMonsterInCircularSector(transform, 180, 3))
            {
                monster.GetComponent<MonsterScript>().IsDamaged(m_logicManager.m_idleAttack, 100f, 0f);
            }
        }
    }

    BTNodeState PlayerDead()
    {
        if(!m_IsPlayerDead)
        {
            m_animator.SetTrigger("PlayerDead");
            m_IsPlayerDead = true;
        }

        return BTNodeState.Success;
    }

    BTNodeState PlayerWait()
    {
        return BTNodeState.Success;
    }

    BTNodeState PlayerMove()
    {
        m_animator.SetBool("PlayerMove", true);
        Vector3 dir = m_target.transform.position - transform.position;
        dir.y = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir.normalized), Time.deltaTime * m_rotationSpeed);

        transform.position = Vector3.MoveTowards(transform.position, m_target.transform.position, Time.deltaTime * MoveSpeed * (1 + MoveSpeedPercentage / 100));

        return BTNodeState.Success;
    }
    BTNodeState ActionCheck()
    {
        m_useSkill = true;
        return BTNodeState.Failure;
    }
    BTNodeState UseSkill()
    {
        m_isAttacking = true;


        m_animator.SetBool("PlayerMove", false);
        if (m_target != this.gameObject)
        {
            Vector3 lookDirection = m_target.transform.position - transform.position;
            lookDirection.y = 0;
            transform.rotation = Quaternion.LookRotation(lookDirection.normalized);
        }
        if (m_useSkill)
        {
            if (m_currentSkill is EmptySkill)
            {
                m_animator.SetTrigger("EmptySkillStart");
            }
            else
            {
                m_animator.SetBool(m_currentSkill.m_skillName, true);
            }
            Mana -= m_currentSkill.m_manaCost;
        }
        else
        {
            m_animator.SetBool("IdleAttack", true);
        }
        return BTNodeState.Success;
    }

    public void TestHandleStageFailed() // animation event
    {
        Debug.Log("플레이어 사망");
        GameManager.Instance.ShowGameOverPopUp();
        Invoke("TestDelayFailed", 2f);
    }

    public void TestDelayFailed()
    {
        m_animator.SetBool("isDead", false);
        Health = Managers.Data.playerHealth;
        GameManager.Instance.HandleStageFailed();
    }

    public void MoveStraight(float speed)
    {
        transform.position += transform.TransformDirection(Vector3.forward) * Time.deltaTime * speed;
    }

    public void AbilityUp(string ability)
    {
        var stat = GetType().GetTypeInfo().GetDeclaredField(ability);
        stat.SetValue(this, (double)stat.GetValue(this) * 2);
    }

    public double GetAbility(string ability)
    {
        //var stat = GetType().GetTypeInfo().GetDeclaredField(ability);
        //return (double)stat.GetValue(this);

        return 0f;
    }

    public float GetCurrentSkillAnimDuration()
    {
        // 현재 애니메이터 상태를 가져옴
        AnimatorStateInfo currentState = m_animator.GetCurrentAnimatorStateInfo(0);

        // Idle 상태가 아닌지 확인 (필요한 애니메이션 상태를 추가로 확인할 수 있음)
        if (m_isAttacking && !currentState.IsName("Idle"))
        {
            // 현재 재생 중인 애니메이션의 클립 정보를 가져옴
            AnimatorClipInfo[] clipInfo = m_animator.GetCurrentAnimatorClipInfo(0);

            if (clipInfo.Length > 0)
            {
                // 현재 재생 중인 클립의 길이를 반환
                return clipInfo[0].clip.length;
            }
        }

        // Idle 상태이거나 재생 중인 클립이 없을 때 0 반환
        return 0f;
    }

    void SetAttackSpeed()
    {
        float attackSpeed = AttackSpeed * (1 + AttackSpeedPercentage / 100);
        m_animator.SetFloat("AttackSpeed", attackSpeed);
    }

    public void AttackFalse()
    {
        m_isAttacking = false;
    }
}
