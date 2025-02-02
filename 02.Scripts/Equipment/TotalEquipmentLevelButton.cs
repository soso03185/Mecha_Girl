using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TotalEquipmentLevelButton : MonoBehaviour
{
    private EquipmentManager m_equipmentManager;
    private TextMeshProUGUI m_text;
    // Start is called before the first frame update
    void Start()
    {
        m_equipmentManager = Managers.Equipment;
        m_text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        m_text.text = m_equipmentManager.m_equipmentTotalLevel[(int)m_equipmentManager.m_currentClickEquipment.Rarity].ToString();
    }
}
