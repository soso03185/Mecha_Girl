using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTween : MonoBehaviour
{
    DG.Tweening.Sequence scaleSequence; // = DOTween.Sequence();
    Vector3 m_originscale;

    public bool _isPlayOnEnable = false;

    private void Start()
    {
        Button btn = transform.GetComponent<Button>();
        scaleSequence = DOTween.Sequence();

        if (btn != null)
        {
            btn.onClick.AddListener(OnTouchTween);
        }
    }

    void OnEnable()
    {
        if (m_originscale == Vector3.zero)
            m_originscale = transform.localScale;

        if (_isPlayOnEnable == false) return;

        // ������Ʈ �ʱ� ũ�⸦ 0.8�� ����
        transform.localScale = m_originscale * 0.75f;

        if (scaleSequence != null) scaleSequence.Kill();

            scaleSequence = DOTween.Sequence().Append(transform.DOScale(m_originscale * 1.1f, 0.15f).SetEase(Ease.InOutSine))  // 1.1��� ������ Ŀ��
                     .Append(transform.DOScale(m_originscale, 0.05f).SetEase(Ease.InOutSine)) // 1��� ������ �پ��
                     .SetUpdate(true).OnComplete(() => transform.DOKill());  // TimeScale �����ϰ� ������Ʈ
    }

    public void OnTouchTween()
    {
        // ������ ����� Ʈ�� �������� �ִٸ� ����
        if (scaleSequence != null) scaleSequence.Kill();
        scaleSequence = DOTween.Sequence().Append(transform.DOScale(m_originscale * 0.9f, 0.08f).SetEase(Ease.InOutSine))  // ������ �۾���
                     .Append(transform.DOScale(m_originscale * 1.1f, 0.05f).SetEase(Ease.InOutSine)) // ������ Ŀ��
                     .Append(transform.DOScale(m_originscale, 0.05f).SetEase(Ease.InOutSine)) // ������
                     .SetUpdate(true)   // TimeScale �����ϰ� ������Ʈ
                     .OnComplete(() => transform.DOKill());
    }

    public void OnParticleTween()
    {
        if (scaleSequence.IsPlaying())
        {
            scaleSequence.Append(transform.DOScale(m_originscale * 1.1f, 0.05f).SetEase(Ease.InOutSine)) // ������ Ŀ��
                         .Append(transform.DOScale(m_originscale, 0.05f).SetEase(Ease.InOutSine)) // ������
                         .SetUpdate(true)   // TimeScale �����ϰ� ������Ʈ
                         .OnComplete(() => transform.DOKill());
        }
        else
        {
            scaleSequence.Append(transform.DOScale(m_originscale * 0.9f, 0.08f).SetEase(Ease.InOutSine))  // ������ �۾���
                         .Append(transform.DOScale(m_originscale * 1.1f, 0.05f).SetEase(Ease.InOutSine)) // ������ Ŀ��
                         .Append(transform.DOScale(m_originscale, 0.05f).SetEase(Ease.InOutSine)) // ������
                         .SetUpdate(true)   // TimeScale �����ϰ� ������Ʈ
                         .OnComplete(() => transform.DOKill());
        }
    }
}
