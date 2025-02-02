using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalSnap : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform contentPanel;
    public RectTransform sampleListItem;

    public HorizontalLayoutGroup HLG;

    private bool isSnapped = false;

    public float snapForce;
    float snapSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int currentItem = Mathf.RoundToInt((0 - contentPanel.localPosition.x / (sampleListItem.rect.width + HLG.spacing)));

        // ��ũ�� �ӵ��� 200 ������ �� snap ������ �����մϴ�.
        if (scrollRect.velocity.magnitude < 200)
        {
            scrollRect.velocity = Vector2.zero; // ��ũ�� �ӵ��� 0���� �����մϴ�.
            snapSpeed += snapForce * Time.deltaTime; // snap �ӵ��� ������ŵ�ϴ�.

            // contentPanel�� ��ġ�� snap ��ġ�� �̵��մϴ�.
            contentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(contentPanel.localPosition.x, 0 - (currentItem * (sampleListItem.rect.width + HLG.spacing)), snapSpeed),
                contentPanel.localPosition.y,
                contentPanel.localPosition.z
            );

            // contentPanel�� ��ġ�� snap ��ġ�� �����ߴ��� Ȯ���մϴ�.
            if (contentPanel.localPosition.x == 0 - (currentItem * (sampleListItem.rect.width + HLG.spacing)))
            {
                isSnapped = true;
            }
            else
            {
                isSnapped = false;
            }
        }
        else
        {
            // ��ũ�� �ӵ��� 200 �̻��� �� snap ������ �������� �ʽ��ϴ�.
            isSnapped = false;
            snapSpeed = 0; // snap �ӵ��� �ʱ�ȭ�մϴ�.
        }
    }

    public void OnStageChangeButtonClick()
    {
        if(isSnapped)
        {
            GameManager.Instance.ChangeStage(Managers.Stage.tempStageLevel);
        }
    }
}
