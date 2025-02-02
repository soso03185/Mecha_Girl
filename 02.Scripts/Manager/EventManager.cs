using System;

public class EventManager : Singleton<EventManager>
{
    public static event Action OnMonsterKilled;
    public static event Action OnStageClear;
    public static event Action<string> OnOpenTab;
    public static event Action<string> OnSkillEquiped;

    public static void MonsterKilled()
    {
        OnMonsterKilled?.Invoke();
    } 

    public static void StageCleared()
    {
        OnStageClear?.Invoke();
    }  
    
    public static void OpenTab(string tabName)
    {
        OnOpenTab?.Invoke(tabName);
    }  

    public static void SkillEquiped(string itemName)
    {
        OnSkillEquiped?.Invoke(itemName);
    }
}
