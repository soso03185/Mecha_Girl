using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ������ ���������� ������ ���� ������ HPbar
/// </summary>
public class BossMonster_HpBar : Singleton<BossMonster_HpBar>
{
    public MonsterScript m_bossMonster;
    Slider m_BossHpSlider;

    private void OnEnable()
    {
        m_BossHpSlider = GetComponent<Slider>();
        m_BossHpSlider.value = 1.0f;
    }

    private void Update()
    {
        if(m_bossMonster != null)
        {
            m_BossHpSlider.value = (float) m_bossMonster.Health / (float) m_bossMonster.MaxHealth;
        }
    }
}
