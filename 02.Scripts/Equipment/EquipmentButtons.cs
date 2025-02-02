using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentButtons : MonoBehaviour
{
    [SerializeField] List<CurrentEquipment> m_currentEquipmentImages = new List<CurrentEquipment>(6);
    private EquipmentManager m_equipmentManager;
    [SerializeField] private EquipmentScrollView m_equipmentScrollView;

    // Start is called before the first frame update
    void Start()
    {
        m_equipmentManager = Managers.Equipment;
    }

    public void SynthesisEquipment()
    {
        m_equipmentManager.SynthesisEquipment();
        m_equipmentScrollView.SetSlot();
    }

    public void SetCurrentPart(int value)
    {
        m_equipmentManager.SetCurrentPart(value);
        m_equipmentScrollView.SetSlot();
    }

    public void BatchSynthesisEquipment()
    {
        m_equipmentManager.BatchSynthesisEquipment();
        m_equipmentScrollView.SetSlot();
    }

    public void LevelUpEquipment()
    {
        m_equipmentManager.LevelUpEquipment();
        m_equipmentScrollView.SetSlot();
    }

    public void EquipOrUnEquip()
    {
        m_equipmentManager.EquipOrUnEquip();
        EquipmentUpdate();
    }

    public void EquipmentUpdate()
    {
        foreach (var image in m_currentEquipmentImages)
        {
            image.EquipmentUpdate();
        }
    }
}
