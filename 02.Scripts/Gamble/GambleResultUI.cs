using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class GambleResultUI : MonoBehaviour
{
    public List<GameObject> m_resultIcons;
    public GameObject m_gameObject;
    public Button m_button;
    private float m_elapsedTime;
    private int m_index;
    private bool m_productionOff;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ShowResult(List<Skill> gambleResult)
    {
        for (int i = 0; i < 30; i++)
        {
            m_resultIcons[i].gameObject.SetActive(false);
        }
        gambleResult.Sort((s1, s2) => s1.m_skillID.CompareTo(s2.m_skillID));
        for (int i = 0; i < gambleResult.Count; i++)
        {
            m_resultIcons[i].gameObject.SetActive(true);
            m_resultIcons[i].transform.GetChild(0).GetComponent<Image>().sprite = gambleResult[i].m_image;
        }
    }

    public void ProductionOnOff(bool b)
    {
        m_productionOff = b; 
    }
}
