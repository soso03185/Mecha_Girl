using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ʃ�丮���� ����� �Լ����� �����ϴ� �̱��� ��ũ��Ʈ
/// </summary>
public class TutorialQuest : Singleton<TutorialQuest>
{
    public Transform[] m_QuestTr;
    [HideInInspector] public List<Transform> m_EquipSkillBtns = new List<Transform>();
    [HideInInspector] public List<Transform> m_SkillSlotBtns = new List<Transform>();
    [HideInInspector] public List<GameObject> m_SkillExplainObjs;

    string[] m_QuestDesc = { 
        "��ų�� ����ֽ��ϴ�!\r\n��ųâ�� �����ּ���.", // 0
        "[+] ��ư�� ���� ��ų�� �������ּ���.",
        "����ִ� ���Կ� ��ų�� �������ּ���.",
        "[+] ��ư�� ���� ��ų�� �������ּ���.",
        "����ִ� ���Կ� ��ų�� �������ּ���.", 
        "[+] ��ư�� ���� ��ų�� �������ּ���.", // 5
        "����ִ� ���Կ� ��ų�� �������ּ���.",
        "������ �����غ��ڽ��ϴ�.\r\n<color=#FF0000>��ų�� �ڵ����� ���</color>�˴ϴ�.",
        "��ų�� �����ϸ� <color=#FF0000>���������� �����</color>�Ǵ� �����ϼ���!",
        "����Ʈ �Ϸ� ��ư�� ������\r\n������ ��������.",
        "<color=#FFD700>��ų</color>�� �ϳ� �� ������ϴ�.\r\n��ų�� �߰��غ�����.", // 10
        "���ο� ��ų�� �����غ�����.",
        "",
        "",
        "",
        "���Ͱ� ���� �������°� �������ϴ�.", // 15
        "��ųâ�� �������!",
        "��ų�� ��ȭ�� �����մϴ�.\r\n��ų ������ ���� ������ ��ų�� �����ϼ���.",
        "<color=#00BFFF>��ų ����Ʈ</color>�� �Ҹ��Ͽ� ��ȭ�� �غ�����!",
        "",
        "��ų�� �������� ��ȭ�ϸ� ��ų�� ������ų �� �ִ�\r\nƮ�������� ����� ���µ˴ϴ�.", // 20
        "", // ����
        "", // ������ (���� ���)
        "", // X ��ư
        "", // ���̺�
        "����â�� �������!",  ///////// 25
        "����Ʈ �������� ���� ��ȭ�� ���׷��̵尡 �����մϴ�!",  
        "", // ������ ��ư
        "Ʃ�丮���� ��� �������ϴ�!",
        "�������� ������ �����մϴ�." // 29
    };

    string[] m_SynergyDesc = {
        "<color=#00bfff>�ó��� ȿ��</color>�� �߻��߽��ϴ�!",
        "���Ͱ� <color=#FF0000>���� �̻�</color>�� �ɷ�����,\r\n�� <color=#FF0000>��ų�� ���� �ִ� �Ӽ�</color>�� ���� �ó����� �߻��� �� �ֽ��ϴ�! ",
        "�ó����� ������ ����Ͽ� �� ���� �������� �ָ�\r\n���������� Ŭ�����ϱ� �����մϴ�!"
    };

    string[] m_MedkitDesc = {
        "������ ���� <color=#00bfff>���� ŰƮ</color>�� ����մϴ�!",
        "�ʵ��� ���� ŰƮ�� <color=#00bfff>��ġ</color>�Ͽ� \r\n���ҳ��� ������ �������ּ���!"
    };

    string[] m_ImmuneDesc = {
        "���Ͱ� <color=#9900CC>�����̻� �鿪</color>�Դϴ�!",
        "������ �Ӽ��� ���� �ó��� ȿ���� ��ȿȭ �� �� ������,\r\n�Ӽ��� ���߾� ��ų�� �缳���ϼ���!"
    };

    [Space(10)]
    public TutorialPanel m_DimPanel;
    public GameObject m_AlphaPanel;
    public TextMeshProUGUI m_TutoAlertText;
    public TextMeshProUGUI m_TutorialText;
    public GameObject m_OtherTutPanelObj;
    public GameObject m_NoObjTutPanel;
    public GameObject m_AutoSkillInfo;
    public GameObject m_ManaKitInfo;

    private bool isTextPlaying = false;    // ���� ������ Ȯ���ϴ� �÷���

    Transform m_QuestParentTr;
    bool m_isTutProgress = false;
    bool m_isMedkitProgress = false;
    
    [HideInInspector]
    public bool m_isSynergyProgress = false;

    public void Start()
    {
        if (TutorialManager.m_TutorialIndex <= 0)
            m_AlphaPanel.SetActive(true);
    }

    public void OnEnable()
    {
        // �̺�Ʈ ���� 
        TutorialManager.m_OnTutorialStart += TutStartCoroutine; 
        TutorialManager.m_OnNextStepButton += NextStepButton;

        TutorialManager.m_OnSynergyTextStart += SynergyTut_Start;
        TutorialManager.m_OnSynergyTextEnd += SynergyTut_End;

        TutorialManager.m_OnMedKitStart += MedKitTutStart;
        TutorialManager.m_OnMedKitEnd += MedKitTutEnd;

        TutorialManager.m_OnSynergyImmuneStart += ImmuneTutStart;
        TutorialManager.m_OnSynergyImmuneEnd += ImmuneTutEnd;
    }
     
    public void OnDisable()
    {
        // �̺�Ʈ ����
        TutorialManager.m_OnTutorialStart -= TutStartCoroutine;
        TutorialManager.m_OnNextStepButton -= NextStepButton;
                        
        TutorialManager.m_OnSynergyTextStart -= SynergyTut_Start;
        TutorialManager.m_OnSynergyTextEnd -= SynergyTut_End;
                        
        TutorialManager.m_OnMedKitStart -= MedKitTutStart;
        TutorialManager.m_OnMedKitEnd -= MedKitTutEnd;
                        
        TutorialManager.m_OnSynergyImmuneStart -= ImmuneTutStart;
        TutorialManager.m_OnSynergyImmuneEnd -= ImmuneTutEnd;
    }
    
    public void OffTutorial()
    {
        var tutTouchObj = m_QuestTr[TutorialManager.m_TutorialIndex];
        m_QuestParentTr = tutTouchObj.parent;

        if (tutTouchObj != null)
        {
            m_NoObjTutPanel.SetActive(false);
            m_QuestTr[TutorialManager.m_TutorialIndex++].parent = m_QuestParentTr;
        }
        else
        {
            TutorialManager.m_TutorialIndex++;
        }

        TutorialManager.m_TutorialIndex = m_QuestTr.Length;
        gameObject.SetActive(false);
        Time.timeScale = Managers.Data.m_timeScale;
    }

    public void AlertTextSetActive(bool _isActive = true)
    {
        if (isTextPlaying) return; // �̹� ���� ���̸� �Լ� ����
      
        isTextPlaying = true; // ���� ����

        float fadeInDuration = 0.7f;
        float fadeOutDuration = 0.7f;
        float stayDuration = 0.5f;

        // �ؽ�Ʈ ������Ʈ Ȱ��ȭ
        m_TutoAlertText.gameObject.SetActive(_isActive);

        // �ʱ� ���İ� ����
        m_TutoAlertText.color = new Color(m_TutoAlertText.color.r, m_TutoAlertText.color.g, m_TutoAlertText.color.b, 0);

        // DOTween ������ ����
        DG.Tweening.Sequence textSequence = DOTween.Sequence();

        // ���̵� ��
        textSequence.Append(m_TutoAlertText.DOFade(1f, fadeInDuration)).SetUpdate(true);

        // �ؽ�Ʈ�� �ӹ��� �ð�
        textSequence.AppendInterval(stayDuration);

        // ���̵� �ƿ�
        textSequence.Append(m_TutoAlertText.DOFade(0f, fadeOutDuration)).SetUpdate(true);

        // ������ �Ϸ� �� �ؽ�Ʈ ��Ȱ��ȭ �� �÷��� ����
        textSequence.OnComplete(() =>
        {
            m_TutoAlertText.gameObject.SetActive(!_isActive);
            isTextPlaying = false; // ������ �������Ƿ� �ٽ� ���� ����
        });

        m_TutoAlertText.DOKill();
    }

    public void SetEquipBtns()
    {
        m_QuestTr[1] = m_EquipSkillBtns[0].transform;
        m_QuestTr[3] = m_EquipSkillBtns[1].transform;
        m_QuestTr[5] = m_EquipSkillBtns[2].transform;
        m_QuestTr[11] = m_EquipSkillBtns[3].transform;

        // ��ų ���� â
        m_QuestTr[17] = m_SkillExplainObjs[1].transform;
    }

    public void SetSlotBtns()
    {
        m_QuestTr[2] = m_SkillSlotBtns[0].transform;
        m_QuestTr[4] = m_SkillSlotBtns[1].transform;
        m_QuestTr[6] = m_SkillSlotBtns[2].transform;
        m_QuestTr[12] = m_SkillSlotBtns[2].transform;
    }

    public void SetTutorialText(string _text)
    {
        m_TutorialText.text = _text;
    }

    public void TutorialStart()
    {
        if (Managers.Data.currentStageLevel >= 4)
            TutorialManager.m_TutorialIndex = m_QuestTr.Length;

        if (TutorialManager.m_TutorialIndex >= m_QuestTr.Length)
        {
          //  m_AutoSkillInfo.SetActive(false);
            m_AlphaPanel.SetActive(false);
            m_NoObjTutPanel.SetActive(false);
            return;
        }

        //if (TutorialManager.m_TutorialIndex == 0 || TutorialManager.m_TutorialIndex == 9) // Player �����Ҷ�
        //    m_AutoSkillInfo.SetActive(true);

        //if (Managers.Data.currentStageLevel >= 3)
        //    m_AutoSkillInfo.SetActive(false);

        var tutTouchObj = m_QuestTr[TutorialManager.m_TutorialIndex];

        m_isTutProgress = true;
        SetTutorialText(m_QuestDesc[TutorialManager.m_TutorialIndex]);
        m_DimPanel.gameObject.SetActive(true);

        if (tutTouchObj == null)
        {
            m_DimPanel.ShowArrowIcon(false);
            m_NoObjTutPanel.SetActive(true);
        }
        else
        {
            m_DimPanel.MoveArrowIcon(tutTouchObj.position);
            m_NoObjTutPanel.SetActive(false);

            // parent �̵�
            m_QuestParentTr = tutTouchObj.parent;
            tutTouchObj.parent = m_DimPanel.m_QuestObjParent.transform;
        }
        Time.timeScale = 0.00000001f; // �ð� ����
    }

    public void NextStepButton()
    {
        if (!m_isTutProgress) return;

        m_isTutProgress = false;
        m_DimPanel.gameObject.SetActive(false);
        m_DimPanel.ShowArrowIcon(false);

        var tutTouchObj = m_QuestTr[TutorialManager.m_TutorialIndex];
        if (tutTouchObj != null)
        {
            m_NoObjTutPanel.SetActive(false);
            m_QuestTr[TutorialManager.m_TutorialIndex++].parent = m_QuestParentTr;
        }
        else
        {
            TutorialManager.m_TutorialIndex++;
        }

        if (!m_isSynergyProgress) // �ó��� Ʃ�丮���� �������� �ƴ϶��
            Time.timeScale = Managers.Data.m_timeScale;

        if (TutorialManager.m_TutorialIndex >= m_QuestTr.Length)
        {
            m_AlphaPanel.SetActive(false);
            m_NoObjTutPanel.SetActive(false);
        }
    }

    public void SynergyTut_Start()
    {
        m_DimPanel.gameObject.SetActive(true);
        m_DimPanel.MoveArrowIcon(m_OtherTutPanelObj.transform.position);
        m_DimPanel.ShowArrowIcon(false);

        m_isSynergyProgress = true;
        m_OtherTutPanelObj.SetActive(true);
        SetTutorialText(m_SynergyDesc[TutorialManager.m_SynergyIndex++]);

        Time.timeScale = 0.00000001f; // �ð� ����
    }

    public void SynergyTut_End()
    {
        if (!m_isSynergyProgress) return;

        m_OtherTutPanelObj.SetActive(false);
        m_DimPanel.gameObject.SetActive(false);
        m_DimPanel.ShowArrowIcon(false);

        m_isSynergyProgress = false;

        if (!m_isTutProgress) // �Ϲ� Ʃ�丮���� �������� �ƴ϶��
            Time.timeScale = Managers.Data.m_timeScale;
    }

    public void MedKitTutStart()
    {
   //     if (Managers.Data.currentStageLevel == 1) return;
        if (m_isTutProgress) return;
        if (m_isSynergyProgress) return;

        m_isMedkitProgress = true;

        SetTutorialText(m_MedkitDesc[TutorialManager.m_MedKitIndex++]);

        m_DimPanel.gameObject.SetActive(true);
        m_DimPanel.MoveArrowIcon(m_OtherTutPanelObj.transform.position);
        m_DimPanel.ShowArrowIcon(false);

        m_OtherTutPanelObj.SetActive(true);

        Time.timeScale = 0.00000001f; // �ð� ����
    }

    public void MedKitTutEnd()
    {
   //     if (Managers.Data.currentStageLevel == 1) return;
        if (!m_isMedkitProgress) return;
        if (m_isSynergyProgress) return;
        if (m_isTutProgress) return;
        if (m_MedkitDesc.Length < TutorialManager.m_MedKitIndex) return;

        m_isMedkitProgress = false;

        m_DimPanel.ShowArrowIcon(false);
        m_DimPanel.gameObject.SetActive(false);
        m_OtherTutPanelObj.SetActive(false);

        Time.timeScale = Managers.Data.m_timeScale;
    }

    public void ImmuneTutStart()
    {
        if (Managers.Data.currentStageLevel == 1) return;
        if (m_isSynergyProgress) return;
        if (m_isMedkitProgress) return;
        if (m_isTutProgress) return;

        SetTutorialText(m_ImmuneDesc[TutorialManager.m_ImmuneIndex++]);
        m_DimPanel.gameObject.SetActive(true);
        m_DimPanel.ShowArrowIcon(false);
        m_OtherTutPanelObj.SetActive(true);
        Time.timeScale = 0.00000001f; // �ð� ����
    }
    
    public void ImmuneTutEnd()
    {
        if (Managers.Data.currentStageLevel == 1) return;
        if (m_isSynergyProgress) return;
        if (m_isMedkitProgress) return;
        if (m_isTutProgress) return;
        if (m_ImmuneDesc.Length < TutorialManager.m_ImmuneIndex) return;

        m_DimPanel.ShowArrowIcon(false);
        m_DimPanel.gameObject.SetActive(false);
        m_OtherTutPanelObj.SetActive(false);
        Time.timeScale = Managers.Data.m_timeScale;
    }

    public void TutStartCoroutine() => StartCoroutine(DelayedSetup());
    
    private IEnumerator DelayedSetup()
    {
        Canvas.ForceUpdateCanvases(); // ���̾ƿ� ������Ʈ ���� ����
        yield return null; // �� ������ ���

        // ���⼭ ��ġ�� ���� ���õ� ���Ŀ� �۾� ����
        TutorialStart();
        yield break;
    }
}
