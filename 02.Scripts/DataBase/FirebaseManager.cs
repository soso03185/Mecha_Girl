using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using Google;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 파이어 베이스와 구글 아이디 연동
/// </summary>
public class FirebaseManager
{
    private static FirebaseManager instance = null;
    public static FirebaseManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FirebaseManager();
            }
            return instance;
        }
    }

    public string m_webClientId = "qr8i98j6r244kv2d29f6i459fjqij81r.apps.googleusercontent.com";
    public string m_nextSceneName = "01_JaeHyeonLoading";
    public string m_userId = "";
    public string m_displayName = "";

    public GoogleSignInConfiguration m_configuration;
    public FirebaseAuth m_auth;

    public void Init()
    {
        m_auth = FirebaseAuth.DefaultInstance;
        CheckFirebaseDependencies();

        m_configuration = new GoogleSignInConfiguration { WebClientId = m_webClientId, RequestEmail = true, RequestIdToken = true };
        GoogleSignIn.Configuration = m_configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWithOnMainThread(OnAuthenticationFinished);
    }

    public void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                    m_auth = FirebaseAuth.DefaultInstance;
                else
                    Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                Debug.LogError("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

    private void SignInWithGoogleOnFirebase(string idToken)
    {      
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        // Firebase에 인증 시도
        m_auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Error occurred during Firebase Sign-In: " + task.Exception);
            }
            else
            {
                Debug.LogError($"Sign In Successful.\n DisplayName: {task.Result.DisplayName} " +
                    $"\n UserId: {task.Result.UserId}");

                // Firebase 로그인 성공 후 userId와 displayName 저장
                m_userId = task.Result.UserId;
                m_displayName = task.Result.DisplayName;

                SceneManager.LoadScene(m_nextSceneName);
            }
        });
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.LogError("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.LogError("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.LogError("Canceled");
        }
        else
        {
            Debug.LogError("Google ID Token = " + task.Result.IdToken);
            Debug.LogError("Email = " + task.Result.Email);
            Debug.LogError("Welcome: " + task.Result.DisplayName + "!");
            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }

}
