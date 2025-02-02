using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAnalysisPanelScrollView : MonoBehaviour
{
    [SerializeField]
    GameObject m_battleAnalysisPanelPrefab;

    public List<GameObject> m_battleAnalysisPanels = new List<GameObject>();

    public void PanelsInstantiate(List<BattleData> battleDatas)
    {
        m_battleAnalysisPanels.Clear();

        foreach (var battleData in battleDatas)
        {
            GameObject panel = Instantiate(m_battleAnalysisPanelPrefab, this.gameObject.transform);
            m_battleAnalysisPanels.Add(panel);
        }
    }
}
