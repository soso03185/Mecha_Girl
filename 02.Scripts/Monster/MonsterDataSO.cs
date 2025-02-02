using GoogleSheet.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataSO", menuName = "Scriptable Object/DataSO")]
public class MonsterDataSO : ScriptableObject
{
    public int MonsterID;
    public string Name;
    public int Level;
    public float Hp;
    public float Atk;
    public float Defense;
    public float MoveSpeed;
    public float AttackSpeed;
    public float AttackRange;
    public int NowExp;
    public int Gold;
    public int Goods;
    public int Goods2;
}
