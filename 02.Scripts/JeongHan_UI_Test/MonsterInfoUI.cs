using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterInfoUI : MonoBehaviour
{
    public TextMeshProUGUI stageLevel;
    public TextMeshProUGUI stageCharacteristic;

    public GameObject monsterInfoSlotPrefab;

    public List<MonsterInfoSlot> monsterInfoSlots;

    public List<DataManager.MonsterInfo> monsterInfos;

    // Start is called before the first frame update
    public void Init(DataManager.StageInfo stageInfo, int Level)
    {
        monsterInfos.Add(Managers.Data.GetMonserInfoByName(stageInfo.monster1Name.ToString()));
        if(stageInfo.monster1Name.ToString() != stageInfo.monster2Name.ToString())
        {
            monsterInfos.Add(Managers.Data.GetMonserInfoByName(stageInfo.monster2Name.ToString()));
        }

        if(stageInfo.isBossStage)
        {
            monsterInfos.Add(Managers.Data.GetMonserInfoByName(stageInfo.bossMonsterName.ToString()));
        }

        for(int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(monsterInfoSlotPrefab, this.transform);
            monsterInfoSlots.Add(go.GetComponent<MonsterInfoSlot>());
        }

        if (stageInfo.monsterTypeCount == 3)
        {
            monsterInfoSlots[2].gameObject.SetActive(true);
            monsterInfoSlots[1].gameObject.SetActive(true);
            monsterInfoSlots[0].gameObject.SetActive(true);
        }
        else if(stageInfo.monsterTypeCount == 2)
        {
            monsterInfoSlots[2].gameObject.SetActive(false);
            monsterInfoSlots[1].gameObject.SetActive(true);
            monsterInfoSlots[0].gameObject.SetActive(true);
        }
        else if(stageInfo.monsterTypeCount == 1)
        {
            monsterInfoSlots[2].gameObject.SetActive(false);
            monsterInfoSlots[1].gameObject.SetActive(false);
            monsterInfoSlots[0].gameObject.SetActive(true);
        }

        for (int i = 0; i < stageInfo.monsterTypeCount; i++)
        {
            //몬스터 이미지 준비되면 주석해제

            var image = SpriteManager.instance.GetMonsterSprite(monsterInfos[i].Name);

            if (image != null)
            {
                monsterInfoSlots[i].monsterImage.sprite = image;
            }

            monsterInfoSlots[i].spawnTypeText.text = "스폰 방식 : " + monsterInfos[i].SpawnType;
            monsterInfoSlots[i].characteristicText.text = "특징 : " + monsterInfos[i].Characteristic;

            monsterInfoSlots[i].HealthText.text = "체력 : " + monsterInfos[i].Hp.ToString();
            monsterInfoSlots[i].AttackText.text = "공격력 : " + monsterInfos[i].Atk.ToString();
            monsterInfoSlots[i].DefenseText.text = "방어력 : " + monsterInfos[i].Defense.ToString();
        }

        stageCharacteristic.text = stageInfo.stageCharacteristic;

        stageLevel.text = "스테이지 " + Level.ToString();
    }

    public void ChangeMonsterInfo(DataManager.StageInfo stageInfo, int Level)
    {
        monsterInfos.Clear();
        monsterInfos.Add(Managers.Data.GetMonserInfoByName(stageInfo.monster1Name.ToString()));
        if (stageInfo.monster1Name.ToString() != stageInfo.monster2Name.ToString())
        {
            monsterInfos.Add(Managers.Data.GetMonserInfoByName(stageInfo.monster2Name.ToString()));
        }

        if (stageInfo.isBossStage)
        {
            monsterInfos.Add(Managers.Data.GetMonserInfoByName(stageInfo.bossMonsterName.ToString()));
        }

        if (stageInfo.monsterTypeCount == 3)
        {
            monsterInfoSlots[2].gameObject.SetActive(true);
            monsterInfoSlots[1].gameObject.SetActive(true);
            monsterInfoSlots[0].gameObject.SetActive(true);
        }
        else if (stageInfo.monsterTypeCount == 2)
        {
            monsterInfoSlots[2].gameObject.SetActive(false);
            monsterInfoSlots[1].gameObject.SetActive(true);
            monsterInfoSlots[0].gameObject.SetActive(true);
        }
        else if (stageInfo.monsterTypeCount == 1)
        {
            monsterInfoSlots[2].gameObject.SetActive(false);
            monsterInfoSlots[1].gameObject.SetActive(false);
            monsterInfoSlots[0].gameObject.SetActive(true);
        }

        for (int i = 0; i < stageInfo.monsterTypeCount; i++)
        {
            //몬스터 이미지 준비되면 주석해제

            var image = SpriteManager.instance.GetMonsterSprite(monsterInfos[i].Name);

            if(image != null)
            {
                monsterInfoSlots[i].monsterImage.sprite = image;
            }

            monsterInfoSlots[i].spawnTypeText.text = monsterInfos[i].SpawnType;
            monsterInfoSlots[i].characteristicText.text = monsterInfos[i].Characteristic;

            monsterInfoSlots[i].HealthText.text = "체력 : " + monsterInfos[i].Hp.ToString();
            monsterInfoSlots[i].AttackText.text = "공격력 : " + monsterInfos[i].Atk.ToString();
            monsterInfoSlots[i].DefenseText.text = "방어력 : " + monsterInfos[i].Defense.ToString();
        }

        stageCharacteristic.text = stageInfo.stageCharacteristic;

        stageLevel.text = "스테이지 " + Level.ToString();
    }

}
