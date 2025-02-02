using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기본 튜토리얼을 제외한 다른 이벤트 튜토리얼 진행 시,
/// 터치하여 넘어가는 이벤트 처리
/// </summary>
public class Other_TutText : MonoBehaviour
{
    public void OnTouch()
    {
        if (TutorialQuest.Instance.m_isSynergyProgress == true)
        {
            TutorialManager.SynergyTextEnd();
            TutorialManager.SynergyTextStart(2);
            TutorialManager.SynergyTextStart(1);
        }

        TutorialManager.MedKitEnd();
        TutorialManager.MedKitStart(1);

        TutorialManager.SynergyImmuneEnd();
        TutorialManager.SynergyImmuneStart(1);
    }

    public void Tutorial_Touch()
    {
        TutorialManager.NextStepButton();
        TutorialManager.TutorialStart(16);
        TutorialManager.TutorialStart(29);
        TutorialManager.TutorialStart(30);
    }
}
