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

    private int myCurrentCardCount;
    private int myMaxCardCount;
    private int myUnitCost;

    private void Start()
    {
        SettingUI();
    }
    private void Update()
    {
        SetColor();
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
    private void SetColor()
    {
        if (cardCountFill.fillAmount >= 1f)
        {
            cardCountFill.ColorGreen();
            UpArrow.ColorGreen();
        }
        else
        {
            cardCountFill.ColorSky();
            UpArrow.ColorSky();
        }
    }
}
