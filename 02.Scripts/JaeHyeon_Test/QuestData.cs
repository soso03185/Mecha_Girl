using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// CSV ������ ����Ʈ �����ͷ� �����ϴ� ��ũ��Ʈ
/// </summary>
public class QuestData : Singleton<QuestData>
{
    public List<Quest> m_quests = new List<Quest>(); // ��ü ����Ʈ ����Ʈ
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
        // CSV ������ TextAsset���� �ε�
        TextAsset csvFile = Resources.Load<TextAsset>(filePath);

        if (csvFile != null)
        {
            // �� ���� �и�
            StringReader reader = new StringReader(csvFile.text);
            bool isFirstRow = true;

            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();

                if (isFirstRow)
                {
                    // ù ��° ���� ����̹Ƿ� ��ŵ
                    isFirstRow = false;
                    continue;
                }

                // CSV ������ �� ���� ��ǥ�� �и�
                string[] values = line.Split(',');

                // Quest ������ ����
                Quest newQuest = new Quest
                {
                    questName = values[1],  // ����Ʈ �̸�
                    description = values[2],  // ����Ʈ ����
                    isCompleted = bool.Parse(values[3])  // �Ϸ� ����
                };

                // QuestObjective ���� �� �Ҵ�
                QuestObjective objective = new QuestObjective
                {
                    questType = values[4],  // ����Ʈ Ÿ��
                    questItemName = values[5],  // ����Ʈ ������ �̸�
                    maxCount = int.Parse(values[6]),  // ���� ī��Ʈ �ƽ�
                    count = int.Parse(values[7]),  // ���� ī��Ʈ
                    isCompleted = bool.Parse(values[8])  // Ŭ���� ����
                };
                newQuest.objectives = objective;

                // Reward ���� �� �Ҵ�
                Reward reward = new Reward
                {
                    rewardName = values[9],  // ���� �׸�
                    amount = int.Parse(values[10])  // ���� ����
                };    

                Reward reward2 = new Reward
                {
                    rewardName = values[11],  // ���� �׸�
                    amount = int.Parse(values[12])  // ���� ����
                };
                newQuest.rewards = new List<Reward> { reward, reward2 };

                // Quest ����Ʈ�� �߰�
                m_quests.Add(newQuest);
            }
        }
        else
        {
            Debug.LogError("CSV ������ ã�� �� �����ϴ�.");
            return;
        } 
    }
}
