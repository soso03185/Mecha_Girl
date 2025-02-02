using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentList : MonoBehaviour
{
    public List<Equipment> m_helmetEquipment;
    public List<Equipment> m_armorEquipment;
    public List<Equipment> m_shoesEquipment;
    public List<Equipment> m_weaponEquipment;
    public List<Equipment> m_ringEquipment;
    public List<Equipment> m_necklaceEquipment;
    public List<List<Equipment>> m_equipmentList = new();

    private EquipmentManager m_equipmentManager;
    private void Start()
    {
        m_equipmentManager = Managers.Equipment;

        m_equipmentList.Add(m_helmetEquipment);    
        m_equipmentList.Add(m_armorEquipment);    
        m_equipmentList.Add(m_shoesEquipment);    
        m_equipmentList.Add(m_weaponEquipment);    
        m_equipmentList.Add(m_ringEquipment);    
        m_equipmentList.Add(m_necklaceEquipment);

        for (int i = 0; i < m_equipmentList.Count; i++)
        {
            m_equipmentManager.m_equipments.Add(new List<EquipmentData>());
            for (int j = 0; j < m_equipmentList[i].Count; j++)
            {
                EquipmentData equipmentData = new EquipmentData();
                equipmentData.currentQuantity = 5;
                equipmentData.Equipment = m_equipmentList[i][j];
                m_equipmentManager.m_equipments[i].Add(equipmentData);
            }
        }

        for(int i = 0; i < 6; i++)
        {
            m_equipmentManager.m_currentEquipment.Add(new());
        }
    }

    public Equipment GetNextEquipment(Equipment equipment)
    {
        List<Equipment> equipmentList = m_equipmentList[(int)equipment.m_part];

        int index = equipmentList.FindIndex(x => x.m_equipmentId.Equals(equipment.m_equipmentId));

        if(index == -1 || index == equipmentList.Count - 1)
        {
            return null;
        }

        return equipmentList[index + 1];
    }
}
