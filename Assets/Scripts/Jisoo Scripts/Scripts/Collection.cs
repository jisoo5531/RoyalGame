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
    public Image UpArrow;
    public TextMeshProUGUI cardCountText;
    public TextMeshProUGUI costText;

    [Space(20)]
    public GameObject Info;
    public GameObject Upgrade;

    private int myCurrentCardCount;
    private int myMaxCardCount;
    private int myUnitCost;

    private void Start()
    {
        Info.SetActive(true);
        Upgrade.SetActive(false);

        SettingUI();
    }
    private void Update()
    {
        CheckAvailableUpgrade();
        SettingUI();
    }    

    private void SettingUI()
    {
        myCurrentCardCount = StartManager.m_Instance.m_unitDatas[mySequence].unit_CurrentCardCount;
        myMaxCardCount = StartManager.m_Instance.m_unitDatas[mySequence].unit_MaxCardCount;
        myUnitCost = StartManager.m_Instance.m_unitDatas[mySequence].cost;
        
        
        cardCountFill.fillAmount = (float)myCurrentCardCount / (float)myMaxCardCount;
        cardCountText.text = $"{myCurrentCardCount} / {myMaxCardCount}".ToString();
        costText.text = $"{myUnitCost}".ToString();        
    }
    /// <summary>
    /// 카드가 다 모여 업그레이드가 가능하면
    /// </summary>
    private void CheckAvailableUpgrade()
    {
        if (cardCountFill.fillAmount >= 1f)
        {
            cardCountFill.ColorGreen();
            UpArrow.ColorGreen();

            Info.SetActive(false);
            Upgrade.SetActive(true);
        }
        else
        {
            cardCountFill.ColorSky();
            UpArrow.ColorSky();

            Info.SetActive(true);
            Upgrade.SetActive(false);
        }
    }
}
