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
    public float duration = 0.4f; // 숫자가 올라가는 데 걸리는 시간

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
        // 숫자가 올라가는 연출
        DOTween.To(() => startValue, x =>
        {
            startValue = x;
            m_TextSP.text = Utility.ToCurrencyString(Mathf.FloorToInt(startValue)).ToString(); // 소수점 버림 처리
        }, endValue, duration).SetDelay(0.5f).SetUpdate(true);
         
        FirestoreManager.Instance.SavePlayerField("SkillPoint", endValue);
    }

    public void Refresh_AP(double startValue, double endValue)
    {
        // 숫자가 올라가는 연출
        DOTween.To(() => (float) startValue, x => 
        {
            startValue = x;
            m_TextAP.text = Utility.ToCurrencyString(Mathf.FloorToInt((float)startValue)).ToString(); // 소수점 버림 처리
            m_TextAbility_AP.text = Utility.ToCurrencyString(Mathf.FloorToInt((float)startValue)).ToString(); // 소수점 버림 처리
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
