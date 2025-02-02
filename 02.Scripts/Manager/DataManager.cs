using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGS;
using static Define;
using System;

[Serializable]
public struct Tripod
{
    public int firstSlot;
    public int secondSlot;
    public int thirdSlot;
}

public class DataManager
{
    public Dictionary<string, MonsterInfo> monsterInfoList = new Dictionary<string, MonsterInfo>(); 

    // 초기화 되면 안되는 정보들
    private int m_currentStageLevel = 1;
    public int currentStageLevel
    {
        get { return m_currentStageLevel; }
        set
        {
            m_currentStageLevel = value;

            if (m_currentStageLevel <= 0)
                m_currentStageLevel = 1;
        }
    }
    public int lastStageLevel = 1;

    public List<int> currentSkillList;
    public List<int> preset1;
    public List<int> preset2;
    public List<int> preset3;

    public int lastActivatedPresetNum;

    public StageType currentStageType = StageType.Challenge;

    public double playerDamage = 100;
    public double playerHealth = 1000;
    public double playerDefense = 10;

    // 스킬 레벨, 트라이포드 정보

    public int slotCount = 0;

    public int maxStageClearLevel = 1;

    public int skillUpgradePoint = 0;
    public double abilityPoint = 0;
    public int is_Tut = 0;

    public float m_timeScale;

    public void Init()
    {
        currentSkillList =  new List<int>();
        preset1 =           new List<int>();
        preset2 =           new List<int>();
        preset3 =           new List<int>();

        currentStageLevel = GameManager.Instance.currentStage;

        if (GameManager.Instance.isCheatOn)
        {
            if (GameManager.Instance.slotCount < 3)
            {
                slotCount = 3;
            }
            else if (GameManager.Instance.slotCount > 5)
                slotCount = 5;
            else
                slotCount = GameManager.Instance.slotCount;
        }
        else
        {
            if (currentStageLevel < 4 && currentStageLevel > 0)
            {
                slotCount = 3;
            }
            else if (currentStageLevel <= 7 && currentStageLevel >= 4)
            {
                slotCount = 4;
            }
            else
                slotCount = 5;
        }

        m_timeScale = 1f;
        for (int i = 0; i < slotCount; i++)
        {
            AddEmptySkill();
        }

        lastActivatedPresetNum = 1; 

    }

    public void AddEmptySkill()
    {
        currentSkillList.Add(4);
        preset1.Add(4);
        preset2.Add(4);
        preset3.Add(4);
    }

    [System.Serializable]
    public struct UserInfo
    {
        public int UserID;
        public string Name;
        public int Level;
        public double Hp;
        public double BasicAtk;
        public double Defense;
        public float MoveSpeed;
        public float LogicChangeSpeed;
        public int NowExp;
        public float CriticalChance;
        public float CriticalMultiplier;
        public int Equipment;
        public int Gold;
        public int Goods;
        public int Goods2;
        public int Goods3;
        public int Costume;
        public int StageLevel;
    }

    [System.Serializable]
    public struct MonsterInfo
    {
        public int MonsterID;
        public string Name;
        public int Level;
        public double Hp;
        public float Atk;
        public float Defense;
        public float MoveSpeed;
        public float AttackSpeed;
        public float AttackRange;
        public int NowExp;
        public int Gold;
        public int Goods;
        public int Goods2;
        public int DetectionRange;
        public string SpawnType;
        public string Characteristic;
    }

    [System.Serializable]
    public struct StageInfo
    {
        public int key;
        public int stageLevel;
        public int maxMonsterCount;
        public MonsterName monster1Name;
        public int monster1Count;
        public SpawnType spawnType1;
        public MonsterName monster2Name;
        public int monster2Count;
        public SpawnType spawnType2;
        public bool isBossStage;
        public string bossMonsterName;
        public int monsterTypeCount;
        public string stageCharacteristic;
        public bool isStageWave;
        public int bossMonsterCount;
    }

    public UGS_DataManager UGS_Data;

    public UserInfo GetUserInfo(int UserID)
    {
        if (UGS_Data == null)
        {
            UGS_Data = GameObject.Find("DataLoadTest").GetComponent<UGS_DataManager>();
        }

        UserInfo userInfo;

        userInfo.UserID = UGS_Data.m_UserDataDic[UserID].UserID;
        userInfo.Name = UGS_Data.m_UserDataDic[UserID].Name;
        userInfo.Level = UGS_Data.m_UserDataDic[UserID].Level;
        userInfo.Hp = UGS_Data.m_UserDataDic[UserID].Hp;
        userInfo.BasicAtk = UGS_Data.m_UserDataDic[UserID].Atk;
        userInfo.Defense = UGS_Data.m_UserDataDic[UserID].Defense;
        userInfo.MoveSpeed = UGS_Data.m_UserDataDic[UserID].MoveSpeed;
        userInfo.LogicChangeSpeed = UGS_Data.m_UserDataDic[UserID].LogicChangeSpeed;
        userInfo.NowExp = UGS_Data.m_UserDataDic[UserID].NowExp;
        userInfo.CriticalChance = UGS_Data.m_UserDataDic[UserID].CriticalChance;
        userInfo.CriticalMultiplier = UGS_Data.m_UserDataDic[UserID].CriticalMultiplier;
        userInfo.Equipment = UGS_Data.m_UserDataDic[UserID].Equipment;
        userInfo.Gold = UGS_Data.m_UserDataDic[UserID].Gold;
        userInfo.Goods = UGS_Data.m_UserDataDic[UserID].Goods;
        userInfo.Goods2 = UGS_Data.m_UserDataDic[UserID].Goods2;
        userInfo.Goods3 = UGS_Data.m_UserDataDic[UserID].Goods3;
        userInfo.Costume = UGS_Data.m_UserDataDic[UserID].Costume;
        userInfo.StageLevel = UGS_Data.m_UserDataDic[UserID].StageLevel;

        return userInfo;
    }

    public MonsterInfo GetMonsterInfo(int monsterID)
    {
        if (UGS_Data == null)
        {
            UGS_Data = GameObject.Find("DataLoadTest").GetComponent<UGS_DataManager>();
        }

        MonsterInfo monsterInfo;
        //if(monsterInfo == null)
        //{
        //    monsterInfo = new MonsterDataSO();
        //}

        monsterInfo.MonsterID = UGS_Data.m_MonsterDataDic[monsterID].MonsterID;
        monsterInfo.Name = UGS_Data.m_MonsterDataDic[monsterID].Name;
        monsterInfo.Level = UGS_Data.m_MonsterDataDic[monsterID].Level;
        monsterInfo.Hp = UGS_Data.m_MonsterDataDic[monsterID].Hp;
        monsterInfo.Atk = UGS_Data.m_MonsterDataDic[monsterID].Atk;
        monsterInfo.Defense = UGS_Data.m_MonsterDataDic[monsterID].Defense;
        monsterInfo.MoveSpeed = UGS_Data.m_MonsterDataDic[monsterID].MoveSpeed;
        monsterInfo.AttackSpeed = UGS_Data.m_MonsterDataDic[monsterID].AttackSpeed;
        monsterInfo.AttackRange = UGS_Data.m_MonsterDataDic[monsterID].AttackRange;
        monsterInfo.NowExp = UGS_Data.m_MonsterDataDic[monsterID].NowExp;
        monsterInfo.Gold = UGS_Data.m_MonsterDataDic[monsterID].Gold;
        monsterInfo.Goods = UGS_Data.m_MonsterDataDic[monsterID].Goods;
        monsterInfo.Goods2 = UGS_Data.m_MonsterDataDic[monsterID].Goods2;
        monsterInfo.DetectionRange = UGS_Data.m_MonsterDataDic[monsterID].DetectionRange;
        monsterInfo.SpawnType = UGS_Data.m_MonsterDataDic[monsterID].SpawnType;
        monsterInfo.Characteristic = UGS_Data.m_MonsterDataDic[monsterID].Characteristic;

        return monsterInfo;
    }

    public void InitMonsterInfo()
    {
        for(int i = 2001; i <= 2022; i++)
        {
            var temp = GetMonsterInfo(i);

            monsterInfoList.Add(temp.Name, temp);
        }
    }

    public MonsterInfo GetMonserInfoByName(string name)
    {
        foreach(var monsterInfo in monsterInfoList)
        {
            if (monsterInfo.Value.Name == name)
            {
                return monsterInfo.Value;
            }
        }
        return default(MonsterInfo);
    }

    public void ClearMonsterInfo()
    {
        monsterInfoList.Clear();
    }

    public StageInfo GetStageInfo(int stageLevel)
    {
        if (UGS_Data == null)
        {
            UGS_Data = GameObject.Find("DataLoadTest").GetComponent<UGS_DataManager>();
        }

        if (UGS_Data.m_StageDataDic.Count < stageLevel)
        {
            Debug.Log("It was last stage");
            //UnityEditor.EditorApplication.isPlaying = false;
        }

        StageInfo stageInfo;

        stageInfo.key = UGS_Data.m_StageDataDic[stageLevel].Key;
        stageInfo.stageLevel = UGS_Data.m_StageDataDic[stageLevel].StageLevel;
        stageInfo.maxMonsterCount = UGS_Data.m_StageDataDic[stageLevel].MaxMonsterCount;
        stageInfo.monster1Name =
            (MonsterName)Enum.Parse(typeof(MonsterName), UGS_Data.m_StageDataDic[stageLevel].Monster1Name);
        stageInfo.monster1Count = UGS_Data.m_StageDataDic[stageLevel].Monster1Count;
        stageInfo.spawnType1 =
            (SpawnType)Enum.Parse(typeof(SpawnType), UGS_Data.m_StageDataDic[stageLevel].SpawnOption1);
        stageInfo.monster2Name =
            (MonsterName)Enum.Parse(typeof(MonsterName), UGS_Data.m_StageDataDic[stageLevel].Monster2Name);
        stageInfo.monster2Count = UGS_Data.m_StageDataDic[stageLevel].Monster2Count;
        stageInfo.spawnType2 =
            (SpawnType)Enum.Parse(typeof(SpawnType), UGS_Data.m_StageDataDic[stageLevel].SpawnOption2);
        stageInfo.isBossStage = System.Convert.ToBoolean(UGS_Data.m_StageDataDic[stageLevel].IsBossStage);
        stageInfo.bossMonsterName = UGS_Data.m_StageDataDic[stageLevel].BossMonster;
        stageInfo.monsterTypeCount = UGS_Data.m_StageDataDic[stageLevel].MonsterTypeCount;
        stageInfo.stageCharacteristic = UGS_Data.m_StageDataDic[stageLevel].StageCharacteristic;
        stageInfo.isStageWave = System.Convert.ToBoolean(UGS_Data.m_StageDataDic[stageLevel].isStageWave);
        stageInfo.bossMonsterCount = UGS_Data.m_StageDataDic[stageLevel].BossMonsterCount;

        return stageInfo;
    }


}
