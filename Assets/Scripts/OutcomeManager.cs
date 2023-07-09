using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OutcomeManager : MonoBehaviour
{
    public Queue<MissionInfo> pendingMissions;
    public MissionInfo actualMission;
    GameObject actualAdventurer;

    public TMP_Text missionName;
    public TMP_Text stateText;
    public TMP_Text descriptionText;

    public GameObject levelUpText;
    public GameObject diedText;

    public Color winColor;
    public Color sucessColor;
    public Color loseColor;

    public bool allowInput;

    private void Start()
    {
        allowInput = false;
        pendingMissions = new Queue<MissionInfo>();
        foreach (MissionInfo mission in GameData.Instance.activeMissions)
        {
            pendingMissions.Enqueue(mission);
        }
        ShowCharacter();
    }

    private void ShowCharacter()
    {
        if (actualAdventurer != null)
        {
            Destroy(actualAdventurer);
            actualAdventurer = null;
        }

        diedText.SetActive(false);
        levelUpText.SetActive(false);

        actualMission = pendingMissions.Dequeue();
        Adventurer adventurer = actualMission.adventurerOnTest;

        actualAdventurer = Instantiate(GameData.Instance.ObtainAdventurerVisuals(adventurer.adventurerType));
        actualAdventurer.transform.position = new Vector3(0f, 0.15f, -1f);
        //Instantiate(GameData.Instance.ObtainWeaponVisuals(adventurer.weaponType), actualAdventurer.GetComponent<AdventurerVisuals>().handGO.transform);

        float randomOutcome = Random.Range(0f, 1f);
        if (adventurer.weaponType == actualMission.favoredWeapon)
        {
            randomOutcome += 0.15f;
        }
        else if (adventurer.weaponType == actualMission.disfavoredWeapon)
        {
            randomOutcome -= 0.15f;
        }

        if (adventurer.adventurerType == actualMission.favouredAdventurer)
        {
            randomOutcome += 0.15f;
        }
        else if (adventurer.adventurerType == actualMission.disfavoredAdventurer)
        {
            randomOutcome -= 0.15f;
        }
        float levelDiference = adventurer.characterLevel - actualMission.level;
        float diferenceMod = 1f;
        if (levelDiference >= 2)
        {
            diferenceMod = 2f;
        }
        randomOutcome += levelDiference * diferenceMod;
        OutcomeType outcomeType;
        if (randomOutcome >= 0.8f)
        {
            outcomeType = OutcomeType.Win;
        }
        else if (randomOutcome >= 0.45f)
        {
            outcomeType = OutcomeType.Sucess;
        }
        else
        {
            outcomeType = OutcomeType.Lose;
        }
        missionName.text = actualMission.title;
        ShowOutcome(outcomeType);
    }

    private void ShowOutcome(OutcomeType outcomeType)
    {
        switch (outcomeType)
        {
            case OutcomeType.Win:
                OutcomeWin();
                break;
            case OutcomeType.Sucess:
                OutcomeSucess();
                break;
            case OutcomeType.Lose:
                OutcomeLose();
                break;
            default:
                break;
        }
        StartCoroutine(WaitForInput());
    }

    private void OutcomeWin()
    {
        actualAdventurer.GetComponent<AdventurerVisuals>().animator.CrossFade("Win", 0f, 0);
        stateText.text = "WIN";
        stateText.color = winColor;
        Adventurer adventurer = actualMission.adventurerOnTest;
        descriptionText.text = adventurer.characterName + " " + actualMission.winText;
        bool hasDied = GameData.Instance.ActualizeAdventurerHP(actualMission.GetHpOnWin(), adventurer);
        if (hasDied)
        {
            diedText.SetActive(true);
            GameData.Instance.activeAdventurers.Remove(adventurer);
            actualAdventurer.GetComponent<AdventurerVisuals>().animator.CrossFade("Death", 0f, 0);
        }
        else
        {
            bool hasLevelUp = GameData.Instance.ActualizeAdventurerXP(actualMission.GetXpOnWin(), adventurer);
            if (hasLevelUp)
            {
                levelUpText.SetActive(true);
            }
        }
    }

    private void OutcomeSucess()
    {
        actualAdventurer.GetComponent<AdventurerVisuals>().animator.CrossFade("Walk", 0f, 0);
        stateText.text = "SUCESS";
        stateText.color = sucessColor;
        Adventurer adventurer = actualMission.adventurerOnTest;
        descriptionText.text = adventurer.characterName + " " + actualMission.successText;
        bool hasDied = GameData.Instance.ActualizeAdventurerHP(actualMission.GetHpOnSucess(), adventurer);
        if (hasDied)
        {
            diedText.SetActive(true);
            GameData.Instance.activeAdventurers.Remove(adventurer);
            actualAdventurer.GetComponent<AdventurerVisuals>().animator.CrossFade("Death", 0f, 0);
        }
        else
        {

            bool hasLevelUp = GameData.Instance.ActualizeAdventurerXP(actualMission.GetXpOnSucess(), adventurer);
            if (hasLevelUp)
            {
                levelUpText.SetActive(true);
            }
        }
    }

    private void OutcomeLose()
    {
        actualAdventurer.GetComponent<AdventurerVisuals>().animator.CrossFade("Lose", 0f, 0);
        stateText.text = "LOSE";
        stateText.color = loseColor;
        Adventurer adventurer = actualMission.adventurerOnTest;
        descriptionText.text = adventurer.characterName + " " + actualMission.loseText;
        bool hasDied = GameData.Instance.ActualizeAdventurerHP(actualMission.GetHpOnLose(), adventurer);
        if (hasDied)
        {
            diedText.SetActive(true);
            GameData.Instance.activeAdventurers.Remove(adventurer);
            actualAdventurer.GetComponent<AdventurerVisuals>().animator.CrossFade("Death", 0f, 0);
        }
        else
        {

            bool hasLevelUp = GameData.Instance.ActualizeAdventurerXP(actualMission.GetXpOnLose(), adventurer);
            if (hasLevelUp)
            {
                levelUpText.SetActive(true);
            }
        }
    }

    private IEnumerator WaitForInput()
    {
        yield return new WaitForSeconds(1.5f);
        allowInput = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && allowInput)
        {
            allowInput = false;
            if (pendingMissions.Count > 0)
            {
                ShowCharacter();
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }
    }
}
