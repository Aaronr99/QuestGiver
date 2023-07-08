using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionCard : MonoBehaviour
{
    public MissionInfo missionInfo;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text lvlText;
    [SerializeField] private TMP_Text moralText;
    [SerializeField] private TMP_Text hpText;

    [SerializeField] private Image background1;
    [SerializeField] private Image background2;

    public void SetCardInfo(MissionInfo pMissionInfo)
    {
        missionInfo = pMissionInfo;
        titleText.text = missionInfo.title;
        lvlText.text = missionInfo.level.ToString();
        moralText.text = missionInfo.moralLoss.ToString();
        hpText.text = missionInfo.hpLoss.ToString();

        Color colorToSet = GameData.Instance.ObtainColorByLevel(missionInfo.level);
        background1.color = colorToSet;
        background2.color = colorToSet;
    }
}
