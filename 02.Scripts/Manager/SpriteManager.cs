using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;

    public Dictionary<string ,Sprite> skillIdSprites = new Dictionary<string, Sprite>();
    public Dictionary<string ,Sprite> monsterSprites = new Dictionary<string, Sprite>();
 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        LoadAllSprites();
    }

    public void LoadAllSprites()
    {
        LoadSprites("Sprites/SkillIcons", skillIdSprites);
        LoadSprites("Sprites/MonsterIcon", monsterSprites);
    }

    // spriteType은 폴더 명
    // 그 안에 있는 Sprite 전부 로드
    public void LoadSprites(string spriteType, Dictionary<string, Sprite> spriteContainer)
    {
        Sprite[] sprites = Managers.Resource.LoadAllSprites(spriteType);
        
        foreach(var child in sprites)
        {
            spriteContainer[child.name] = child; 
        }
    }

    public Sprite GetMonsterSprite(string name)
    {
        if (monsterSprites.ContainsKey(name))
        {
            return monsterSprites[name];
        }
        else
            return null;
    }
}
