using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �⺻ Ʃ�丮���� ������ �ٸ� �̺�Ʈ Ʃ�丮�� ���� ��,
/// ��ġ�Ͽ� �Ѿ�� �̺�Ʈ ó��
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
