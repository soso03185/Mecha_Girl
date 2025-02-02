using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionSlotInGame : MonoBehaviour
{
    [SerializeField]
    Image m_skillIcon;

    public Skill m_skillData;

    public SkillToolTip m_skillToolTip;

    public GameObject m_slotParticle;

    public void SetActionSlotInGame(Skill data, SkillToolTip skillToolTip)
    {
        m_skillData = data;
        m_skillIcon.sprite = data.m_image;
        m_skillToolTip = skillToolTip;
    }

    public void ShowToolTip()
    {
        m_skillToolTip.gameObject.SetActive(true);
        m_skillToolTip.SetToolTip(m_skillData);
    }
}
