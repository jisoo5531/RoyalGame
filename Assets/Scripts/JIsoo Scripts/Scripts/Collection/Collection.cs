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

    private UnitData_SO unitData;
    private int myCurrentCardCount;
    private int myMaxCardCount;
    private int myUnitCost;

    private void Start()
    {
        InfoButton.SetActive(true);
        upgradeButton.SetActive(false);

        unitData = StartManager.m_Instance.m_unitDatas[mySequence];
        SettingUI(unitData, cardCountFill, cardCountText, costText);
    }
    private void Update()
    {
        CheckAvailableUpgrade(cardCountFill, upArrow, InfoButton, upgradeButton);
        SettingUI(unitData, cardCountFill, cardCountText, costText);
    }

    public void SettingUI(UnitData_SO unitData, Image cardCountFill, TextMeshProUGUI cardCountText, TextMeshProUGUI costText)
    {
        myCurrentCardCount = unitData.unit_CurrentCardCount;
        myMaxCardCount = unitData.unit_MaxCardCount;
        myUnitCost = unitData.cost;


        cardCountFill.fillAmount = (float)myCurrentCardCount / (float)myMaxCardCount;
        cardCountText.text = $"{myCurrentCardCount} / {myMaxCardCount}".ToString();
        costText.text = $"{myUnitCost}".ToString();
    }
    /// <summary>
    /// 카드가 다 모여 업그레이드가 가능하면
    /// </summary>
    public void CheckAvailableUpgrade(Image cardCountFill, Image upArrow, GameObject InfoButton = null, GameObject upgradeButton = null)
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
        }        
    }
    public void OnClickUpgradeButton(int number)
    {
        Upgrade upgrade = FindObjectOfType<Upgrade>();

        UnitData_SO unitData = StartManager.m_Instance.m_unitDatas[number];

        upgrade.SetInfo(unitData);
    }
}