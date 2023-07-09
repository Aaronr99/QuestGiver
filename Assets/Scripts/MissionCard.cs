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
    [SerializeField] private TMP_Text dangerText;
    public TMP_Text fitText;

    /*[SerializeField] private OutcomeField winOutcome;
    [SerializeField] private OutcomeField successOutcome;
    [SerializeField] private OutcomeField loseOutcome;*/
    [SerializeField] private GameObject panel;



    [SerializeField] private Image background1;
    [SerializeField] private Image background2;

    private Color originalColor;
    private GameObject copyGO;

    private bool clickActive;

    public Color lowColor;
    public Color mediumColor;
    public Color highColor;
    public Color deadlyColor;

    public AudioSource audioSource;
    public AudioClip dealClip;
    public AudioClip clickClip;

    private void Awake()
    {
        clickActive = false;
        originalColor = GetComponent<Image>().color;
    }

    public void SetCardInfo(MissionInfo pMissionInfo)
    {
        missionInfo = pMissionInfo;
        titleText.text = missionInfo.title;
        lvlText.text = missionInfo.level.ToString();

        switch (missionInfo.dangerLevel)
        {
            case DangerLevel.Low:
                dangerText.text = "LOW DANGER";
                dangerText.color = lowColor;
                break;
            case DangerLevel.Medium:
                dangerText.text = "MEDIUM DANGER";
                dangerText.color = mediumColor;
                break;
            case DangerLevel.High:
                dangerText.text = "HIGH DANGER";
                dangerText.color = highColor;
                break;
            case DangerLevel.Deadly:
                dangerText.text = "DEADLY";
                dangerText.color = deadlyColor;
                break;
            default:
                break;
        }

        Color colorToSet = GameData.Instance.ObtainColorByLevel(missionInfo.level);
        background1.color = colorToSet;
        background2.color = colorToSet;
        copyGO = Instantiate(gameObject, QuestGManager.Instance.topCanvas.transform);
        copyGO.transform.localScale = Vector3.one * 0.5f;
        copyGO.SetActive(false);
        copyGO.GetComponent<MissionCard>().enabled = false;
        copyGO.GetComponent<AudioSource>().Stop();
        copyGO.GetComponent<AudioSource>().enabled = false;
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
        audioSource.clip = clickClip;
        audioSource.Play();
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
            QuestGManager.Instance.AssignMissionOnCharacter(missionInfo);
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
