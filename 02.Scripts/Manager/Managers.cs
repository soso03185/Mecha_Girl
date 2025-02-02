using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;
    public static Managers Instance 
    { 
        get
        {
            Init();

            return s_instance;
        }
    }

    public static ResourceManager Resource => Instance.resource;
    public static PoolManager Pool => Instance.pool;
    public static DataManager Data => Instance.data;
    public static MonsterManager Monsters => Instance.monsters;
    public static StageManager Stage => Instance.stage;
    public static SkillManager Skill => Instance.skill;
    public static EquipmentManager Equipment => Instance.equipment;
    public static CurrencyManager Currency => Instance.currency;

    PoolManager pool = new PoolManager();
    ResourceManager resource = new ResourceManager();
    DataManager data = new DataManager();
    MonsterManager monsters = new MonsterManager();
    StageManager stage = new StageManager();
    SkillManager skill = new SkillManager();
    EquipmentManager equipment = new EquipmentManager();
    CurrencyManager currency = new CurrencyManager();

    static void Init()
    {
        if (s_instance == null)
        {
            Debug.Log("√ ±‚»≠");

            GameObject go = null; //= GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance.pool.Init();
            s_instance.skill.Init();
            s_instance.data.Init();
            s_instance.currency.Init();
            
        }
    }

}