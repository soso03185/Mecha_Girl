using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillToolTip : MonoBehaviour
{
    public TextMeshProUGUI m_skillName;
    public TextMeshProUGUI m_manaCost;
    public TextMeshProUGUI m_skillExplain;

    Sequence scaleSequence;

    private void OnEnable()
    {
        // 열리고 닫히는 시퀀스 설정
        if (scaleSequence != null) scaleSequence.Kill();

        scaleSequence = DOTween.Sequence().Append(transform.DOScaleY(1.1f, 0.13f).SetEase(Ease.OutBack)) // 0에서 1.2로 커짐
            .Append(transform.DOScaleY(1f, 0.1f).SetEase(Ease.InOutSine)); // 1.2에서 1로 줄어듦
    }

    public void SetToolTip(Skill data)
    {
        m_skillName.text = data.m_skillKorName;
        m_manaCost.text = "마나 소모량 : " + data.m_manaCost;
        m_skillExplain.text = data.m_skillExplanation;
    }
}
