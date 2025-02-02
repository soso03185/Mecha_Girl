
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 현재 사용하지 않는 스크립트.
/// 화면 우측에서 자동으로 tip 정보가 나와서 연출할 예정이었음.
/// </summary>
public class TutAutoInfoTween : MonoBehaviour
{
    public RectTransform m_uiElement; // 이동할 UI 엘리먼트
    public float m_duration = 0.5f; // 이동 시간
    private Vector2 m_originalPosition;

    DG.Tweening.Sequence scaleSequence;

    private void Start()
    {
        m_uiElement = GetComponent<RectTransform>();
        m_originalPosition = m_uiElement.anchoredPosition;
    }

    public void MoveLeftAndReturn()
    {
        float moveDistance = m_uiElement.rect.width; // UI 엘리먼트의 너비만큼 이동

        scaleSequence = DOTween.Sequence();
        scaleSequence.Append(m_uiElement.DOAnchorPosX(m_originalPosition.x - moveDistance, m_duration)) // 왼쪽으로 이동
                .AppendInterval(1f) // 1초 대기
                .Append(m_uiElement.DOAnchorPosX(m_originalPosition.x, m_duration)) // 원래 자리로 돌아감
                .SetUpdate(true); // 타임스케일 0에서도 동작
        scaleSequence.Play();
    }
}