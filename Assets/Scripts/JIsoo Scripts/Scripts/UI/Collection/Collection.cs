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

    private int cardId;

    private void Start()
    {
        InfoButton.SetActive(true);
        upgradeButton.SetActive(false);

    }

    public void InitUI(CharacterData characterData)
    {
        this.cardId = characterData.cardId;

        SettingUI(characterData.currentCardCount, characterData.maxCardCount, characterData.cost, characterData.name, characterData.level, cardCountFill, cardCountText, costText, nameText, levelText);
    }

    public void SettingUI(int currentCard, int maxCard, int cost, string name, int level, Image cardCountFill, TextMeshProUGUI cardCountText, TextMeshProUGUI costText, TextMeshProUGUI nameText, TextMeshProUGUI levelText)
    {
        cardCountFill.fillAmount = (float)currentCard / maxCard;
        cardCountText.text = $"{currentCard} / {maxCard}";
        costText.text = $"{cost}";
        nameText.text = name;
        levelText.text = $"레벨 {level}";
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
    public void OnClickUpgradeButton()
    {
        if(cardId != 3 && cardId != 8)
        {
            upgrade.Setinfo(settingUnit.GetUnitData(DatabaseManager.Instance.userId, cardId));
        }
        else if(cardId == 3)
        {
            upgrade.Setinfo(settingUnit.GetMagicData(DatabaseManager.Instance.userId, cardId));
        }
        else if(cardId == 8)
        {
            upgrade.Setinfo(settingUnit.GetTowerData(DatabaseManager.Instance.userId, cardId));
        }
    }
}