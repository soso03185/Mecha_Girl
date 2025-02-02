using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum MonsterState
    {
        spawn,
        move,
        attack,
        hit,
        dead,
        stun
    };

    public enum MonsterType
    {
        Ranged,
        Melee
    }

    public enum MonsterName
    {
        Brute_Robot_01,
        Worker_Robot_2,
        Boxy_Robot,
        Bot_Robot,
        Brute_Robot_Black,
        Brute_Robot_Brown,
        Brute_Robot_Silver,
        Capsule_Robot_Black,
        Capsule_Robot_Blue,
        Cute_Robot,
        Gripper_Robot,
        Metal_Robot,
        Nexus_Robot,
        Tanker_Robot,
        SM_Veh_Mech_06,
        Bot_Robot2,
        Boxy_Robot2,
        Capsule_Robot_Black2,
        Capsule_Robot_Blue2,
        Cute_Robot2
    }

    public enum SpawnType
    {
        NormalAroundPlayer,
        NormalOnPreset,
        DelayAroundPlayer,
        DelayOnPreset,
        GroupAroundPlayer,
        GroupOnPreset,
        WaveSpawnAroundPlayer,
        WaveSpawnOnPreset,
        Boss
    }

    public enum StageType
    {
        Idle,
        Challenge
    }

    public enum Rarity
    {
        C,
        B,
        A,
        S,
    }

    public enum EffectType
    {
        VE,
        PS,
        Script
    }

}

