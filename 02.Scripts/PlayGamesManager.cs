using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Google;
using System.Threading.Tasks;
using Firebase.Extensions;

/// <summary>
/// GPGS 사용 이후 파이어베이스 구글 로그인 연동
/// </summary>
public class PlayGamesManager : MonoBehaviour
{
    private FirebaseManager m_firebaseManager;
    public string m_nextSceneName = "01_JaeHyeonLoading";

    void Start()
    {
        // 초기화
        PlayGamesPlatform.Activate();

        // 로그인 시도
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    // 구글 로그인 처리
    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            Debug.Log("구글 로그인 성공!");
            Debug.Log($"유저 닉네임: {Social.localUser.userName}");
        }
        else
        {
            Debug.LogError($"구글 로그인 실패: {status}");         
        }
    }

    public void StartButton()
    {
        Debug.LogError("Firebase 초기화");

        // FirebaseManager 초기화
        m_firebaseManager = FirebaseManager.Instance;
        m_firebaseManager.Init();
    }
}
