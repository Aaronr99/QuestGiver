using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestGManager : MonoBehaviour
{
    #region Singleton
    private static QuestGManager instance;

    public static QuestGManager Instance
    {
        get
        {
            // Verificar si ya existe una instancia y devolverla
            if (instance == null)
            {
                // Buscar la instancia en la escena
                instance = FindObjectOfType<QuestGManager>();

                // Si no se encuentra, crear una nueva instancia
                if (instance == null)
                {
                    Debug.Log("ALGO ESTA MAL CONFIGURADO EN LA ESCENA FALTA EL QuestGManager");
                }
            }

            return instance;
        }
    }
    #endregion
    public GameObject charPanel;
    public TMP_Text charLevelText;
    public TMP_Text charHpText;
    public TMP_Text charNameText;
    public TMP_Text weeksText;

    public GameObject missionBoardGO;
    public Transform missionBoard;

    public List<GameObject> adventurersList;
    public Queue<GameObject> pendingAdventurers;
    public GameObject actualAdventurer;

    public GameObject topCanvas;
    private void Start()
    {
        pendingAdventurers = new Queue<GameObject>();
        adventurersList = new List<GameObject>();

        charPanel.SetActive(false);
        missionBoardGO.SetActive(false);
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
            pendingAdventurers.Enqueue(adventurerGO);
        }
        StartCoroutine(MoveCharacters());
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

    public void EnableCharacterAsignation()
    {
        charPanel.SetActive(true);
        missionBoardGO.SetActive(true);
        actualAdventurer = pendingAdventurers.Dequeue();

        Adventurer adventurer = actualAdventurer.GetComponent<AdventurerVisuals>().relationedAdventurer;
        charNameText.text = adventurer.characterName;
        charHpText.text = "HP: " + adventurer.hpTotal.ToString();
        charLevelText.text = "LEVEL: " + adventurer.characterLevel.ToString();
    }

    public void AssignMissionOnCharacter(MissionInfo missionInfo)
    {
        charPanel.SetActive(false);
        missionBoardGO.SetActive(false);
        missionInfo.adventurerOnTest = actualAdventurer.GetComponent<AdventurerVisuals>().relationedAdventurer;
        GameData.Instance.activeMissions.Add(missionInfo);
        actualAdventurer = null;
        if (pendingAdventurers.Count > 0)
        {
            StartCoroutine(MoveCharacters());
        }
        else
        {
            // Go to other Scene
        }
    }

    private IEnumerator MoveCharacters()
    {
        foreach (GameObject adventurerGO in adventurersList)
        {
            adventurerGO.GetComponent<AdventurerVisuals>().MoveCharacter();
        }
        yield return new WaitForSeconds(2f);
        EnableCharacterAsignation();
    }
}
