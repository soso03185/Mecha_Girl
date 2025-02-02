using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Ʃ�丮�� ��������Ʈ�� �����ϴ� �Ŵ���
/// </summary>
public class TutorialManager
{
    public static event Action m_OnTutorialStart;
    public static event Action m_OnNextStepButton;
                               
    public static event Action m_OnSynergyTextStart;
    public static event Action m_OnSynergyTextEnd;
                               
    public static event Action m_OnMedKitStart;
    public static event Action m_OnMedKitEnd;
                               
    public static event Action m_OnSynergyImmuneStart;
    public static event Action m_OnSynergyImmuneEnd;

    public static int m_TutorialIndex = 0;
    public static int m_SynergyIndex = 0;
    public static int m_MedKitIndex = 0;
    public static int m_ImmuneIndex = 0;

    public static void TutorialStart(int _tutIndex)
    {
        if (_tutIndex >= 9) // Ʃ�丮�� 9���ķδ� 2������������ ����
        {
            if (Managers.Data.currentStageLevel < 2) return;
        }

        if (_tutIndex >= 15) // Ʃ�丮�� 15���ķδ� 3������������ ����
        {
            if (Managers.Data.currentStageLevel < 3) return;
        }
  
        if (_tutIndex == m_TutorialIndex)
            m_OnTutorialStart?.Invoke();
    }

    public static void SynergyTextStart(int _synIndex)
    {
        if (_synIndex == m_SynergyIndex)
            m_OnSynergyTextStart?.Invoke();
    }

    public static void MedKitStart(int _medIndex)
    {
        if (m_MedKitIndex == _medIndex)
            m_OnMedKitStart?.Invoke();
    }

    public static void SynergyImmuneStart(int _immuneIndex)
    {
        if (m_ImmuneIndex == _immuneIndex)
            m_OnSynergyImmuneStart?.Invoke();
    }
    
    public static void SynergyImmuneEnd() => m_OnSynergyImmuneEnd?.Invoke();
    public static void NextStepButton() => m_OnNextStepButton?.Invoke();
    public static void SynergyTextEnd() => m_OnSynergyTextEnd?.Invoke();
    public static void MedKitEnd() => m_OnMedKitEnd?.Invoke();
}
