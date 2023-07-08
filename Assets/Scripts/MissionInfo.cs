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
    public int hpLoss;

    public AdventurerType favouredAdventurer;
    public AdventurerType disfavoredAdventurer;

    public WeaponType favoredWeapon;
    public WeaponType disfavoredWeapon;

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
        return hpLoss;
    }

    public int GetHpOnLose()
    {
        return hpLoss * 4;
    }

    public int GetXpOnWin()
    {
        return level * 55;
    }

    public int GetXpOnSucess()
    {
        return level * 25;
    }

    public int GetXpOnLose()
    {
        return level * 5;
    }
}
