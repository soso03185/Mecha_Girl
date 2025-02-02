using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class SkillScrollView : Singleton<SkillScrollView>
{
    // 스킬 갯수
    // public int skillCount;

    // 스킬 목록
    public List<GameObject> skillList = new List<GameObject>();
    public CircularLayout circularLayout;

    [SerializeField] Canvas m_SkillSettingCanvas;

    // Start is called before the first frame update
    public void Start()
    {
        SkillList mySkillList = SkillList.Instance;

        for (int i = 0; i < mySkillList.skillList.Count; i++)
        {
            if (mySkillList.skillList[i].m_skillName == "EmptySkill")
            {
                continue;
            }
            else
            {
                //GameObject slot = Managers.Pool.GetPool("SkillSlotSample").GetGameObject("UI/SkillSlotSample");
                GameObject slot = Managers.Resource.Instantiate("UI/SkillSlotSample");
                slot.transform.SetParent(this.GetComponent<RectTransform>());
                slot.transform.localScale = new Vector3(1, 1, 1);

                skillList.Add(slot);

                var skillSlot = skillList[skillList.Count - 1].GetComponent<SkillSlot>();

                // 스킬 데이터 추가
                skillSlot.skillData = mySkillList.skillList[i];
                // 스킬 이름 추가
                skillSlot.skillName.text = mySkillList.skillList[i].GetComponent<Skill>().m_skillKorName;
                // 스킬 레벨 추가
                skillSlot.skillLevel.text = "Lv." + mySkillList.skillList[i].GetComponent<Skill>().m_skillLevel.ToString();
                // 스킬 이미지 추가
                skillSlot.skillImage.sprite = mySkillList.skillList[i].GetComponent<Skill>().m_image;
            }
        }

        int[] targetID = { 0, 1, 2, 3 }; // Tutorial Skills id
        foreach (var NumId in targetID)
        {
            foreach (var child in skillList)
            {
                var skillSlot = child.GetComponent<SkillSlot>();

                if (skillSlot.skillData.m_skillID == NumId)
                {
                    if (TutorialQuest.Instance)
                    {
                        TutorialQuest.Instance.m_EquipSkillBtns.Add(skillSlot.equipButton.transform);
                        TutorialQuest.Instance.m_SkillExplainObjs.Add(skillSlot.skillImage.gameObject);
                    }
                }
            }
        }

        if(GameManager.Instance.isCheatOn)
        {
            CheatSkillSlot();
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                skillList[i].GetComponent<SkillSlot>().lockImage.SetActive(false);
            }
        }

        if (TutorialQuest.Instance)
        {
            TutorialQuest.Instance.SetEquipBtns();
        }
    }

    public void ChangeEquipButton(int _skillIndex)
    {
        foreach (var child in skillList)
        {
            var skillcard = child.GetComponent<SkillSlot>();
            if (skillcard.skillData.m_skillID == _skillIndex)
            {
                skillcard.m_UnEquipBtn.SetActive(false);
            }
        }
    }

    public void CheatSkillSlot()
    {
        foreach (var child in skillList)
        {
            child.GetComponent<SkillSlot>().lockImage.SetActive(false);
        }
    }

    public void OpenSkillSlot(int stageLevel)
    {
        if (stageLevel >= 10)
            return;
        if (stageLevel == 1)
        {
            stageLevel = 3;
        }
        else if (stageLevel == 2 || stageLevel == 3 || stageLevel == 4)
        {
            stageLevel = 4;
        }
        else
            stageLevel += 0;

        for(int i = 0; i < stageLevel; i++)
        {
            skillList[i].GetComponent<SkillSlot>().lockImage.SetActive(false);
        }
    }

    public void ResetAllSkillData()
    {
        var sp = Managers.Data.skillUpgradePoint;

        foreach (var skillSlot in circularLayout.activatedPreset.myPreset)
        {
            skillSlot.GetComponent<ActionSlot>().skillData = SkillList.Instance.GetSkillByName("EmptySkill");
            skillSlot.GetComponent<ActionSlot>().transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = SkillList.Instance.GetSkillByName("EmptySkill").m_image;
        }

        foreach(var child in skillList)
        {
            child.GetComponent<SkillSlot>().m_UnEquipBtn.gameObject.SetActive(false);
            child.GetComponent<SkillSlot>().equipButton.gameObject.SetActive(true);
            Managers.Data.skillUpgradePoint += child.GetComponent<SkillSlot>().skillData.m_skillLevel;
            Managers.Data.skillUpgradePoint -= 1;
            child.GetComponent<SkillSlot>().skillData.m_skillLevel = 1;
            child.GetComponent<SkillSlot>().skillLevel.text = "Lv. " + 1;
            SkillTabManager.Instance.skillLevel.text = "Lv. " + 1;
            SkillTabManager.Instance.skillData = child.GetComponent<SkillSlot>().skillData;
            child.GetComponent<SkillSlot>().skillData.ResetSkill();
            child.GetComponent<SkillSlot>().skillData.SetSkillExplanation();
            SkillTabManager.Instance.SetSkillTab();
        }

        BaseUIManager.instance.Refresh_SP(sp, Managers.Data.skillUpgradePoint);

        if(!circularLayout.isPresetChanged)
        {
            circularLayout.isPresetChanged = true;
        }
        circularLayout.SavePreset();
    }
}

