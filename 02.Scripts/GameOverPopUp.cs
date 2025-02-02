using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverPopUp : MonoBehaviour
{
    public void OnEnable()
    {
        Time.timeScale = 0.001f;
    }

    public void OnDisable()
    {
        Time.timeScale = Managers.Data.m_timeScale;
    }
}
