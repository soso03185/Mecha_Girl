using DG.Tweening;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using static QuestManager;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// ����Ʈ ������ ���Ұ� ���õ� �Լ��� ��ũ��Ʈ
/// </summary>
public class QuestManager : Singleton<QuestManager>
{
    public Quest m_activeQuest; // ���� ���� ����Ʈ ����Ʈ
    [SerializeField] TextMeshProUGUI m_QuestCountText;
    [SerializeField] TextMeshProUGUI m_QuestTypeText;
    [SerializeField] Transform m_QuestButton;
    [SerializeField] GameObject m_ClearEffect;
    [SerializeField] ParticleSystem m_RewardParticle_SP;
    [SerializeField] ParticleSystem m_RewardParticle_AP;

    [SerializeField] GameObject m_RewardIcon_SP;
    [SerializeField] GameObject m_RewardIcon_AP;
    [SerializeField] TextMeshProUGUI m_RewardAmount_SP;
    [SerializeField] TextMeshProUGUI m_RewardAmount_AP;

    [SerializeField] GameObject m_QuestClearImage;

    private static QuestManager instance;
    private Vector3 m_originButtonScale;

    Sequence m_scaleButtonClick;
    Sequence m_scaleClearPassive;

    public override void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // �̹� �����ϴ� ��� ���� ������ ������Ʈ ����
        }
    }

    void OnEnable()
    {
        EventManager.OnMonsterKilled += HandleMonsterKilled; // �̺�Ʈ ����
        EventManager.OnStageClear += HandleStageCleared; // �̺�Ʈ ����
        EventManager.OnSkillEquiped += HandleEquiped; // �̺�Ʈ ����
        EventManager.OnOpenTab += HandleOpenTab; // �̺�Ʈ ����
    }

    void OnDisable()
    {
        EventManager.OnMonsterKilled -= HandleMonsterKilled; // �̺�Ʈ ����
        EventManager.OnStageClear -= HandleStageCleared; // �̺�Ʈ ����
        EventManager.OnSkillEquiped -= HandleEquiped; // �̺�Ʈ ����
        EventManager.OnOpenTab -= HandleOpenTab; // �̺�Ʈ ����
    }

    void Start()
    {
        m_originButtonScale = m_QuestButton.localScale;
        AddQuest(QuestData.Instance.m_quests[QuestData.Instance.m_questIndex]);
    }

    public void RefreshText()
    {
        if (m_activeQuest.objectives.questType == "Kill_Monster")
        {
            m_activeQuest.objectives.count = QuestData.Instance.m_monsterKillCount;
        }
        else if (m_activeQuest.objectives.questType == "Stage_Clear")
        {
            m_activeQuest.objectives.count = Managers.Data.currentStageLevel;
        }

        m_QuestCountText.text = $"{m_activeQuest.objectives.count} / {m_activeQuest.objectives.maxCount}";
        CompleteButtonTween();
    }

    public bool QuestNullCheck()
    {
        if (m_activeQuest == null)
        {
            Debug.LogError($"�������� ����Ʈ�� �����ϴ�. !");
            return true;
        }
        return false;
    }

    public void HandleMonsterKilled()
    {
        if (QuestNullCheck()) return;

        if (m_activeQuest.objectives.questType == "Kill_Monster")
        {
            QuestData.Instance.m_monsterKillCount++;
            m_activeQuest.objectives.count = QuestData.Instance.m_monsterKillCount;
            RefreshText();
        }
    }

    public void HandleStageCleared()
    {
        if (QuestNullCheck()) return;

        if (m_activeQuest.objectives.questType == "Stage_Clear")
        {
            m_activeQuest.objectives.count = Managers.Data.currentStageLevel + 1;
            RefreshText();
        }
    }

    public void HandleOpenTab(string _tabName)
    {
        if (QuestNullCheck()) return;

        foreach (var quest in QuestData.Instance.m_quests)
        {
            if (quest.objectives.questType == "Open_Tab" && quest.objectives.questItemName == _tabName)
                quest.objectives.count++;
        }

        if (m_activeQuest.objectives.questType == "Open_Tab" &&
            m_activeQuest.objectives.questItemName == _tabName)
        {
            m_activeQuest.objectives.count++;

            if (m_activeQuest.objectives.count >= m_activeQuest.objectives.maxCount)
            {
                m_activeQuest.objectives.count = Mathf.Min(m_activeQuest.objectives.count, m_activeQuest.objectives.maxCount);
            }

            RefreshText();
        }
    }

    public void HandleEquiped(string _itemName)
    {
        if (QuestNullCheck()) return;

        foreach (var quest in QuestData.Instance.m_quests)
        {
            if (quest.objectives.questType == "Equip_Skill" && quest.objectives.questItemName == _itemName)
                quest.objectives.count++;
        }

        if (m_activeQuest.objectives.questType == "Equip_Skill" &&
            m_activeQuest.objectives.questItemName == _itemName)
        {
            m_activeQuest.objectives.count++;

            if (m_activeQuest.objectives.count >= m_activeQuest.objectives.maxCount)
            {
                m_activeQuest.objectives.count = Mathf.Min(m_activeQuest.objectives.count, m_activeQuest.objectives.maxCount);
            }

            RefreshText();
        }
    }

    // ����Ʈ �Ϸ� ���θ� üũ�ϴ� �޼ҵ�
    public void CheckQuestCompletion()
    {
        if (m_activeQuest.objectives.count >= m_activeQuest.objectives.maxCount)
        {
            m_activeQuest.objectives.isCompleted = true;
        }

        if (m_activeQuest.CheckIfComplete())
        {
            m_activeQuest.isCompleted = true;
            m_QuestButton.localScale = new Vector3(1, 1, 1);

            Debug.Log($"����Ʈ �Ϸ��: {m_activeQuest.questName}");
            GiveRewards(m_activeQuest);

            m_QuestClearImage.SetActive(false);

            if (m_ClearEffect)
            {
                m_ClearEffect.SetActive(true);
            }

            if (QuestData.Instance.m_questIndex < QuestData.Instance.m_quests.Count - 1)
            {
                AddQuest(QuestData.Instance.m_quests[++QuestData.Instance.m_questIndex]);
                FirestoreManager.Instance.SavePlayerField("NowQuest", QuestData.Instance.m_questIndex);
            }
            else
            {
                Debug.LogError("����Ʈ ��� �Ϸ� !");
            }

            TutorialManager.NextStepButton();
            TutorialManager.TutorialStart(10);
        }

        if (m_scaleButtonClick != null) m_scaleButtonClick.Kill();

        m_scaleButtonClick = DOTween.Sequence()
               .Append(m_QuestButton.DOScale(0.9f, 0.08f).SetEase(Ease.InOutSine)) // �ε巯�� Ease ����
               .Append(m_QuestButton.DOScale(m_originButtonScale, 0.15f))  // ���� ũ��� �ǵ�����
               .SetUpdate(true).OnComplete(() => m_QuestButton.DOKill());  // Ÿ�ӽ����Ͽ� ������� ������Ʈ
    }

    public void CompleteButtonTween()
    {
        if (m_activeQuest.objectives.count >= m_activeQuest.objectives.maxCount)
        {
            m_QuestClearImage.SetActive(true);

            if (m_scaleClearPassive != null) m_scaleClearPassive.Kill();

            m_scaleClearPassive = DOTween.Sequence()
                .Append(m_QuestButton.DOScale(1.1f, 0.35f).SetEase(Ease.InOutSine)) // 1.1��� Ȯ��
                .Append(m_QuestButton.DOScale(1f, 0.35f).SetEase(Ease.InOutSine))   // ���� ũ��� ���
                .SetLoops(-1, LoopType.Restart) // ���� �ݺ�
                .SetUpdate(true);               // Ÿ�ӽ����� ����
        }
    }

    // ����Ʈ �߰� �޼ҵ�
    public void AddQuest(Quest newQuest)
    {
        if (QuestData.Instance.m_quests.Contains(newQuest))
        {
            m_activeQuest = newQuest;
            m_QuestTypeText.text = newQuest.description;
            RefreshText();
            Debug.Log($"����Ʈ �߰���: {newQuest.questName}");

            foreach (var reward in m_activeQuest.rewards)
            {
                if (reward.rewardName == "SP")
                {
                    m_RewardAmount_SP.text = reward.amount.ToString();

                    if (reward.amount > 0)
                        m_RewardIcon_SP.SetActive(true);
                    else
                        m_RewardIcon_SP.SetActive(false);
                }
                if (reward.rewardName == "AP")
                {
                    m_RewardAmount_AP.text = reward.amount.ToString();

                    if (reward.amount > 0)
                        m_RewardIcon_AP.SetActive(true);
                    else
                        m_RewardIcon_AP.SetActive(false);
                }
            }
        }
        else
        {
            m_activeQuest = null;
            Debug.LogError("���� ������ ����Ʈ�� �����ϴ�.");
        }

        if (m_activeQuest.objectives.count < m_activeQuest.objectives.maxCount)
        {
            m_scaleClearPassive.Kill();
        }
    }

    // ���� ���� �޼ҵ�
    private void GiveRewards(Quest quest)
    {
        foreach (var reward in quest.rewards)
        {
            Debug.Log($"���� ����: {reward.rewardName}, ����: {reward.amount}");

            // Emission ��⿡ ����
            ParticleSystem.EmissionModule emission;

            if (reward.rewardName == "SP")
            {
                var sp = Managers.Data.skillUpgradePoint;

                Managers.Data.skillUpgradePoint += reward.amount;

                BaseUIManager.instance.Refresh_SP(sp, Managers.Data.skillUpgradePoint);

                //  if (m_RewardParticle_SP.gameObject.activeSelf)
                //      return;

                emission = m_RewardParticle_SP.emission;
                SetReward(emission, reward);

                m_RewardParticle_SP.gameObject.SetActive(true);
            }
            else if (reward.rewardName == "AP")
            {
                var ap = Managers.Data.abilityPoint;

                Managers.Data.abilityPoint += reward.amount;

                BaseUIManager.instance.Refresh_AP(ap, Managers.Data.abilityPoint);

                if (m_RewardParticle_AP.gameObject.activeSelf)
                    return;

                emission = m_RewardParticle_AP.emission;
                SetReward(emission, reward);

                m_RewardParticle_AP.gameObject.SetActive(true);
            }
        }
    }

    void SetReward(ParticleSystem.EmissionModule emission, Reward reward)
    {
        // Burst �迭 ��������
        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];

        // ��� Burst�� count ���� newBurstCount�� ����
        for (int i = 0; i < bursts.Length; i++)
        {
            bursts[i].count = reward.amount;

            if (reward.amount >= 30)
                bursts[i].count = 30;
        }

        // ����� Burst �迭�� �ٽ� ����
        emission.SetBursts(bursts);
    }

}

[System.Serializable]
public class Quest
{
    public int questID;
    public string questName;
    public string description;
    public bool isCompleted;
    public QuestObjective objectives; // ����Ʈ ��ǥ
    public List<Reward> rewards = new List<Reward>(); // ����Ʈ �Ϸ� ���� ����Ʈ

    // ����Ʈ �Ϸ� ���� Ȯ�� �޼ҵ�
    public bool CheckIfComplete()
    {
        if (!objectives.isCompleted)
            return false;

        return true;
    }
}

[System.Serializable]
public class QuestObjective
{
    public string questType;
    public bool isCompleted;
    public string questItemName;
    public int maxCount;
    public int count;
}

[System.Serializable]
public class Reward
{
    public string rewardName;
    public int amount; // ���� ����
}