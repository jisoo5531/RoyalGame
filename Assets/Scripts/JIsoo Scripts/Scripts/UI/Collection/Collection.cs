using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

    private UnitData unitData;
    private int myCurrentCardCount;
    private int myMaxCardCount;
    private int myUnitCost;

    private void Start()
    {
        InfoButton.SetActive(true);
        upgradeButton.SetActive(false);

        unitData = GameManager.m_Instance.uniData[mySequence];        
        SettingUI(unitData, cardCountFill, cardCountText, costText);
    }
    private void Update()
    {
        CheckAvailableUpgrade(cardCountFill, upArrow, InfoButton, upgradeButton);
        SettingUI(unitData, cardCountFill, cardCountText, costText);
    }

    public void SettingUI(UnitData unitData, Image cardCountFill, TextMeshProUGUI cardCountText, TextMeshProUGUI costText)
    {
        myCurrentCardCount = unitData.cardInfo.unit_CurrentCardCount;
        myMaxCardCount = unitData.cardInfo.unit_MaxCardCount;
        myUnitCost = unitData.unitInfo.unitStat.cost;


        cardCountFill.fillAmount = (float)myCurrentCardCount / (float)myMaxCardCount;
        cardCountText.text = $"{myCurrentCardCount} / {myMaxCardCount}".ToString();
        costText.text = $"{myUnitCost}".ToString();
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
        Upgrade upgrade = FindObjectOfType<Upgrade>();

        UnitData unitData = GameManager.m_Instance.uniData[number];

        upgrade.SetInfo(unitData);
    }
}