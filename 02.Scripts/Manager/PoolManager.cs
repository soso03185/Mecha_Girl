using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager
{
    public Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();

    // 부모 자동 생성
    public void CreatePool(string path, int size)
    {
        ObjectPool pool = new ObjectPool();
        pool.CreateObject(path, size);

        // 풀 안에서 찾을떄는 파일이름만 가지고 찾도록
        string name = Managers.Resource.SubStringPath(path);

        pools.Add(name, pool);
    }
    
    // 부모 지정
    public void CreatePool(string path, int size, Transform parent)
    {
        ObjectPool pool = new ObjectPool();
        pool.CreateObject(path, size, parent);

        // 풀 안에서 찾을떄는 파일이름만 가지고 찾도록
        string name = Managers.Resource.SubStringPath(path);

        pools.Add(name, pool);
    }

    // 부모 지정
    public void CreatePoolInFile(string filename)
    {
        ObjectPool pool = new ObjectPool();
        pool.CreateObjectsInFile(filename);

        // 풀 안에서 찾을떄는 파일이름만 가지고 찾도록
        string name = Managers.Resource.SubStringPath(filename);

        pools.Add(name, pool);
    }

    public void Init()
    {
        CreatePool("Monsters/Boxy_Robot", 70);
        CreatePool("Monsters/Bot_Robot", 100);
        CreatePool("Monsters/Bot_Robot2", 100);
        CreatePool("Monsters/Cute_Robot", 70);
        CreatePool("Monsters/Cute_Robot2", 70);
        CreatePool("Monsters/Metal_Robot", 70);
        CreatePool("Monsters/Tanker_Robot", 70);
        CreatePool("Monsters/Gripper_Robot", 70);
        CreatePool("Monsters/Nexus_Robot", 70);
        CreatePool("Monsters/SM_Veh_Mech_06", 1);

        CreatePool("MedKit", 50);

        //CreatePool("Monsters/Brute_Robot_Black", 50);
        //CreatePool("Monsters/Brute_Robot_Brown", 50);
        //CreatePool("Monster/TechRobot", 100);
        //CreatePool("Monster/MetalRobot", 100);
        //CreatePool("Monster/Rob01", 100);
        //CreatePool("SkillObject/Rob1BulletBlue", 100);
        GameObject m_canvas = Managers.Resource.Instantiate("UI/HpCanvas");

        GameObject.DontDestroyOnLoad(m_canvas);

        m_canvas.name = "HpCanvas";

        Managers.Pool.CreatePool("UI/Slider", 200, m_canvas.transform);

        //CreatePool("Monster/Slime", 1);
        CreatePool("UI/EquipIcon", 72);
        CreatePool("UI/SkillSlotSample", 40);
        CreatePool("UI/EmptySkillSlot", 40);
        //UI
        CreatePool("UI/UI_JaeHyeon/Ranking_Card", 10);
        CreatePool("UI/UI_JaeHyeon/Inventory_Card", 10);
        CreatePool("UI/UI_JaeHyeon/Attendance_Card", 10);

        // 대미지 텍스트
        CreatePool("UI/DamageText", 100);
        CreatePool("UI/YellowDamageText", 100);
        CreatePool("UI/BlueDamageText", 100);
        CreatePool("UI/PurpleDamageText", 100);
        CreatePool("UI/ImmuneText", 100);

        GameObject touchEffectCanvas = Managers.Resource.Instantiate("UI/TouchEffectCanvas");

        GameObject.DontDestroyOnLoad(touchEffectCanvas);

        // 마우스 터치 이펙트
        CreatePool("UI/TouchEffect", 20, touchEffectCanvas.transform);
        CreatePool("UI/LevelUpEffect", 20, touchEffectCanvas.transform);

        // 이펙트
        CreatePool("Particle/WaterHit", 100);
        CreatePool("Particle/Electro hit", 100);
        CreatePool("Particle/Magnetic", 100);
        CreatePool("Particle/LightningHit", 100);

        

    }

    public ObjectPool GetPool(string path)
    {
        foreach(var pool in pools)
        {
            if (pool.Key == path)
            {
                return pool.Value;
            }
        }

        return null;
    }

    public GameObject GetGameObject(string path)
    {
        foreach (var pool in pools)
        {
            if (pool.Key == path)
            {
                return pool.Value.GetGameObject(path);
            }
        }
        return null;
    }

    public void ResetPool(string path)
    {
        foreach(var go in pools[path].objectsList)
        {
            pools[path].activeCount = 0;
            go.SetActive(false);
        }
    }

    public void ResetPool()
    {
        foreach(var pool in pools)
        {
            pool.Value.activeCount = 0;
            foreach(var go in pool.Value.objectsList)
            {
                go.SetActive(false);
            }
        }
    }
}
