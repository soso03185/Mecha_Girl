using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �������� ���� �ÿ� ������ ���� �������� ���� Text
/// </summary>
public class ManaGetPopText : MonoBehaviour
{
    public RectTransform m_textTransform;  // UI �ؽ�Ʈ�� RectTransform
    float m_moveDistance = 80;   // �ؽ�Ʈ�� �̵��� �Ÿ�
    float m_moveDuration = 0.4f;    // �ִϸ��̼� ���� �ð�
    private Vector2 m_originalPosition;    // �ؽ�Ʈ�� ���� ��ġ

    private void Awake()
    {
        m_textTransform = GetComponent<RectTransform>();

        // ������ �� �ؽ�Ʈ�� ���� ��ġ ����
        m_originalPosition = m_textTransform.anchoredPosition;
    }

    void OnEnable()
    {
        m_textTransform.DOAnchorPosY(m_originalPosition.y + m_moveDistance, m_moveDuration).SetEase(Ease.InOutSine).OnComplete(
            () => ResetPosition());
    }

    void ResetPosition()
    {
        // ���� ��ġ�� ���� �̵�
        m_textTransform.anchoredPosition = m_originalPosition;
        gameObject.SetActive(false);
    }
}
