using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

[System.Serializable]
public class OBJ
{
    public Image unitBackground;
    public Image unitImage;
    public TextMeshProUGUI unitName;
    public TextMeshProUGUI unitGrade;
    public TMP_Text unitLevel;
    public TextMeshProUGUI unitCardCount;
    public TextMeshProUGUI remainCardCount;
    public TMP_Text unitCurrentCardCount;
}

[Serializable]
public class UnitInfo
{
    public int damage;
    public int hp;
    public float attackSpeed;
    public int moveSpeed;
}

public class CharacterInfo
{
    public int characterID { get; set; }
    public Sprite characterSprite { get; set; }
    public string CharacterName { get; set; }
    public string CharacterGrade { get; set; }
    public int CharacterLevel { get; set; }
    public int CharacterCurrentCardCount { get; set; }
    public int CharacterMaxCardCount { get; set; }
}

public class OnClickOpenChest : MonoBehaviour, IPointerClickHandler
{
    public GameObject displayAll;
    public Transform gainCards;
    public GameObject giftCard;
    public GameObject card_Open;

    public MoveCard MJ_MoveCard;
    public Epic_ShakeBox MJ_ShakeBox;

    private SettingChest chest;

    public UnitInfo[] unitInfos;

    public OBJ unitOBJ;

    public KeyValuePair<CharacterInfo, int> characterInfo;


    public int clickCount = 0;
    public bool isOpenClick;

    public FirstChestCondition chestCondition; 


    private void Awake()
    {
        chest = GetComponent<SettingChest>();

        MJ_MoveCard = GetComponentInChildren<MoveCard>();
        MJ_ShakeBox = GetComponentInChildren<Epic_ShakeBox>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(clickCount > chest.randomUnits.Count)
        {
            ClosePanel();
            chestCondition.CheckNewbie();
            return;
        }
        else if (chest.totalRemainCard == 0 || clickCount == chest.randomUnits.Count)
        {
            SettingCardInfoManager.Instance.UpdateUserInfo(8);
            DisplayAll();
            clickCount++;
            return;
        }

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
            characterInfo = chest.GetCharInfo(clickCount);
            clickCount++;
            Set_UI();
        }
    }

    #region UI Set

    public void Set_UI()
    {
        unitOBJ.remainCardCount.text = chest.totalRemainCard.ToString();

        Set_Grade();
        Set_UnitInfo(characterInfo.Value);
    }
    private void Set_Grade()
    {
        string grade = null;
        switch (characterInfo.Key.CharacterGrade)
        {
            case "ÀÏ¹Ý":
                unitOBJ.unitBackground.ColorNormal();
                unitOBJ.unitGrade.ColorBlue();
                grade = "ÀÏ¹Ý Ä«µå";
                break;
            case "Èñ±Í":
                unitOBJ.unitBackground.ColorRare();
                unitOBJ.unitGrade.ColorRare();
                grade = "Èñ±Í Ä«µå";
                break;
            case "¿µ¿õ":
                unitOBJ.unitBackground.ColorEpic();
                unitOBJ.unitGrade.ColorEpic();
                grade = "¿µ¿õ Ä«µå";
                break;
            default:
                break;
        }
        unitOBJ.unitGrade.text = grade;
    }
    private void Set_UnitInfo(int amount)
    {
        unitOBJ.unitImage.sprite = characterInfo.Key.characterSprite;
        unitOBJ.unitCardCount.text = $"X {amount}";
        unitOBJ.unitName.text = characterInfo.Key.CharacterName;
        if (characterInfo.Key.CharacterLevel == 0)
        {
            unitOBJ.unitLevel.text = "1·¹º§";
        }
        else
        {
            unitOBJ.unitLevel.text = $"{characterInfo.Key.CharacterLevel}·¹º§";
        }
        if (characterInfo.Key.CharacterCurrentCardCount == 0 && characterInfo.Key.CharacterMaxCardCount == 0)
        {
            unitOBJ.unitCurrentCardCount.text = "1/2";
        }
        else
        {
            unitOBJ.unitCurrentCardCount.text = $"{characterInfo.Key.CharacterCurrentCardCount}/{characterInfo.Key.CharacterMaxCardCount}";
        }

        if (SettingCardInfoManager.Instance.IsNewbie())
        {
            SettingCardInfoManager.Instance.InsertNewCard(characterInfo.Key.characterID,
                unitInfos[characterInfo.Key.characterID].damage, unitInfos[characterInfo.Key.characterID].hp,
                unitInfos[characterInfo.Key.characterID].attackSpeed, unitInfos[characterInfo.Key.characterID].moveSpeed);
        }
        else
        {
            SettingCardInfoManager.Instance.UpdateCard(characterInfo.Key.characterID, 11); // 11 ¼öÁ¤
        }

        GameObject addResultReward = Instantiate(giftCard, gainCards);
        OpenBoxUnitDisplay displayOBJ = addResultReward.GetComponent<OpenBoxUnitDisplay>();

        SaveData(displayOBJ);
    }

    private void SaveData(OpenBoxUnitDisplay displayOBJ)
    {
        displayOBJ.backGround.color = unitOBJ.unitBackground.color;
        displayOBJ.unitImage.sprite = unitOBJ.unitImage.sprite;
        displayOBJ.unitCount.text = unitOBJ.unitCardCount.text;
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
