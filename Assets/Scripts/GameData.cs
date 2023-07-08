using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    #region Singleton
    private static GameData instance;

    public static GameData Instance
    {
        get
        {
            // Verificar si ya existe una instancia y devolverla
            if (instance == null)
            {
                // Buscar la instancia en la escena
                instance = FindObjectOfType<GameData>();

                // Si no se encuentra, crear una nueva instancia
                if (instance == null)
                {
                    Debug.Log("ALGO ESTA MAL CONFIGURADO EN LA ESCENA FALTA EL GameData");
                    // Crear un nuevo GameObject para el Singleton
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<GameData>();
                    singletonObject.name = "GameDataSingleton";
                }
            }

            return instance;
        }
    }
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (!initialized)
        {
            weekCount = 1;
            initialized = true;
        }
    }

    public List<LevelAndColor> levelAndColors;

    public List<Adventurer> activeAdventurers;

    public List<ClassAndVisuals> classAndVisuals;

    public List<WeaponsAndVisuals> weaponsAndVisuals;

    public int weekCount;

    public bool initialized;


    public Color ObtainColorByLevel(int pLevel)
    {
        foreach (var levelColor in levelAndColors)
        {
            if (levelColor.levell == pLevel)
            {
                return levelColor.color;
            }
        }
        return Color.gray;
    }

    public GameObject ObtainAdventurerVisuals(AdventurerType adventurerType)
    {
        foreach (ClassAndVisuals classAndVisual in classAndVisuals)
        {
            if (adventurerType == classAndVisual.adventurerType)
            {
                return classAndVisual.visualGO;
            }
        }
        return null;
    }

    public GameObject ObtainWeaponVisuals(WeaponType weaponType)
    {
        foreach (WeaponsAndVisuals weaponAndVisual in weaponsAndVisuals)
        {
            if (weaponType == weaponAndVisual.weaponType)
            {
                return weaponAndVisual.weaponGO;
            }
        }
        return null;
    }

    private List<string> randomNames = new List<string>()
    {
            "Aaryn", "Aaro", "Aarus", "Abramus", "Abrahm", "Abyl", "Abelus", "Adannius", "Adanno", "Aedam", "Adym", "Adamus",
            "Aedrian", "Aedrio", "Aedyn", "Aidyn", "Aelijah", "Elyjah", "Aendro", "Androe", "Aenry", "Hynroe", "Hynrus",
            "Aethan", "Aethyn", "Aevan", "Evyn", "Evanus", "Alecks", "Alyx", "Alexandyr", "Xandyr", "Alyn", "Alaen", "Andrus",
            "Aendrus", "Anglo", "Aenglo", "Anglus", "Antony", "Antonyr", "Astyn", "Astinus", "Axelus", "Axyl", "Benjamyn",
            "Benjamyr", "Braidyn", "Brydus", "Braddeus", "Brandyn", "Braendyn", "Bryus", "Bryne", "Bryn", "Branus", "Caeleb",
            "Caelyb", "Caerlos", "Carlus", "Cameryn", "Camerus", "Cartus", "Caertero", "Charlus", "Chaerles", "Chyrles",
            "Christophyr", "Christo", "Chrystian", "Chrystan", "Connorus", "Connyr", "Daemian", "Damyan", "Daenyel", "Danyel",
            "Davyd", "Daevo", "Dominac", "Dylaen", "Dylus", "Elius", "Aeli", "Elyas", "Helius", "Helian", "Emilyan", "Emilanus",
            "Emmanus", "Emynwell", "Ericus", "Eryc", "Eryck", "Ezekius", "Zeckus", "Ezekio", "Ezrus", "Yzra", "Gabrael",
            "Gaebriel", "Gael", "Gayl", "Gayel", "Gaeus", "Gavyn", "Gaevyn", "Goshwa", "Joshoe", "Graysus", "Graysen",
            "Gwann", "Ewan", "Gwyllam", "Gwyllem", "Haddeus", "Hudsyn", "Haesoe", "Haesys", "Haesus", "Handus", "Handyr",
            "Hantus", "Huntyr", "Haroldus", "Haryld", "Horgus", "Horus", "Horys", "Horyce", "Hosea", "Hosius", "Iaen", "Yan",
            "Ianus", "Ivaen", "Yvan", "Jaecoby", "Jaecob", "Jaeden", "Jaedyn", "Jaeremiah", "Jeremus", "Jasyn", "Jaesen",
            "Jaxon", "Jaxyn", "Jaxus", "Johnus", "Jonus", "Jonaeth", "Jonathyn", "Jordus", "Jordyn", "Josaeth", "Josephus",
            "Josaeus", "Josayah", "Jovanus", "Giovan", "Julyan", "Julyo", "Jyck", "Jaeck", "Jacus", "Kaevin", "Kevyn", "Vinkus",
            "Laevi", "Levy", "Levius", "Landyn", "Laendus", "Leo", "Leonus", "Leonaerdo", "Leonyrdo", "Lynardus", "Lincon",
            "Lyncon", "Linconus", "Logaen", "Logus", "Louis", "Lucius", "Lucae", "Lucaen", "Lucaes", "Lucoe", "Lucus", "Lyam",
            "Maeson", "Masyn", "Maetho", "Mathoe", "Matteus", "Matto", "Maxus", "Maximus", "Maximo", "Maxymer", "Mychael",
            "Mygwell", "Miglus", "Mythro", "Mithrus", "Naemo", "Naethyn", "Nathanus", "Naethynel", "Nicholaes", "Nycholas",
            "Nicholys", "Nicolus", "Nolyn", "Nolanus", "Olivyr", "Alivyr", "Olivus", "Oscarus", "Oscoe", "Raen", "Ryn",
            "Robertus", "Robett", "Bertus", "Romyn", "Romanus", "Ryderus", "Ridyr", "Samwell", "Saemuel", "Santegus",
            "Santaegus", "Sybasten", "Bastyen", "Tago", "Aemo", "Tagus", "Theodorus", "Theodus", "Thaeodore", "Thomys",
            "Thomas", "Tommus", "Tylus", "Tilyr", "Uwyn", "Oewyn", "Victor", "Victyr", "Victorus", "Vincynt", "Vyncent",
            "Vincentus", "Wyttus", "Wyaett", "Xavius", "Havius", "Xavyer", "Yago", "Tyago", "Tyego", "Ysaac", "Aisaac",
            "Ysaiah", "Aisiah", "Siahus", "Zacharus", "Zachar", "Zachaery"
    };

    public string GetRandomName()
    {
        int randomIndex = Random.Range(0, randomNames.Count);
        return randomNames[randomIndex];
    }

    public List<PrefixByAdventurer> prefixByAdventurers;

    public string GetRandomPrefix(AdventurerType adventurerType)
    {
        List<PrefixByAdventurer> adventurersPrefixses = prefixByAdventurers.FindAll(x => x.adventurerType == adventurerType);
        int randomPrefix = UnityEngine.Random.Range(0, adventurersPrefixses.Count);
        return adventurersPrefixses[randomPrefix].prefix;
    }

    public List<MissionInfo> missionInfos;

    public MissionInfo GetRandomMission(int missionLevel)
    {
        List<MissionInfo> missionsPerLevel = missionInfos.FindAll(x => x.level == missionLevel);
        int randomIndex = UnityEngine.Random.Range(0, missionsPerLevel.Count);
        return missionsPerLevel[randomIndex];
    }

    public GameObject missionPrefab;

    public List<Dificulty> dificulties;

    public Dificulty GetDificulty()
    {
        int dificultyValue = Mathf.RoundToInt(weekCount / 4);
        if (dificultyValue <= 0)
        {
            dificultyValue = 1;
        }
        return dificulties.Find(x => x.value == dificultyValue);
    }

    public List<MissionInfo> activeMissions;
}

[System.Serializable]
public class Dificulty
{
    public int value;
    public List<int> cardLevels;
}

[System.Serializable]
public class ClassAndVisuals
{
    public AdventurerType adventurerType;
    public GameObject visualGO;
}

[System.Serializable]
public class WeaponsAndVisuals
{
    public WeaponType weaponType;
    public GameObject weaponGO;
}

[System.Serializable]
public class PrefixByAdventurer
{
    public AdventurerType adventurerType;
    public string prefix;
}

[System.Serializable]
public class LevelAndColor
{
    public int levell;
    public Color color;
}
