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

            // �������� �����Ͽ� �ִϸ��̼��� ���������� ����
            Sequence sequence = DOTween.Sequence();

            //// ���� �������� ȸ���ϸ鼭 ȭ�� ������ �����ϴ�
            //sequence.Append(currentPage.DOLocalRotate(new Vector3(0, 0, 90), duration).SetEase(Ease.InOutSine));

            currentPage.gameObject.SetActive(false);
            // ���� �������� �ʱ� ���·� �����ϰ� Ȱ��ȭ
            sequence.AppendCallback(() =>
            {
                //currentPage.gameObject.SetActive(false);
                nextPage.gameObject.SetActive(true);
                nextPage.localRotation = Quaternion.Euler(0, 0, -90);
            });

            // ���� �������� ȸ����Ű�鼭 ��鸮�� ȿ�� �߰�
            sequence.Append(nextPage.DOLocalRotate(new Vector3(0, 0, -15), shakeDuration).SetEase(Ease.InOutSine));
            sequence.Append(nextPage.DOLocalRotate(new Vector3(0, 0, 15), shakeDuration).SetEase(Ease.InOutSine));
            sequence.Append(nextPage.DOLocalRotate(new Vector3(0, 0, 0), shakeDuration).SetEase(Ease.InOutSine));

            // ���� �������� ȸ����Ű�鼭 ȭ�鿡 ������ �մϴ�
            sequence.Append(nextPage.DOLocalRotate(Vector3.zero, duration).SetEase(Ease.InOutSine));
        }
    }
}

