using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Define;
using UnityEngine.SceneManagement;

public class StageManager
{
    // 죽은 몬스터 수
    public int deadMonsterCount;

    // 보스 스테이지 한정 제한 시간
    public float timeLimit;

    // 제한 시간 UI

    // 배경 리소스 프리팹

    public DataManager.StageInfo stageInfo;

    public List<SpawnOptions> spawnList = new List<SpawnOptions>();

    public static event Action OnStageFailed;

    public static event Action OnStageCleared;

    string m_ReturnSceneName = "03_JeongHanScene";
    [System.Serializable]
    public struct SpawnOptions
    {
        public SpawnType spawnType;
        public MonsterName monsterName;
        public int monsterCount;
    }

    SpawnOptions monster1Spawn;
    SpawnOptions monster2Spawn;

    public int tempStageLevel;

    public bool isStageChanged;

    public bool SpawnNextWave = false;
    public bool SpawnBoss = false;

    public void BeginStage(int stageLevel, StageType stageType)
    {
        // 다음 진행될 스테이지 정보 불러오기
        //GetStageData(stageLevel);

        // 정보 바탕으로 몬스터 스폰
        BeginSpawn(stageType);
    }

    public void ResetStageInfo(GameObject player)
    {
        SceneManager.LoadScene(2);

        Managers.Monsters.ResetMonsterDist();

        SpawnManager.spawnInstance.StopSpawning();
       
        if(spawnList.Count == 1 && stageInfo.isBossStage)
        {
            Managers.Pool.ResetPool(spawnList[0].monsterName.ToString());
            Managers.Pool.ResetPool(stageInfo.bossMonsterName);
        }
        else if(spawnList.Count == 1 && !stageInfo.isBossStage)
        {
            Managers.Pool.ResetPool(spawnList[0].monsterName.ToString());
        }
        else if(spawnList.Count == 2 && !stageInfo.isBossStage)
        {
            for (int i = 0; i < spawnList.Count; i++)
            {
                Managers.Pool.ResetPool(spawnList[i].monsterName.ToString());
            }
        }
        else if(spawnList.Count == 2 && stageInfo.isBossStage)
        {
            for (int i = 0; i < spawnList.Count; i++)
            {
                Managers.Pool.ResetPool(spawnList[i].monsterName.ToString());
            }

            if (stageInfo.isBossStage)
            {
                Managers.Pool.ResetPool(stageInfo.bossMonsterName);
            }
        }

       

        Managers.Pool.ResetPool("MedKit");
        Managers.Pool.ResetPool("Slider");

        SpawnManager.spawnInstance.spawnMonsterCount = 0;
        SpawnManager.spawnInstance.currentMonsterCount = 0;
        SpawnManager.spawnInstance.isAllSpawned = false;

        deadMonsterCount = 0;
        spawnList.Clear();
        // 플레이어 정보들 초기화
        player.GetComponent<Player>().m_logicManager.Reset();
        player.GetComponent<Player>().Health = 100;
        player.GetComponent<Player>().m_IsPlayerDead = false;

        isStageChanged = true;

        if(GameManager.Instance.StageInfoCanvas != null)
        {
            if(GameManager.Instance.StageInfoCanvas.gameObject.activeSelf)
            {
                GameManager.Instance.StageInfoCanvas.gameObject.SetActive(false);
            }
        }
    }

    public void GetStageData(int stageLevel)
    {
        stageInfo = Managers.Data.GetStageInfo(stageLevel);

        // 엑셀에서 구조체 불러오는게 안되서 하드코딩 해놓음
        // 나중에 DB 붙이면 수정할 예정
        monster1Spawn.spawnType = stageInfo.spawnType1;
        monster1Spawn.monsterName = stageInfo.monster1Name;
        monster1Spawn.monsterCount = stageInfo.monster1Count;
        spawnList.Add(monster1Spawn);

        if(stageInfo.monster1Name != stageInfo.monster2Name)
        {
            monster2Spawn.spawnType = stageInfo.spawnType2;
            monster2Spawn.monsterName = stageInfo.monster2Name;
            monster2Spawn.monsterCount = stageInfo.monster2Count;
            spawnList.Add(monster2Spawn);
        }
    }

    public void BeginSpawn(StageType stageType)
    {
        SpawnManager.spawnInstance.isSpawn = true;

        if (stageInfo.isStageWave)
        {
            SpawnManager.spawnInstance.StartSpawning(stageInfo.maxMonsterCount, spawnList[0].monsterCount, spawnList[0].monsterName.ToString(), spawnList[0].spawnType, stageType);
        }
        else
        {
            if (stageInfo.isBossStage)
            {
                SpawnManager.spawnInstance.StartSpawning(stageInfo.maxMonsterCount, stageInfo.bossMonsterCount, stageInfo.bossMonsterName, SpawnType.Boss, stageType);
            }
            foreach (var temp in spawnList)
            {
                SpawnManager.spawnInstance.StartSpawning(stageInfo.maxMonsterCount, temp.monsterCount, temp.monsterName.ToString(), temp.spawnType, stageType);
            }
        }
    }

    public static void CheckStageCleared()
    {
        OnStageCleared?.Invoke();
    }

    public static void CheckStageFailed()
    {
        OnStageFailed?.Invoke();
    }

    public void CheckStageLevel(int stageLevel)
    {
        if(tempStageLevel != stageLevel)
        {
            tempStageLevel = stageLevel;
        }
    }
}
