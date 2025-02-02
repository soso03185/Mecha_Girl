using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;
using static DataManager;
using static Define;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager spawnInstance;

    public Transform target;

    private bool isGameOver = false;
    public int spawnMonsterCount;
    public int currentMonsterCount;
    public int killedMonsterCount;

    private List<Vector3> monsterSpawnPos = new List<Vector3>();
    public float spawnRadius = 10f;


    public int groupSpawnMinRange;

    public int groupSpawnMaxRange;

    public float normalSpawnTime;
    public float delaySpawnTimeMin;
    public float delaySpawnTimeMax;
    public float groupSpawnTime;
    public float bossSpawnTime;

    public bool isSpawn;
    public bool isAllSpawned = false;

    private List<Coroutine> enumerators = new List<Coroutine>();
    private Coroutine normalSpawnCoroutine;
    private Coroutine normalSpawnCoroutineTest;
    private Coroutine delaySpawnCoroutine;
    private Coroutine delaySpawnCoroutineTest;
    private Coroutine groupSpawnCoroutine;
    private Coroutine groupSpawnCoroutineTest;
    private Coroutine bossSpawnCoroutineTest;

    public GameObject FollowingSpawnPreset1;
    public GameObject FollowingSpawnPreset2;
    public GameObject FollowingSpawnPreset3;
    public GameObject FollowingSpawnPreset4;

    private Vector3 originPos1;
    private Vector3 originPos2;
    private Vector3 originPos3;
    private Vector3 originPos4;

    public GameObject spawnPreset1;
    public GameObject spawnPreset2;
    public GameObject spawnPreset3;
    public GameObject spawnPreset4;
    public GameObject spawnPreset5;
    public GameObject spawnPreset6;

    private Vector3 mapMinBounds = new Vector3(-39, 0, -39);
    private Vector3 mapMaxBounds = new Vector3(39, 0, 39);

    bool m_isInitEnd = false;

    private void Awake()
    {
        spawnInstance = this;
    }

    public void Init()
    {
        target = GameManager.Instance.player.transform;

        originPos1 = FollowingSpawnPreset1.transform.position - target.position;
        originPos2 = FollowingSpawnPreset2.transform.position - target.position;
        originPos3 = FollowingSpawnPreset3.transform.position - target.position;
        originPos4 = FollowingSpawnPreset4.transform.position - target.position;
        m_isInitEnd = true;
    }

    public void Update()
    {
        if(m_isInitEnd)
        {
            FollowingSpawnPreset1.transform.position = target.position + originPos1;
            FollowingSpawnPreset2.transform.position = target.position + originPos2;
            FollowingSpawnPreset3.transform.position = target.position + originPos3;
            FollowingSpawnPreset4.transform.position = target.position + originPos4;
        }
        
        if(Managers.Stage.SpawnNextWave)
        {
            if(Managers.Stage.stageInfo.monster1Name == Managers.Stage.stageInfo.monster2Name)
            {
                StartSpawning(Managers.Stage.stageInfo.maxMonsterCount, Managers.Stage.stageInfo.monster2Count, Managers.Stage.stageInfo.monster2Name.ToString(), Managers.Stage.stageInfo.spawnType2, StageType.Challenge);
            }
            else
                StartSpawning(Managers.Stage.stageInfo.maxMonsterCount, Managers.Stage.spawnList[1].monsterCount, Managers.Stage.spawnList[1].monsterName.ToString(), Managers.Stage.spawnList[1].spawnType, StageType.Challenge);
            Managers.Stage.SpawnNextWave = false;
        }

        if(Managers.Stage.SpawnBoss)
        {
            StartSpawning(Managers.Stage.stageInfo.maxMonsterCount, Managers.Stage.stageInfo.bossMonsterCount, Managers.Stage.stageInfo.bossMonsterName, SpawnType.Boss, StageType.Challenge);
            Managers.Stage.SpawnBoss = false;
        }
        //if(Managers.Data.currentStageType == StageType.Idle && spawnMonsterCount <= 0)
        //{
        //    isSpawn = true;
        //}
        //
        //if (Managers.Data.currentStageType == StageType.Idle && spawnMonsterCount <= 0)
        //{
        //    isSpawn = true;
        //}

    }

    public void StartSpawning(int stageMaxMonsterCount, int monsterCount, string monsterName, SpawnType spawnType, StageType stageType)
    {
        switch (spawnType)
        {
            case SpawnType.NormalAroundPlayer:
                normalSpawnCoroutine = StartCoroutine(this.NormalSpawnAroundPlayer(monsterCount, monsterName, stageMaxMonsterCount, stageType));
                enumerators.Add(normalSpawnCoroutine);
                break;
            case SpawnType.NormalOnPreset:
                normalSpawnCoroutineTest = StartCoroutine(this.NormalSpawnOnPreset(monsterCount, monsterName, stageMaxMonsterCount, stageType));
                enumerators.Add(normalSpawnCoroutineTest);
                break;
            case SpawnType.DelayAroundPlayer:
                delaySpawnCoroutine = StartCoroutine(this.DelaySpawnAroundPlayer(monsterCount, monsterName, stageMaxMonsterCount, stageType));
                enumerators.Add(delaySpawnCoroutine);
                break;
            case SpawnType.DelayOnPreset:
                delaySpawnCoroutineTest = StartCoroutine(this.DelaySpawnOnPreset(monsterCount, monsterName, stageMaxMonsterCount, stageType));
                enumerators.Add(delaySpawnCoroutineTest);
                break;
            case SpawnType.GroupAroundPlayer:
                groupSpawnCoroutine = StartCoroutine(this.GroupSpawnAroundPlayer(monsterCount, monsterName, stageMaxMonsterCount, stageType));
                enumerators.Add(groupSpawnCoroutine);
                break;
            case SpawnType.GroupOnPreset:
                groupSpawnCoroutineTest = StartCoroutine(this.GroupSpawnOnPreset(monsterCount, monsterName, stageMaxMonsterCount, stageType));
                enumerators.Add(groupSpawnCoroutineTest);
                break;
            case SpawnType Boss:
                bossSpawnCoroutineTest = StartCoroutine(BossSpawn(monsterCount, monsterName, stageMaxMonsterCount));
                enumerators.Add(bossSpawnCoroutineTest);
                break;
        }
    }

    public void StopSpawning(int stageMaxMonsterCount)
    {
        if (spawnMonsterCount == stageMaxMonsterCount)
        {
            isAllSpawned = true;
            Debug.Log("모든 몬스터 소환 완료");
            StopAllCoroutines();
        }
    }

    public void StopSpawning()
    {
        isSpawn = false;
        for (int i = 0; i < enumerators.Count; i++)
        {
            StopCoroutine(enumerators[i]);
        }
        enumerators.Clear();
    }

    bool IsInsideMapBounds(Vector3 pos)
    {
        return pos.x >= mapMinBounds.x && pos.x <= mapMaxBounds.x &&
        pos.z >= mapMinBounds.z && pos.z <= mapMaxBounds.z;
    }

    IEnumerator NormalSpawnAroundPlayer(int maxMonsterCount, string _monsterName, int stageMaxMonster, StageType stageType)
    {
        if(stageType == StageType.Challenge)
        {
            int monsterCount = 0;
            while (!isGameOver)
            {
                if (monsterCount <= maxMonsterCount)
                {
                    monsterCount++;
                }

                if (monsterCount <= maxMonsterCount && spawnMonsterCount < stageMaxMonster)
                {
                    Vector3 spawnPosition = GetRandomPositionAroundPlayer();
                    spawnPosition.y = 0;
                    yield return new WaitForSeconds(normalSpawnTime);
                    
                    SpawnMonster(_monsterName, spawnPosition, stageMaxMonster);
                }
                else
                {
                    yield return null;
                }

                
            }
        }
        else if (stageType == StageType.Idle)
        {
            while (!isGameOver)
            {
                Vector3 spawnPosition = GetRandomPositionAroundPlayer();
                spawnPosition.y = 0;
                yield return new WaitForSeconds(normalSpawnTime);
                SpawnMonster(_monsterName, spawnPosition);
            }
        }
    }

    IEnumerator NormalSpawnOnPreset(int maxMonsterCount, string _monsterName, int stageMaxMonster, StageType stageType)
    {
        if (stageType == StageType.Challenge)
        {
            int monsterCount = 0;
            while (!isGameOver)
            {
                int spawnPlaneNum = Random.Range(1, 7);
                if (monsterCount <= maxMonsterCount)
                {
                    monsterCount++;
                }

                if (monsterCount <= maxMonsterCount && spawnMonsterCount < stageMaxMonster)
                {
                    Vector3 spawnPosition = SpawnMonsterOnPreset(spawnPlaneNum);
                    spawnPosition.y = 0;
                    yield return new WaitForSeconds(normalSpawnTime);
                    SpawnMonster(_monsterName, spawnPosition, stageMaxMonster);
                }
                else
                {
                    yield return null;
                }
            }
        }
        else if (stageType == StageType.Idle)
        {
            while (!isGameOver)
            {
                int spawnPlaneNum = Random.Range(1, 7);
                Vector3 spawnPosition = SpawnMonsterOnPreset(spawnPlaneNum);
                spawnPosition.y = 0;
                yield return new WaitForSeconds(normalSpawnTime);
                SpawnMonster(_monsterName, spawnPosition);
            }
        }
    }


    IEnumerator DelaySpawnAroundPlayer(int maxMonsterCount, string _monsterName, int stageMaxMonster, StageType stageType)
    {
        if(isSpawn)
        {
            if (stageType == StageType.Challenge)
            {
                int monsterCount = 0;
                while (!isGameOver)
                {
                    if(monsterCount <= maxMonsterCount)
                    {
                        monsterCount++;
                    }

                    if (monsterCount <= maxMonsterCount && spawnMonsterCount < stageMaxMonster)
                    {
                        Vector3 spawnPosition = GetRandomPositionAroundPlayer();
                        spawnPosition.y = 0;
                        yield return new WaitForSeconds(Random.Range(delaySpawnTimeMin, delaySpawnTimeMax));
                        SpawnMonster(_monsterName, spawnPosition, stageMaxMonster);
                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
            else if(stageType == StageType.Idle)
            {
                while (!isGameOver)
                {
                    Vector3 spawnPosition = GetRandomPositionAroundPlayer();
                    spawnPosition.y = 0;
                    yield return new WaitForSeconds(Random.Range(delaySpawnTimeMin, delaySpawnTimeMax));
                    SpawnMonster(_monsterName, spawnPosition, stageMaxMonster);
                }
            }
        }
    }

    IEnumerator DelaySpawnOnPreset(int maxMonsterCount, string _monsterName, int stageMaxMonster, StageType stageType)
    {
        if (isSpawn)
        {
            if (stageType == StageType.Challenge)
            {
                int monsterCount = 0;
                while (!isGameOver)
                {
                    int spawnPlaneNum = Random.Range(1, 7);
                    if (monsterCount <= maxMonsterCount)
                    {
                        monsterCount++;
                    }

                    if (monsterCount <= maxMonsterCount && spawnMonsterCount < stageMaxMonster)
                    {
                        Vector3 spawnPosition = SpawnMonsterOnPreset(spawnPlaneNum);
                        spawnPosition.y = 0;
                        yield return new WaitForSeconds(Random.Range(delaySpawnTimeMin, delaySpawnTimeMax));
                        SpawnMonster(_monsterName, spawnPosition, stageMaxMonster);
                    }
                    else
                    {
                        yield return null;
                    }
                }
            }
            else if (stageType == StageType.Idle)
            {
                while (!isGameOver)
                {
                    int spawnPlaneNum = Random.Range(1, 7);
                    Vector3 spawnPosition = SpawnMonsterOnPreset(spawnPlaneNum);
                    spawnPosition.y = 0;
                    yield return new WaitForSeconds(Random.Range(delaySpawnTimeMin, delaySpawnTimeMax));
                    SpawnMonster(_monsterName, spawnPosition, stageMaxMonster);
                }
            }
        }
    }

    IEnumerator GroupSpawnAroundPlayer(int maxMonsterCount, string _monsterName, int stageMaxMonster, StageType stageType)
    {
        if(stageType == StageType.Challenge)
        {
            int monsterCount = 0;
            while (!isGameOver)
            {
                if (monsterCount <= maxMonsterCount)
                {
                    monsterCount++;
                }

                if (monsterCount <= maxMonsterCount && spawnMonsterCount < stageMaxMonster)
                {
                    int groupSize = Random.Range(groupSpawnMinRange, groupSpawnMaxRange);
                    int spawnPlaneCount = Random.Range(3, 6);

                    for (int j = 0; j < spawnPlaneCount; j++)
                    {
                        int spawnPlaneNum = Random.Range(1, 7);
                        for (int i = 0; i < groupSize; i++)
                        {
                            Vector3 spawnPosition = GetRandomPositionAroundPlayer();
                            spawnPosition.y = 0;
                            SpawnMonster(_monsterName, spawnPosition, stageMaxMonster);
                        }
                    }

                    yield return new WaitForSeconds(groupSpawnTime);
                }
                else
                {
                    yield return null;
                    continue;
                }
            }
        }
        else if(stageType == StageType.Idle)
        {
            while (!isGameOver)
            {
                int groupSize = Random.Range(groupSpawnMinRange, groupSpawnMaxRange);
                int spawnPlaneCount = Random.Range(3, 6);

                for(int j = 0; j < spawnPlaneCount; j++)
                {
                    int spawnPlaneNum = Random.Range(1, 7);
                    for (int i = 0; i < groupSize; i++)
                    {
                        Vector3 spawnPosition = GetRandomPositionAroundPlayer();
                        spawnPosition.y = 0;
                        SpawnMonster(_monsterName, spawnPosition, stageMaxMonster);
                    }
                }

                yield return new WaitForSeconds(groupSpawnTime);
            }
        }
    }

    IEnumerator GroupSpawnOnPreset(int maxMonsterCount, string _monsterName, int stageMaxMonster, StageType stageType)
    {
        if (stageType == StageType.Challenge)
        {
            int monsterCount = 0;
            while (!isGameOver)
            {
                if (monsterCount <= maxMonsterCount)
                {
                    monsterCount++;
                }

                if (monsterCount <= maxMonsterCount && spawnMonsterCount < stageMaxMonster)
                {
                    int groupSize = Random.Range(groupSpawnMinRange, groupSpawnMaxRange);
                    int spawnPlaneCount = Random.Range(3, 6);

                    for (int j = 0; j < spawnPlaneCount; j++)
                    {
                        int spawnPlaneNum = Random.Range(1, 7);
                        for (int i = 0; i < groupSize; i++)
                        {
                            Vector3 spawnPosition = SpawnMonsterOnPreset(spawnPlaneNum);
                            spawnPosition.y = 0;
                            SpawnMonster(_monsterName, spawnPosition, stageMaxMonster);
                        }
                    }

                    yield return new WaitForSeconds(groupSpawnTime);
                }
                else
                {
                    yield return null;
                    continue;
                }
            }
        }
        else if (stageType == StageType.Idle)
        {
            while (!isGameOver)
            {
                int groupSize = Random.Range(groupSpawnMinRange, groupSpawnMaxRange);
                int spawnPlaneCount = Random.Range(3, 6);

                for (int j = 0; j < spawnPlaneCount; j++)
                {
                    int spawnPlaneNum = Random.Range(1, 7);
                    for (int i = 0; i < groupSize; i++)
                    {
                        Vector3 spawnPosition = SpawnMonsterOnPreset(spawnPlaneNum);
                        spawnPosition.y = 0;
                        SpawnMonster(_monsterName, spawnPosition, stageMaxMonster);
                    }
                }

                yield return new WaitForSeconds(groupSpawnTime);
            }
        }
    }
    IEnumerator BossSpawn(int maxMonsterCount, string _monsterName, int stageMaxMonster)
    {
        int monsterCount = 0;
        while (!isGameOver)
        {
            if (monsterCount <= maxMonsterCount)
            {
                monsterCount++;
            }

            if (monsterCount <= maxMonsterCount && spawnMonsterCount < stageMaxMonster)
            {
                int a = Random.Range(0, 2);
                Vector3 spawnPosition = SpawnMonsterAroundPlayer(a);
                
                spawnPosition.y += 10;
                SpawnMonster(_monsterName, spawnPosition, stageMaxMonster);
                yield return new WaitForSeconds(bossSpawnTime);
            }
            else
            {
                yield return null;
            }
        }
    }

    void SpawnMonster(string _monsterName, Vector3 spawnPos, int stageMaxMonsterCount)
    {
        if (isSpawn && Managers.Pool.GetPool("Slider").objectsList.Count != 0 && IsInsideMapBounds(spawnPos))
        {
            spawnMonsterCount++;
            currentMonsterCount++;
            var Monster = Managers.Pool.GetPool(_monsterName).GetGameObject("Monsters/" + _monsterName, spawnPos);
            if(Monster == null)
            {
                Debug.Log("몬스터 오류");
            }
            Monster.GetComponent<MonsterScript>().Init();
            monsterSpawnPos.Add(spawnPos);

            if (spawnMonsterCount == stageMaxMonsterCount)
            {
                isSpawn = false;
                Debug.Log("소환끝");
                StopSpawning(stageMaxMonsterCount);
            }
        }
    }

    void SpawnMonster(string monsterName, Vector3 spawnPos)
    {
        if(isSpawn && IsInsideMapBounds(spawnPos))
        {
            var Monster = Managers.Pool.GetPool(monsterName).GetGameObject("Monsters/" + monsterName, spawnPos);
            var test = Monster.GetComponent<MonsterScript>();
            test.Init();

            monsterSpawnPos.Add(spawnPos);
        }
    }

    Vector3 CalculateFormationOffset(int index, int groupSize)
    {
        float angle = (float)index / groupSize * Mathf.PI * 2f;
        float radius = 3f;
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        return new Vector3(x, 0f, z);
    }

    Vector3 GetRandomPositionAroundPlayer()
    {
        float spawnRadius = Random.Range(7f, 15f);
        Vector2 randomDirection2D = Random.insideUnitCircle.normalized * spawnRadius;

        Vector3 randomDirection3D = new Vector3(randomDirection2D.x, 0f, randomDirection2D.y);

        Vector3 randomPosition = target.position + randomDirection3D;

        return randomPosition;
    }

    Vector3 GetRandomPositionAroundPlayerForBossMonster()
    {
        float spawnRadius = Random.Range(3f, 5f);
        Vector2 randomDirection2D = Random.insideUnitCircle.normalized * spawnRadius;

        Vector3 randomDirection3D = new Vector3(randomDirection2D.x, 0f, randomDirection2D.y);

        Vector3 randomPosition = target.position + randomDirection3D;

        return randomPosition;
    }

    public Vector3 SpawnMonsterAroundPlayer(int a)
    {
        if(isSpawn)
        {
            switch(a)
            {
                case 0:
                    return GetRandomPosition(FollowingSpawnPreset1);
                case 1:
                    return GetRandomPosition(FollowingSpawnPreset2);
                case 2:
                    return GetRandomPosition(FollowingSpawnPreset3);
                case 3:
                    return GetRandomPosition(FollowingSpawnPreset4);
            }
        }
            return new Vector3(0,0,0);
    }

    public Vector3 SpawnMonsterOnPreset(int a)
    {
        if (isSpawn)
        {
            switch (a)
            {
                case 1:
                    return GetRandomPosition(spawnPreset1);
                case 2:
                    return GetRandomPosition(spawnPreset2);
                case 3:
                    return GetRandomPosition(spawnPreset3);
                case 4:
                    return GetRandomPosition(spawnPreset4);
                case 5:
                    return GetRandomPosition(spawnPreset5);
                case 6:
                    return GetRandomPosition(spawnPreset6);
            }
        }
        return new Vector3(0, 0, 0);
    }

    public Vector3 GetRandomPosition(GameObject plane)
    {
        Vector3 originPos = plane.transform.position;

        BoxCollider collider = plane.GetComponent<BoxCollider>();

        float range_X = collider.bounds.size.x;
        float range_Z = collider.bounds.size.y;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);

        Vector3 randomPos = new Vector3(range_X, 0f, range_Z);

        Vector3 respawnPos = originPos + randomPos;
        return respawnPos;
    }

}
