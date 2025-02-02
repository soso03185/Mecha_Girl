using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class RotateActionLogic : MonoBehaviour
{
    public RectTransform rectTransform;
    private string slotPath = "UI/EmptySlot2";

    public List<ActionSlotInGame> slotList = new List<ActionSlotInGame>();

    public List<Vector3> initialPositions = new List<Vector3>();
    public List<Vector3> initialScales = new List<Vector3>();
    private List<float> angleOffsets = new List<float>(); // 각 슬롯의 초기 각도 오프셋
    public List<Vector3> myInitPos = new List<Vector3>();
    public float bounceDistance = 100f; // 바깥으로 튀어나가는 거리
    public int pathResolution = 5; // 경로를 쪼개는 개수
    public SkillToolTip m_skillToolTip;
    public float rotationDuration = 0.5f;
    public Vector3 rotationCenter = Vector3.zero; // 회전 중심
    public int slotCount = 3;

    public float baseRadius; // 기준 반지름 값
    public float radius;

    public bool isRotating = false;
    public bool isClosedButtonClicked = false;

    Vector3[] targetPosition = new Vector3[8];

    public int slotIndex;

    public Transform m_CircleImage;

    // Start is called before the first frame update
    public void Init()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        slotCount = ActionLogicManager.Instance.m_actionLogic.Count;
        for (int i = 0; i < slotCount; i++)
        {
            BeginSlotSetting(i);
            //initialPositions.Add(slotList[i].transform.GetChild(0).localPosition);
            //initialScales.Add(slotList[i].transform.localScale);
        }

        m_CircleImage.DORotate(new Vector3(0, 0, 3600), 1400, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Yoyo);
        UpdateRadius();
        SetNewSlotPosCircle();
        slotList[0].m_slotParticle.SetActive(true);
    }


    // Update is called once per frame
    void Update()
    {
        slotIndex = ActionLogicManager.Instance.CurrentIndex;

        if (slotIndex == 0 && Managers.Stage.isStageChanged)
        {
            ResetToInitialPositions();
        }
    }

    public void BeginSlotSetting(int i)
    {
        GameObject newSlot = Managers.Resource.Instantiate(slotPath, this.transform);
        //newSlot.transform.GetChild(1).gameObject.SetActive(true);
        //newSlot.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(OpenSkillCanvas);
        newSlot.name = newSlot.name + i.ToString();
        slotList.Add(newSlot.GetComponent<ActionSlotInGame>());

        // 가장 처음에 현재 장착되어있는 스킬 바탕으로 스킬 이미지랑 스킬 데이터 넣어주기
        slotList[i].SetActionSlotInGame(ActionLogicManager.Instance.m_actionLogic[i], m_skillToolTip);
    }

    public void ActivateSkillIcon()
    {
//        slotList[slotIndex].transform.GetChild(0).DOScale(maxScale, 1).SetEase(Ease.OutSine);
 //       slotList[slotIndex].transform.GetChild(0).DOLocalMoveY(initialPositions[slotIndex].y + moveOffset, GameManager.Instance.player.GetComponent<Player>().GetCurrentSkillAnimDuration()).SetEase(Ease.OutSine);  // 위로 살짝 이동

        Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);

        // 목표보다 좀 더 크게 커졌다가 다시 작아지는 크기
        Vector3 overScale = new Vector3(1.8f, 1.8f, 1.8f);

        // DOTween Sequence 생성
        DG.Tweening.Sequence jellySequence = DOTween.Sequence();

        // 1에서 2.2까지 커지기
        jellySequence.Append(slotList[slotIndex].transform.GetChild(0).DOScale(overScale, 0.2f).SetEase(Ease.OutQuad));

        // 2.2에서 2로 줄어들기
        jellySequence.Append(slotList[slotIndex].transform.GetChild(0).DOScale(targetScale, 0.1f).SetEase(Ease.InQuad));

        // 실행
        jellySequence.Play();
    }

    public void DeactivateSkillIcon()
    {
        // 위치와 크기를 원래대로 돌림
   //     slotList[slotIndex].transform.GetChild(0).DOScale(initialScales[slotIndex], 1f).SetEase(Ease.InSine);  // 크기 원상 복구
    //    slotList[slotIndex].transform.GetChild(0).DOLocalMoveY(initialPositions[slotIndex].y, 1f).SetEase(Ease.InSine);  // 원래 위치로 돌아감

        Vector3 targetScale = new Vector3(1.0f, 1.0f, 1.0f);

        // 목표보다 좀 더 크게 커졌다가 다시 작아지는 크기
        Vector3 overScale = new Vector3(1.8f, 1.8f, 1.8f);

        // DOTween Sequence 생성
        DG.Tweening.Sequence jellySequence = DOTween.Sequence();
        
        // 1에서 2.2까지 커지기
        jellySequence.Append(slotList[slotIndex].transform.GetChild(0).DOScale(overScale, 0.2f).SetEase(Ease.OutQuad));

        // 2.2에서 2로 줄어들기
        jellySequence.Append(slotList[slotIndex].transform.GetChild(0).DOScale(targetScale, 0.2f).SetEase(Ease.InQuad));


        // 실행
        jellySequence.Play();
    }

    public void SetNewSlotPosCircle()
    {
        angleOffsets.Clear();

        float startAngle = 90f; // 첫 번째 슬롯을 12시 방향으로 설정
        for (int i = 0; i < slotList.Count; i++)
        {
            float angle = startAngle - (360f / slotList.Count) * i;
            angleOffsets.Add(angle);
            Vector3 position = CalculatePosition(angle, radius);
            slotList[i].transform.localPosition = position;
        }
    }
    private Vector3 CalculatePosition(float angle, float customRadius)
    {
        // 각도를 라디안으로 변환하여 원형 경로를 따라 위치 계산
        float radian = angle * Mathf.Deg2Rad;
        float x = this.transform.GetChild(0).localPosition.x + Mathf.Cos(radian) * customRadius;
        float y = this.transform.GetChild(0).localPosition.y + Mathf.Sin(radian) * customRadius;
        return new Vector3(x, y, 0);
    }


    public void RotateImage()
    {
        // 각도를 360도 / slotList.Count만큼 회전하여 한 칸 옆으로 이동
        float angleShift = 360f / slotList.Count;

        // 모든 슬롯의 각도 오프셋을 갱신
        for (int i = 0; i < slotList.Count; i++)
        {
            angleOffsets[i] += angleShift;

            if (angleOffsets[i] >= 360)
            {
                angleOffsets[i] = angleOffsets[i] - 360;
            }

            if (angleOffsets[i] % 90 == 0 && angleOffsets[i] == 90)
            {
                angleOffsets[i] = 90;
            }
            float newAngle = angleOffsets[i];

           if(newAngle == 90)
           {
               slotList[i].m_slotParticle.SetActive(true);
           }
           else
               slotList[i].m_slotParticle.SetActive(false);

            // 현재 위치와 목표 위치 계산
            Vector3 currentPos = slotList[i].transform.localPosition;
            Vector3 targetPos = CalculatePosition(newAngle, radius);

            // 경로 포인트들을 저장할 리스트 생성
            List<Vector3> pathPoints = new List<Vector3> { currentPos };

            // 각도를 여러 단계로 나누어 작은 원형 경로 생성
            for (int j = 1; j <= pathResolution; j++)
            {
                float interpolatedAngle = Mathf.LerpAngle(angleOffsets[i] - angleShift, newAngle, j / (float)pathResolution);
                float interpolatedRadius = j == pathResolution ? radius : radius + bounceDistance;
                Vector3 pathPoint = CalculatePosition(interpolatedAngle, interpolatedRadius);
                pathPoints.Add(pathPoint);
            }

            if (slotList.Count == 3)
            {
                rotationDuration = 0.55f;
            }
            else if (slotList.Count == 4)
            {
                rotationDuration = 0.525f;
            }
            else
                rotationDuration = 0.5f;

            // DOLocalPath로 슬롯을 경로를 따라 이동하게 함
            slotList[i].transform.DOLocalPath(pathPoints.ToArray(), rotationDuration, PathType.CatmullRom)
                .SetEase(Ease.OutQuad); // Ease 설정으로 부드럽게
        }
    }

    public void ResetToInitialPositions()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            //initialPositions[i] = myInitPos[i];
            //targetPosition[i] = myInitPos[i];
            //slotList[i].transform.position = myInitPos[i];
        }
        Managers.Stage.isStageChanged = false;
    }

    private void UpdateRadius()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // 기준 해상도(예: 1920x1080)를 기준으로 비율을 계산하여 반지름을 조정합니다.
        float baseWidth = 1920f;
        float baseHeight = 1080f;

        float widthRatio = screenWidth / baseWidth;
        float heightRatio = screenHeight / baseHeight;

        // 반지름 값을 너비와 높이 비율의 평균값을 사용하여 조정합니다.
        radius = baseRadius * (widthRatio + heightRatio) / 2;
    }
}

