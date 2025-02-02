using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

/// <summary>
/// ���� �÷��� �ÿ� �پ��� ���� Ÿ�̸� UI
/// </summary>
public class GameTimer : Singleton<GameTimer>
{
    [SerializeField] TextMeshProUGUI m_TextResult_Failed;
    [SerializeField] TextMeshProUGUI m_TimerText;
    [SerializeField] Image m_GaugeImage;
    //[SerializeField] GameObject ;
    [ReadOnly] public float m_TimeRemaining;
    public float m_TotalTime;

    bool m_isHalf = false;
    bool m_isCalled = false;

    Sequence mySequence;

    float m_elapsedTime = 1f;

    public bool isTimeOver = false;

    public void Init()
    {
        m_TextResult_Failed.gameObject.SetActive(false);
        m_TimeRemaining = m_TotalTime;
        // �̹���
        m_GaugeImage.fillAmount = m_TimeRemaining / m_TotalTime;
        m_TimerText.text = m_TimeRemaining.ToString("F2");
        m_isHalf = true;
        isTimeOver = false;
        m_TimerText.transform.localScale = new Vector3(1, 1, 1);
        m_GaugeImage.color = new Color(1, 0.75f, 0, 1);
        m_TimerText.colorGradient = new VertexGradient(Color.white, Color.white, new Color(1, 0.5f, 0, 1), new Color(1, 0.5f, 0, 1));
    }

    void Update()
    {
        if (GameManager.Instance.isGameStart)
        {
            // �ð��� ���������� Ÿ�̸� ����
            if (m_TimeRemaining > 0)
            {
                m_TimeRemaining -= Time.deltaTime;

                // �ð��� 0 ���Ϸ� �������� �ʵ��� ����
                m_TimeRemaining = Mathf.Max(m_TimeRemaining, 0);

                // Ÿ�̸� ���� �Ҽ��� �� �ڸ����� ǥ��
                m_TimerText.text = m_TimeRemaining.ToString("F2");

                // �̹���
                m_GaugeImage.fillAmount = m_TimeRemaining / m_TotalTime;
                if (m_TimeRemaining <= 10f)
                {
                    m_elapsedTime += Time.deltaTime;
                    if (m_elapsedTime >= 1f)
                    {
                        m_TimerText.transform.localScale = new Vector3(1.3f, 1.3f, 1.3f);
                        m_elapsedTime -= 1f;
                    }
                    else
                    {
                        m_TimerText.transform.localScale = m_TimerText.transform.localScale - new Vector3(Time.deltaTime * 0.3f, Time.deltaTime * 0.3f, Time.deltaTime * 0.3f);
                    }
                    m_GaugeImage.color = new Color(1, 0, 0.3f, 1);
                    m_TimerText.colorGradient = new VertexGradient(Color.white, Color.white, Color.red, Color.red);
                }
                else
                {
                    m_GaugeImage.color = new Color(1, 0.75f, 0, 1);
                    m_TimerText.colorGradient = new VertexGradient(Color.white, Color.white, new Color(1, 0.5f, 0, 1), new Color(1, 0.5f, 0, 1));
                }
            }
            else
            {
                //m_TextResult_Failed.gameObject.SetActive(true);
                m_TimerText.transform.localScale = new Vector3(1, 1, 1);
                m_GaugeImage.color = new Color(1, 0.75f, 0, 1);
                m_TimerText.colorGradient = new VertexGradient(Color.white, Color.white, new Color(1, 0.5f, 0, 1), new Color(1, 0.5f, 0, 1));
                isTimeOver = true;

                if (m_isCalled == false)
                {
                    GameManager.Instance.ShowGameOverPopUp();
                    m_isCalled = true;

                    if(GameManager.Instance.isPopUpOn)
                    {
                        Invoke("StageFailed", 0.5f);
                    }
                    else
                        StageFailed();
                }
            }

            if (m_TimeRemaining < m_TotalTime * 0.5f)
            {
                if (!m_isHalf)
                {
                    m_isHalf = true;

                }
            }
        }
    }

    void StageFailed()
    {
        StageManager.CheckStageFailed();
        m_isCalled = false;
    }
}

