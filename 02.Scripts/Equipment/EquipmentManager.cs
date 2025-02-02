using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class EquipmentData
{
    public Equipment Equipment;
    public int currentQuantity;
}

public class EquipmentManager
{
    public readonly List<List<EquipmentData>> m_equipments = new();
    private List<EquipmentData> m_currentPartEquipments = new();
    public List<Equipment> m_currentEquipment = new List<Equipment>(6);
    public Equipment m_currentClickEquipment = new Equipment();
    public List<int> m_equipmentTotalLevel = new() {0,0,0,0 };

    private Part m_currentPart;
    private int m_currentIndex;

    public void SynthesisEquipment()
    {
        
        if (m_currentPartEquipments[m_currentIndex].currentQuantity >= 5 && m_currentIndex < m_equipments[(int)m_currentPart].Count - 1)
        {
            m_currentPartEquipments[m_currentIndex].currentQuantity -= 5;
            m_currentPartEquipments[m_currentIndex + 1].currentQuantity += 1;
        }
    }

    public void BatchSynthesisEquipment()
    {
        for (int i = 0; i < m_currentPartEquipments.Count - 1; i++)
        {
            if (m_currentPartEquipments[i].currentQuantity >= 5)
            {
                m_currentPartEquipments[i].currentQuantity -= 5;
                m_currentPartEquipments[i + 1].currentQuantity += 1;
            }
        }

        foreach(var i in m_currentPartEquipments)
        {
            Debug.Log(i.currentQuantity);
        }
    }

    public void LevelUpEquipment()
    {
        m_currentPartEquipments[m_currentIndex].Equipment.LevelUp();
    }

    public void SetCurrentPart(int part)
    {
        m_currentPart = (Part)part;
        m_currentPartEquipments = m_equipments[(int)m_currentPart];
        if (m_currentEquipment[(int)m_currentPart] != null)
        {
            m_currentIndex = m_currentPartEquipments.FindIndex(x => x.Equipment.m_equipmentId.Equals(m_currentEquipment[(int)m_currentPart].m_equipmentId));
        }
        m_currentClickEquipment = m_equipments[(int)m_currentPart][m_currentIndex].Equipment;
    }

    public void SetCurrentIndex(int index)
    {
        m_currentIndex = index;
        m_currentClickEquipment = m_equipments[(int)m_currentPart][m_currentIndex].Equipment;
    }

    public int GetCurrentPart()
    {
        return (int)m_currentPart;
    }

    public int GetCurrentIndex()
    {
        return m_currentIndex;
    }
    public List<EquipmentData> GetCurrentEquipmentData()
    {
        return m_currentPartEquipments;
    }

    public void EquipOrUnEquip()
    {
        if (m_currentEquipment[(int)m_currentPart].m_equipmentId != m_currentPartEquipments[m_currentIndex].Equipment.m_equipmentId)
        {
            m_currentEquipment[(int)m_currentPart] = m_currentPartEquipments[m_currentIndex].Equipment;
        }
        else
        {
            m_currentEquipment[(int)m_currentPart] = default;
        }
    }
}
