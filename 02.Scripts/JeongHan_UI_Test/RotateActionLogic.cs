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
    private List<float> angleOffsets = new List<float>(); // �� ������ �ʱ� ���� ������
    public List<Vector3> myInitPos = new List<Vector3>();
    public float bounceDistance = 100f; // �ٱ����� Ƣ����� �Ÿ�
    public int pathResolution = 5; // ��θ� �ɰ��� ����
    public SkillToolTip m_skillToolTip;
    public float rotationDuration = 0.5f;
    public Vector3 rotationCenter = Vector3.zero; // ȸ�� �߽�
    public int slotCount = 3;

    public float baseRadius; // ���� ������ ��
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

        // ���� ó���� ���� �����Ǿ��ִ� ��ų �������� ��ų �̹����� ��ų ������ �־��ֱ�
        slotList[i].SetActionSlotInGame(ActionLogicManager.Instance.m_actionLogic[i], m_skillToolTip);
    }

    public void ActivateSkillIcon()
    {
//        slotList[slotIndex].transform.GetChild(0).DOScale(maxScale, 1).SetEase(Ease.OutSine);
 //       slotList[slotIndex].transform.GetChild(0).DOLocalMoveY(initialPositions[slotIndex].y + moveOffset, GameManager.Instance.player.GetComponent<Player>().GetCurrentSkillAnimDuration()).SetEase(Ease.OutSine);  // ���� ��¦ �̵�

        Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);

        // ��ǥ���� �� �� ũ�� Ŀ���ٰ� �ٽ� �۾����� ũ��
        Vector3 overScale = new Vector3(1.8f, 1.8f, 1.8f);

        // DOTween Sequence ����
        DG.Tweening.Sequence jellySequence = DOTween.Sequence();

        // 1���� 2.2���� Ŀ����
        jellySequence.Append(slotList[slotIndex].transform.GetChild(0).DOScale(overScale, 0.2f).SetEase(Ease.OutQuad));

        // 2.2���� 2�� �پ���
        jellySequence.Append(slotList[slotIndex].transform.GetChild(0).DOScale(targetScale, 0.1f).SetEase(Ease.InQuad));

        // ����
        jellySequence.Play();
    }

    public void DeactivateSkillIcon()
    {
        // ��ġ�� ũ�⸦ ������� ����
   //     slotList[slotIndex].transform.GetChild(0).DOScale(initialScales[slotIndex], 1f).SetEase(Ease.InSine);  // ũ�� ���� ����
    //    slotList[slotIndex].transform.GetChild(0).DOLocalMoveY(initialPositions[slotIndex].y, 1f).SetEase(Ease.InSine);  // ���� ��ġ�� ���ư�

        Vector3 targetScale = new Vector3(1.0f, 1.0f, 1.0f);

        // ��ǥ���� �� �� ũ�� Ŀ���ٰ� �ٽ� �۾����� ũ��
        Vector3 overScale = new Vector3(1.8f, 1.8f, 1.8f);

        // DOTween Sequence ����
        DG.Tweening.Sequence jellySequence = DOTween.Sequence();
        
        // 1���� 2.2���� Ŀ����
        jellySequence.Append(slotList[slotIndex].transform.GetChild(0).DOScale(overScale, 0.2f).SetEase(Ease.OutQuad));

        // 2.2���� 2�� �پ���
        jellySequence.Append(slotList[slotIndex].transform.GetChild(0).DOScale(targetScale, 0.2f).SetEase(Ease.InQuad));


        // ����
        jellySequence.Play();
    }

    public void SetNewSlotPosCircle()
    {
        angleOffsets.Clear();

        float startAngle = 90f; // ù ��° ������ 12�� �������� ����
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
        // ������ �������� ��ȯ�Ͽ� ���� ��θ� ���� ��ġ ���
        float radian = angle * Mathf.Deg2Rad;
        float x = this.transform.GetChild(0).localPosition.x + Mathf.Cos(radian) * customRadius;
        float y = this.transform.GetChild(0).localPosition.y + Mathf.Sin(radian) * customRadius;
        return new Vector3(x, y, 0);
    }


    public void RotateImage()
    {
        // ������ 360�� / slotList.Count��ŭ ȸ���Ͽ� �� ĭ ������ �̵�
        float angleShift = 360f / slotList.Count;

        // ��� ������ ���� �������� ����
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

            // ���� ��ġ�� ��ǥ ��ġ ���
            Vector3 currentPos = slotList[i].transform.localPosition;
            Vector3 targetPos = CalculatePosition(newAngle, radius);

            // ��� ����Ʈ���� ������ ����Ʈ ����
            List<Vector3> pathPoints = new List<Vector3> { currentPos };

            // ������ ���� �ܰ�� ������ ���� ���� ��� ����
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

            // DOLocalPath�� ������ ��θ� ���� �̵��ϰ� ��
            slotList[i].transform.DOLocalPath(pathPoints.ToArray(), rotationDuration, PathType.CatmullRom)
                .SetEase(Ease.OutQuad); // Ease �������� �ε巴��
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

        // ���� �ػ�(��: 1920x1080)�� �������� ������ ����Ͽ� �������� �����մϴ�.
        float baseWidth = 1920f;
        float baseHeight = 1080f;

        float widthRatio = screenWidth / baseWidth;
        float heightRatio = screenHeight / baseHeight;

        // ������ ���� �ʺ�� ���� ������ ��հ��� ����Ͽ� �����մϴ�.
        radius = baseRadius * (widthRatio + heightRatio) / 2;
    }
}

