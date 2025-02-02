using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageFailedUI : MonoBehaviour
{
    public RectTransform uiElement; // �ִϸ��̼��� �� UI�� RectTransform
    private Vector3 initialPosition; // �ʱ� ��ġ �����

    private void Awake()
    {
        // ���� ��ġ ����
        uiElement = this.GetComponent<RectTransform>();
        initialPosition = uiElement.localPosition;
    }

    public void ShowAndBounce(float height, float speed = 0.5f)
    {
        // UI Ȱ��ȭ
        uiElement.gameObject.SetActive(true);

        // �ʱ� ��ġ���� ���� �÷��� ���� ��ġ�� �����մϴ�.
        uiElement.localPosition = initialPosition + new Vector3(0, height, 0);

        // �ٿ �ִϸ��̼�
        uiElement.DOLocalMove(initialPosition, speed)
            .SetEase(Ease.OutBounce) // ���� Ƣ�� ȿ��
            .OnComplete(() =>
            {
                // �ִϸ��̼��� ������ 2�� �� �������
                Invoke("HideUIElement", 2f);
            });
    }

    private void HideUIElement()
    {
        // UI ��Ȱ��ȭ
        uiElement.gameObject.SetActive(false);
    }
}
