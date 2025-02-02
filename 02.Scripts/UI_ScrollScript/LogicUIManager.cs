using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LogicUIManager : MonoBehaviour
{
    public GameObject m_Contents = null;
    public GameObject m_LogicPrefab = null;
    public int m_LogicCount = 5;

    public List<LogicCard> m_LogicCards = new List<LogicCard>();
    public List<LogicCard> m_LogicDatas = new List<LogicCard>();
    public LogicCard m_TestEmptyCard;

    [HideInInspector] public GameObject m_DummyCard;

    float m_RectWidth = 0f;
    int m_cardIndex;

    public int CardIndex
    {
        get => m_cardIndex;
        set
        {
            if (m_cardIndex != value)
            {
                m_cardIndex = value;
                DragCardMove();
            }
        }
    }

    public void DragCardMove()
    {
        Debug.Log("DragCardMove");
        m_TestEmptyCard.gameObject.SetActive(true);
        m_TestEmptyCard.transform.SetSiblingIndex(CardIndex);
    }

    private void Awake()
    {
        m_RectWidth = GetComponent<RectTransform>().rect.width;
    }

    public void Start()
    {
        m_DummyCard = Instantiate(m_LogicPrefab, transform);
        m_DummyCard.SetActive(false);
        m_DummyCard.name = "DummyCard";

        m_TestEmptyCard.gameObject.SetActive(false);

        // Logic Card First Init
        for (int i = 0; i < m_LogicCount; i++)
        {
            LogicCard go = Instantiate(m_LogicPrefab, m_Contents.transform).GetComponent<LogicCard>();
            m_LogicCards.Add(go);
        }
    }

    public void DragCard(PointerEventData eventData, Vector3 cardPos)
    {
        m_DummyCard.transform.position = eventData.position;

        // Card EndPosition And Scrolling
        if (eventData.position.x >= m_RectWidth)
            Debug.Log("Scrolling to Right");
        else if (eventData.position.x <= 0)
            Debug.Log("Scrolling to Left");
        else
        {
            // Drag.x < SelectCard.x
            if (eventData.position.x < cardPos.x)
            {
                for (int i = 0; i < m_LogicCards.Count; i++)
                    if (eventData.position.x < m_LogicCards[i].transform.position.x)
                    {
                        CardIndex = i;
                        break;
                    }
            }
            else
            {
                for (int i = m_LogicCards.Count - 1; i >= 0; i--)
                    if (eventData.position.x >= m_LogicCards[i].transform.position.x)
                    {
                        CardIndex = i + 1;
                        break;
                    }
            }
            Debug.Log("CardIndex = " + CardIndex);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Parent Begin");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Parent Drag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Parent End");
    }
}
