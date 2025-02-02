using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// �ε� ��
/// </summary>
public class LoadingScreen : Singleton<LoadingScreen>
{
    public Slider m_progressBar; // UI �����̴��� ���α׷��� �ٸ� �����մϴ�.
    string m_moveSceneName = "01_JaeHyeonScene";

    public void Start()
    {
        MoveToScene(m_moveSceneName);
    }

    public void MoveToScene(string _sceneName)
    {
        m_moveSceneName = _sceneName;
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        // �񵿱� �ε� ����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_moveSceneName);

        // �ε� ������� ������Ʈ�մϴ�.
        while (!asyncLoad.isDone)
        {
            if(m_progressBar != null)
            {
                m_progressBar.value = asyncLoad.progress; // 0���� 1�� ����˴ϴ�.
            }
            yield return null;
        }
    }
}