using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour
{
    public TextMeshProUGUI unitName;

    public Image unitImage;
    public Image cardCountFill;
    public Image upArrow;
    public Image gradeAndType;

    public TextMeshProUGUI costText;
    public TextMeshProUGUI cardCountText;
    
    

    public void SetInfo(UnitData_SO unitData)
    {                
        unitName.text = $"·¹º§ {unitData.unit_Level} {unitData.unitName}".ToString();
       
        unitImage.sprite = unitData.iconSprite;
        costText.text = unitData.cost.ToString();

        Collection collection = FindObjectOfType<Collection>();

        collection.SettingUI(unitData, cardCountFill, cardCountText, costText);
        collection.CheckAvailableUpgrade(cardCountFill, upArrow);               
    }
}
