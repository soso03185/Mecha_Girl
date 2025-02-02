using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentImage : MonoBehaviour
{
    private Image equipmentImage;
    private EquipmentManager m_equipmentManager;
    void Start()
    {
        equipmentImage = GetComponent<Image>();
        m_equipmentManager = Managers.Equipment;
    }

    void Update()
    {
        equipmentImage.sprite = m_equipmentManager.m_currentClickEquipment.m_image;
    }
}
