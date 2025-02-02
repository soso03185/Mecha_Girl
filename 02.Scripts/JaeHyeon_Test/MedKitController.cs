using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 몬스터가 드랍하는 마나 아이템
/// </summary>
public class MedKitController : MonoBehaviour
{
    void OnEnable()
    {
        // 시계방향으로 부드럽게 회전
        transform.DORotate(new Vector3(0f, 360f, 0f), 9, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)  // 일정한 속도로 회전
            .SetLoops(-1, LoopType.Restart);  // 무한 반복

        // 둥둥
        transform.DOMoveY(transform.position.y + 0.3f, 2) // (floatHeight, duration)
            .SetEase(Ease.InOutSine)  // 부드럽게 움직이기 위한 Ease 설정
            .SetLoops(-1, LoopType.Yoyo);  // 무한 반복으로 위아래로 움직임
    }

    private void OnDisable()
    {
        transform.DOKill();
    }
}
