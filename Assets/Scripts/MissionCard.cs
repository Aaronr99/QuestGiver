using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MissionCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public MissionInfo missionInfo;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text lvlText;
    [SerializeField] private OutcomeField winOutcome;
    [SerializeField] private OutcomeField successOutcome;
    [SerializeField] private OutcomeField loseOutcome;
    [SerializeField] private GameObject panel;

    [SerializeField] private Image background1;
    [SerializeField] private Image background2;

    private Color originalColor;
    private GameObject copyGO;

    private bool clickActive;
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        clickActive = false;
        originalColor = GetComponent<Image>().color;
    }

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
        copyGO = Instantiate(gameObject, QuestGManager.Instance.topCanvas.transform);
        copyGO.transform.localScale = Vector3.one * 0.5f;
        copyGO.SetActive(false);
        copyGO.GetComponent<MissionCard>().enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.blue;
        Debug.Log("ENTER");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = originalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        clickActive = true;
        panel.SetActive(true);
        copyGO.SetActive(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        clickActive = false;
        panel.SetActive(false);
        copyGO.SetActive(false);
        if (Input.mousePosition.y > 195)
        {
            Debug.Log("APPLY MISISON");
            Destroy(copyGO);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (clickActive)
        {
            copyGO.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }

}
