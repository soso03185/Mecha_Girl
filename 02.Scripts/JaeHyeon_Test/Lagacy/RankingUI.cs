using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingUI : MonoBehaviour
{
    [SerializeField] GameObject m_RankingCard;
    [SerializeField] Transform m_RankingCardTransform;
    [SerializeField] int m_RankingCardCount;

    string m_CardName = "Ranking_Card";

    private void Start()
    {
        for (int i = 0; i < m_RankingCardCount; i++)
        {
           // GameObject go = Instantiate(m_RankingCard, m_RankingCardTransform);
            Managers.Pool.GetPool(m_CardName).GetGameObject("UI/UI_JaeHyeon/" + m_CardName).transform.SetParent(m_RankingCardTransform);
        }
    }
}
