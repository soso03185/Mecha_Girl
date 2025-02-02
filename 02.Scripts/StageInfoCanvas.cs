using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoCanvas : MonoBehaviour
{
    public Button gameStartButton;
    public GameObject stagePanel;
    public Button clostButton;
    public StageLevel stageLevel;
    public void OnEnable()
    {
        stageLevel.Init();
    }
}
