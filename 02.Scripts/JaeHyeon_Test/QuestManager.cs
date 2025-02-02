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
/// 퀘스트 옵저버 역할과 관련된 함수들 스크립트
/// </summary>
public class QuestManager : Singleton<QuestManager>
{
    public Quest m_activeQuest; // 진행 중인 퀘스트 리스트
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
            Destroy(gameObject); // 이미 존재하는 경우 새로 생성된 오브젝트 삭제
        }
    }

    void OnEnable()
    {
        EventManager.OnMonsterKilled += HandleMonsterKilled; // 이벤트 구독
        EventManager.OnStageClear += HandleStageCleared; // 이벤트 구독
        EventManager.OnSkillEquiped += HandleEquiped; // 이벤트 구독
        EventManager.OnOpenTab += HandleOpenTab; // 이벤트 구독
    }

    void OnDisable()
    {
        EventManager.OnMonsterKilled -= HandleMonsterKilled; // 이벤트 해제
        EventManager.OnStageClear -= HandleStageCleared; // 이벤트 해제
        EventManager.OnSkillEquiped -= HandleEquiped; // 이벤트 해제
        EventManager.OnOpenTab -= HandleOpenTab; // 이벤트 해제
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
            Debug.LogError($"진행중인 퀘스트가 없습니다. !");
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

    // 퀘스트 완료 여부를 체크하는 메소드
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

            Debug.Log($"퀘스트 완료됨: {m_activeQuest.questName}");
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
                Debug.LogError("퀘스트 모두 완료 !");
            }

            TutorialManager.NextStepButton();
            TutorialManager.TutorialStart(10);
        }

        if (m_scaleButtonClick != null) m_scaleButtonClick.Kill();

        m_scaleButtonClick = DOTween.Sequence()
               .Append(m_QuestButton.DOScale(0.9f, 0.08f).SetEase(Ease.InOutSine)) // 부드러운 Ease 설정
               .Append(m_QuestButton.DOScale(m_originButtonScale, 0.15f))  // 원본 크기로 되돌리기
               .SetUpdate(true).OnComplete(() => m_QuestButton.DOKill());  // 타임스케일에 상관없이 업데이트
    }

    public void CompleteButtonTween()
    {
        if (m_activeQuest.objectives.count >= m_activeQuest.objectives.maxCount)
        {
            m_QuestClearImage.SetActive(true);

            if (m_scaleClearPassive != null) m_scaleClearPassive.Kill();

            m_scaleClearPassive = DOTween.Sequence()
                .Append(m_QuestButton.DOScale(1.1f, 0.35f).SetEase(Ease.InOutSine)) // 1.1배로 확대
                .Append(m_QuestButton.DOScale(1f, 0.35f).SetEase(Ease.InOutSine))   // 원래 크기로 축소
                .SetLoops(-1, LoopType.Restart) // 무한 반복
                .SetUpdate(true);               // 타임스케일 무시
        }
    }

    // 퀘스트 추가 메소드
    public void AddQuest(Quest newQuest)
    {
        if (QuestData.Instance.m_quests.Contains(newQuest))
        {
            m_activeQuest = newQuest;
            m_QuestTypeText.text = newQuest.description;
            RefreshText();
            Debug.Log($"퀘스트 추가됨: {newQuest.questName}");

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
            Debug.LogError("진행 가능한 퀘스트가 없습니다.");
        }

        if (m_activeQuest.objectives.count < m_activeQuest.objectives.maxCount)
        {
            m_scaleClearPassive.Kill();
        }
    }

    // 보상 지급 메소드
    private void GiveRewards(Quest quest)
    {
        foreach (var reward in quest.rewards)
        {
            Debug.Log($"보상 지급: {reward.rewardName}, 수량: {reward.amount}");

            // Emission 모듈에 접근
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
        // Burst 배열 가져오기
        ParticleSystem.Burst[] bursts = new ParticleSystem.Burst[emission.burstCount];

        // 모든 Burst의 count 값을 newBurstCount로 변경
        for (int i = 0; i < bursts.Length; i++)
        {
            bursts[i].count = reward.amount;

            if (reward.amount >= 30)
                bursts[i].count = 30;
        }

        // 변경된 Burst 배열을 다시 설정
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
    public QuestObjective objectives; // 퀘스트 목표
    public List<Reward> rewards = new List<Reward>(); // 퀘스트 완료 보상 리스트

    // 퀘스트 완료 여부 확인 메소드
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
    public int amount; // 보상 수량
}