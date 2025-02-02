using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageLevel : MonoBehaviour
{
    public int stageCount;

    public int levelCount = 1;

    public GameObject stageLevelPrefab;
    public RectTransform centerBox;

    public List<StageLevelUI> stageLevelUIList = new List<StageLevelUI>();

    public GraphicRaycaster raycaster; // GraphicRaycaster 컴포넌트
    public EventSystem eventSystem; // EventSystem 컴포넌트

    private StageLevelUI currentlyTouchingUI;

    public MonsterInfoUI monsterInfoUI;

    private List<DataManager.StageInfo> stageInfos = new List<DataManager.StageInfo>();

    public Button changeButton;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 1; i <= stageCount; i++)
        {
            GameObject go = Instantiate(stageLevelPrefab, this.transform);
            go.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            go.GetComponent<StageLevelUI>().level = i;
            go.name = go.name + i.ToString();
            //go.GetComponent<Button>().onClick.AddListener(() => OnButtonClick(go));
            stageLevelUIList.Add(go.GetComponent<StageLevelUI>());
        }

    }

    public void OnDestroy()
    {
        stageLevelUIList.Clear();
        stageInfos.Clear();
        currentlyTouchingUI = null;
    }

    public void Init()
    {
        if (stageInfos.Count == 0)
        {
            for (int i = 1; i <= stageCount; i++)
            {
                var stageInfo = Managers.Data.GetStageInfo(i);
                stageInfos.Add(stageInfo);
            }

            monsterInfoUI.Init(stageInfos[Managers.Data.currentStageLevel - 1], Managers.Data.currentStageLevel);
        }
        else
            monsterInfoUI.ChangeMonsterInfo(stageInfos[Managers.Data.currentStageLevel - 1], Managers.Data.currentStageLevel);

    }

    private void OnEnable()
    {
        Managers.Data.lastStageLevel = 1;
        MoveToCurrentStage();

        if (!GameManager.Instance.isCheatOn)
        {
            DeactivateSlot();
        }
        else
            ActivateSlot();
    }

    public void Update()
    {
        GetTouchingObject();
        if(currentlyTouchingUI != null)
        {
            Managers.Stage.CheckStageLevel(int.Parse(currentlyTouchingUI.GetComponentInChildren<TextMeshProUGUI>().text));
            monsterInfoUI.ChangeMonsterInfo(stageInfos[currentlyTouchingUI.level-1], currentlyTouchingUI.level);

            if (currentlyTouchingUI.level > Managers.Data.maxStageClearLevel && !GameManager.Instance.isCheatOn)
            {
                changeButton.interactable = false;
            }
            else
                changeButton.interactable = true;
        }

    }

    public void GetTouchingObject()
    {
        for (int i = 0; i < stageCount; i++)
        {
            if (stageLevelUIList[i].isTouched == true)
            {
                currentlyTouchingUI = stageLevelUIList[i];
            }
        }
    }

    public void MoveToCurrentStage()
    {
        if (Managers.Data.lastStageLevel >= Managers.Data.currentStageLevel) 
        {
            int temp = Managers.Data.lastStageLevel - Managers.Data.currentStageLevel;
            for (int i = 0; i <= temp; i++)
            {
                this.GetComponent<RectTransform>().localPosition = new Vector3(i * stageLevelUIList[i].GetComponent<RectTransform>().rect.width + i * this.GetComponent<HorizontalLayoutGroup>().spacing, 0, 0);
            }
            currentlyTouchingUI = stageLevelUIList[Managers.Data.lastStageLevel];
        }
        else if(Managers.Data.lastStageLevel < Managers.Data.currentStageLevel)
        {
            int temp = Managers.Data.currentStageLevel - Managers.Data.lastStageLevel;
            for (int i = 0; i <= temp; i++)
            {
                this.GetComponent<RectTransform>().localPosition = new Vector3(i * -stageLevelUIList[i].GetComponent<RectTransform>().rect.width - i * this.GetComponent<HorizontalLayoutGroup>().spacing, 0, 0);
            }
            currentlyTouchingUI = stageLevelUIList[Managers.Data.lastStageLevel];
        }
    }

    public void DeactivateSlot()
    {
        foreach(var stageLevelUI in stageLevelUIList)
        {
            if(stageLevelUI.level >= Managers.Data.maxStageClearLevel + 1)
            {
                stageLevelUI.GetComponent<Button>().interactable = false;
            }
            else
                stageLevelUI.GetComponent<Button>().interactable = true;
        }
    }

    public void ActivateSlot()
    {
        foreach (var stageLevelUI in stageLevelUIList)
        {
            if (stageLevelUI.level >= Managers.Data.maxStageClearLevel)
            {
                stageLevelUI.GetComponent<Button>().interactable = true;
            }
        }
    }

    public Vector3[] CheckCenterBoxUI()
    {
        // centerBox의 네 모서리 좌표를 구합니다.
        Vector3[] corners = new Vector3[4];
        centerBox.GetWorldCorners(corners);

        return corners;
    }
}
