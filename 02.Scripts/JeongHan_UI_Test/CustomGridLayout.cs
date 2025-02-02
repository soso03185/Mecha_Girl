using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGridLayout : MonoBehaviour
{
    public GameObject blankSpacePrefab; // 빈 슬롯
    public Transform imageParent; // 부모 객체인 패널

    public int minSlotCount;
    public int maxSlotCount;

    public float spacing = 10f; // 이미지 간격

    public float rowSpacing = 30f;
    public float columnSpacing;

    public float topMargin; // 상단 마진
    public float sideMargin; // 측면 마진

    private List<GameObject> slots = new List<GameObject>();
    private List<GameObject> removedSlots = new List<GameObject>();

    private int originalIndexA;
    private int originalIndexB;
    void Start()
    {
        for(int i = 0; i < 8; i++)
        {
            BeginSlotSetting();
        }    
    }

    private void BeginSlotSetting()
    {
        GameObject newSlot = Instantiate(blankSpacePrefab, imageParent);
        slots.Add(newSlot);
        UpdateLayout();
    }

    public void AddImage()
    {
        if (slots.Count >= minSlotCount && slots.Count <= maxSlotCount)
        {
            if(removedSlots.Count > 0)
            {
                foreach(GameObject slot in removedSlots)
                {
                    removedSlots.Remove(slot);
                    slots.Add(slot);
                    slot.SetActive(true);
                    UpdateLayout();
                    return;
                }
            }
        }
    }

    public void RemoveImage()
    {
        if(slots.Count >= minSlotCount && slots.Count <= maxSlotCount)
        {
            GameObject slotToRemove = slots[slots.Count - 1];
            slots.RemoveAt(slots.Count - 1);
            removedSlots.Add(slotToRemove);
            slotToRemove.SetActive(false);
            UpdateLayout();
        }
    }

    public int GetSlotCount()
    {
        return slots.Count;
    }

    public void FinalizeSwapSlots(GameObject slotA, GameObject slotB)
    {
        int indexA = slots.IndexOf(slotA);
        int indexB = slots.IndexOf(slotB);

        if(indexA != -1 && indexB != -1)
        {
            slots[indexA] = slotA;
            slots[indexB] = slotB;

            UpdateLayout();
        }
    }

    public void SwapSlotsTemporarily(GameObject slotA, GameObject slotB)
    {
        originalIndexA = slots.IndexOf(slotA);
        originalIndexB = slots.IndexOf(slotB);

        if (originalIndexA != -1 && originalIndexB != -1)
        {
            slots[originalIndexA] = slotB;
            slots[originalIndexB] = slotA;

            UpdateLayout();
        }
    }

    void UpdateLayout()
    {
        int totalImageCount = slots.Count;
        if (totalImageCount >= minSlotCount && totalImageCount <= maxSlotCount) 
        {
            switch (totalImageCount)
            {
                case 4:
                    columnSpacing = 150;
                    break;
                case 5 or 6:
                    columnSpacing = 75f;
                    break;
                case 7 or 8:
                    columnSpacing = 25f;
                    break;
            }


            int upperRowCount = Mathf.CeilToInt(totalImageCount / 2f);
            int lowerRowCount = totalImageCount - upperRowCount;

            RectTransform parentRect = imageParent.GetComponent<RectTransform>();
            float parentWidth = parentRect.rect.width;

            float upperSpacing = (parentWidth - (upperRowCount * slots[0].GetComponent<RectTransform>().rect.width) - ((upperRowCount - 1) * columnSpacing)) / 2;
            float lowerSpacing = (parentWidth - (lowerRowCount * slots[0].GetComponent<RectTransform>().rect.width) - ((lowerRowCount - 1) * columnSpacing)) / 2;

            for (int i = 0; i < slots.Count; i++)
            {
                RectTransform rectTransform = slots[i].GetComponent<RectTransform>();
                int row = (i < upperRowCount) ? 0 : 1;
                int column = (row == 0) ? i : i - upperRowCount;

                float xPos = sideMargin + (rectTransform.rect.width + columnSpacing) * column;
                float yPos = -topMargin - (rectTransform.rect.height + rowSpacing) * row;

                if (row == 0)
                {
                    xPos += upperSpacing;
                }
                else
                {
                    xPos += lowerSpacing;
                }

                rectTransform.pivot = parentRect.pivot;
                rectTransform.anchoredPosition = new Vector2(xPos - parentRect.rect.width / 2, yPos + parentRect.rect.height / 2 - 20);
            }
        }
    }
}
