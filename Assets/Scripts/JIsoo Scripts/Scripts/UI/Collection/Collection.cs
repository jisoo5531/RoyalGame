using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Reflection;

public class Collection : MonoBehaviour
{
    /// <summary>
    /// TODO : 정렬 할 때는 맞춰서
    /// </summary>
    public int mySequence;

    [Space(20)]
    public Image cardCountFill;
    public Image upArrow;
    public TextMeshProUGUI cardCountText;
    public TextMeshProUGUI costText;

    [Space(20)]
    public GameObject InfoButton;
    public GameObject upgradeButton;

    private UnitData_SO unitData;
    private int myCurrentCardCount;
    private int myMaxCardCount;
    private int myUnitCost;

    private int cardId;

    private void Start()
    {
        InfoButton.SetActive(true);
        upgradeButton.SetActive(false);

        //unitData = StartManager.m_Instance.m_unitDatas[mySequence];
        //SettingUI(unitData, cardCountFill, cardCountText, costText);
    }
    private void Update()
    {
        //CheckAvailableUpgrade(cardCountFill, upArrow, InfoButton, upgradeButton);
        //SettingUI(unitData, cardCountFill, cardCountText, costText);
    }

    //public void InitUI(int cardID, int currentCard, int maxCard, int cost, string grade, string name, int level)
    //{
    //    this.cardId = cardID;
    //    SettingUI(currentCard, maxCard, cost, cardCountFill, cardCountText, costText);
    //}
    public void InitUI(CharacterData characterData)
    {
        this.cardId = characterData.cardId;
        SettingUI(characterData.currentCardCount, characterData.maxCardCount, characterData.cost, cardCountFill, cardCountText, costText);
    }

    public void SettingUI(int currentCard, int maxCard, int cost, Image cardCountFill, TextMeshProUGUI cardCountText, TextMeshProUGUI costText)
    {
        cardCountFill.fillAmount = currentCard / maxCard;
        cardCountText.text = $"{currentCard} / {maxCard}";
        costText.text = $"{cost}";
    }
    /// <summary>
    /// 카드가 다 모여 업그레이드가 가능하면
    /// </summary>
    public bool CheckAvailableUpgrade(Image cardCountFill, Image upArrow, GameObject InfoButton = null, GameObject upgradeButton = null)
    {
        if (cardCountFill.fillAmount >= 1f)
        {
            cardCountFill.ColorGreen();
            upArrow.ColorGreen();

            if (InfoButton != null && upgradeButton != null)
            {
                InfoButton.SetActive(false);
                upgradeButton.SetActive(true);
            }
            return true;
        }
        else
        {
            cardCountFill.ColorSky();
            upArrow.ColorSky();

            if (InfoButton != null && upgradeButton != null)
            {
                InfoButton.SetActive(true);
                upgradeButton.SetActive(false);

            }
            return false;
        }
    }
    public void OnClickUpgradeButton(int number)
    {
        //Upgrade upgrade = FindObjectOfType<Upgrade>();

        CharacterData unitData = SettingCardInfoManager.instance.charData[number];

        //upgrade.SetInfo(unitData);
    }
}