using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Part
{
    Helmet,
    Armor,
    Shoes,
    Weapon,
    Ring,
    Necklace
}

public class Equipment : MonoBehaviour, IGamble
{
    public Define.Rarity Rarity { get; set; }
    public float Chance { get; set; }
    public Part m_part;
    public int m_grade;

    public Sprite m_image;
    public int m_level;
    public int m_equipmentId;
    public double m_ability;
    public double m_abilityPerLevel;
    public void LevelUp()
    {
        m_level++;
        m_ability += m_abilityPerLevel;
        Managers.Equipment.m_equipmentTotalLevel[(int)Rarity]++;
    }
}
