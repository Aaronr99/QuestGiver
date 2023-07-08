using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestGManager : MonoBehaviour
{
    public TMP_Text charLevelText;
    public TMP_Text charHpText;
    public TMP_Text charNameText;
    public TMP_Text weeksText;

    public Transform missionBoard;

    public List<GameObject> adventurersList;
    public Queue<GameObject> pendingAdventurers;

    private void Start()
    {
        adventurersList = new List<GameObject>();
        weeksText.text = GameData.Instance.weekCount.ToString();
        GenerateMissions();
        GenerateAdventurers();
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
            adventurerGO.transform.position = new Vector3(-4f * cont, 0f, -1);
            cont++;
            adventurerGO.GetComponent<AdventurerVisuals>().relationedAdventurer = adventurer;
            GameObject weaponGO = Instantiate(GameData.Instance.ObtainWeaponVisuals(adventurer.weaponType), adventurerGO.GetComponent<AdventurerVisuals>().handGO.transform);
            adventurersList.Add(adventurerGO);
        }
        StartCoroutine(MoveCharacter());
    }

    private void GenerateMissions()
    {
        Dificulty targetDificulty = GameData.Instance.GetDificulty();
        for (int i = 0; i < 5; i++)
        {
            GameObject cardGO = Instantiate(GameData.Instance.missionPrefab, missionBoard);
            MissionCard card = cardGO.GetComponent<MissionCard>();
            MissionInfo missionInfo = Instantiate(GameData.Instance.GetRandomMission(targetDificulty.cardLevels[i]));
            card.SetCardInfo(missionInfo);
        }
    }

    private IEnumerator MoveCharacter()
    {
        float speedMod;
        foreach (GameObject adventurerGO in adventurersList)
        {
            speedMod = Mathf.Abs(adventurerGO.transform.position.x) / 2.5f;
            if (speedMod < 1f)
            {
                speedMod = 1f;
            }
            Vector3 targetPosition = adventurerGO.transform.position + Vector3.right * 4f;
            adventurerGO.GetComponent<AdventurerVisuals>().animator.CrossFade("Walk", 0.1f, 0);
            Quaternion originalRotation = adventurerGO.transform.rotation;
            Quaternion targetRotation = Quaternion.Euler(0f, 90f, 0f);
            while (Quaternion.Angle(adventurerGO.transform.rotation, targetRotation) > 0.5f)
            {
                adventurerGO.transform.rotation = Quaternion.RotateTowards(adventurerGO.transform.rotation, targetRotation, 150f * Time.deltaTime * speedMod);
                yield return new WaitForEndOfFrame();
            }
            while (Vector3.Distance(adventurerGO.transform.position, targetPosition) > 0.05f)
            {
                adventurerGO.transform.position = Vector3.MoveTowards(adventurerGO.transform.position, targetPosition, Time.deltaTime * 2f * speedMod);
                yield return new WaitForEndOfFrame();
            }
            while (Quaternion.Angle(adventurerGO.transform.rotation, originalRotation) > 0.5f)
            {
                adventurerGO.transform.rotation = Quaternion.RotateTowards(adventurerGO.transform.rotation, originalRotation, 150f * Time.deltaTime * speedMod);
                yield return new WaitForEndOfFrame();
            }
            adventurerGO.GetComponent<AdventurerVisuals>().animator.CrossFade("Iddle", 0.1f, 0);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Finished");
        yield return null;
    }
}
