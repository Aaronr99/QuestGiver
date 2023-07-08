using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestGManager : MonoBehaviour
{
    public TMP_Text charLevelText;
    public TMP_Text charHpText;
    public TMP_Text charMoralText;
    public TMP_Text charNameText;
    public TMP_Text weeksText;

    private void Start()
    {
        GenerateAdventurers();
        GenerateMissions();
    }

    private void GenerateAdventurers()
    {
        while (GameData.Instance.activeAdventurers.Count < 5)
        {
            Adventurer newAdventurer = ScriptableObject.CreateInstance<Adventurer>();
            int randomType = UnityEngine.Random.Range(0, 6);
            newAdventurer.adventurerType = (AdventurerType)randomType;
            newAdventurer.hpTotal = newAdventurer.GetMaxHP();
            newAdventurer.totalMoral = 1f;
            newAdventurer.characterLevel = 1;
            newAdventurer.characterName = GameData.Instance.GetRandomName() + " The " + GameData.Instance.GetRandomPrefix(newAdventurer.adventurerType);
            randomType = UnityEngine.Random.Range(0, 5);
            newAdventurer.weaponType = (WeaponType)randomType;
            GameData.Instance.activeAdventurers.Add(newAdventurer);
        }
        int cont = 1;
        foreach (Adventurer adventurer in GameData.Instance.activeAdventurers)
        {
            GameObject adventurerGO = Instantiate(GameData.Instance.ObtainAdventurerVisuals(adventurer.adventurerType));
            adventurerGO.transform.position = new Vector3(-5f, 0f, 0.4f) * cont;
            cont++;
            adventurerGO.GetComponent<AdventurerVisuals>().relationedAdventurer = adventurer;
            GameObject weaponGO = Instantiate(GameData.Instance.ObtainWeaponVisuals(adventurer.weaponType), adventurerGO.GetComponent<AdventurerVisuals>().handGO.transform);

        }
    }

    private void GenerateMissions()
    {
        //throw new NotImplementedException();
    }

}
