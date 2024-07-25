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
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;

    [Space(20)]
    public GameObject InfoButton;
    public GameObject upgradeButton;

    [Space(20)]
    public SettingUnit settingUnit;
    public Upgrade upgrade;

    public int cardId;
    private CharacterData characterInfo;
    private bool isCanUpgrade = false;

    private void Start()
    {
        InfoButton.SetActive(true);
        upgradeButton.SetActive(false);

    }

    public void InitUI(CharacterData characterData)
    {
        this.cardId = characterData.cardId;
        characterInfo = characterData;
        SettingUI(characterData.currentCardCount, characterData.maxCardCount, characterData.cost, characterData.name, characterData.level, cardCountFill, cardCountText, costText, nameText, levelText);
    }

    public void SettingUI(int currentCard, int maxCard, int cost, string name, int level, Image cardCountFill, TextMeshProUGUI cardCountText, TextMeshProUGUI costText, TextMeshProUGUI nameText, TextMeshProUGUI levelText)
    {
        cardCountFill.fillAmount = (float)currentCard / maxCard;
        cardCountText.text = $"{currentCard} / {maxCard}";
        costText.text = $"{cost}";
        nameText.text = name;
        levelText.text = $"레벨 {level}";

        if (currentCard < maxCard)
        {
            cardCountFill.ColorSky();
            upArrow.ColorSky();
        }
    }

    public void CheckCardCount()
    {
        isCanUpgrade = CheckAvailableUpgrade(characterInfo.currentCardCount, characterInfo.maxCardCount, upArrow);
    }

    /// <summary>
    /// 카드가 다 모여 업그레이드가 가능하면
    /// </summary>
    public bool CheckAvailableUpgrade(int currentCardCount, int maxCardCount, Image upArrow)
    {
        if (currentCardCount >= maxCardCount)
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
    public void OnClickUpgradeButton()
    {
        upgrade.collection = this;
        if (cardId != 3 && cardId != 8)
        {
            upgrade.Setinfo(settingUnit.GetUnitData(DatabaseManager.Instance.userId, cardId), isCanUpgrade);
        }
        else if (cardId == 3)
        {
            upgrade.Setinfo(settingUnit.GetMagicData(DatabaseManager.Instance.userId, cardId), isCanUpgrade);
        }
        else if (cardId == 8)
        {
            upgrade.Setinfo(settingUnit.GetTowerData(DatabaseManager.Instance.userId, cardId), isCanUpgrade);
        }
    }
}