using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataBaseDefine
{
   string m_collectionName;
   string m_nextScene;
}

[FirestoreData]
public struct Counter
{
    [FirestoreProperty]
    public int Count { get; set; }

    [FirestoreProperty]
    public string UpdateBy { get; set; }
}

[FirestoreData]
public struct PlayerData
{
    [FirestoreProperty] public int Is_TutorialClear { get; set; }
    [FirestoreProperty] public int MaxClearStage { get; set; }
    [FirestoreProperty] public int SkillPoint { get; set; }
    [FirestoreProperty] public int AbilityPoint { get; set; }
    [FirestoreProperty] public int SkillSlotCount { get; set; }
    [FirestoreProperty] public int EquipSkills { get; set; }
    [FirestoreProperty] public int EquipTriPods { get; set; }
    [FirestoreProperty] public int NowQuest { get; set; }
    [FirestoreProperty] public int AtkLevel { get; set; }
    [FirestoreProperty] public int HealthLevel { get; set; }
    [FirestoreProperty] public int DefLevel { get; set; }

}

[FirestoreData]
public struct UserData
{                       
    [FirestoreProperty] public string Name { get; set; }
    [FirestoreProperty] public int Level { get; set; }
    [FirestoreProperty] public int NowExp { get; set; }
    [FirestoreProperty] public int Stage { get; set; }
    [FirestoreProperty] public int Gold { get; set; }
    [FirestoreProperty] public int Goods { get; set; }
    [FirestoreProperty] public int Goods2 { get; set; }
    [FirestoreProperty] public int Costume { get; set; }
    [FirestoreProperty] public float Hp { get; set; }
    [FirestoreProperty] public float Atk { get; set; }
    [FirestoreProperty] public float Def { get; set; }
    [FirestoreProperty] public float MoveSpeed { get; set; }
    [FirestoreProperty] public float AttackSpeed { get; set; }
    [FirestoreProperty] public float AttackRange { get; set; }
    [FirestoreProperty] public float LogicChangeSpeed { get; set; }
    [FirestoreProperty] public float CriticalChance { get; set; }
    [FirestoreProperty] public float CriticalMultiplier { get; set; }                        
}                       