using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowToggle : MonoBehaviour
{
    public GameObject window;

    Sequence scaleSequence;
    Vector3 m_originscale;

    public static bool m_isClickedFirst = false;

    private void Start()
    {
        scaleSequence = DOTween.Sequence();
        m_originscale = transform.localScale;

        if (m_isClickedFirst == false) TweenScale();
    }

    public void ToggleWindow()
    {
        if (window.activeSelf)
        {
            window.SetActive(false);
        }
        else
        {
            window.SetActive(true);
            EventManager.OpenTab("BattleSystem");
        }

        if (scaleSequence != null) scaleSequence.Kill();
        
        scaleSequence = DOTween.Sequence().Append(transform.DOScale(m_originscale * 0.8f, 0.08f).SetEase(Ease.InOutSine))  // ������
                     .Append(transform.DOScale(m_originscale * 1.1f, 0.05f).SetEase(Ease.InOutSine)) // ������
                     .Append(transform.DOScale(m_originscale, 0.05f).SetEase(Ease.InOutSine)) // ������
                     .SetUpdate(true)  // TimeScale �����ϰ� ������Ʈ
                     .OnComplete(() => transform.DOKill());

        m_isClickedFirst = true;
    }

    public void TweenScale()
    {
        if (scaleSequence != null) scaleSequence.Kill();

        scaleSequence = DOTween.Sequence()
            .Append(transform.DOScale(1.05f, 0.35f).SetEase(Ease.InOutSine)) // 1.1��� Ȯ��
            .Append(transform.DOScale(1f, 0.35f).SetEase(Ease.InOutSine))   // ���� ũ��� ���
            .SetLoops(-1, LoopType.Restart) // ���� �ݺ�
            .SetUpdate(true);               // Ÿ�ӽ����� ����

    }
}
