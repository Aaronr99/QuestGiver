using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/MissionInfo")]
public class MissionInfo : ScriptableObject
{
    [TextArea]
    public string title;
    public int level;
    public float moralLoss;
    public int hpLoss;
}
