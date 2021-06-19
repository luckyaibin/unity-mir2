using System;
using System.Collections.Generic;
using UnityEngine;

public class MirSpell
{

    //Warrior

    //Fencing = 1,
    //Slaying = 2,
    //Thrusting = 3,
    //HalfMoon = 4,
    //ShoulderDash = 5,
    //TwinDrakeBlade = 6,
    //Entrapment = 7,
    //FlamingSword = 8,
    //LionRoar = 9,
    //CrossHalfMoon = 10,
    //BladeAvalanche = 11,
    //ProtectionField = 12,
    //Rage = 13,
    //CounterAttack = 14,
    //SlashingBurst = 15,
    //Fury = 16,
    //ImmortalSkin = 17,

    //Wizard

    //FireBall = 31,

    public static void fireBall()
    {
        new FireBallBuilder().build();
    }




    //Repulsion = 32,
    public static void repulsion()
    {
        new RepulsionBuilder().build();
    }

    //ElectricShock = 33,
    public static void electricShock()
    {
        new ElectricShockBuilder().build();
    }

    //GreatFireBall = 34,
    public static void greatFireBall()
    {
        new GreatFireBallBuilder().build();
    }
    //HellFire = 35,
    //ThunderBolt = 36,
    public static void ThunderBolt()
    {
        new ThunderBoltBuilder().build();
    }

    //Teleport = 37,
    //FireBang = 38,
    //FireWall = 39,
    //Lightning = 40,
    //FrostCrunch = 41,
    public static void frostCrunch()
    {
        new FrostCrunchBuilder().build();
    }
    //ThunderStorm = 42,
    //MagicShield = 43,
    //TurnUndead = 44,
    //Vampirism = 45,
    //IceStorm = 46,
    //FlameDisruptor = 47,
    //Mirroring = 48,
    //FlameField = 49,
    //Blizzard = 50,
    //MagicBooster = 51,
    //MeteorStrike = 52,
    //IceThrust = 53,
    //FastMove = 54,
    //StormEscape = 55,

    //Taoist

    //Healing = 61,
    //SpiritSword = 62,
    //Poisoning = 63,
    //SoulFireBall = 64,
    //SummonSkeleton = 65,
    //Hiding = 67,
    //MassHiding = 68,
    //SoulShield = 69,
    //Revelation = 70,
    //BlessedArmour = 71,
    //EnergyRepulsor = 72,
    //TrapHexagon = 73,
    //Purification = 74,
    //MassHealing = 75,
    //Hallucination = 76,
    //UltimateEnhancer = 77,
    //SummonShinsu = 78,
    //Reincarnation = 79,
    //SummonHolyDeva = 80,
    //Curse = 81,
    //Plague = 82,
    //PoisonCloud = 83,
    //EnergyShield = 84,
    //PetEnhancer = 85,
    //HealingCircle = 86,

    //Assassin

    //FatalSword = 91,
    //DoubleSlash = 92,
    //Haste = 93,
    //FlashDash = 94,
    //LightBody = 95,
    //HeavenlySword = 96,
    //FireBurst = 97,
    //Trap = 98,
    //PoisonSword = 99,
    //MoonLight = 100,
    //MPEater = 101,
    //SwiftFeet = 102,
    //DarkBody = 103,
    //Hemorrhage = 104,
    //CrescentSlash = 105,
    //MoonMist = 106,

    //Archer

    //Focus = 121,
    //StraightShot = 122,
    //DoubleShot = 123,
    //ExplosiveTrap = 124,
    //DelayedExplosion = 125,
    //Meditation = 126,
    //BackStep = 127,
    //ElementalShot = 128,
    //Concentration = 129,
    //Stonetrap = 130,
    //ElementalBarrier = 131,
    //SummonVampire = 132,
    //VampireShot = 133,
    //SummonToad = 134,
    //PoisonShot = 135,
    //CrippleShot = 136,
    //SummonSnakes = 137,
    //NapalmShot = 138,
    //OneWithNature = 139,
    //BindingShot = 140,
    //MentalState = 141,

    //Custom

    //Blink = 151,
    //Portal = 152,
    //BattleCry = 153,

    //Map Events

    //DigOutZombie = 200,
    //Rubble = 201,
    //MapLightning = 202,
    //MapLava = 203,
    //MapQuake1 = 204,
    //MapQuake2 = 205
}


