using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[System.Serializable]
public class OBJ
{
    public Image unitBackground;
    public Image unitImage;
    public TextMeshProUGUI unitName;
    public TextMeshProUGUI unitGrade;
    public TextMeshProUGUI unitCardCount;
    public TextMeshProUGUI remainCardCount;
}

public class OnClickOpenChest : MonoBehaviour, IPointerClickHandler
{
    public GameObject displayAll;
    public Transform gainCards;
    public GameObject giftCard;
    public GameObject card_Open;


    private MJ_MoveCard MJ_MoveCard;
    private MJ_OpenCard MJ_OpenCard;
    private MJ_ShakeBox MJ_ShakeBox;

    private Chest chest;

    public OBJ unitOBJ;

    private KeyValuePair<int, (Chest.Grade, int)>? reward;
    private UnitData_SO uniData = null;

    private int clickCount = 0;
    public bool isOpenClick;
    private bool isEnd = false;

    private void Awake()
    {
        chest = GetComponent<Chest>();

        MJ_MoveCard = GetComponentInChildren<MJ_MoveCard>();
        MJ_OpenCard = GetComponentInChildren<MJ_OpenCard>();
        MJ_ShakeBox = GetComponentInChildren<MJ_ShakeBox>();

        if (MJ_OpenCard != null)
        {
            MJ_OpenCard.openChest = this;
        }
        // TODO : 테스트용 노말 상자
        chest.OpenEpicChest();

        isOpenClick = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (chest.totalRemainCard == 0)
        {
            DisplayAll();
            return;
        }

        if (isOpenClick)
        {            
            if (MJ_ShakeBox != null)
            {
                MJ_ShakeBox.DOAction();
            }
            if (MJ_MoveCard != null)
            {
                MJ_MoveCard.CardActive();
            }

            if (chest != null)
            {
                reward = chest.OnClickOpenCard(clickCount);
                clickCount++;

                Set_UI();
            }
        }        
        if (MJ_OpenCard != null)
        {
            Debug.Log("클릭");
            MJ_OpenCard.Chanege();
        }
    }

    #region UI Set

    private void Set_UI()
    {
        foreach (UnitData_SO unit in StartManager.m_Instance.m_unitDatas)
        {
            if (unit.unit_ID == reward.Value.Key)
            {
                uniData = unit;
                break;
            }
        }

        unitOBJ.remainCardCount.text = chest.totalRemainCard.ToString();

        Set_Grade();
        Set_UnitInfo();
    }
    private void Set_Grade()
    {
        string grade = null;
        switch (uniData.grade)
        {
            case Grade.Normal:
                unitOBJ.unitBackground.ColorNormal();
                unitOBJ.unitGrade.ColorBlue();
                grade = "일반 카드";
                break;
            case Grade.Rare:
                unitOBJ.unitBackground.ColorRare();
                unitOBJ.unitGrade.ColorRare();
                grade = "희귀 카드";
                break;
            case Grade.Epic:
                unitOBJ.unitBackground.ColorEpic();
                unitOBJ.unitGrade.ColorEpic();
                grade = "영웅 카드";
                break;
            default:
                break;
        }
        unitOBJ.unitGrade.text = grade;
    }
    private void Set_UnitInfo()
    {
        unitOBJ.unitImage.sprite = uniData.iconSprite;
        unitOBJ.unitCardCount.text = $"X{reward.Value.Value.Item2}";
        unitOBJ.unitName.text = uniData.unitName;


        GameObject addResultReward = Instantiate(giftCard, gainCards);
        OpenBoxUnitDisplay displayOBJ = addResultReward.GetComponent<OpenBoxUnitDisplay>();

        SaveData(displayOBJ);
    }

    private void SaveData(OpenBoxUnitDisplay displayOBJ)
    {
        displayOBJ.backGround.color = unitOBJ.unitBackground.color;
        displayOBJ.unitImage.sprite = unitOBJ.unitImage.sprite;
        displayOBJ.unitCount.text = unitOBJ.unitCardCount.text;

        uniData.unit_CurrentCardCount += reward.Value.Value.Item2;
    }

    #endregion

    private void DisplayAll()
    {
        card_Open.SetActive(false);
        displayAll.SetActive(true);        
    }
    public void ClosePanel()
    {
        displayAll.SetActive(false);

        transform.parent.gameObject.SetActive(false);        
    }
}
