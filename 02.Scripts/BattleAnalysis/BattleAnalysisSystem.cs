using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class BattleData
{
    public Skill skill;
    public double cumnulativeDamage;
    public double damagePerSecond;
    public float damageContribution;
    public int totalAttack;
    public double maxDamage;
    public double minDamage;
    public double avgDamage;
}

public class BattleAnalysisSystem : MonoBehaviour
{
    List<BattleData> m_battleDatas = new List<BattleData>();
    public List<BattleAnalysisPanelScrollView> m_scrollView = new List<BattleAnalysisPanelScrollView>();
    public TextMeshProUGUI m_totalTimeText;
    public TextMeshProUGUI m_totalDamageText;
    public TextMeshProUGUI m_avgDamagePerSecondText;
    public TextMeshProUGUI m_DamagedText;
    public TextMeshProUGUI m_avgExpPerSecondText;
    public TextMeshProUGUI m_killCountText;
    double m_totalDamage = 0;
    double m_avgDamagePerSecond = 0;
    double m_Damaged = 0;
    double m_avgExpPerSecond = 0;
    int m_killCount = 0;
    float m_totalTime = 0;
    // Start is called before the first frame update

    float m_elapsedTime;
    public void Initialize(List<Skill> skillList, Skill idleAttack)
    {
        m_battleDatas.Clear();

        List<Skill> skills = new List<Skill>();
        foreach (Skill skill in skillList)
        {
            skills.Add(skill);
        }
        skills.Add(idleAttack);

        for(int i = 0; i < skills.Count; i++)
        {
            if (skills[i] is BuffSkill)
                continue;
            BattleData newBattleData = new BattleData();
            newBattleData.skill = skills[i];
            newBattleData.cumnulativeDamage = 0;
            newBattleData.damagePerSecond = 0;
            newBattleData.damageContribution = 0;
            newBattleData.totalAttack = 0;
            newBattleData.maxDamage = 0;
            newBattleData.minDamage = 0;
            newBattleData.avgDamage = 0;
            m_battleDatas.Add(newBattleData);
        }

        foreach(var scrollView in m_scrollView)
        {
            scrollView.PanelsInstantiate(m_battleDatas);
        }

        m_totalDamage = 0;
        m_Damaged = 0;
        m_totalTime = 0;
        m_killCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_elapsedTime += Time.deltaTime;
        if(m_elapsedTime >= 1f)
        {
            m_totalTime += m_elapsedTime;
            m_elapsedTime = 0f;
            m_avgDamagePerSecond = m_totalDamage / m_totalTime;
            m_avgExpPerSecond = m_Damaged / m_totalTime;
            string minuteText;
            string secondText;

            if(((int)m_totalTime % 60) < 10)
            {
                secondText = "0" + ((int)m_totalTime % 60).ToString();
            }
            else
            {
                secondText = ((int)m_totalTime % 60).ToString();
            }
            if (((int)m_totalTime % 60) < 10)
            {
                minuteText = "0" + ((int)m_totalTime / 60).ToString();
            }
            else
            {
                minuteText = ((int)m_totalTime / 60).ToString();
            }

            m_totalTimeText.text = "전투 시간 : " + minuteText + " : " + secondText;
            

            foreach (BattleData data in m_battleDatas)
            {
                data.damagePerSecond = data.cumnulativeDamage / m_totalTime; 
                if(m_totalDamage > 0)
                    data.damageContribution = (float)(data.cumnulativeDamage / m_totalDamage) * 100f;
            }
        }
        
        foreach(var scrollView in m_scrollView)
        {
            for(int i = 0; i < scrollView.m_battleAnalysisPanels.Count; i++)
            {
                scrollView.m_battleAnalysisPanels[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = m_battleDatas[i].skill.m_image;
                scrollView.m_battleAnalysisPanels[i].transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = m_battleDatas[i].skill.m_skillKorName;
                scrollView.m_battleAnalysisPanels[i].transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = Utility.ToCurrencyString(m_battleDatas[i].cumnulativeDamage).ToString();
                    float tmp = m_battleDatas[i].damageContribution;
                scrollView.m_battleAnalysisPanels[i].transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>().text = ((int)tmp).ToString() + "%";           
                scrollView.m_battleAnalysisPanels[i].transform.GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = Utility.ToCurrencyString(m_battleDatas[i].damagePerSecond).ToString();
                scrollView.m_battleAnalysisPanels[i].transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>().text = m_battleDatas[i].totalAttack.ToString();
                scrollView.m_battleAnalysisPanels[i].transform.GetChild(6).gameObject.GetComponent<TextMeshProUGUI>().text = Utility.ToCurrencyString(m_battleDatas[i].avgDamage).ToString();
                scrollView.m_battleAnalysisPanels[i].transform.GetChild(7).gameObject.GetComponent<TextMeshProUGUI>().text = Utility.ToCurrencyString(m_battleDatas[i].maxDamage).ToString();
                scrollView.m_battleAnalysisPanels[i].transform.GetChild(8).gameObject.GetComponent<TextMeshProUGUI>().text = Utility.ToCurrencyString(m_battleDatas[i].minDamage).ToString();
            }
        }

        m_avgExpPerSecondText.text = "평균 경험치 : " + Utility.ToCurrencyString(m_avgExpPerSecond).ToString() + " ( 1초당 ) ";
        m_avgDamagePerSecondText.text = "평균 대미지 : " + Utility.ToCurrencyString(m_avgDamagePerSecond).ToString() + " ( 1초당 )";
    }

    public void DamageDataUpdate(Skill skill, double damage)
    {
        m_totalDamage += damage;
        m_totalDamageText.text = "총합 대미지 : " + Utility.ToCurrencyString(m_totalDamage).ToString();
        int index =  m_battleDatas.FindIndex(x => x.skill.m_skillID == skill.m_skillID);
        m_battleDatas[index].cumnulativeDamage += damage;
        m_battleDatas[index].totalAttack++;
        if(damage > m_battleDatas[index].maxDamage)
        {
            m_battleDatas[index].maxDamage = damage; 
        }

        if (m_battleDatas[index].minDamage == 0 || m_battleDatas[index].minDamage > damage)
        {
            m_battleDatas[index].minDamage = damage;
        }

        m_battleDatas[index].avgDamage = m_battleDatas[index].cumnulativeDamage / m_battleDatas[index].totalAttack;
    }

    public void KillCountUpdate()
    {
        m_killCount++;
        m_killCountText.text = "몬스터 처치 : " + m_killCount;
    }

    public void DamagedUpdate(double damage)
    {
        m_Damaged += damage;
        m_DamagedText.text = "받은 피해량 : " + Utility.ToCurrencyString(m_Damaged).ToString();
    }
}
