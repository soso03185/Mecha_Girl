using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public TextMeshProUGUI m_equipmentLevel;
    public TextMeshProUGUI m_equipmentGrade;
    public Image m_equipmentImage;
    public Slider m_equipmentSlider;

    public Equipment m_equipmentData;

    public int m_index;

    private EquipmentManager m_equipmentManager;
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClick);
        m_equipmentManager = Managers.Equipment;
    }
    public void SetSlot(EquipmentData data)
    {
        m_equipmentData = data.Equipment;

        m_equipmentLevel.text = data.Equipment.m_level.ToString();
        m_equipmentGrade.text = data.Equipment.m_grade.ToString();
        m_equipmentImage.sprite = data.Equipment.m_image;
        m_equipmentSlider.value = data.currentQuantity / 5f;
    }

    public void SetIndex(int i)
    {
        m_index = i;
    }

    private void OnClick()
    {
        m_equipmentManager.SetCurrentIndex(m_index);
    }
}
