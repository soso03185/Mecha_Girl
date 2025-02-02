using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapManager : MonoBehaviour
{
    public List<GameObject> Tab = new List<GameObject>();
    public Button[] TabBtn;
    public Button[] IdleTabBtn;

    private GameObject currentTab;

    public CanvasSetting canvasSetting;

    public bool isReadyToTab = false;

    public int lastClickedTab;

    public static TapManager instance;

    public void Awake()
    {
        canvasSetting = GameObject.Find("CanvasSetting").GetComponent<CanvasSetting>();

        for (int i = 0; i < 4; i++)
        {
            Tab[i] = canvasSetting.contentsCanvas[i].gameObject;
        }

        if (instance == null)
        {
            instance = this;
        }
    }

    public void Start()
    {
        int a = 0;
    }

    public void Update()
    {
        for(int i = 0; i < 4; i++)
        {
            if (Tab[i].activeSelf) 
            {
                currentTab = Tab[i];
                TabBtn[i].gameObject.SetActive(false);
                IdleTabBtn[i].gameObject.SetActive(true);
            }
            else
            {
                TabBtn[i].gameObject.SetActive(true);
                IdleTabBtn[i].gameObject.SetActive(false);
            }
        }
    }

    public void TabClick(int n)
    {
        lastClickedTab = n;

        for (int i = 0; i < Tab.Count; i++)
        {
            // 스킬 세팅 창일 때
            if (Tab[i].gameObject.name == "SkillSettingCanvas")
            {
                if (SkillTabManager.Instance != null)
                {
                    if (Tab[0].activeSelf)
                    {
                        SkillTabManager.Instance.skillScrollView.circularLayout.CompareSkillSlot();
                        if (!SkillTabManager.Instance.skillScrollView.circularLayout.isSkillChanged)
                        {
                            Tab[i].SetActive(i == n);
                            currentTab = Tab[i];
                        }
                        else
                        {
                            isReadyToTab = true;
                        }
                    }
                }
            }

            if (!isReadyToTab)
            {
                Tab[i].SetActive(i == n);
                currentTab = Tab[i];
            }
        }

        TutorialManager.NextStepButton();
        TutorialManager.TutorialStart(1);
        TutorialManager.TutorialStart(11);
        TutorialManager.TutorialStart(17); // 스킬 설명창 오픈 단계
        if (n == 1)
        {
            EventManager.OpenTab("StatUpgrade");
            TutorialManager.TutorialStart(26);
        }
    }

    public void closeTab()
    {
        TutorialManager.NextStepButton();
        TutorialManager.TutorialStart(8);
        TutorialManager.TutorialStart(14);
        TutorialManager.TutorialStart(24);
        TutorialManager.TutorialStart(28);

        if (SkillTabManager.Instance != null)
        {
            SkillTabManager.Instance.skillScrollView.circularLayout.CompareSkillSlot();
            if (SkillTabManager.Instance.skillScrollView.circularLayout.isSkillChanged)
                return;
            else
            {
                if (currentTab != null)
                {
                    for(int i = 0; i < Tab.Count; i++)
                    {
                        if(currentTab = Tab[i])
                        {
                            Tab[i].SetActive(false);
                        }
                    }
                }
            }
        }
        else
        {
            if (currentTab != null)
            {
                for (int i = 0; i < Tab.Count; i++)
                {
                    if (currentTab = Tab[i])
                    {
                        Tab[i].SetActive(false);
                    }
                }
            }
        }

    }

    public void ActiveTab(int n)
    {
        for(int i = 0; i < Tab.Count;i++)
        {
            Tab[i].SetActive(i == n);
            currentTab = Tab[i];
        }
    }
}
