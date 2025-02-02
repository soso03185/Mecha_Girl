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
/// 튜토리얼의 내용과 함수들을 관리하는 싱글톤 스크립트
/// </summary>
public class TutorialQuest : Singleton<TutorialQuest>
{
    public Transform[] m_QuestTr;
    [HideInInspector] public List<Transform> m_EquipSkillBtns = new List<Transform>();
    [HideInInspector] public List<Transform> m_SkillSlotBtns = new List<Transform>();
    [HideInInspector] public List<GameObject> m_SkillExplainObjs;

    string[] m_QuestDesc = { 
        "스킬이 비어있습니다!\r\n스킬창을 열어주세요.", // 0
        "[+] 버튼을 눌러 스킬을 선택해주세요.",
        "비어있는 슬롯에 스킬을 장착해주세요.",
        "[+] 버튼을 눌러 스킬을 선택해주세요.",
        "비어있는 슬롯에 스킬을 장착해주세요.", 
        "[+] 버튼을 눌러 스킬을 선택해주세요.", // 5
        "비어있는 슬롯에 스킬을 장착해주세요.",
        "전투를 시작해보겠습니다.\r\n<color=#FF0000>스킬은 자동으로 사용</color>됩니다.",
        "스킬을 변경하면 <color=#FF0000>스테이지가 재시작</color>되니 주의하세요!",
        "퀘스트 완료 버튼을 눌러서\r\n보상을 받으세요.",
        "<color=#FFD700>스킬</color>을 하나 더 얻었습니다.\r\n스킬을 추가해보세요.", // 10
        "새로운 스킬을 장착해보세요.",
        "",
        "",
        "",
        "몬스터가 점점 강해지는게 느껴집니다.", // 15
        "스킬창을 열어보세요!",
        "스킬은 강화가 가능합니다.\r\n스킬 설명을 통해 최적의 스킬을 선택하세요.",
        "<color=#00BFFF>스킬 포인트</color>를 소모하여 강화를 해보세요!",
        "",
        "스킬을 일정수준 강화하면 스킬을 변형시킬 수 있는\r\n트라이포드 기능이 오픈됩니다.", // 20
        "", // 적용
        "", // 나가기 (우측 상단)
        "", // X 버튼
        "", // 세이브
        "스탯창을 열어보세요!",  ///////// 25
        "퀘스트 보상으로 받은 재화로 업그레이드가 가능합니다!",  
        "", // 나가기 버튼
        "튜토리얼이 모두 끝났습니다!",
        "본격적인 게임을 시작합니다." // 29
    };

    string[] m_SynergyDesc = {
        "<color=#00bfff>시너지 효과</color>가 발생했습니다!",
        "몬스터가 <color=#FF0000>상태 이상</color>에 걸렸을때,\r\n각 <color=#FF0000>스킬이 갖고 있는 속성</color>에 따라 시너지가 발생할 수 있습니다! ",
        "시너지를 적절히 사용하여 더 많은 데미지를 주면\r\n스테이지를 클리어하기 유리합니다!"
    };

    string[] m_MedkitDesc = {
        "적들은 가끔 <color=#00bfff>마나 키트</color>를 드랍합니다!",
        "필드의 마나 키트를 <color=#00bfff>터치</color>하여 \r\n기계소녀의 마나를 공급해주세요!"
    };

    string[] m_ImmuneDesc = {
        "몬스터가 <color=#9900CC>상태이상 면역</color>입니다!",
        "몬스터의 속성에 따라 시너지 효과가 무효화 될 수 있으니,\r\n속성에 맞추어 스킬을 재설정하세요!"
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

    private bool isTextPlaying = false;    // 연출 중인지 확인하는 플래그

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
        // 이벤트 구독 
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
        // 이벤트 해제
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
        if (isTextPlaying) return; // 이미 연출 중이면 함수 종료
      
        isTextPlaying = true; // 연출 시작

        float fadeInDuration = 0.7f;
        float fadeOutDuration = 0.7f;
        float stayDuration = 0.5f;

        // 텍스트 오브젝트 활성화
        m_TutoAlertText.gameObject.SetActive(_isActive);

        // 초기 알파값 설정
        m_TutoAlertText.color = new Color(m_TutoAlertText.color.r, m_TutoAlertText.color.g, m_TutoAlertText.color.b, 0);

        // DOTween 시퀀스 생성
        DG.Tweening.Sequence textSequence = DOTween.Sequence();

        // 페이드 인
        textSequence.Append(m_TutoAlertText.DOFade(1f, fadeInDuration)).SetUpdate(true);

        // 텍스트가 머무는 시간
        textSequence.AppendInterval(stayDuration);

        // 페이드 아웃
        textSequence.Append(m_TutoAlertText.DOFade(0f, fadeOutDuration)).SetUpdate(true);

        // 시퀀스 완료 시 텍스트 비활성화 및 플래그 해제
        textSequence.OnComplete(() =>
        {
            m_TutoAlertText.gameObject.SetActive(!_isActive);
            isTextPlaying = false; // 연출이 끝났으므로 다시 실행 가능
        });

        m_TutoAlertText.DOKill();
    }

    public void SetEquipBtns()
    {
        m_QuestTr[1] = m_EquipSkillBtns[0].transform;
        m_QuestTr[3] = m_EquipSkillBtns[1].transform;
        m_QuestTr[5] = m_EquipSkillBtns[2].transform;
        m_QuestTr[11] = m_EquipSkillBtns[3].transform;

        // 스킬 설명 창
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

        //if (TutorialManager.m_TutorialIndex == 0 || TutorialManager.m_TutorialIndex == 9) // Player 등장할때
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

            // parent 이동
            m_QuestParentTr = tutTouchObj.parent;
            tutTouchObj.parent = m_DimPanel.m_QuestObjParent.transform;
        }
        Time.timeScale = 0.00000001f; // 시간 정지
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

        if (!m_isSynergyProgress) // 시너지 튜토리얼이 진행중이 아니라면
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

        Time.timeScale = 0.00000001f; // 시간 정지
    }

    public void SynergyTut_End()
    {
        if (!m_isSynergyProgress) return;

        m_OtherTutPanelObj.SetActive(false);
        m_DimPanel.gameObject.SetActive(false);
        m_DimPanel.ShowArrowIcon(false);

        m_isSynergyProgress = false;

        if (!m_isTutProgress) // 일반 튜토리얼이 진행중이 아니라면
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

        Time.timeScale = 0.00000001f; // 시간 정지
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
        Time.timeScale = 0.00000001f; // 시간 정지
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
        Canvas.ForceUpdateCanvases(); // 레이아웃 업데이트 강제 실행
        yield return null; // 한 프레임 대기

        // 여기서 위치가 전부 세팅된 이후에 작업 실행
        TutorialStart();
        yield break;
    }
}
