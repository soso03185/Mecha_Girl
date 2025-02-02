using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlotButtonScript : MonoBehaviour
{
    public GameObject fourSlotButton;
    public GameObject fiveSlotButton;
    public GameObject sixSlotButton;
    public GameObject sevenSlotButton;
    public GameObject eightSlotButton;

    public GameObject slotPanel;
    private CustomGridLayout gridLayout;

    private void Start()
    {
        gridLayout = slotPanel.GetComponent<CustomGridLayout>();

        // �� ��ư�� Ŭ�� �̺�Ʈ �߰�
        fourSlotButton.GetComponent<Button>().onClick.AddListener(() => ChangeSlotCount(4));
        fiveSlotButton.GetComponent<Button>().onClick.AddListener(() => ChangeSlotCount(5));
        sixSlotButton.GetComponent<Button>().onClick.AddListener(() => ChangeSlotCount(6));
        sevenSlotButton.GetComponent<Button>().onClick.AddListener(() => ChangeSlotCount(7));
        eightSlotButton.GetComponent<Button>().onClick.AddListener(() => ChangeSlotCount(8));
    }

    void ChangeSlotCount(int count)
    {
        // ���� ���� ������ ��ǥ ���� ������ ���Ͽ� �ʿ��� ��ŭ ������ �߰��ϰų� �����մϴ�.
        int currentSlotCount = gridLayout.GetSlotCount();

        while (currentSlotCount > count)
        {
            gridLayout.RemoveImage();
            currentSlotCount--;
        }

        while (currentSlotCount < count)
        {
            gridLayout.AddImage();
            currentSlotCount++;
        }
    }
}
