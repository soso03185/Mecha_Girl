
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;
using System; 

/// <summary>
/// Firestore에 저장 및 로드하는 스크립트
/// </summary>
public class FirestoreManager : Singleton<FirestoreManager>
{
    ListenerRegistration m_registration;
    FirebaseFirestore m_db;

    public void LoadPlayerData(Action<PlayerData> onDataLoaded)
    {
        m_db = FirebaseFirestore.DefaultInstance;
        m_registration = m_db.Collection("User_Sheets").Document(FirebaseManager.Instance.m_userId).Listen(snapshot =>
        {
            if (snapshot.Exists)
            {
                Debug.LogError("Document data loaded:");
                foreach (var field in snapshot.ToDictionary())
                {
                    Debug.Log($"{field.Key}: {field.Value}");
                }

                PlayerData counter = snapshot.ConvertTo<PlayerData>();
                onDataLoaded?.Invoke(counter);
            }
            else
            {
                Debug.LogError("Document does not exist.");
            }
        });
    }
    //  사용법  //
    //  LoadPlayerData(counter =>
    //    {
    //        // 여기에서 counter를 사용하여 데이터를 처리합니다.
    //        Debug.Log($"PlayerData loaded: {counter}");
    //    });


    public void SavePlayerField(string fieldName, object value)
    {
        m_db = FirebaseFirestore.DefaultInstance;
        DocumentReference countRef = m_db.Collection("User_Sheets").Document(FirebaseManager.Instance.m_userId);

        countRef.UpdateAsync(new Dictionary<string, object> { { fieldName, value } })
            .ContinueWithOnMainThread(task =>
            {
#if UNITY_EDITOR
                if (task.IsCompleted) Debug.Log($"{fieldName} has been successfully updated!");
                else Debug.LogError($"Error updating {fieldName}: {task.Exception}");
#endif
            });
    }
    // 사용법 //
    // SavePlayerField("AtkLevel", oldAtkLevel + 1);

    //private void OnDestroy()
    //{
    //    registration.Stop();
    //}
}

