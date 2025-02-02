using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerable : ICreature
{
    public double DamageAmplification { get; set; }
    public double MaxMana { get; set; }
    public float ManaPerSecond { get; set; }
    public double ManaAmplification { get; set; }
    public double Mana {  get; set; }
    public float ElementAmp {  get; set; }
}
