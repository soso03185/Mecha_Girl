
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���� ������� �ʴ� ��ũ��Ʈ.
/// ȭ�� �������� �ڵ����� tip ������ ���ͼ� ������ �����̾���.
/// </summary>
public class TutAutoInfoTween : MonoBehaviour
{
    public RectTransform m_uiElement; // �̵��� UI ������Ʈ
    public float m_duration = 0.5f; // �̵� �ð�
    private Vector2 m_originalPosition;

    DG.Tweening.Sequence scaleSequence;

    private void Start()
    {
        m_uiElement = GetComponent<RectTransform>();
        m_originalPosition = m_uiElement.anchoredPosition;
    }

    public void MoveLeftAndReturn()
    {
        float moveDistance = m_uiElement.rect.width; // UI ������Ʈ�� �ʺ�ŭ �̵�

        scaleSequence = DOTween.Sequence();
        scaleSequence.Append(m_uiElement.DOAnchorPosX(m_originalPosition.x - moveDistance, m_duration)) // �������� �̵�
                .AppendInterval(1f) // 1�� ���
                .Append(m_uiElement.DOAnchorPosX(m_originalPosition.x, m_duration)) // ���� �ڸ��� ���ư�
                .SetUpdate(true); // Ÿ�ӽ����� 0������ ����
        scaleSequence.Play();
    }
}