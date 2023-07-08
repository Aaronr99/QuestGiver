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
    [SerializeField] private OutcomeField winOutcome;
    [SerializeField] private OutcomeField successOutcome;
    [SerializeField] private OutcomeField loseOutcome;


    [SerializeField] private Image background1;
    [SerializeField] private Image background2;

    public void SetCardInfo(MissionInfo pMissionInfo)
    {
        missionInfo = pMissionInfo;
        titleText.text = missionInfo.title;
        lvlText.text = missionInfo.level.ToString();

        winOutcome.hpValueText.text = missionInfo.GetHpOnWin().ToString();
        winOutcome.xpValueText.text = missionInfo.GetXpOnWin().ToString();

        successOutcome.hpValueText.text = missionInfo.GetHpOnSucess().ToString();
        successOutcome.xpValueText.text = missionInfo.GetXpOnSucess().ToString();

        loseOutcome.hpValueText.text = missionInfo.GetHpOnLose().ToString();
        loseOutcome.xpValueText.text = missionInfo.GetXpOnLose().ToString();

        Color colorToSet = GameData.Instance.ObtainColorByLevel(missionInfo.level);
        background1.color = colorToSet;
        background2.color = colorToSet;
    }
}
