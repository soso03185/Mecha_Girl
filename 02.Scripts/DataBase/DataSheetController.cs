using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 내에서 입력한 값과 UGS 연동
/// </summary>
public class DataSheetController : MonoBehaviour
{
    [SerializeField] private string m_userId = "DummyUser";
    [SerializeField] private string m_displayName = "DummyDisplay";

    [Space(15f)]
    [Header("User ID")]
    [SerializeField] private TextMeshProUGUI m_userIdText;
    [SerializeField] private TextMeshProUGUI m_displayIdText;

    private void Start()
    {
        if (FirebaseManager.Instance.m_userId == null) return;

        m_userId = FirebaseManager.Instance.m_userId;
        m_displayName = FirebaseManager.Instance.m_displayName;
        m_userIdText.text += m_userId;
        m_displayIdText.text += m_displayName;

        CheckIfUserExists(m_userId);
    }

    private void CheckIfUserExists(string userId)
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        var userDocRef = firestore.Collection("User_Sheets").Document(userId); 

        userDocRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Firestore 조회 중 오류 발생: " + task.Exception);
                return;
            }

            var documentSnapshot = task.Result;

            if (documentSnapshot.Exists)
            {
                // 기존 유저일 경우
                Debug.LogError("Existing user logged in");
            }
            else
            {
                // 새로운 유저일 경우
                Debug.LogError("New user detected");

                // 새로운 유저를 위한 초기 설정
                CreateNewUserDocument(userId);
            }
        });
    }

    private void CreateNewUserDocument(string userId)
    {
        var firestore = FirebaseFirestore.DefaultInstance;
        var userDocRef = firestore.Collection("User_Sheets").Document(userId);

        var userData = new Dictionary<string, object>
    {
        // Key 이름이 변수명과 같아야 함.
        { "createdAt", Timestamp.GetCurrentTimestamp() },
        { "Is_TutorialClear", 0 },
        { "MaxClearStage", 1 },
        { "SkillPoint", 1 },
        { "AbilityPoint", 1 },
        { "SkillSlotCount", 1 },
        { "EquipSkills", 1 },
        { "EquipTriPods", 1 },
        { "NowQuest", 1 },
        { "AtkLevel", 1 },
        { "HealthLevel", 1 },
        { "DefLevel", 1 }, 
    };

        userDocRef.SetAsync(userData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.LogError("New user document created");
            }
            else
            {
                Debug.LogError("Failed to create user document: " + task.Exception);
            }
        });
    }
}
