using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : ScriptableObject
{
    public string characterName;
    public int characterLevel;
    public int hpTotal;
    public float totalMoral;
    public AdventurerType adventurerType;
    public WeaponType weaponType;
    public int GetMaxHP()
    {
        int baseHP = 5;
        switch (adventurerType)
        {
            case AdventurerType.Goblin:
                baseHP = 4;
                break;
            case AdventurerType.Kimono:
                baseHP = 5;
                break;
            case AdventurerType.Knight:
                baseHP = 6;
                break;
            case AdventurerType.Ninja:
                baseHP = 4;
                break;
            case AdventurerType.Pirate:
                baseHP = 5;
                break;
            case AdventurerType.Viking:
                baseHP = 6;
                break;
            case AdventurerType.Wizard:
                baseHP = 4;
                break;
            default:
                break;
        }
        return baseHP * characterLevel;
    }
}
