using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class BaseUIManager: MonoBehaviour
{
    public static BaseUIManager instance;

    public TextMeshProUGUI m_monsterCount;
    public TextMeshProUGUI m_TextAP;
    public TextMeshProUGUI m_TextAbility_AP;
    public TextMeshProUGUI m_TextSP;
    public TextMeshProUGUI m_StageCount;

    public GameObject m_BossHpBarObj;
    public float duration = 0.4f; // ���ڰ� �ö󰡴� �� �ɸ��� �ð�

    public GameObject speedUpButton;
    public GameObject speedDownButton;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        m_TextAP.text = Managers.Data.abilityPoint.ToString();
        m_TextAbility_AP.text = Managers.Data.abilityPoint.ToString();

        m_TextSP.text = Managers.Data.skillUpgradePoint.ToString();
    }

    void Update()
    {     
        Refresh();
    }

    public void Refresh()
    {       
        m_monsterCount.text = (Managers.Stage.stageInfo.maxMonsterCount - SpawnManager.spawnInstance.killedMonsterCount).ToString();
    }

    public void Refresh_SP(float startValue, float endValue)
    {
        // ���ڰ� �ö󰡴� ����
        DOTween.To(() => startValue, x =>
        {
            startValue = x;
            m_TextSP.text = Utility.ToCurrencyString(Mathf.FloorToInt(startValue)).ToString(); // �Ҽ��� ���� ó��
        }, endValue, duration).SetDelay(0.5f).SetUpdate(true);
         
        FirestoreManager.Instance.SavePlayerField("SkillPoint", endValue);
    }

    public void Refresh_AP(double startValue, double endValue)
    {
        // ���ڰ� �ö󰡴� ����
        DOTween.To(() => (float) startValue, x => 
        {
            startValue = x;
            m_TextAP.text = Utility.ToCurrencyString(Mathf.FloorToInt((float)startValue)).ToString(); // �Ҽ��� ���� ó��
            m_TextAbility_AP.text = Utility.ToCurrencyString(Mathf.FloorToInt((float)startValue)).ToString(); // �Ҽ��� ���� ó��
        }, endValue, duration).SetDelay(0.5f).SetUpdate(true);
  
        FirestoreManager.Instance.SavePlayerField("AbilityPoint", endValue);
    }

    public void SpeedUp()
    {
        Managers.Data.m_timeScale = 1.5f;
        Time.timeScale = Managers.Data.m_timeScale;
        speedDownButton.SetActive(true);
        speedUpButton.SetActive(false);
    }

    public void SpeedDown()
    {
        Managers.Data.m_timeScale = 1f;
        Time.timeScale = Managers.Data.m_timeScale;
        speedDownButton.SetActive(false);
        speedUpButton.SetActive(true);
    }

}
