using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmptySlotAlert : MonoBehaviour
{
    Sequence scaleSequence;

    public void Start()
    {
        scaleSequence = DOTween.Sequence();

        if (GameManager.Instance.player != null)
        {
            GameManager.Instance.player.GetComponent<Player>().emptySkillAlert += new System.EventHandler(EmptySlotAlertOn);
        }        
    }

    void EmptySlotAlertOn(object obj, EventArgs e)
    {
        gameObject.SetActive(true);

        // 열리고 닫히는 시퀀스 설정
        if (scaleSequence != null) scaleSequence.Kill();

        scaleSequence = DOTween.Sequence().Append(transform.DOScaleY(1.1f, 0.3f).SetEase(Ease.OutBack)) // 0에서 1.2로 커짐
            .Append(transform.DOScaleY(1f, 0.15f).SetEase(Ease.InOutSine)) // 1.2에서 1로 줄어듦
            .AppendInterval(0.5f) // 1초 대기
            .Append(transform.DOScaleY(1.1f, 0.15f).SetEase(Ease.InOutSine)) // 다시 1에서 1.2로 커짐
            .Append(transform.DOScaleY(0f, 0.3f).SetEase(Ease.InBack)) // 1.2에서 0으로 줄어듦 (닫히는 느낌)
            .OnComplete(() => transform.gameObject.SetActive(false)); // 애니메이션이 끝난 후 비활성화
    }
}
