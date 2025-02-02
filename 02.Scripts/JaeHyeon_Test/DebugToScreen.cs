using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class DebugToScreen : MonoBehaviour
{
    string m_myLog;
    HashSet<string> m_loggedErrors = new HashSet<string>();  // 이미 출력된 오류를 추적
    Queue<string> m_myLogQueue = new Queue<string>();

    void OnEnable() => Application.logMessageReceived += HandleLog;

    void OnDisable() => Application.logMessageReceived -= HandleLog;

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Error)
        {
            // 오류 로그가 이미 출력된 로그인지 확인
            string logEntry = "[Error] : " + logString;

            if (!m_loggedErrors.Contains(logEntry))  // 중복된 오류 로그가 아닌 경우에만
            {
                m_loggedErrors.Add(logEntry);  // 새로운 오류 로그를 기록
                m_myLogQueue.Enqueue(logEntry);  // 큐에 추가

                // 오류의 스택 추적도 추가
                string stackTraceEntry = "\n" + stackTrace;
                m_myLogQueue.Enqueue(stackTraceEntry);
            }
        }
    }

    void OnGUI()
    {
        m_myLog = string.Empty;

        // 큐에 있는 오류 로그만 화면에 출력
        foreach (string log in m_myLogQueue)
        {
            m_myLog += log;
        }

        GUILayout.Label(m_myLog);
    }
}
