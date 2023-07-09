using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MissionInfo")]
public class MissionInfo : ScriptableObject
{
    public Adventurer adventurerOnTest;
    [TextArea]
    public string title;
    public int level;

    public AdventurerType favouredAdventurer;
    public AdventurerType disfavoredAdventurer;

    public WeaponType favoredWeapon;
    public WeaponType disfavoredWeapon;

    public DangerLevel dangerLevel;

    [TextArea]
    public string winText;
    [TextArea]
    public string successText;
    [TextArea]
    public string loseText;

    public int GetHpOnWin()
    {
        return 0;
    }

    public int GetHpOnSucess()
    {
        return (Convert.ToInt32(dangerLevel) + 1) * 5;
    }

    public int GetHpOnLose()
    {
        return (Convert.ToInt32(dangerLevel) + 1) * 15;
    }

    public int GetXpOnWin()
    {
        return level * (Convert.ToInt32(dangerLevel) + 1) * 70;
    }

    public int GetXpOnSucess()
    {
        return level * (Convert.ToInt32(dangerLevel) + 1) * 40;
    }

    public int GetXpOnLose()
    {
        return level * (Convert.ToInt32(dangerLevel) + 1) * 10;
    }
}
