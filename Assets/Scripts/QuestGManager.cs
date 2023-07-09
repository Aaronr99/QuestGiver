using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public Image hpBar;
    public Image xpBar;

    public GameObject topCanvas;
    public List<MissionCard> missionCards;

    public Color goodFitColor;
    public Color badFitColor;

    private void Start()
    {
        Invoke("StartProcess", 0.5f);
    }

    private void StartProcess()
    {
        pendingAdventurers = new Queue<GameObject>();
        adventurersList = new List<GameObject>();
        GameData.Instance.activeMissions = new List<MissionInfo>();
        missionCards = new List<MissionCard>();

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
            int randomType = UnityEngine.Random.Range(0, 7);
            newAdventurer.adventurerType = (AdventurerType)randomType;
            newAdventurer.characterLevel = 1;
            newAdventurer.hpTotal = newAdventurer.GetMaxHP();
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
            missionCards.Add(card);
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
        hpBar.fillAmount = (float)adventurer.hpTotal / (float)adventurer.GetMaxHP();
        xpBar.fillAmount = (float)adventurer.experience / (float)adventurer.GetMaxExperience();

        foreach (var card in missionCards)
        {
            int fitNumber = 3;
            if (card.missionInfo.favoredWeapon == adventurer.weaponType)
            {
                fitNumber++;
            }
            else if (card.missionInfo.disfavoredWeapon == adventurer.weaponType)
            {
                fitNumber--;
            }
            if (card.missionInfo.favouredAdventurer == adventurer.adventurerType)
            {
                fitNumber++;
            }
            else if (card.missionInfo.disfavoredAdventurer == adventurer.adventurerType)
            {
                fitNumber--;
            }
            switch (fitNumber)
            {
                case 1:
                    card.fitText.text = "Very bad fit";
                    card.fitText.color = badFitColor;
                    break;
                case 2:
                    card.fitText.text = "Bad fit";
                    card.fitText.color = badFitColor;
                    break;
                case 3:
                    card.fitText.text = "";
                    break;
                case 4:
                    card.fitText.text = "Good fit";
                    card.fitText.color = goodFitColor;
                    break;
                case 5:
                    card.fitText.text = "Very good fit";
                    card.fitText.color = goodFitColor;
                    break;
                default:
                    Debug.LogError("BAD NUMBERS");
                    break;
            }
        }
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
            GameData.Instance.weekCount += 1;
            SceneManager.LoadScene(1);
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
