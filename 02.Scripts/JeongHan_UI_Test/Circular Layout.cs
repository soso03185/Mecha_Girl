using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using DG.Tweening;
using static DataManager;


public class CircularLayout : MonoBehaviour
{
    public static CircularLayout Instance;

    public GameObject slotImage;
    public RectTransform rectTransform;
    private string slotPath = "UI/EmptySlot";
    private string emptySlotPath = "UI/EmptySlot 1";

    public GameObject emptySlot;

    public Skill m_skillData;

    public int slotCount;

    public float baseRadius; // ���� ������ ��
    public float radius;

    public SkillPreset preset1;
    public SkillPreset preset2;
    public SkillPreset preset3;

    public List<SkillPreset> presets;

    public List<Button> presetButton;

    public SkillPreset activatedPreset;

    public SkillList skillList;

    public List<GameObject> presetParentList = new List<GameObject>();

    // ���� Ȱ��ȭ �Ǿ��ִ� ������ ��ȣ
    public int presetNum;

    private int originalIndexA;
    private int originalIndexB;

    private GraphicRaycaster gr;

    public Sprite selectedSprite;
    
    public GameObject savePopUp;
    public GameObject presetSavePopUP;
    public RotateActionLogic actionSlot;

    public GameObject canvas;

    public bool isSkillChanged = false;
    public bool isPresetChanged = false;

    public GameObject m_unEquipButton;

    public bool slotAnimActive = false;

    public float targetScale = 1.1f; // Ŀ���� ���� ����
    private float elapsedTime = 0f;
    private float totalDuration = 0.45f;

    private Vector3 initialScale;
    private bool isScalingUp = true;

    public GameObject dim;

    public List<Tripod> tripodDatas;

    public void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        skillList = GameObject.FindGameObjectWithTag("SkillList").GetComponent<SkillList>();
        actionSlot = GameObject.FindGameObjectWithTag("ActionSlot").GetComponent<RotateActionLogic>();

        gr = GetComponentInParent<GraphicRaycaster>();
       
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        slotCount = Managers.Data.slotCount;

        // ToDo : ����� DataManager�� �ִ� �ӽ� ��ų �ε��� ������ ������ ������ �޾ƿ��µ� 
        //        �� ��� ���� DB�� ����Ǿ� �ִ� ������ �����¸��� ������ �� �޾ƿ��� �������� �����ؾ���

        BeginSlotSetting(preset1);
        BeginSlotSetting(preset2);
        BeginSlotSetting(preset3);

        switch (Managers.Data.lastActivatedPresetNum)
        {
            case 1: activatedPreset = preset1; break;
            case 2: activatedPreset = preset2; break;
            case 3: activatedPreset = preset3; break;
        }

        TurnOnPreset(activatedPreset);

        UpdateRadius();

        SetNewSlotPos(preset1, true);
        SetNewSlotPos(preset2, false);
        SetNewSlotPos(preset3, false);

        initialScale = preset1.myPreset[0].transform.localScale;

        SetTripodData();
    }

    public void SlotSetting()
    {
        slotCount++;
        FirestoreManager.Instance.SavePlayerField("SkillSlotCount", slotCount);

        AddSkillSlot(preset1);
        AddSkillSlot(preset2);
        AddSkillSlot(preset3);

        Managers.Data.AddEmptySkill();

        switch (Managers.Data.lastActivatedPresetNum)
        {
            case 1: activatedPreset = preset1; break;
            case 2: activatedPreset = preset2; break;
            case 3: activatedPreset = preset3; break;
        }

        SetNewSlotPos(preset1, activatedPreset == preset1);
        SetNewSlotPos(preset2, activatedPreset == preset2);
        SetNewSlotPos(preset3, activatedPreset == preset3);
        SetTripodData();
    }

    public void AddSkillSlot(SkillPreset preset)
    {
        GameObject newSlot = Managers.Resource.Instantiate(slotPath, preset.gameObject.transform);
        newSlot.GetComponent<ActionSlot>().skillData = SkillList.Instance.GetSkillByName("EmptySkill");
        newSlot.GetComponent<ActionSlot>().skillSlotNum.text = (preset.myPreset.Count + 1).ToString();        
        preset.myPreset.Add(newSlot.GetComponent<ActionSlot>());
    }

    public void OnDisable()
    {
        isSkillChanged = false;
        savePopUp.SetActive(false);
    }

    public void Update()
    {
        // ���� �������� �������� ������ ���
        if(slotAnimActive)
        {
            float t = (float)elapsedTime / totalDuration;

            if (isScalingUp)
            {
                foreach (var slot in activatedPreset.myPreset)
                {
                    slot.transform.localScale = Vector3.Lerp(initialScale, initialScale * targetScale, t);
                    //slot.transform.GetChild(2).gameObject.SetActive(true);
                    //slot.transform.GetChild(3).gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (var slot in activatedPreset.myPreset)
                {
                    slot.transform.localScale = Vector3.Lerp(initialScale * targetScale, initialScale, t);
                    //slot.transform.GetChild(2).gameObject.SetActive(true);
                    //slot.transform.GetChild(3).gameObject.SetActive(true);
                }
            }

            // ��ǥ �����ӿ� �����ϸ� ������ �ٲ�
            if (elapsedTime >= totalDuration)
            {
                elapsedTime = 0f;
                isScalingUp = !isScalingUp;
            }
            else
            {
                elapsedTime += Time.unscaledDeltaTime;
            }
        }
    }

    public void StopSlotAnim()
    {
        slotAnimActive = false;
        selectedSprite = null;
        m_skillData = null;
        ResetSlotScale();
    }

    public void ResetSlotScale()
    {
        foreach(var slot in activatedPreset.myPreset)
        {
            slot.transform.localScale = initialScale;
        }
    }

    public void SetTripodData()
    {
        tripodDatas.Clear();
        foreach (var skill in activatedPreset.myPreset)
        {
            tripodDatas.Add(skill.GetComponent<ActionSlot>().skillData.m_tripod);
        }
    }

    public void BeginSlotSetting(SkillPreset preset)
    {
        preset.transform.parent = this.transform;
        presetParentList.Add(preset.gameObject);

        preset.gameObject.SetActive(false);

        for (int i = 0; i < slotCount; i++)
        {
            GameObject newSlot = Managers.Resource.Instantiate(slotPath, preset.gameObject.transform);
            newSlot.name = newSlot.name + i.ToString();
            preset.myPreset.Add(newSlot.GetComponent<ActionSlot>());
            preset.myPreset[i].skillSlotNum.text = (i+1).ToString();
            switch(preset.presetNum)
            {
                case 1:
                    preset.myPreset[i].GetComponent<ActionSlot>().skillData = skillList.GetSkill(Managers.Data.preset1[i]);
                    if(TutorialQuest.Instance) TutorialQuest.Instance.m_SkillSlotBtns.Add(newSlot.transform);
                    break;
                case 2:
                    preset.myPreset[i].GetComponent<ActionSlot>().skillData = skillList.GetSkill(Managers.Data.preset2[i]);
                    break;
                case 3:
                    preset.myPreset[i].GetComponent<ActionSlot>().skillData = skillList.GetSkill(Managers.Data.preset3[i]);
                    break;
            }

            // ���� ó���� ���� �����Ǿ��ִ� ��ų �������� ��ų �̹����� ��ų ������ �־��ֱ�
            //preset.myPreset[i].GetComponent<ActionSlot>().skillData = m_ActionLogicManager.GetComponent<ActionLogicManager>().m_actionLogic[i];
        }
        presets.Add(preset);

        if (TutorialQuest.Instance) TutorialQuest.Instance.SetSlotBtns();

        //���� ��ü�� ȸ��
        //Vector3 currentRotation = newSlot.transform.eulerAngles;

        //currentRotation.z = angle * 180f / Mathf.PI - 90;

        //newSlot.transform.eulerAngles = currentRotation;
    }

    public void TurnOffPreset(SkillPreset preset)
    {
        
        // ������ �θ� ������Ʈ ����
        foreach (var parentObj in presetParentList)
        {
            if(parentObj.name == preset.presetName)
            {
                parentObj.SetActive(false);
            }
        }
    }

    public void TurnOnPreset(SkillPreset preset)
    {
        for(int i =0; i < presets.Count; i++)
        {
            if(preset.presetNum == 1)
            {
                //presetButton[0].GetComponent<Image>().color = new Color32(255, 142, 0, 255);
                //presetButton[0].transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 142, 0, 255);
                presetButton[0].interactable = false;
                presetButton[1].interactable = true;
                presetButton[2].interactable = true;
                break;
            }
            else if(preset.presetNum == 2)
            {
                presetButton[0].interactable = true;
                presetButton[1].interactable = false;
                presetButton[2].interactable = true;
                break;
            }
            else if(preset.presetNum == 3)
            {
                presetButton[0].interactable = true;
                presetButton[1].interactable = true;
                presetButton[2].interactable = false;
                break;
            }
        }

        foreach (var skillCard in SkillTabManager.Instance.skillScrollView.skillList)
        {
            skillCard.GetComponent<SkillSlot>().m_UnEquipBtn.gameObject.SetActive(false);
            foreach (var child in activatedPreset.myPreset)
            {
                if (child.GetComponent<ActionSlot>().skillData.m_skillID == skillCard.GetComponent<SkillSlot>().skillData.m_skillID)
                {
                    skillCard.GetComponent<SkillSlot>().m_UnEquipBtn.gameObject.SetActive(true);
                }
            }
        }

        // ������ �θ� ������Ʈ �ѱ�
        foreach (var parentObj in presetParentList)
        {
            if (parentObj.name == preset.presetName)
            {
                parentObj.SetActive(true);
            }
        }
    }

    public void SetNewSlotPos(SkillPreset preset, bool isActive)
    {
        for (int i = 0; i < slotCount; i++)
        {
            float angle = -i * (Mathf.PI * 2) / slotCount + Mathf.PI / 2;

            preset.myPreset[i].transform.position = transform.position + (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius);
        }
    }

    public void SavePreset()
    {
        foreach (var preset in presets)
        {
            // ���� Ȱ��ȭ �� ������ ����
            if (activatedPreset.presetNum == preset.presetNum && isPresetChanged)
            {
                // �ʱ�ȭ ���� �ʴ� ������ ������ ���� ������ ���� ����
                switch(preset.presetNum)
                {
                    case 1:
                        for(int i = 0; i < slotCount; i++)
                        {
                            Managers.Data.preset1[i] = preset.myPreset[i].GetComponent<ActionSlot>().skillData.m_skillID;
                        }
                        break;
                    case 2:
                        for (int i = 0; i < slotCount; i++)
                        {
                            Managers.Data.preset2[i] = preset.myPreset[i].GetComponent<ActionSlot>().skillData.m_skillID;
                        }
                        break;
                    case 3:
                        for (int i = 0; i < slotCount; i++)
                        {
                            Managers.Data.preset3[i] = preset.myPreset[i].GetComponent<ActionSlot>().skillData.m_skillID;
                        }
                        break;
                }
            }
        }
    }

    public void ChangePreset(int presetNum)
    {
        // ���� ������ �����ϰ�
        SavePreset();

        TurnOffPreset(activatedPreset);

        // ���ο� ������ Ȱ��ȭ �����ְ�
        foreach (var preset in presets)
        {
            if (preset.presetNum == presetNum)
            {
                activatedPreset = preset;
            }
        }

        TurnOnPreset(activatedPreset);

        Managers.Data.lastActivatedPresetNum = activatedPreset.presetNum;

        isPresetChanged = false;
    }

    // ������ ���� �� �̹��� ��ȯ
    public void SetSkillSlotImage(Sprite newSprite, Skill skillData)
    {
        selectedSprite = newSprite;
        m_skillData = skillData;
    }

    // ��ų����â ���� ���� �ִ��� �Ǵ�
    public void CompareSkillSlot()
    {
        if (activatedPreset != null)
        {
            // ��ų�� �ٲ��� �ʾ����� �������� ���� �����Ǿ��ִ� ��ų ����Ʈ�� �ٸ� ��쿡
            if (!isSkillChanged)
            {
                for (int i = 0; i < slotCount; i++)
                {
                    if (Managers.Data.currentSkillList[i] != activatedPreset.myPreset[i].GetComponent<ActionSlot>().skillData.m_skillID)
                    {
                        isSkillChanged = true;
                    }
                }
            }
        }

            // ���� ���� �Ǿ��ִ� ��ų�� Ʈ�������� ������ ���� �ٸ� ��쿡
        for(int i =0; i < slotCount; i++)
        {
            if (!tripodDatas[i].Equals(activatedPreset.myPreset[i].GetComponent<ActionSlot>().skillData.m_tripod))
            {
                isSkillChanged = true;
            }
        }

        if(isSkillChanged)
        {
            savePopUp.SetActive(true);
        }
        else
        {
            canvas.SetActive(false);
        }
    }

    public void ResetSkillData()
    {
        for (int i = 0; i < slotCount; i++)
        {
            var skillData = SkillList.Instance.GetSkill(Managers.Data.currentSkillList[i]);
            activatedPreset.myPreset[i].GetComponent<ActionSlot>().skillData = skillData;
            activatedPreset.myPreset[i].transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = skillData.m_image;
            SkillScrollView.Instance.ChangeEquipButton(skillData.m_skillID);
            m_unEquipButton.SetActive(false);
            isPresetChanged = false;
        }
    }


    // ������� ���� ��ư ������ �� �׼� ���� â ��ų ����
    public void ChangeSkillData()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Managers.Data.currentSkillList[i] =
                activatedPreset.myPreset[i].GetComponent<ActionSlot>().skillData.m_skillID;
        }
        ActionLogicManager.Instance.ChangeSkill();

        if(actionSlot == null)
        {
            actionSlot = GameObject.FindGameObjectWithTag("ActionSlot").GetComponent<RotateActionLogic>();
        }
        for (int i = 0; i < slotCount; i++)
        {
            actionSlot.slotList[i].GetComponentInChildren<Image>().sprite = ActionLogicManager.Instance.m_actionLogic[i].m_image;
        }

        SetTripodData();
    }

    public void OnYesButton()
    {
        if(TapManager.instance.isReadyToTab)
        {
            TapManager.instance.ActiveTab(TapManager.instance.lastClickedTab);
            TapManager.instance.isReadyToTab = false;
        }

        ChangeSkillData();
        GameManager.Instance.RestartCurrentStage();
        actionSlot.isClosedButtonClicked = true;
        savePopUp.SetActive(false);
        canvas.SetActive(false);

        ActionLogicManager.Instance.m_battleAnalysisSystem.m_scrollView.Clear();

        Managers.Pool.ResetPool("Slider");
        // ���������� Ȱ��ȭ �Ǿ��ִ� ������ ����
        SavePreset();
        Managers.Data.lastActivatedPresetNum = activatedPreset.presetNum;

        TutorialManager.NextStepButton();
        TutorialManager.TutorialStart(25);
    }

    public void OnNoButton()
    {
        isSkillChanged = false;
        savePopUp.SetActive(false);
    }

    private void UpdateRadius()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // ���� �ػ�(��: 1920x1080)�� �������� ������ ����Ͽ� �������� �����մϴ�.
        float baseWidth = 1080;
        float baseHeight = 1920f;

        float widthRatio = screenWidth / baseWidth;
        float heightRatio = screenHeight / baseHeight;

        // ������ ���� �ʺ�� ���� ������ ��հ��� ����Ͽ� �����մϴ�.
        radius = baseRadius * (widthRatio + heightRatio) / 2;
    }

}
