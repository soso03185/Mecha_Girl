using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLevelUI : MonoBehaviour
{
    public StageLevel stageLevel;
    private RectTransform rectTransform;

    public int level;
    public bool isTouched;

    // Start is called before the first frame update
    void Start()
    {
        stageLevel = GetComponentInParent<StageLevel>();
        rectTransform = this.GetComponent<RectTransform>();
    }

    private void Update()
    {
        // 현재 모든 StageLevelUI들이 다 CenterBox 안에 있는지 position값으로 체크되고있는데 최적화 작업 필요
        // 불필요한 애들까지 너무 많이 돌고있음 
        IsWithinCenterBox();
    }

    void IsWithinCenterBox()
    {
        Vector3[] corners = stageLevel.CheckCenterBoxUI();
        float minX = corners[0].x;
        float maxX = corners[2].x;
        float minY = corners[0].y;
        float maxY = corners[2].y;

        if (rectTransform.localToWorldMatrix.GetPosition().x >= minX && rectTransform.localToWorldMatrix.GetPosition().x <= maxX && rectTransform.localToWorldMatrix.GetPosition().y >= minY && rectTransform.localToWorldMatrix.GetPosition().y <= maxY)
        {
            isTouched = true;
        }
        else
        {
            isTouched = false;
        }
    }
}
