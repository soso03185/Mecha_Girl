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

        // 오브젝트 초기 크기를 0.8로 설정
        transform.localScale = m_originscale * 0.75f;

        if (scaleSequence != null) scaleSequence.Kill();

            scaleSequence = DOTween.Sequence().Append(transform.DOScale(m_originscale * 1.1f, 0.15f).SetEase(Ease.InOutSine))  // 1.1배로 서서히 커짐
                     .Append(transform.DOScale(m_originscale, 0.05f).SetEase(Ease.InOutSine)) // 1배로 서서히 줄어듦
                     .SetUpdate(true).OnComplete(() => transform.DOKill());  // TimeScale 무시하고 업데이트
    }

    public void OnTouchTween()
    {
        // 이전에 실행된 트윈 시퀀스가 있다면 종료
        if (scaleSequence != null) scaleSequence.Kill();
        scaleSequence = DOTween.Sequence().Append(transform.DOScale(m_originscale * 0.9f, 0.08f).SetEase(Ease.InOutSine))  // 서서히 작아짐
                     .Append(transform.DOScale(m_originscale * 1.1f, 0.05f).SetEase(Ease.InOutSine)) // 서서히 커짐
                     .Append(transform.DOScale(m_originscale, 0.05f).SetEase(Ease.InOutSine)) // 서서히
                     .SetUpdate(true)   // TimeScale 무시하고 업데이트
                     .OnComplete(() => transform.DOKill());
    }

    public void OnParticleTween()
    {
        if (scaleSequence.IsPlaying())
        {
            scaleSequence.Append(transform.DOScale(m_originscale * 1.1f, 0.05f).SetEase(Ease.InOutSine)) // 서서히 커짐
                         .Append(transform.DOScale(m_originscale, 0.05f).SetEase(Ease.InOutSine)) // 서서히
                         .SetUpdate(true)   // TimeScale 무시하고 업데이트
                         .OnComplete(() => transform.DOKill());
        }
        else
        {
            scaleSequence.Append(transform.DOScale(m_originscale * 0.9f, 0.08f).SetEase(Ease.InOutSine))  // 서서히 작아짐
                         .Append(transform.DOScale(m_originscale * 1.1f, 0.05f).SetEase(Ease.InOutSine)) // 서서히 커짐
                         .Append(transform.DOScale(m_originscale, 0.05f).SetEase(Ease.InOutSine)) // 서서히
                         .SetUpdate(true)   // TimeScale 무시하고 업데이트
                         .OnComplete(() => transform.DOKill());
        }
    }
}
