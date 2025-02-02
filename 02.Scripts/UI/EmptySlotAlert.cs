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

        // ������ ������ ������ ����
        if (scaleSequence != null) scaleSequence.Kill();

        scaleSequence = DOTween.Sequence().Append(transform.DOScaleY(1.1f, 0.3f).SetEase(Ease.OutBack)) // 0���� 1.2�� Ŀ��
            .Append(transform.DOScaleY(1f, 0.15f).SetEase(Ease.InOutSine)) // 1.2���� 1�� �پ��
            .AppendInterval(0.5f) // 1�� ���
            .Append(transform.DOScaleY(1.1f, 0.15f).SetEase(Ease.InOutSine)) // �ٽ� 1���� 1.2�� Ŀ��
            .Append(transform.DOScaleY(0f, 0.3f).SetEase(Ease.InBack)) // 1.2���� 0���� �پ�� (������ ����)
            .OnComplete(() => transform.gameObject.SetActive(false)); // �ִϸ��̼��� ���� �� ��Ȱ��ȭ
    }
}
