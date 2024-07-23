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

    public MJ_MoveCard MJ_MoveCard;
    private MJ_OpenCard MJ_OpenCard;
    public MJ_ShakeBox MJ_ShakeBox;

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

        MJ_MoveCard = GetComponentInChildren<MJ_MoveCard>();
        MJ_OpenCard = GetComponentInChildren<MJ_OpenCard>();
        MJ_ShakeBox = GetComponentInChildren<MJ_ShakeBox>();

        if (MJ_OpenCard != null)
        {
            MJ_OpenCard.openChest = this;
        }
        // TODO : 테스트용 노말 상자
        //chest.OpenEpicChest();

        //isOpenClick = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(chest.totalRemainCard);
        if(clickCount > chest.randomUnits.Count)
        {
            ClosePanel();
            print("kkkk");
            chestCondition.CheckNewbie();
            return;
        }
        else if (chest.totalRemainCard == 0 || clickCount == chest.randomUnits.Count)
        {
            SettingCardInfoManager.instance.UpdateUserInfo(8);
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

        if (MJ_OpenCard != null)
        {
            Debug.Log("클릭");
            MJ_OpenCard.Chanege();
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
            case "일반":
                unitOBJ.unitBackground.ColorNormal();
                unitOBJ.unitGrade.ColorBlue();
                grade = "일반 카드";
                break;
            case "희귀":
                unitOBJ.unitBackground.ColorRare();
                unitOBJ.unitGrade.ColorRare();
                grade = "희귀 카드";
                break;
            case "영웅":
                unitOBJ.unitBackground.ColorEpic();
                unitOBJ.unitGrade.ColorEpic();
                grade = "영웅 카드";
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
            unitOBJ.unitLevel.text = "1레벨";
        }
        else
        {
            unitOBJ.unitLevel.text = $"{characterInfo.Key.CharacterLevel}레벨";
        }
        if (characterInfo.Key.CharacterCurrentCardCount == 0 && characterInfo.Key.CharacterMaxCardCount == 0)
        {
            unitOBJ.unitCurrentCardCount.text = "1/2";
        }
        else
        {
            unitOBJ.unitCurrentCardCount.text = $"{characterInfo.Key.CharacterCurrentCardCount}/{characterInfo.Key.CharacterMaxCardCount}";
        }

        if (SettingCardInfoManager.instance.IsNewbie())
        {
            SettingCardInfoManager.instance.InsertNewCard(characterInfo.Key.characterID,
                unitInfos[characterInfo.Key.characterID].damage, unitInfos[characterInfo.Key.characterID].hp,
                unitInfos[characterInfo.Key.characterID].attackSpeed, unitInfos[characterInfo.Key.characterID].moveSpeed);
        }
        else
        {
            SettingCardInfoManager.instance.UpdateCard(characterInfo.Key.characterID, 11); // 11 수정
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

        //uniData.unit_CurrentCardCount += reward.Value.Value.Item2;
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
