using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using TMPro;
using Unity.VisualScripting;

public class SkillTabManager : MonoBehaviour
{
    public static SkillTabManager Instance;

    private GameObject clickedObject;
    [HideInInspector] public SkillSlot activeSkill;
    public Image skillImage;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillLevel;
    public TextMeshProUGUI skillExplanation;
    public TextMeshProUGUI skillManaCost;
    public TextMeshProUGUI skillAttribute;
    public TextMeshProUGUI skillRange;
    public TextMeshProUGUI skillElementAmp;

    public GameObject skillExplainPanel;

    public Vector3 initScrollViewPos;
    public SkillScrollView skillScrollView;

    public Skill skillData;

    public TripodUI tripodUI;

    public TextMeshProUGUI skillPointText;

    public List<TripodButton> tripodButtons;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
   

    public void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        skillExplainPanel.SetActive(false);
        skillScrollView.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                clickedObject = results.FirstOrDefault(result => result.gameObject.CompareTag("SkillSlot")).gameObject;
            }

        }
        
        if (skillPointText != null)
        {
            skillPointText.text = Managers.Data.skillUpgradePoint.ToString();
        }
    }

    public void OpenSkillTab()
    {
        activeSkill = clickedObject.GetComponent<SkillSlot>();
        skillData = activeSkill.skillData;
        SetSkillTab();
        SetTripod();
        skillExplainPanel.SetActive(true);

        TutorialManager.NextStepButton();
        TutorialManager.TutorialStart(18);
    }

    public void CloseSkillTab()
    {
        if (TutorialQuest.Instance != null)
            TutorialQuest.Instance.m_DimPanel.ShowBotArrowIcon(false);
        TutorialManager.NextStepButton();
        TutorialManager.TutorialStart(23);

        skillExplainPanel.SetActive(false);
    }

    public void NextSkill()
    {
        if (skillExplainPanel.activeSelf)
        {
            for (int i = 0; i < skillScrollView.skillList.Count; i++)
            {
                if (skillScrollView.skillList[i].GetComponent<SkillSlot>() == activeSkill && i != skillScrollView.skillList.Count - 1 && !skillScrollView.skillList[i + 1].GetComponent<SkillSlot>().lockImage.activeSelf)
                {
                    activeSkill = skillScrollView.skillList[i + 1].GetComponent<SkillSlot>();
                    skillData = activeSkill.skillData;
                    SetSkillTab();
                    SetTripod();
                    tripodUI.SetTripodLock(skillData.m_skillLevel);
                    tripodUI.SetTripod(skillData.m_tripod.firstSlot, skillData.m_tripod.secondSlot, skillData.m_tripod.thirdSlot);
                    return;
                }
            }
        }
    }

    public void PreviousSkill()
    {
        if (skillExplainPanel.activeSelf)
        {
            for (int i = 0; i < skillScrollView.skillList.Count; i++)
            {
                if (skillScrollView.skillList[i].GetComponent<SkillSlot>() == activeSkill && i > 0 && !skillScrollView.skillList[i - 1].GetComponent<SkillSlot>().lockImage.activeSelf)
                {
                    activeSkill = skillScrollView.skillList[i - 1].GetComponent<SkillSlot>();
                    skillData = activeSkill.skillData;
                    SetSkillTab();
                    SetTripod();
                    tripodUI.SetTripodLock(skillData.m_skillLevel);
                    tripodUI.SetTripod(skillData.m_tripod.firstSlot, skillData.m_tripod.secondSlot, skillData.m_tripod.thirdSlot);
                    return;
                }
            }
        }
    }

    public void SkillLevelUp()
    {
        if (skillData.m_skillLevel < 10 && Managers.Data.skillUpgradePoint > 0)
        {
            skillData.m_skillLevel++;
            skillLevel.text = "Lv." + skillData.m_skillLevel.ToString();

            var effect = Managers.Pool.GetPool("LevelUpEffect").GetGameObject("UI/LevelUpEffect", skillLevel.transform.parent.transform.position);
            StartCoroutine(ReturnEffect(effect));
            // 스크롤뷰 스킬레벨
            activeSkill.transform.Find("Skill_Level").GetComponent<TextMeshProUGUI>().text = "Lv." + skillData.m_skillLevel.ToString();

            BaseUIManager.instance.Refresh_SP(Managers.Data.skillUpgradePoint, Managers.Data.skillUpgradePoint - 1);
            Managers.Data.skillUpgradePoint--;

            skillData.SkillLevelUp();
            skillData.SetSkillExplanation();
            SetSkillTab();
            TutorialManager.NextStepButton();
            TutorialManager.TutorialStart(19);
            TutorialManager.TutorialStart(20);
      //      TutorialManager.TutorialStart(21);
        }
    }

    IEnumerator ReturnEffect(GameObject effect)
    {
        yield return new WaitForSeconds(1);

        Managers.Pool.GetPool("LevelUpEffect").ReturnObject(effect);
    }

    public void ResetCurrentSkill()
    {
        skillData.ResetSkill();
        Managers.Data.skillUpgradePoint += skillData.m_skillLevel;
        Managers.Data.skillUpgradePoint -= 1;
        skillData.SkillLevelReset();
        skillLevel.text = "Lv." + 1;
        // 스크롤뷰 스킬레벨
        activeSkill.transform.Find("Skill_Level").GetComponent<TextMeshProUGUI>().text = "Lv." + 1;
        tripodUI.SetTripodLock(1);
        tripodUI.ResetTripod(0, 0, 0);
        skillData.SetSkillExplanation();
        SetSkillTab();
    }

    public void SetSkillTab()
    {
        skillLevel.text = "Lv." + skillData.m_skillLevel.ToString();
        skillName.text = skillData.m_skillKorName;
        skillImage.sprite = skillData.m_image;
        skillExplanation.text = skillData.m_skillExplanation;
        skillManaCost.text = skillData.m_manaCost.ToString();
        if (skillData.m_attribute == Attribute.Water)
        {
            skillAttribute.text = "물";
            skillAttribute.color = Color.blue;
            skillName.color = Color.blue;
        }
        else if (skillData.m_attribute == Attribute.Electricity)
        {
            skillAttribute.text = "전기";
            skillAttribute.color = Color.yellow;
            skillName.color = Color.yellow;
        }
        else if (skillData.m_attribute == Attribute.Magnetic)
        {
            skillAttribute.text = "자기장";
            skillAttribute.color = new Color(0.6f, 0, 1, 1);
            skillName.color = new Color(0.6f, 0, 1, 1);
        }
        else if (skillData.m_attribute == Attribute.Void)
        {
            skillAttribute.text = "무속성";
            skillAttribute.color = Color.black;
            skillName.color = Color.black;
        }

        if (skillData.m_skillRange == 1000)
        {
            skillRange.text = "없음";
        }
        else
        {
            skillRange.text = skillData.m_skillRange.ToString();
        }
        skillElementAmp.text = skillData.m_elementAmp.ToString() + "%";
    }

    public void SetTripod()
    {
        for (int i = 0; i < tripodButtons.Count; i++)
        {
            tripodButtons[i].SetIndex(i);
            tripodButtons[i].SetActivatedIcon();
            tripodButtons[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = skillData.m_tripodDatas[i].tripodName;
        }
    }
}
