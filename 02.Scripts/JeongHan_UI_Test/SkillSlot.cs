using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public TextMeshProUGUI skillLevel;
    public TextMeshProUGUI skillName;
    public Image skillImage;
    public Button equipButton;
    public Button skillExplanationButton;
    public GameObject circularLayout;
    public GameObject lockImage;

    public int[] m_TripodIndex = new int[3] {0,0,0};
    public Skill skillData;

    public GameObject m_UnEquipBtn;

    public void Start()
    {
        equipButton.onClick.AddListener(OnSkillEquipClick);
        skillExplanationButton.onClick.AddListener(OnSkillSlotClick);
        skillImage.sprite = skillData.m_image;
        circularLayout = GameObject.Find("CircularSkill");

        foreach(var child in ActionLogicManager.Instance.m_actionLogic)
        {
            if (child.m_skillName == skillName.text)
            {
                m_UnEquipBtn.SetActive(true);
                return;
            }
        }
    }

    private void OnSkillEquipClick() // Equip Skill
    {
        // 현재 클릭된 스킬의 이미지와 데이터를 넘김 
        circularLayout.GetComponent<CircularLayout>().SetSkillSlotImage(skillImage.sprite, skillData);
        circularLayout.GetComponent<CircularLayout>().m_unEquipButton = m_UnEquipBtn;

        circularLayout.GetComponent<CircularLayout>().slotAnimActive = true;
        circularLayout.GetComponent<CircularLayout>().dim.SetActive(true);

        TutorialManager.NextStepButton();
        TutorialManager.TutorialStart(2);
        TutorialManager.TutorialStart(4);
        TutorialManager.TutorialStart(6);
        TutorialManager.TutorialStart(12);
    }

    public void OnUnEquipButton()
    {
        foreach(var child in circularLayout.GetComponent<CircularLayout>().activatedPreset.myPreset)
        {
            if(child.GetComponent<ActionSlot>().skillData.m_skillID == skillData.m_skillID)
            {
                child.GetComponent<ActionSlot>().skillData = SkillList.Instance.GetSkillByName("EmptySkill");
                child.GetComponent<ActionSlot>().transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = SkillList.Instance.GetSkillByName("EmptySkill").m_image;
            }
        }
        // 현재 클릭된 스킬의 이미지와 데이터를 넘김 
        //circularLayout.GetComponent<CircularLayout>().SetSkillSlotImage(SkillList.instance.GetSkillByName("EmptySkill").m_image, SkillList.instance.GetSkillByName("EmptySkill"));
        m_UnEquipBtn.SetActive(false);
    //    circularLayout.GetComponent<CircularLayout>().m_unEquipButton = m_UnEquipBtn;
    }

    public void OnSkillSlotClick() // 스킬 정보창 PopUp
    {
        SkillTabManager.Instance.OpenSkillTab();
    }
}
