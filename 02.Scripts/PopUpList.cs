using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PopUpList : MonoBehaviour
{
    public List<GameObject> popUpList = new List<GameObject>();

    public GameObject StageFailedUI;
    public GameObject StageClearedUI;
    private CanvasSetting canvasSetting;

    public GameObject EndingPopUp;
    public void Start()
    {
        canvasSetting = GetComponentInParent<CanvasSetting>();
    }

    public void Update()
    {
        if(canvasSetting != null)
        {
            if (canvasSetting.contentsCanvas[0].gameObject.activeSelf)
            {
                foreach (var popUp in popUpList)
                {
                    if (popUp.gameObject.activeSelf)
                    {
                        popUp.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Button>().interactable = false;
                    }
                }
            }
            else
            {
                foreach (var popUp in popUpList)
                {
                    if (popUp.gameObject.activeSelf)
                    {
                        popUp.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<Button>().interactable = true;
                    }
                }
            }

            if (canvasSetting.contentsCanvas[1].gameObject.activeSelf)
            {
                foreach (var popUp in popUpList)
                {
                    if (popUp.gameObject.activeSelf)
                    {
                        popUp.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().interactable = false;
                    }
                    else
                        popUp.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().interactable = true;
                }
            }
            else
            {
                foreach (var popUp in popUpList)
                {
                    if (popUp.gameObject.activeSelf)
                    {
                        popUp.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().interactable = true;
                    }
                }
            }
        }
    }

    public void NextStage()
    {
        GameManager.Instance.HandleStageCleared();
    }
}

