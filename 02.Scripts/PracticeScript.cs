using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using GoogleSheet.Type;

public class PracticeScript : MonoBehaviour
{
    public RectTransform currentPage;
    public RectTransform nextPage;

    public float duration = 1.0f;
    public float strength = 50.0f;
    public int vibrato = 10;
    public float randomness = 90f;
    public float shakeDuration = 1.0f;

    private void Start()
    {
        DOTween.Init(false, true, LogBehaviour.Verbose).SetCapacity(200, 50);

        //ShackUI();
        
    }

    private void Update()
    {
       if(Input.GetMouseButtonDown(0))
        {
            RotatePage();
        }
    }


    public void ShackUI()
    {
        if(currentPage != null)
        {
            currentPage.DOShakeAnchorPos(duration, strength, vibrato);
        }
    }

    public void RotatePage()
    {
        if (currentPage != null && nextPage != null)
        {
            currentPage.pivot = new Vector2(0.5f, 0f);
            nextPage.pivot = new Vector2(0.5f, 0f);

            // 시퀀스를 생성하여 애니메이션을 순차적으로 실행
            Sequence sequence = DOTween.Sequence();

            //// 현재 페이지를 회전하면서 화면 밖으로 보냅니다
            //sequence.Append(currentPage.DOLocalRotate(new Vector3(0, 0, 90), duration).SetEase(Ease.InOutSine));

            currentPage.gameObject.SetActive(false);
            // 다음 페이지를 초기 상태로 설정하고 활성화
            sequence.AppendCallback(() =>
            {
                //currentPage.gameObject.SetActive(false);
                nextPage.gameObject.SetActive(true);
                nextPage.localRotation = Quaternion.Euler(0, 0, -90);
            });

            // 다음 페이지를 회전시키면서 흔들리는 효과 추가
            sequence.Append(nextPage.DOLocalRotate(new Vector3(0, 0, -15), shakeDuration).SetEase(Ease.InOutSine));
            sequence.Append(nextPage.DOLocalRotate(new Vector3(0, 0, 15), shakeDuration).SetEase(Ease.InOutSine));
            sequence.Append(nextPage.DOLocalRotate(new Vector3(0, 0, 0), shakeDuration).SetEase(Ease.InOutSine));

            // 다음 페이지를 회전시키면서 화면에 들어오게 합니다
            sequence.Append(nextPage.DOLocalRotate(Vector3.zero, duration).SetEase(Ease.InOutSine));
        }
    }
}

