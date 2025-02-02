using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 화면 내에 FPS 출력
/// </summary>
public class FrameCounter : MonoBehaviour
{
    public Color m_color;
    public int m_size;

    float m_deltaTime = 0;
    bool m_isShow = true;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void OnGUI()
    {
        if(m_isShow)
        {
            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(30, 30, Screen.width, Screen.height);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = m_size;
            style.normal.textColor = m_color;

            float ms = m_deltaTime * 1000f;
            float fps = 1.0f / m_deltaTime;
            string text = string.Format("{0:0.} FPS ({1:0.0} ms)", fps, ms);

            GUI.Label(rect, text, style);
        }
    }
}
