using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGS;
using GoogleSheet.Type;
using System;
using System.IO;

/// <summary>
/// 구글 시트에서 문서를 관리하는 UGS의 데이터 컨트롤러
/// </summary>
public class UGS_DataManager : MonoBehaviour
{
    public Dictionary<int, ProjectCustom_UserData.UserData> m_UserDataDic = new Dictionary<int, ProjectCustom_UserData.UserData>();

    public Dictionary<int, ProjectCustom_UserData.StandardData> m_StandardDataDic = new Dictionary<int, ProjectCustom_UserData.StandardData>();

    public Dictionary<int, ProjectCustom_MonsterData.MonsterData> m_MonsterDataDic = new Dictionary<int, ProjectCustom_MonsterData.MonsterData>();

    public Dictionary<int, ProjectCustom_StageData.StageData> m_StageDataDic = new Dictionary<int, ProjectCustom_StageData.StageData>();

    public enum ShowDataType
    {
        User = 0,
        Standard = 1,
        Monster = 2,
        Stage = 3
    }

    void Awake()
    {
        var obj = FindObjectsOfType<UGS_DataManager>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        UnityGoogleSheet.LoadAllData();
        UserDataLocalLoad();
        StandardDataLocalLoad();
        MonsterDataLocalLoad();
        StageDataLocalLoad();

        Managers.Data.InitMonsterInfo();

    }
    
    //------------------------------------ StandardData ------------------------------------
    public void StandardDataLocalLoad()
    {
        Debug.Log("StandardData_LocalLoad");

        m_StandardDataDic.Clear();

        foreach (var value in ProjectCustom_UserData.StandardData.StandardDataList)
        {
            m_StandardDataDic.Add(value.Level, value);
        }
    }

    public void StandardDataLiveLoad()
    {
        Debug.Log("StandardData_LiveLoad");

        m_StandardDataDic.Clear();

        UnityGoogleSheet.LoadFromGoogle<int, ProjectCustom_UserData.StandardData>((list, map) => {
            list.ForEach(x => {
                m_StandardDataDic.Add(x.Level, x);
            });
        }, true);
    }

    //------------------------------------ UserData ------------------------------------
    public void UserDataLocalLoad()
    {
        Debug.Log("UserData_LocalLoad");

        m_UserDataDic.Clear();

        foreach (var value in ProjectCustom_UserData.UserData.UserDataList)
        {
            m_UserDataDic.Add(value.UserID, value);
        }
    }
    public void UserDataLiveLoad()
    {
        Debug.Log("UserData_LiveLoad");

        m_UserDataDic.Clear();

        UnityGoogleSheet.LoadFromGoogle<int, ProjectCustom_UserData.UserData>((list, map) => {
            list.ForEach(x => {
                m_UserDataDic.Add(x.UserID, x);
            });
        }, true);
    }

    //------------------------------------ Monster ------------------------------------
    public void MonsterDataLocalLoad()
    {
        Debug.Log("MonsterData_LocalLoad");

        m_MonsterDataDic.Clear();

        foreach (var value in ProjectCustom_MonsterData.MonsterData.MonsterDataList)
        {
            m_MonsterDataDic.Add(value.MonsterID, value);
        }
    }
    public void MonsterDataLiveLoad()
    {
        Debug.Log("MonsterData_LiveLoad");

        m_MonsterDataDic.Clear();

        UnityGoogleSheet.LoadFromGoogle<int, ProjectCustom_MonsterData.MonsterData>((list, map) => {
            list.ForEach(x => {
                m_MonsterDataDic.Add(x.MonsterID, x);
            });
        }, true);
    }

    //------------------------------------ Stage ------------------------------------
    public void StageDataLocalLoad()
    {
        Debug.Log("StageData_LocalLoad");

        m_StageDataDic.Clear();

        foreach (var value in ProjectCustom_StageData.StageData.StageDataList)
        {
            m_StageDataDic.Add(value.StageLevel, value);
        }
    }

    public void StageDataLiveLoad()
    {
        Debug.Log("StageData_LiveLoad");

        m_StageDataDic.Clear();

        UnityGoogleSheet.LoadFromGoogle<int, ProjectCustom_StageData.StageData>((list, map) =>
        {
            list.ForEach(x =>
            {
                m_StageDataDic.Add(x.StageLevel, x);
            });
        }, true);
    }

    //------------------------------------ Show ------------------------------------
    [VisibleEnum(typeof(ShowDataType))]
    public void ShowData(int _showDataType)
    {
        switch (_showDataType)
        {
            case (int)ShowDataType.User:
                {
                    foreach (var child in m_UserDataDic)
                        Debug.Log($"PlayerName: {child.Value.Name}, UserID: {child.Value.UserID}");
                }
                break;
            case (int)ShowDataType.Standard:
                {
                    foreach (var child in m_StandardDataDic)
                        Debug.Log($"Level: {child.Value.Level}, NeedExp: {child.Value.NeedExp}");
                }
                break;
            case (int)ShowDataType.Monster:
                {
                    foreach (var child in m_MonsterDataDic)
                        Debug.Log($"MonsterName: {child.Value.Name}, MonsterID: {child.Value.MonsterID}");
                }
                break;
            case (int)ShowDataType.Stage:
                {
                    foreach (var child in m_StageDataDic)
                        Debug.Log($"StageLevel: {child.Value.StageLevel}");
                }
                break;
        }
    }

}