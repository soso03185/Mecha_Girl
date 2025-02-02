using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPanelManager : MonoBehaviour
{
    public GameObject Tab;

    private GameObject currentTab;

    public GameObject[] targetImage;
    public GameObject[] Buttons;
    public void ButtonClick(int n)
    {
        Tab.SetActive(true);
    }

    public void CloseTab()
    {
        if(currentTab != null)
        {
            currentTab.SetActive(false);
            currentTab = null;
        }
    }

}
