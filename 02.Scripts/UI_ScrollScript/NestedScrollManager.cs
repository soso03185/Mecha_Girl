using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NestedScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;
    public Transform contentTr;
    public Slider tapSlider;
    public RectTransform[] BtnRect, BtnImgRect;

    const int SIZE = 4;
    float[] pos = new float[SIZE];
    float distance, curPos, targetPos;
    bool isDrag = false;
    int targetIndex = 0;

    void Start()
    {
        // 거리에 따라 0~1인 pos 대입
        distance = 1f / (SIZE - 1);

        for (int i = 0; i < SIZE; i++)
        {
            pos[i] = distance * i;
        }
    }

    float SetPos()
    {
        // 절반거리를 기준으로 가까운 위치를 반환
        for (int i = 0; i < SIZE; i++)
        {
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                return pos[i];
            }
        }
        return 0;
    }

    void Update()
    {
        tapSlider.value = scrollbar.value;

        if (!isDrag)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);

            // 목표 버튼은 크기가 커짐
            for (int i = 0; i < SIZE; i++)
            {
                BtnRect[i].sizeDelta = new Vector2(i == targetIndex ? 360 : 180, BtnRect[i].sizeDelta.y);
            }
        }

        if (Time.time < 0.1f) return;

        for(int i = 0; i < SIZE; i++)
        {
            Vector3 BtnTargetPos = BtnRect[i].anchoredPosition3D;
            Vector3 BtnTargetScale = Vector3.one;
            bool textActive = false;

            if (i == targetIndex)
            {
                BtnTargetPos.y = -23f;
                BtnTargetScale = new Vector3(1.2f, 1.2f, 1);
                textActive = true;
            }

            BtnImgRect[i].anchoredPosition3D = Vector3.Lerp(BtnImgRect[i].anchoredPosition3D, BtnTargetPos, 0.25f);
            BtnImgRect[i].localScale = Vector3.Lerp(BtnImgRect[i].localScale, BtnTargetScale, 0.25f);
            BtnImgRect[i].transform.GetChild(0).gameObject.SetActive(textActive);
        }
    }

    public void OnBeginDrag(PointerEventData eventData) => curPos = SetPos();
    public void OnDrag(PointerEventData eventData) => isDrag = true;

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;
        targetPos = SetPos();

        // 절반 거리를 넘지 않아도 마우스를 빠르게 이동하면
        if (curPos == targetPos)
        {
            // 스크롤이 왼쪽으로 빠르게 이동시 목표가 하나 감소
            if(eventData.delta.x > 18 && curPos - distance >= 0)
            {
                --targetIndex;
                targetPos = curPos - distance;
            }
            else if(eventData.delta.x < -18 && curPos + distance <= 1.01f)
            {
                ++targetIndex;
                targetPos = curPos + distance;
            }
        }

        // 목표가 수직스크롤이고, 옆에서 옮겨왔다면 수직스크롤을 맨 위로 올림
        for(int i = 0; i < SIZE; i++)
        {
            if(contentTr.GetChild(i).GetComponent<ScrollScript>() && curPos != pos[i] && targetPos == pos[i])
            {
                contentTr.GetChild(i).GetChild(1).GetComponent<Scrollbar>().value = 1;
            }
        }
    }

    public void TapClick(int n)
    {
        targetIndex = n;
        targetPos = pos[n];
    }
}
