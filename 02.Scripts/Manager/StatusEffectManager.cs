using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{
    public Dictionary<string, BuffDebuff> m_buffdebuff = new Dictionary<string, BuffDebuff>();
    public Dictionary<string, ContinuousDamage> m_continuousDamage = new Dictionary<string, ContinuousDamage>();
    public Dictionary<string, AbnormalStatus> m_abnormalStatus = new Dictionary<string, AbnormalStatus>();
    public Dictionary<string, Barrier> m_barrier = new Dictionary<string, Barrier>();
    void Awake()
    {
        m_abnormalStatus.Add("Stun", new Stun(null, null, 0f));
        m_abnormalStatus.Add("Flooding", new Flooding(null, null, 0f));
        m_abnormalStatus.Add("Frenzy", new Frenzy(null, null, 0f));
        m_abnormalStatus.Add("Rage", new Rage(null, null, 0f));
        
        m_continuousDamage.Add("ElectricShock", new ElectricShock(null, null, 0f, 0f));
        m_continuousDamage.Add("Tinnitus", new Tinnitus(null, null, 0f, 0f));
       
        m_buffdebuff.Add("DefenseIncrease", new DefenseIncrease(null, null, 0f, 0f));
        m_buffdebuff.Add("SpeedIncrease", new SpeedIncrease(null, null, 0f, 0f));
        m_buffdebuff.Add("AdditionalDamage", new AdditionalDamage(null, null, 0f, 0f));
        m_buffdebuff.Add("DamageIncrease", new DamageIncrease(null, null, 0f, 0f));
        m_buffdebuff.Add("ManaRecovery", new ManaRecovery(null, null, 0f, 0f));
        m_buffdebuff.Add("CriticalMultiplierIncrease", new CriticalMultiplierIncrease(null, null, 0f, 0f));
        m_buffdebuff.Add("CriticalChanceIncrease", new CriticalChanceIncrease(null, null, 0f, 0f));
        m_buffdebuff.Add("AttackSpeedIncrease", new AttackSpeedIncrease(null, null, 0f, 0f));
        m_buffdebuff.Add("ContinuousAdditionalDamage", new ContinuousAdditionalDamage(null, null, 0f, 0f));

        m_barrier.Add("MechanicBarrier", new MechanicBarrier(null, null, 0f, 0f));
    }
    
}
