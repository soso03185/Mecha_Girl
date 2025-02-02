using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TotalPowerUI : MonoBehaviour
{
    // UI
    public TextMeshProUGUI m_totalPowerText;
    public TextMeshProUGUI m_totalPowerValue;
    public TextMeshProUGUI m_totalPowerChange;
    public Image m_totalPowerUp;
    public Image m_totalPowerDown;
    public Image m_totalPowerImage1;
    public Image m_totalPowerImage2;
    // 데이터
    Player m_player;
    SkillList m_skillList;

    // 변수
    float m_elapsedTime;
    double m_prevTotalPower;
    double m_curTotalPower;
    bool m_isOn;

    // Start is called before the first frame update
    void Start()
    {
        m_player = GameManager.Instance.player.GetComponent<Player>();
        m_skillList = SkillList.Instance;
        m_prevTotalPower = CalculateTotalPower();
        m_curTotalPower = m_prevTotalPower;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isOn)
        {
            m_elapsedTime += Time.deltaTime;
            if (m_elapsedTime < 1)
            {
                m_totalPowerValue.text = Utility.ToCurrencyString(Mathf.Lerp((float)m_prevTotalPower, (float)m_curTotalPower, m_elapsedTime)).ToString();
            }
            else if (m_elapsedTime <= 2)
            {
                m_totalPowerValue.text = Utility.ToCurrencyString(m_curTotalPower).ToString();
                Color color = m_totalPowerChange.color;
                color.a = 2 - m_elapsedTime;
                m_totalPowerChange.color = color;
                Color colorAll = new Color(1, 1, 1, 2 - m_elapsedTime);
                Color color2 = new Color(0, 0, 0, (2 - m_elapsedTime) * 0.85f);
                m_totalPowerText.color = colorAll;
                m_totalPowerValue.color = colorAll;
                if (m_curTotalPower > m_prevTotalPower)
                {
                    m_totalPowerUp.color = colorAll;
                }
                else
                {
                    m_totalPowerDown.color = colorAll;
                }
                m_totalPowerImage1.color = color2;
                m_totalPowerImage2.color = color2;
            }
            else
            {
                m_totalPowerUp.gameObject.SetActive(false);
                m_totalPowerDown.gameObject.SetActive(false);
                gameObject.SetActive(false);
                m_elapsedTime = 0;
                m_prevTotalPower = m_curTotalPower;
                m_isOn = false;
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    double CalculateTotalPower()
    {
        if(m_player == null) 
            m_player = GameManager.Instance.player.GetComponent<Player>();

        double playerTotalPower = m_player.Damage * 50 + m_player.Defense * 1.5 + m_player.MaxHealth * 1.5 + m_player.ElementAmp * 50;
        double skillTotalPower = 0;
        foreach (Skill skill in m_skillList.skillList)
        {
            skillTotalPower += skill.m_skillLevel * 25;
        }

        return playerTotalPower + skillTotalPower;
    }

    void OnEnable()
    {
        if (m_curTotalPower > 0)
        {
            m_curTotalPower = CalculateTotalPower();
        }
        if (m_curTotalPower != m_prevTotalPower)
        {
            m_isOn = true;
            m_totalPowerChange.text = Utility.ToCurrencyString(Mathf.Abs((float)(m_curTotalPower - m_prevTotalPower))).ToString();
            Color colorAll = new Color(1, 1, 1, 1);
            Color color2 = new Color(0, 0, 0, 0.85f);
            gameObject.SetActive(true);
            if (m_curTotalPower > m_prevTotalPower)
            {
                m_totalPowerChange.color = Color.green;
                m_totalPowerUp.gameObject.SetActive(true);
                m_totalPowerUp.color = colorAll;
            }
            else if (m_curTotalPower < m_prevTotalPower)
            {
                m_totalPowerChange.color = Color.red;
                m_totalPowerDown.gameObject.SetActive(true);
                m_totalPowerDown.color = colorAll;
            }
            m_totalPowerText.color = colorAll;
            m_totalPowerValue.color = colorAll;
            m_totalPowerImage1.color = color2;
            m_totalPowerImage2.color = color2;
        }
    }
}
