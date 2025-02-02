using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityPanel : MonoBehaviour
{
    public TextMeshProUGUI m_abilityLevelText;
    public TextMeshProUGUI m_abilityGapText;
    public TextMeshProUGUI m_abilityCostText;

    public GameObject m_backgroundParticle;
    public GameObject m_upgradeParticle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPanel(AbilityData data)
    {
        m_abilityCostText.text = Utility.ToCurrencyString(data.nextCost).ToString();
        m_abilityGapText.text = "+" + Utility.ToCurrencyString(data.abilityGap).ToString();
        m_abilityLevelText.text = "Lv. " + data.level.ToString();
        //m_upgradeParticle = 
    }
}
