using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

[Serializable]
public struct AbilityData
{
    public int level;
    public double nextCost;
    public double abilityGap;
}

public class AbilitySystem : MonoBehaviour
{
    public Player m_player;


    public List<AbilityData> abilityData_Damage = new List<AbilityData>();
    public List<AbilityData> abilityData_Health = new List<AbilityData>();
    public List<AbilityData> abilityData_Defense = new List<AbilityData>();

    private List<List<AbilityData>> abilityDataList = new List<List<AbilityData>>();

    private List<int> currentLevelList = new List<int>();

    public List<AbilityPanel> m_abilityPanels = new List<AbilityPanel>();
    void Awake()
    {
        abilityDataList.Add(abilityData_Damage);
        abilityDataList.Add(abilityData_Health);
        abilityDataList.Add(abilityData_Defense);

        currentLevelList.Add(0);
        currentLevelList.Add(0);
        currentLevelList.Add(0);
    }

    private void Start()
    {
        m_player = GameManager.Instance.player.GetComponent<Player>();

        Init();
    }

    public void LevelUp(int index)
    {
        if (Managers.Data.abilityPoint < abilityDataList[index][currentLevelList[index]].nextCost)
            return;

        var ap = Managers.Data.abilityPoint;

        Managers.Data.abilityPoint -= abilityDataList[index][currentLevelList[index]].nextCost;

        BaseUIManager.instance.Refresh_AP(ap, Managers.Data.abilityPoint);

        if (index == 0)
        {
            m_player.Damage += abilityDataList[index][currentLevelList[index]].abilityGap;
            Managers.Data.playerDamage = m_player.Damage;
            m_abilityPanels[index].m_backgroundParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            m_abilityPanels[index].m_upgradeParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            m_abilityPanels[index].m_backgroundParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            m_abilityPanels[index].m_upgradeParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
        else if (index == 1)
        {
            m_player.MaxHealth += abilityDataList[index][currentLevelList[index]].abilityGap;
            m_player.Health += abilityDataList[index][currentLevelList[index]].abilityGap;
            Managers.Data.playerHealth = m_player.MaxHealth;
            m_abilityPanels[index].m_backgroundParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            m_abilityPanels[index].m_upgradeParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            m_abilityPanels[index].m_backgroundParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            m_abilityPanels[index].m_upgradeParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
        else if (index == 2)
        {
            m_player.Defense += abilityDataList[index][currentLevelList[index]].abilityGap;
            Managers.Data.playerDefense = m_player.Defense;
            m_abilityPanels[index].m_backgroundParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            m_abilityPanels[index].m_upgradeParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            m_abilityPanels[index].m_backgroundParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            m_abilityPanels[index].m_upgradeParticle.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
        }
        currentLevelList[index]++;
        m_abilityPanels[index].SetPanel(abilityDataList[index][currentLevelList[index]]);

        TutorialManager.NextStepButton();
        TutorialManager.TutorialStart(27);
    }

    public void Init()
    { 
        var ap = Managers.Data.abilityPoint;
        for (int i = 0; i < abilityDataList.Count; i++)
        {
            for(int j = 0; j < currentLevelList[i]; j++)
            {
                Managers.Data.abilityPoint += abilityDataList[i][j].nextCost;
            }
        } 
        BaseUIManager.instance.Refresh_AP(ap, Managers.Data.abilityPoint);

        for (int i = 0; i < currentLevelList.Count; i++)
        {
            currentLevelList[i] = 0;
        }

        for (int i = 0; i < abilityDataList.Count; i++)
        {
            m_abilityPanels[i].SetPanel(abilityDataList[i][currentLevelList[i]]);
        }

        m_player.Damage = m_player.initDamage;
        m_player.MaxHealth = m_player.initHealth;
        m_player.Defense = m_player.initDefense;
        Managers.Data.playerDamage = m_player.initDamage;
        Managers.Data.playerHealth = m_player.initHealth;
        Managers.Data.playerDefense = m_player.initDefense;
    }
}
