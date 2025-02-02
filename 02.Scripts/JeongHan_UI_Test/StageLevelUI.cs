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
        // ���� ��� StageLevelUI���� �� CenterBox �ȿ� �ִ��� position������ üũ�ǰ��ִµ� ����ȭ �۾� �ʿ�
        // ���ʿ��� �ֵ���� �ʹ� ���� �������� 
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
