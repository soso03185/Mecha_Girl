using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// CSV 파일을 퀘스트 데이터로 변경하는 스크립트
/// </summary>
public class QuestData : Singleton<QuestData>
{
    public List<Quest> m_quests = new List<Quest>(); // 전체 퀘스트 리스트
    public int m_questIndex = 0;
    public int m_monsterKillCount = 0;
    public int m_getManaCount = 0;

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        LoadQuestCSVData("Data/Quest_List");
    }

    void LoadQuestCSVData(string filePath)
    {
        // CSV 파일을 TextAsset으로 로드
        TextAsset csvFile = Resources.Load<TextAsset>(filePath);

        if (csvFile != null)
        {
            // 각 줄을 분리
            StringReader reader = new StringReader(csvFile.text);
            bool isFirstRow = true;

            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();

                if (isFirstRow)
                {
                    // 첫 번째 줄은 헤더이므로 스킵
                    isFirstRow = false;
                    continue;
                }

                // CSV 파일의 각 줄을 쉼표로 분리
                string[] values = line.Split(',');

                // Quest 데이터 생성
                Quest newQuest = new Quest
                {
                    questName = values[1],  // 퀘스트 이름
                    description = values[2],  // 퀘스트 설명
                    isCompleted = bool.Parse(values[3])  // 완료 여부
                };

                // QuestObjective 생성 및 할당
                QuestObjective objective = new QuestObjective
                {
                    questType = values[4],  // 퀘스트 타입
                    questItemName = values[5],  // 퀘스트 아이템 이름
                    maxCount = int.Parse(values[6]),  // 조건 카운트 맥스
                    count = int.Parse(values[7]),  // 조건 카운트
                    isCompleted = bool.Parse(values[8])  // 클리어 여부
                };
                newQuest.objectives = objective;

                // Reward 생성 및 할당
                Reward reward = new Reward
                {
                    rewardName = values[9],  // 보상 항목
                    amount = int.Parse(values[10])  // 보상 개수
                };    

                Reward reward2 = new Reward
                {
                    rewardName = values[11],  // 보상 항목
                    amount = int.Parse(values[12])  // 보상 개수
                };
                newQuest.rewards = new List<Reward> { reward, reward2 };

                // Quest 리스트에 추가
                m_quests.Add(newQuest);
            }
        }
        else
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다.");
            return;
        } 
    }
}
