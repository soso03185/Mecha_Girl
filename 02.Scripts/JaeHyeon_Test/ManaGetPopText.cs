using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 마나 아이템을 먹을 시에 게이지 위에 떠오르는 마나 Text
/// </summary>
public class ManaGetPopText : MonoBehaviour
{
    public RectTransform m_textTransform;  // UI 텍스트의 RectTransform
    float m_moveDistance = 80;   // 텍스트가 이동할 거리
    float m_moveDuration = 0.4f;    // 애니메이션 지속 시간
    private Vector2 m_originalPosition;    // 텍스트의 원래 위치

    private void Awake()
    {
        m_textTransform = GetComponent<RectTransform>();

        // 시작할 때 텍스트의 원래 위치 저장
        m_originalPosition = m_textTransform.anchoredPosition;
    }

    void OnEnable()
    {
        m_textTransform.DOAnchorPosY(m_originalPosition.y + m_moveDistance, m_moveDuration).SetEase(Ease.InOutSine).OnComplete(
            () => ResetPosition());
    }

    void ResetPosition()
    {
        // 원래 위치로 순간 이동
        m_textTransform.anchoredPosition = m_originalPosition;
        gameObject.SetActive(false);
    }
}
