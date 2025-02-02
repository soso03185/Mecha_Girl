using DG.Tweening;
using System.ComponentModel;
using TMPro;
using Unity.Collections;
using UnityEngine;

/// <summary>
/// 튜토리얼 패널에 관한 모든 내용을 담을 예정이었으나,
/// 튜토리얼 진행시에 터치 해달라는 화살표 컨트롤러로 사용중
/// </summary>
public class TutorialPanel : MonoBehaviour
{
    public Transform m_ArrowParent; // ArrowIcon
    public Transform m_QuestObjParent; // Quest Object Origin Position
    public Transform m_BottomArrow;

    float moveDistance = 20f; // 오브젝트가 이동할 거리
    float duration = 0.7f;    // 이동에 걸리는 시간

    public void MoveArrowIcon(Vector2 _arrowStartPos)
    {
        m_ArrowParent.gameObject.SetActive(true);

        m_ArrowParent.DOKill();
        m_ArrowParent.transform.position = _arrowStartPos;

        // 현재 위치에서 moveDistance 만큼 위로 이동
        m_ArrowParent.DOMoveY(m_ArrowParent.transform.position.y + moveDistance, duration)
            .SetEase(Ease.InOutSine) // 부드럽게 이동
            .SetLoops(-1, LoopType.Yoyo).SetUpdate(true); // 반복적으로 위아래 이동
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