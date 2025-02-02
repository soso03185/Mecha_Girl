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

        // 스크롤 속도가 200 이하일 때 snap 동작을 수행합니다.
        if (scrollRect.velocity.magnitude < 200)
        {
            scrollRect.velocity = Vector2.zero; // 스크롤 속도를 0으로 설정합니다.
            snapSpeed += snapForce * Time.deltaTime; // snap 속도를 증가시킵니다.

            // contentPanel의 위치를 snap 위치로 이동합니다.
            contentPanel.localPosition = new Vector3(
                Mathf.MoveTowards(contentPanel.localPosition.x, 0 - (currentItem * (sampleListItem.rect.width + HLG.spacing)), snapSpeed),
                contentPanel.localPosition.y,
                contentPanel.localPosition.z
            );

            // contentPanel의 위치가 snap 위치에 도달했는지 확인합니다.
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
            // 스크롤 속도가 200 이상일 때 snap 동작을 수행하지 않습니다.
            isSnapped = false;
            snapSpeed = 0; // snap 속도를 초기화합니다.
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
