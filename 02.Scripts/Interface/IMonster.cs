using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonster : ICreature
{    
    public float AdditionalDamage { get; set; }
    public float ContinuousAdditionalDamage { get; set; }
    public double IsDamaged(Skill skill, double skillCoefficient, float additionalDamage, float additionalCriticalChance = 0f, double additionalCiriticalMultiplier = 0f, float durabilityNegation = 0f);

    public double IsContinuosDamaged(Skill skill, double skillCoefficient);
    public Attribute Attribute { get; set; }

}
