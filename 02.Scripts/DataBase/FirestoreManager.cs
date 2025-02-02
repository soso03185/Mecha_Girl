
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;
using System; 

/// <summary>
/// Firestore�� ���� �� �ε��ϴ� ��ũ��Ʈ
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
    //  ����  //
    //  LoadPlayerData(counter =>
    //    {
    //        // ���⿡�� counter�� ����Ͽ� �����͸� ó���մϴ�.
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
    // ���� //
    // SavePlayerField("AtkLevel", oldAtkLevel + 1);

    //private void OnDestroy()
    //{
    //    registration.Stop();
    //}
}

