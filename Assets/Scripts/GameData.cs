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

    public List<LevelAndColor> levelAndColors;

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
}

[System.Serializable]
public class LevelAndColor
{
    public int levell;
    public Color color;
}
