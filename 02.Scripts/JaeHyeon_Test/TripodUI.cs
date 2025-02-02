using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// 트라이포드 UI를 관리하는 스크립트
/// </summary>
public class TripodUI : MonoBehaviour
{
    [SerializeField] SkillTabManager m_skillTabManager;
    [SerializeField] GameObject m_TripodPopUp;
    [SerializeField] Button m_TripodPopUp_ReleaseButton;
    [SerializeField] TextMeshProUGUI m_TripodPopUp_ExplainText;
    [SerializeField] TextMeshProUGUI m_TripodPopUp_TripodName;
    [SerializeField] Image m_TripodPopUp_Icon;

    [SerializeField] TripodButton[] m_CheckText_Lv1 = new TripodButton[3];
    [SerializeField] TripodButton[] m_CheckText_Lv2 = new TripodButton[3];
    [SerializeField] TripodButton[] m_CheckText_Lv3 = new TripodButton[3];
    [SerializeField] GameObject[] m_Dim = new GameObject[3];

    public int m_TriPod_Lv1 = 0;
    public int m_TriPod_Lv2 = 0;
    public int m_TriPod_Lv3 = 0;

    int m_LevelValue;
    int m_TypeValue;

    int m_SkillLevel;

    public RectTransform m_contentPos;
    private Vector3 m_initPos;

    public bool m_isTopSelected = false;
    public bool m_isMidSelected = false;
    public bool m_isBottomSelected = false;

    public void Awake()
    {
        m_initPos = m_contentPos.localPosition;
    }
    public void OnEnable()
    {
        m_SkillLevel = SkillTabManager.Instance.skillData.m_skillLevel;

        SetTripodLock(m_SkillLevel);

        m_TriPod_Lv1 = SkillTabManager.Instance.activeSkill.skillData.m_tripod.firstSlot;
        m_TriPod_Lv2 = SkillTabManager.Instance.activeSkill.skillData.m_tripod.secondSlot;
        m_TriPod_Lv3 = SkillTabManager.Instance.activeSkill.skillData.m_tripod.thirdSlot;

        //for (int i = 0; i < 3; i++)
        //{
        //    m_CheckText_Lv1[i].SetIconLight();
        //    m_CheckText_Lv2[i].SetIconLight();
        //    m_CheckText_Lv3[i].SetIconLight();
        //}

        if (m_TriPod_Lv1 - 1 >= 0) m_CheckText_Lv1[m_TriPod_Lv1 - 1].SetIconLight();
        if (m_TriPod_Lv2 - 1 >= 0) m_CheckText_Lv2[m_TriPod_Lv2 - 1].SetIconLight();
        if (m_TriPod_Lv3 - 1 >= 0) m_CheckText_Lv3[m_TriPod_Lv3 - 1].SetIconLight();

        m_contentPos.localPosition = m_initPos;
    }

    public void Update()
    {
        if (m_TriPod_Lv1 != 0)
        {
            m_isTopSelected = true;
        }
        else
        {
            m_isTopSelected = false;
            m_isMidSelected = false;
        }

        if (m_TriPod_Lv2 != 0)
        {
            m_isMidSelected = true;
        }
        else
        {
            m_isMidSelected = false;
            m_isBottomSelected = false;
        }

        if (m_TriPod_Lv3 != 0)
        {
            m_isBottomSelected = true;
        }
        else
        {
            m_isBottomSelected = false;
        }

        if (m_isTopSelected)
        {
            for (int i = 0; i < 3; i++)
            {
                m_CheckText_Lv1[i].SetIconDark();
                if (SkillTabManager.Instance.skillData.m_skillLevel >= 6)
                {
                    m_CheckText_Lv2[i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    m_CheckText_Lv2[i].GetComponent<Button>().interactable = false;
                }
            }
            m_CheckText_Lv1[m_TriPod_Lv1 - 1].SetIconLight();

            if (!m_isMidSelected)
            {
                for (int i = 0; i < 3; i++)
                {
                    m_CheckText_Lv2[i].SetIconLight();
                }
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                m_CheckText_Lv2[i].GetComponent<Button>().interactable = false;
                m_CheckText_Lv3[i].GetComponent<Button>().interactable = false;

                m_CheckText_Lv1[i].SetIconLight();
                m_CheckText_Lv2[i].SetIconDark();
                m_CheckText_Lv3[i].SetIconDark();
            }
        }

        if (m_isMidSelected)
        {
            for (int i = 0; i < 3; i++)
            {
                m_CheckText_Lv2[i].SetIconDark();
                if (SkillTabManager.Instance.skillData.m_skillLevel >= 9)
                {
                    m_CheckText_Lv3[i].GetComponent<Button>().interactable = true;
                }
                else
                {
                    m_CheckText_Lv3[i].GetComponent<Button>().interactable = false;
                }

            }
            m_CheckText_Lv2[m_TriPod_Lv2 - 1].SetIconLight();

            if (!m_isBottomSelected)
            {
                for (int i = 0; i < 3; i++)
                {
                    m_CheckText_Lv3[i].SetIconLight();
                }
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                m_CheckText_Lv3[i].GetComponent<Button>().interactable = false;
                m_CheckText_Lv3[i].SetIconDark();
            }
        }

        if (m_isBottomSelected)
        {
            for (int i = 0; i < 3; i++)
            {
                m_CheckText_Lv3[i].SetIconDark();
            }
            m_CheckText_Lv3[m_TriPod_Lv3 - 1].SetIconLight();
        }

    }

    public void ReleaseButton()
    {
        switch (m_LevelValue)
        {
            case 1:
                for (int i = 0; i < 3; i++)
                {
                    m_CheckText_Lv1[i].SetIconLight();
                }
                SkillTabManager.Instance.activeSkill.skillData.SetTripod(1, 0);
                SkillTabManager.Instance.activeSkill.skillData.SetTripod(2, 0);
                SkillTabManager.Instance.activeSkill.skillData.SetTripod(3, 0);
                m_TriPod_Lv1 = 0;
                m_TriPod_Lv2 = 0;
                m_TriPod_Lv3 = 0;
                break;

            case 2:
                for (int i = 0; i < 3; i++)
                {
                    m_CheckText_Lv2[i].SetIconLight();
                }
                SkillTabManager.Instance.activeSkill.skillData.SetTripod(2, 0);
                SkillTabManager.Instance.activeSkill.skillData.SetTripod(3, 0);
                m_TriPod_Lv2 = 0;
                m_TriPod_Lv3 = 0;
                break;

            case 3:
                for (int i = 0; i < 3; i++)
                {
                    m_CheckText_Lv3[i].SetIconLight();
                }
                SkillTabManager.Instance.activeSkill.skillData.SetTripod(3, 0);
                m_TriPod_Lv3 = 0;
                break;
        }
        SkillTabManager.Instance.activeSkill.skillData.SetSkillExplanation();
        SkillTabManager.Instance.SetSkillTab();
        m_TripodPopUp.SetActive(false);
    }

    public void ApplyButton()
    {
        switch (m_LevelValue)
        {
            case 1:
                foreach (var child in m_CheckText_Lv1)
                {
                    child.SetIconDark();
                }
                m_TriPod_Lv1 = m_TypeValue;
                m_CheckText_Lv1[m_TypeValue - 1].SetIconLight();
                SkillTabManager.Instance.activeSkill.skillData.SetTripod(1, m_TriPod_Lv1);

                if (TutorialQuest.Instance != null)
                    TutorialQuest.Instance.m_DimPanel.ShowBotArrowIcon(true);
                TutorialManager.NextStepButton();
                TutorialManager.TutorialStart(22);
                break;

            case 2:
                foreach (var child in m_CheckText_Lv2)
                {
                    child.SetIconDark();
                }
                m_TriPod_Lv2 = m_TypeValue;
                m_CheckText_Lv2[m_TypeValue - 1].SetIconLight();
                SkillTabManager.Instance.activeSkill.skillData.SetTripod(2, m_TriPod_Lv2);
                break;

            case 3:
                foreach (var child in m_CheckText_Lv3)
                {
                    child.SetIconDark();
                }
                m_TriPod_Lv3 = m_TypeValue;
                m_CheckText_Lv3[m_TypeValue - 1].SetIconLight();
                SkillTabManager.Instance.activeSkill.skillData.SetTripod(3, m_TriPod_Lv3);
                break;
        }
        SkillTabManager.Instance.activeSkill.skillData.SetSkillExplanation();
        SkillTabManager.Instance.SetSkillTab();
        m_TripodPopUp.SetActive(false);
    }

    public void SetTripodExplainText(int _tripodLevelValue)
    {
        m_TripodPopUp_ExplainText.text = m_skillTabManager.skillData.m_tripodDatas[(_tripodLevelValue - 1) * 3 + (m_TypeValue - 1)].tripodExplanation;
        m_TripodPopUp_Icon.sprite = m_skillTabManager.skillData.m_tripodDatas[(_tripodLevelValue - 1) * 3 + (m_TypeValue - 1)].tripodSprite;
        m_TripodPopUp_TripodName.text = m_skillTabManager.skillData.m_tripodDatas[(_tripodLevelValue - 1) * 3 + (m_TypeValue - 1)].tripodName;
    }

    public void TripodType_1(int num)
    {
        if (m_TriPod_Lv1 == num)
            m_TripodPopUp_ReleaseButton.interactable = true;
        else
            m_TripodPopUp_ReleaseButton.interactable = false;

        m_TripodPopUp.SetActive(true);
        m_TypeValue = num;
        m_LevelValue = 1;

        SetTripodExplainText(1);

        TutorialManager.NextStepButton();
        TutorialManager.TutorialStart(21);
    }

    public void TripodType_2(int num)
    {
        if (m_TriPod_Lv2 == num)
            m_TripodPopUp_ReleaseButton.interactable = true;
        else
            m_TripodPopUp_ReleaseButton.interactable = false;

        m_TripodPopUp.SetActive(true);
        m_TypeValue = num;
        m_LevelValue = 2;

        SetTripodExplainText(2);
    }

    public void TripodType_3(int num)
    {
        if (m_TriPod_Lv3 == num) m_TripodPopUp_ReleaseButton.interactable = true;
        else m_TripodPopUp_ReleaseButton.interactable = false;

        m_TripodPopUp.SetActive(true);
        m_TypeValue = num;
        m_LevelValue = 3;

        SetTripodExplainText(3);
    }

    public void SetTripodLock(int skillLevel)
    {
        m_SkillLevel = skillLevel;
        if (skillLevel >= 1 && skillLevel <= 2)
        {
            skillLevel = 1;
        }
        else if (skillLevel >= 3 && skillLevel <= 5)
        {
            skillLevel = 3;
        }
        else if (skillLevel >= 6 && skillLevel <= 8)
        {
            skillLevel = 6;
        }
        else
            skillLevel = 9;

        for (int i = 0; i < 3; i++)
        {
            //m_CheckText_Lv1[i].SetIconLight();
            //m_CheckText_Lv2[i].SetIconLight();
            //m_CheckText_Lv3[i].SetIconLight();
        }

        switch (skillLevel)
        {
            case 1:
                for (int i = 0; i < 3; i++)
                {
                    m_CheckText_Lv1[i].GetComponent<Button>().interactable = false;
                    //m_Lock_Lv1[i].gameObject.SetActive(true);
                    m_CheckText_Lv1[i].SetDeactivatedIcon();

                    m_CheckText_Lv2[i].GetComponent<Button>().interactable = false;
                    //m_Lock_Lv2[i].gameObject.SetActive(true);
                    m_CheckText_Lv2[i].SetDeactivatedIcon();

                    m_CheckText_Lv3[i].GetComponent<Button>().interactable = false;
                    //m_Lock_Lv3[i].gameObject.SetActive(true);
                    m_CheckText_Lv3[i].SetDeactivatedIcon();

                    m_Dim[i].gameObject.SetActive(true);
                }
                break;
            case 3:
                for (int i = 0; i < 3; i++)
                {
                    m_CheckText_Lv1[i].GetComponent<Button>().interactable = true;
                    //m_Lock_Lv1[i].gameObject.SetActive(false);
                    m_CheckText_Lv1[i].SetActivatedIcon();
                    m_CheckText_Lv2[i].SetDeactivatedIcon();
                    m_CheckText_Lv3[i].SetDeactivatedIcon();

                    if (SkillTabManager.Instance.activeSkill.skillData.m_tripod.firstSlot - 1 >= 0)
                        m_CheckText_Lv1[SkillTabManager.Instance.activeSkill.skillData.m_tripod.firstSlot - 1].SetIconLight();
                }
                m_Dim[0].gameObject.SetActive(false);
                m_Dim[1].gameObject.SetActive(true);
                m_Dim[2].gameObject.SetActive(true);

                break;
            case 6:
                for (int i = 0; i < 3; i++)
                {
                    m_CheckText_Lv1[i].GetComponent<Button>().interactable = true;
                    //m_Lock_Lv1[i].gameObject.SetActive(false);
                    m_CheckText_Lv1[i].SetActivatedIcon();

                    m_CheckText_Lv2[i].GetComponent<Button>().interactable = true;
                    //m_Lock_Lv2[i].gameObject.SetActive(false);
                    m_CheckText_Lv2[i].SetActivatedIcon();
                    m_CheckText_Lv3[i].SetDeactivatedIcon();

                    if (SkillTabManager.Instance.activeSkill.skillData.m_tripod.firstSlot - 1 >= 0)
                        m_CheckText_Lv1[SkillTabManager.Instance.activeSkill.skillData.m_tripod.firstSlot - 1].SetIconLight();
                    if (SkillTabManager.Instance.activeSkill.skillData.m_tripod.secondSlot - 1 >= 0)
                        m_CheckText_Lv2[SkillTabManager.Instance.activeSkill.skillData.m_tripod.secondSlot - 1].SetIconLight();
                }
                m_Dim[0].gameObject.SetActive(false);
                m_Dim[1].gameObject.SetActive(false);
                m_Dim[2].gameObject.SetActive(true);
                break;
            case 9:
                for (int i = 0; i < 3; i++)
                {
                    m_CheckText_Lv1[i].GetComponent<Button>().interactable = true;
                    //m_Lock_Lv1[i].gameObject.SetActive(false);
                    m_CheckText_Lv1[i].SetActivatedIcon();

                    m_CheckText_Lv2[i].GetComponent<Button>().interactable = true;
                    //m_Lock_Lv2[i].gameObject.SetActive(false);
                    m_CheckText_Lv2[i].SetActivatedIcon();

                    m_CheckText_Lv3[i].GetComponent<Button>().interactable = true;
                    //m_Lock_Lv3[i].gameObject.SetActive(false);
                    m_CheckText_Lv3[i].SetActivatedIcon();

                    if (SkillTabManager.Instance.activeSkill.skillData.m_tripod.firstSlot - 1 >= 0)
                        m_CheckText_Lv1[SkillTabManager.Instance.activeSkill.skillData.m_tripod.firstSlot - 1].SetIconLight();
                    if (SkillTabManager.Instance.activeSkill.skillData.m_tripod.secondSlot - 1 >= 0)
                        m_CheckText_Lv2[SkillTabManager.Instance.activeSkill.skillData.m_tripod.secondSlot - 1].SetIconLight();
                    if (SkillTabManager.Instance.activeSkill.skillData.m_tripod.thirdSlot - 1 >= 0)
                        m_CheckText_Lv3[SkillTabManager.Instance.activeSkill.skillData.m_tripod.thirdSlot - 1].SetIconLight();
                }
                m_Dim[0].gameObject.SetActive(false);
                m_Dim[1].gameObject.SetActive(false);
                m_Dim[2].gameObject.SetActive(false);
                break;
        }
    }

    public void ResetTripod(int value1, int value2, int value3)
    {
        for (int i = 0; i < 3; i++)
        {
            m_CheckText_Lv1[i].SetIconLight();
            m_CheckText_Lv2[i].SetIconLight();
            m_CheckText_Lv3[i].SetIconLight();
            m_TriPod_Lv1 = 0;
            m_TriPod_Lv2 = 0;
            m_TriPod_Lv3 = 0;
            SkillTabManager.Instance.activeSkill.skillData.SetTripod(1, 0);
            SkillTabManager.Instance.activeSkill.skillData.SetTripod(2, 0);
            SkillTabManager.Instance.activeSkill.skillData.SetTripod(3, 0);
        }
    }

    public void SetTripod(int value1, int value2, int value3)
    {
        for (int i = 0; i < 3; i++)
        {
            m_TriPod_Lv1 = value1;
            m_TriPod_Lv2 = value2;
            m_TriPod_Lv3 = value3;
            SkillTabManager.Instance.activeSkill.skillData.SetTripod(1, value1);
            SkillTabManager.Instance.activeSkill.skillData.SetTripod(2, value2);
            SkillTabManager.Instance.activeSkill.skillData.SetTripod(3, value3);
        }
    }
}
