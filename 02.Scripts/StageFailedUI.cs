using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageFailedUI : MonoBehaviour
{
    public RectTransform uiElement; // 애니메이션을 줄 UI의 RectTransform
    private Vector3 initialPosition; // 초기 위치 저장용

    private void Awake()
    {
        // 시작 위치 저장
        uiElement = this.GetComponent<RectTransform>();
        initialPosition = uiElement.localPosition;
    }

    public void ShowAndBounce(float height, float speed = 0.5f)
    {
        // UI 활성화
        uiElement.gameObject.SetActive(true);

        // 초기 위치보다 위로 올려서 시작 위치를 설정합니다.
        uiElement.localPosition = initialPosition + new Vector3(0, height, 0);

        // 바운스 애니메이션
        uiElement.DOLocalMove(initialPosition, speed)
            .SetEase(Ease.OutBounce) // 통통 튀는 효과
            .OnComplete(() =>
            {
                // 애니메이션이 끝나고 2초 후 사라지기
                Invoke("HideUIElement", 2f);
            });
    }

    private void HideUIElement()
    {
        // UI 비활성화
        uiElement.gameObject.SetActive(false);
    }
}
