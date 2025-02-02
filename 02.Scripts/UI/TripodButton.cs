using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TripodButton : MonoBehaviour
{
    int index;
    Color darkColor = new Color(0.5f, 0.5f, 0.5f, 1);
    Color lightColor = new Color(1, 1, 1, 1);
    public void SetActivatedIcon()
    {
        gameObject.GetComponent<Image>().sprite = SkillTabManager.Instance.skillData.m_tripodDatas[index].tripodSprite;
    }

    public void SetDeactivatedIcon()
    {
        gameObject.GetComponent<Image>().sprite = SkillTabManager.Instance.skillData.m_tripodDatas[index].deactivatedTripodSprite;
    }

    public void SetIndex(int i)
    {
        index = i;
    }

    public void SetIconDark()
    {
        gameObject.GetComponent<Image>().color = darkColor;
    }

    public void SetIconLight()
    {
        gameObject.GetComponent <Image>().color = lightColor;
    }
}
