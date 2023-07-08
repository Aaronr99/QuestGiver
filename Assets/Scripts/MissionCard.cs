using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionCard : MonoBehaviour
{
    public string title;
    public int level;
    public float moralLoss;
    public int hpLoss;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text lvlText;
    [SerializeField] private TMP_Text moralText;
    [SerializeField] private TMP_Text hpText;

    [SerializeField] private Image background1;
    [SerializeField] private Image background2;

    public void SetCardInfo(string pTitle, int pLevel, int pMoral, int pHp)
    {
        title = pTitle;
        level = pLevel;
        moralLoss = pMoral;
        hpLoss = pHp;
        titleText.text = title;
        lvlText.text = level.ToString();
        moralText.text = moralLoss.ToString();
        hpText.text = hpLoss.ToString();

        Color colorToSet = GameData.Instance.ObtainColorByLevel(level);
        background1.color = colorToSet;
        background2.color = colorToSet;
    }
}
