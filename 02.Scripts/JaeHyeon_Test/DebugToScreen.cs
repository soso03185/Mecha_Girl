using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class DebugToScreen : MonoBehaviour
{
    string m_myLog;
    HashSet<string> m_loggedErrors = new HashSet<string>();  // �̹� ��µ� ������ ����
    Queue<string> m_myLogQueue = new Queue<string>();

    void OnEnable() => Application.logMessageReceived += HandleLog;

    void OnDisable() => Application.logMessageReceived -= HandleLog;

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error)
        {
            // ���� �αװ� �̹� ��µ� �α����� Ȯ��
            string logEntry = "[Error] : " + logString;

            if (!m_loggedErrors.Contains(logEntry))  // �ߺ��� ���� �αװ� �ƴ� ��쿡��
            {
                m_loggedErrors.Add(logEntry);  // ���ο� ���� �α׸� ���
                m_myLogQueue.Enqueue(logEntry);  // ť�� �߰�

                // ������ ���� ������ �߰�
                string stackTraceEntry = "\n" + stackTrace;
                m_myLogQueue.Enqueue(stackTraceEntry);
            }
        }
    }

    void OnGUI()
    {
        m_myLog = string.Empty;

        // ť�� �ִ� ���� �α׸� ȭ�鿡 ���
        foreach (string log in m_myLogQueue)
        {
            m_myLog += log;
        }

        GUILayout.Label(m_myLog);
    }
}
