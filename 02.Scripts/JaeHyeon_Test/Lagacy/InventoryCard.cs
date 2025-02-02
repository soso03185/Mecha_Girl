using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class InventoryCard : MonoBehaviour
{
    public string m_ItemName = "Item";
    public GameObject m_HaveItem;
    public int m_HaveCount;

    public Sprite m_CardSprite;
    public Rarity m_Rarity = Rarity.C;
    
    [HideInInspector] public GameObject m_ItemPopup;
    Image m_Image;

    private void Awake()
    {
        m_Image = GetComponent<Image>();
    }

    private void Start()
    {
        m_Image.sprite = m_CardSprite;
    }

    public void CheckItem() // Button Event
    {
        if (m_ItemPopup != null) 
            m_ItemPopup.SetActive(true);

        m_Image.sprite = m_CardSprite;
        Debug.Log("Check Item !");
    }
}
