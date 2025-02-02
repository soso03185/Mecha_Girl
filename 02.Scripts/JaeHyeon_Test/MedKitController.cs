using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// ���Ͱ� ����ϴ� ���� ������
/// </summary>
public class MedKitController : MonoBehaviour
{
    void OnEnable()
    {
        // �ð�������� �ε巴�� ȸ��
        transform.DORotate(new Vector3(0f, 360f, 0f), 9, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)  // ������ �ӵ��� ȸ��
            .SetLoops(-1, LoopType.Restart);  // ���� �ݺ�

        // �յ�
        transform.DOMoveY(transform.position.y + 0.3f, 2) // (floatHeight, duration)
            .SetEase(Ease.InOutSine)  // �ε巴�� �����̱� ���� Ease ����
            .SetLoops(-1, LoopType.Yoyo);  // ���� �ݺ����� ���Ʒ��� ������
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}
