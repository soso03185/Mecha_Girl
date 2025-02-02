using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class ManaGaugeUI : Singleton<ManaGaugeUI>
{
    [SerializeField] int m_MaxMana;
    [SerializeField] Slider m_ManaSlider;
    [SerializeField] TextMeshProUGUI m_ManaText;

    public float m_Mana 
    {
        get { return m_mana; }
        set 
        {
            m_mana = value;
            m_ManaSlider.value = m_mana / m_MaxMana;
        }
    }
    float m_mana;

    private void Start()
    {
        m_ManaSlider.value = m_Mana / m_MaxMana;
    }

    public void Update()
    {
        m_ManaText.text = m_Mana.ToString();
    }
}
