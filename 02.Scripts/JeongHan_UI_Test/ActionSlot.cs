using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionSlot : MonoBehaviour
{
    public CircularLayout circularLayout;
    public Skill skillData;
    [SerializeField] Image m_Skill_Icon;

    public TextMeshProUGUI skillSlotNum;

    public bool isEquipped = false;
    // Start is called before the first frame update
    void Awake()
    {
        circularLayout = GetComponentInParent<CircularLayout>();
    }

    void Start()
    {
        if(m_Skill_Icon != null)
            m_Skill_Icon.sprite = skillData.m_image;

    }

    public void OnSkillSlotClicked()
    {
        if (isEquipped && circularLayout.selectedSprite == null)
        {
            foreach (var skill in SkillScrollView.Instance.skillList)
            {
                if (skill.GetComponent<SkillSlot>().skillData == skillData)
                {
                    skill.GetComponent<SkillSlot>().OnUnEquipButton();
                    isEquipped = false;
                    return;
                }
            }
        }

        if (circularLayout.selectedSprite != null && this.transform.GetChild(0).GetComponent<Image>().sprite != circularLayout.selectedSprite)
        {
            isEquipped = true;
            // UnEquip Btn
            SkillScrollView.Instance.ChangeEquipButton(skillData.m_skillID);
            circularLayout.m_unEquipButton.SetActive(true);
            //

            this.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = circularLayout.selectedSprite;
            EventManager.SkillEquiped(circularLayout.m_skillData.m_skillName);
            skillData = circularLayout.m_skillData;
            circularLayout.selectedSprite = null;
            circularLayout.m_skillData = null;
            circularLayout.dim.SetActive(false);
            circularLayout.ResetSlotScale();
            circularLayout.slotAnimActive = false;
            SoundManager.Instance.PlaySFX("SkillEquipSound");
            if (!circularLayout.isPresetChanged)
            {
                circularLayout.isPresetChanged = true;
            }

         //   if(circularLayout.activatedPreset.FindSkillCountByName("EmptySkill") <= 0)
            {
                TutorialManager.NextStepButton();
                TutorialManager.TutorialStart(3);
                TutorialManager.TutorialStart(5);
                TutorialManager.TutorialStart(7);
                TutorialManager.TutorialStart(13);
            }
        }
    }
}
