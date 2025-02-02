using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttendanceCheckCard : MonoBehaviour
{
    Sprite m_MySprite;
    public Sprite m_CardSprite;
    public GameObject m_RewardItem;

    public Material m_Material;

    private void Awake()
    {
        m_MySprite = GetComponent<Image>().sprite;
    }

    private void Start()
    {
        m_MySprite = m_CardSprite;
    }

    public void GetReward() // Button Event
    {
        Debug.Log("Get Reward !");
    }
}
