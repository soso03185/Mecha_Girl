using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using System.Security.Cryptography;
using Unity.VisualScripting;
using static Define;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject m_CardPrefab;
    [SerializeField] GameObject m_ItemPopup;
    [SerializeField] Transform m_ContentTransform;

    List<InventoryCard> m_InventoryDatas = new List<InventoryCard>();
    List<InventoryCard> m_InventoryCardList = new List<InventoryCard>();

    string m_CardName = "Inventory_Card";
    public Sprite TestSpr;

    private void Start()
    {
        InventoryCard card1 = new InventoryCard();
        InventoryCard card2 = new InventoryCard();
        InventoryCard card3 = new InventoryCard();
        InventoryCard card4 = new InventoryCard();

        card1.m_ItemName = "apple";
        card2.m_ItemName = "banana";
        card3.m_ItemName = "shit";
        card4.m_ItemName = "afk_Arena";

        card1.m_Rarity = Rarity.S;
        card2.m_Rarity = Rarity.A;
        card3.m_Rarity = Rarity.B;
        card4.m_Rarity = Rarity.C;

        card1.m_HaveCount = 4;
        card2.m_HaveCount = 1;
        card3.m_HaveCount = 6;
        card4.m_HaveCount = 3;

        m_InventoryDatas.Add(card1);
        m_InventoryDatas.Add(card2);
        m_InventoryDatas.Add(card3);
        m_InventoryDatas.Add(card4);

        Refresh(m_InventoryDatas);
    }
    
    public void Refresh(List<InventoryCard> listData)
    {
        // 풀링 오브젝트 사용.  UI_card.


        // Todo: Use ObjectPooling
        foreach (var child in m_InventoryCardList) 
        {
           // Destroy(child.gameObject);
            Managers.Pool.GetPool(m_CardName).ReturnObject(child.gameObject);
        }
        m_InventoryCardList.Clear();

        foreach(var child in listData)
        {
            GameObject go = Managers.Pool.GetPool(m_CardName).GetGameObject("UI/UI_JaeHyeon/" + m_CardName);
            go.transform.SetParent(m_ContentTransform);

            //  GameObject go = Instantiate(m_CardPrefab, this.transform);
            InventoryCard card = go.GetComponent<InventoryCard>();

            // Todo : Card Data binding
            card.m_ItemName  = child.m_ItemName;
            card.m_Rarity    = child.m_Rarity;
            card.m_HaveCount = child.m_HaveCount;            
            card.m_ItemPopup = m_ItemPopup;
            
            card.m_CardSprite = TestSpr;
            m_InventoryCardList.Add(card);
        }
    }

    public void BtnSortByRare()
    {
        var sortData = from card in m_InventoryDatas orderby card.m_Rarity descending select card;
        Bind(sortData);
    }

    public void BtnSortByName()
    {
        var sortData = from card in m_InventoryDatas orderby card.m_ItemName descending select card;
        Bind(sortData);
    }

    public void BtnSortByCount()
    {
        var sortData = from card in m_InventoryDatas orderby card.m_HaveCount descending select card;
        Bind(sortData);
    }

    public void Bind(IOrderedEnumerable<InventoryCard> sortData)
    {
        if (sortData != null)
        {
            List<InventoryCard> inventoryList = new List<InventoryCard>();
            foreach (var data in sortData)
            {
                inventoryList.Add(data);
            }

            Refresh(inventoryList);
        }
    }
}
