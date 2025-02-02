using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentScrollView : MonoBehaviour
{
    private int SlotCount = 4;
    public List<EquipmentSlot> slots = new List<EquipmentSlot>();
    public EquipmentSlot m_equipmentSlot;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < SlotCount; i++)
        {
            EquipmentSlot slot = Instantiate(m_equipmentSlot.gameObject).GetComponent<EquipmentSlot>();
            slot.SetIndex(i);
            slots.Add(slot);
            slot.transform.SetParent(this.GetComponent<RectTransform>());
        }
    }

    public void SetSlot()
    {
        int i = 0;
        List<EquipmentData> equipmentList = Managers.Equipment.GetCurrentEquipmentData();
        foreach (var data in equipmentList)
        {
            slots[i].SetSlot(data);
            i++;
        }
    }

}
