using DG.Tweening;
using System.ComponentModel;
using TMPro;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// Ʃ�丮�� �гο� ���� ��� ������ ���� �����̾�����,
/// Ʃ�丮�� ����ÿ� ��ġ �ش޶�� ȭ��ǥ ��Ʈ�ѷ��� �����
/// </summary>
public class TutorialPanel : MonoBehaviour
{
    public Transform m_ArrowParent; // ArrowIcon
    public Transform m_QuestObjParent; // Quest Object Origin Position
    public Transform m_BottomArrow;

    float moveDistance = 20f; // ������Ʈ�� �̵��� �Ÿ�
    float duration = 0.7f;    // �̵��� �ɸ��� �ð�

    public void MoveArrowIcon(Vector2 _arrowStartPos)
    {
        m_ArrowParent.gameObject.SetActive(true);

        m_ArrowParent.DOKill();
        m_ArrowParent.transform.position = _arrowStartPos;

        // ���� ��ġ���� moveDistance ��ŭ ���� �̵�
        m_ArrowParent.DOMoveY(m_ArrowParent.transform.position.y + moveDistance, duration)
            .SetEase(Ease.InOutSine) // �ε巴�� �̵�
            .SetLoops(-1, LoopType.Yoyo).SetUpdate(true); // �ݺ������� ���Ʒ� �̵�
    }

    public void ShowArrowIcon(bool _isShow)
    {
        m_ArrowParent.gameObject.SetActive(_isShow);
    }

    public void ShowBotArrowIcon(bool _isShow)
    {
        m_BottomArrow.gameObject.SetActive(_isShow);
    }
}