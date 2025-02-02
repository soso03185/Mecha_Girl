using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static bool isFireBaseLoad = false;
    public bool isPlayerDead;
    public bool isStageClear;

    public int currentStage = 2;

    public int slotCount = 0;
    private DataManager.UserInfo userInfo;

    public GameObject player;

    public TextMeshProUGUI stageLevelUI;

    public EventHandler PlayerAppearance;
    public EventHandler showStageInfoEnd;

    public StageInfoCanvas StageInfoCanvas;

    public bool isGameStart = false;

    public bool isCheatOn = false;

    public CanvasSetting canvasSetting;

    public bool isPopUpOn = false;

    public List<GameObject> synergyParticles;

    static bool IsFirstMapLoad = true;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;    
        }
        else
        {
            Debug.Log("GameManager already exists");
            Destroy(gameObject);
        }

    }

    private void OnEnable()
    {
        StageManager.OnStageCleared += HandleStageCleared;
        StageManager.OnStageFailed += HandleStageFailed;

        GameTimer.Instance.Init();
    }

    private void OnDisable()
    {
        StageManager.OnStageCleared -= HandleStageCleared;
        StageManager.OnStageFailed -= HandleStageFailed;
    }

    void Start()
    {
        Init();
    }
    
    public void Init()
    {        
        player = Managers.Resource.Instantiate("Player");
        player.transform.position = new Vector3(0, -4, 0);

        // ���̾�̽� ������ �ε�
        FirestoreManager.Instance.LoadPlayerData(counter =>
        {
            if (!isFireBaseLoad)
            {
                Debug.Log("LoadPlayerData");
                isFireBaseLoad = true;

                Managers.Data.maxStageClearLevel = counter.MaxClearStage;
                Managers.Data.currentStageLevel = Managers.Data.maxStageClearLevel;
                Managers.Data.skillUpgradePoint = counter.SkillPoint;
                Managers.Data.abilityPoint = counter.AbilityPoint;
                UpdateUIWithPlayerData();

                // ��ų ���� ����
                // if (counter.SkillSlotCount < 3) Managers.Data.slotCount = 3;
                // else Managers.Data.slotCount = counter.SkillSlotCount;
                //// Managers.Data.AddEmptySkillSlots();
                
                // ����Ʈ ������
                QuestData.Instance.m_questIndex = counter.NowQuest;
                // QuestManager.Instance.AddQuest(QuestData.Instance.quests[QuestData.Instance.questIndex]);

                // �������� ������
                Managers.Stage.GetStageData(Managers.Data.currentStageLevel);
                BaseUIManager.instance.m_StageCount.text = Managers.Data.currentStageLevel.ToString();
                BaseUIManager.instance.m_BossHpBarObj.SetActive(Managers.Stage.stageInfo.isBossStage);
                IsFirstMapLoad = false;

                if (Managers.Data.maxStageClearLevel > 1)
                    TutorialQuest.Instance.OffTutorial();
            }
        });

        if (IsFirstMapLoad == false)
        {
            // �������� ������
            Managers.Stage.GetStageData(Managers.Data.currentStageLevel);
            BaseUIManager.instance.m_StageCount.text = Managers.Data.currentStageLevel.ToString();
            BaseUIManager.instance.m_BossHpBarObj.SetActive(Managers.Stage.stageInfo.isBossStage);
        }

        userInfo = Managers.Data.GetUserInfo(1001);

        // �÷��̾� ���� ����
        player.GetComponent<Player>().playerAppearanceEnd += StageStart; 
        PlayerAppearance(this, EventArgs.Empty);

        // ĵ����-�÷��̾� ����
        canvasSetting = GameObject.Find("CanvasSetting").GetComponent<CanvasSetting>();
        canvasSetting.contentsCanvas[1].GetComponent<AbilitySystem>().m_player = player.GetComponent<Player>();

        // �������� ���� ĵ���� Active ����
        StageInfoCanvas = canvasSetting.contentsCanvas[5].GetComponent<StageInfoCanvas>();
        StageInfoCanvas.gameObject.SetActive(false);

        // ��ų����Ʈ �ʱ�ȭ
        SkillList.Instance.Init();
    }

    private void UpdateUIWithPlayerData()
    {
        BaseUIManager.instance.m_TextAP.text = Managers.Data.abilityPoint.ToString();
        BaseUIManager.instance.m_TextAbility_AP.text = Managers.Data.abilityPoint.ToString();
        BaseUIManager.instance.m_TextSP.text = Managers.Data.skillUpgradePoint.ToString();
    }

    public void ShowStageInfo()
    {
        if(StageInfoCanvas != null && !StageInfoCanvas.gameObject.activeSelf)
        {
            StageInfoCanvas.GetComponent<Canvas>().enabled = true;
            StageInfoCanvas.gameObject.SetActive(true);
            StageInfoCanvas.clostButton.interactable = true;
            StageInfoCanvas.clostButton.onClick.AddListener(CloseStageInfoAndStartCombat);
            //StageInfoCanvas.gameStartButton.gameObject.SetActive(true);
            StageInfoCanvas.stagePanel.gameObject.SetActive(false);
            //Time.timeScale = 0.001f;
        }
    }

    private void CloseStageInfoAndStartCombat()
    {
        StageInfoCanvas.clostButton.interactable = true;
        StageInfoCanvas.gameStartButton.onClick.RemoveListener(CloseStageInfoAndStartCombat); // ������ ����
        //StageInfoCanvas.gameStartButton.gameObject.SetActive(false);
        StageInfoCanvas.stagePanel.gameObject.SetActive(true);
        //StageStart();
    }

    void StageStart(object sender, EventArgs e)
    {
        if(Managers.Data.currentStageLevel >= 4)
        {
            ShowStageInfo();
        }

        isGameStart = true;
        Time.timeScale = Managers.Data.m_timeScale;
        SpawnManager.spawnInstance.Init();
        // ���� ���� �Ǿ����� ���� ������ �������� �������� ����
        // currentStage DB�� �ִ� ������ �ٲ������
        Managers.Stage.BeginStage(Managers.Data.currentStageLevel, Managers.Data.currentStageType);
    }

    void StageStart()
    {
        isGameStart = true;
        Time.timeScale = Managers.Data.m_timeScale;
        SpawnManager.spawnInstance.Init();
        // ���� ���� �Ǿ����� ���� ������ �������� �������� ����
        // currentStage DB�� �ִ� ������ �ٲ������
        Managers.Stage.BeginStage(Managers.Data.currentStageLevel, Managers.Data.currentStageType);
    }

    public void HandleStageFailed()
    {
        //SetStageType(StageType.Idle);
        Managers.Stage.ResetStageInfo(player);
        Debug.Log($"Stage Failed");
        player.GetComponent<Animator>().Play("PlayerMove");
    }



    public void ShowGameOverPopUp()
    {
        foreach(var canvas in canvasSetting.contentsCanvas)
        {
            if (canvas.gameObject.name.Contains("PopUpCanvas"))
                continue;
            else if(canvas.gameObject.activeSelf)
            {
                canvasSetting.contentsCanvas[4].GetComponent<PopUpList>().StageFailedUI.SetActive(true);
                canvasSetting.contentsCanvas[4].GetComponent<PopUpList>().StageFailedUI.GetComponent<StageFailedUI>().ShowAndBounce(200f);
                isPopUpOn = true;
                return;
            }
        }

        isPopUpOn = false;
        if (GameTimer.Instance.isTimeOver)
        {
            canvasSetting.contentsCanvas[4].GetComponent<PopUpList>().popUpList[0].gameObject.SetActive(true);
        }
        else
            canvasSetting.contentsCanvas[4].GetComponent<PopUpList>().popUpList[1].gameObject.SetActive(true);
    }


    public void HandleStageCleared()
    {
        if(Managers.Data.currentStageLevel == 9)
        {
            //�˾� ����
            canvasSetting.contentsCanvas[4].GetComponent<PopUpList>().EndingPopUp.SetActive(true);
            Time.timeScale = 0.001f;
            return;
        }

        Managers.Data.maxStageClearLevel++;
        FirestoreManager.Instance.SavePlayerField("MaxClearStage", Managers.Data.maxStageClearLevel);

        Managers.Data.currentStageLevel++;

        // ��ų ���� �ϳ��� ��� ����
        if (SkillScrollView.Instance != null && Managers.Data.maxStageClearLevel <= Managers.Data.currentStageLevel)
        {
            SkillScrollView.Instance.OpenSkillSlot(Managers.Data.currentStageLevel);
        }

        if(!isCheatOn)
        {
            // ��ų ���� �ϳ� �߰�
            if (Managers.Data.maxStageClearLevel == 4)
            {
                CircularLayout.Instance.SlotSetting();
            }
            // ��ų ���� �ϳ� �߰�
            if (Managers.Data.maxStageClearLevel == 7)
            {
                CircularLayout.Instance.SlotSetting();
            }
        }

        Managers.Stage.ResetStageInfo(player);

        EventManager.StageCleared();
        Debug.Log($"Stage Clear");
    }

    public void SetStageType(StageType stageType)
    {
        Managers.Data.currentStageType = stageType;
    }

    public void OnChallengeButtonClick()
    {
        if(Managers.Data.currentStageType == StageType.Idle)
        {
            Managers.Data.currentStageLevel++;
            SetStageType(StageType.Challenge);
            Managers.Stage.ResetStageInfo(player);
        }
    }

    public void NextStage()
    {
        Managers.Data.currentStageLevel++;
        SetStageType(StageType.Challenge);
        Managers.Stage.ResetStageInfo(player);
    }

    public void PreviousStage()
    {
        Managers.Data.currentStageLevel--;
        SetStageType(StageType.Challenge);
        Managers.Stage.ResetStageInfo(player);
    }

    public void ChangeStage(int stageLevel)
    {
        Managers.Data.lastStageLevel = Managers.Data.currentStageLevel;
        Managers.Data.currentStageLevel = stageLevel;
        SetStageType(StageType.Challenge);
        Managers.Stage.ResetStageInfo(player);
    }

    public void RestartCurrentStage()
    {
        SetStageType(StageType.Challenge);
        Managers.Stage.ResetStageInfo(player);
    }

    public DataManager.UserInfo GetUserInfo()
    {
        return userInfo;
    }
}
