using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 로딩 씬
/// </summary>
public class LoadingScreen : Singleton<LoadingScreen>
{
    public Slider m_progressBar; // UI 슬라이더로 프로그레스 바를 연결합니다.
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
        // 비동기 로딩 시작
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(m_moveSceneName);

        // 로딩 진행률을 업데이트합니다.
        while (!asyncLoad.isDone)
        {
            if(m_progressBar != null)
            {
                m_progressBar.value = asyncLoad.progress; // 0에서 1로 변경됩니다.
            }
            yield return null;
        }
    }
}