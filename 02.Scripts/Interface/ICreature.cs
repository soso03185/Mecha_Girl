using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICreature 
{
    public double MaxHealth { get; set; }
    public double HealthPerSecond { get; set; }
    public double Defense { get; set; }
    public float DefensePercentage { get; set; }
    public double Damage { get; set; }
    public float DamagePercentage { get; set; }
    public float CriticalChance { get; set; }
    public double CriticalMultiplier { get; set; }
    public float MoveSpeed { get; set; }
    public float MoveSpeedPercentage { get; set; }
    public float AttackSpeed { get; set; }
    public float AttackSpeedPercentage { get; set; }

    public Dictionary<KeyValuePair<Skill, string>, BuffDebuff> BuffDebuff { get; set; }
    public Dictionary<string, ContinuousDamage> ContinuousDamage { get; set; }
    public Dictionary<string, AbnormalStatus> AbnormalStatus { get; set; }
    public List<Barrier> Barrier { get; set; }
    List<KeyValuePair<Skill, string>> BuffDebuffStrings { get; set; }
    List<string> ContinuousDamageStrings { get; set; }
    List<Barrier> BarrierOnDestroy { get; set; }
    List<string> AbnormalStatusStrings { get; set; }


}
