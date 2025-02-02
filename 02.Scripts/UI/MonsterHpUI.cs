using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpUI : MonoBehaviour
{
    private MonsterScript m_monster;

    public Slider hpSlider;
    public Slider backSlider;
    public bool backHpHit = false;

    private RectTransform m_rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        hpSlider = GetComponent<Slider>();
        //backSlider = GetComponentInChildren<Slider>();
    }

    private void OnEnable()
    {
        hpSlider.value = 1f;
        backSlider.value = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_monster != null)
        {
            m_rectTransform.position =
                Camera.main.WorldToScreenPoint(m_monster.transform.position) + new Vector3(0, 70, 0);
        }

        //if((m_monster.GetComponent<MonsterScript>().isDamaged))
        //{
        //    hpSlider.value = Mathf.Lerp((float)hpSlider.value, (float)m_monster.GetComponent<MonsterScript>().HP /
        //                                       (float)m_monster.GetComponent<MonsterScript>().monsterInfo.Hp, Time.deltaTime * 5f);
        //}

        if(backHpHit)
        {
            backSlider.value = Mathf.Lerp(backSlider.value, hpSlider.value, Time.deltaTime * 10f);
            if(hpSlider.value >= backSlider.value - 0.01f)
            {
                backHpHit = false;
                backSlider.value = hpSlider.value;
            }
        }

    }

    public void ResetValue()
    {
        if(m_monster != null)
        {
            GetComponent<Slider>().value = (float)(m_monster.Health /
                                                   m_monster.monsterInfo.Hp);
        }
        
    }

    public void SetMonster(MonsterScript monster)
    {
        m_monster = monster;
    }

    public void Dmg()
    {
        Invoke("BackHpFun", 0.5f);
    }

    void BackHpFun()
    {
        backHpHit = true;
    }

    public void DeleteTarget()
    {
        m_monster = null;
    }
}
