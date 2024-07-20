using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UnitStat
{
    public GameObject hpOBJ;
    public GameObject damageOBJ;
}
public class Upgrade : MonoBehaviour
{
    public TextMeshProUGUI unitName;

    public Image unitImage;
    public Image cardCountFill;
    public Image upArrow;
    public Image gradeAndType;

    public TextMeshProUGUI costText;
    public TextMeshProUGUI cardCountText;

    public Transform unitStats;

    [Space(20)]
    public UnitStat unitStatList;
    

    public void SetInfo(UnitData_SO unitData)
    {                
        unitName.text = $"·¹º§ {unitData.unit_Level} {unitData.unitName}".ToString();
       
        unitImage.sprite = unitData.iconSprite;
        costText.text = unitData.cost.ToString();

        Collection collection = FindObjectOfType<Collection>();

        collection.SettingUI(unitData, cardCountFill, cardCountText, costText);
        collection.CheckAvailableUpgrade(cardCountFill, upArrow);

        SetStats(unitData);        
    }

    public void SetStats(UnitData_SO unitData)
    {
        foreach (Transform child in unitStats)
        {
            Destroy(child.gameObject);
        }

        Set_HPObj(unitData);
        Set_DamageObj(unitData);
    }
    private void Set_HPObj(UnitData_SO unitData)
    {
        GameObject hpStat_OBJ = Instantiate(unitStatList.hpOBJ, unitStats);
        UI_UpgradeStatText uiHp = hpStat_OBJ.GetComponent<UI_UpgradeStatText>();

        if (uiHp != null)
        {
            uiHp.value.text = unitData.maxHp.ToString();
            uiHp.upgradeValue.text = $"+ {unitData.Get_Upgrade_HP()}";
        }
    }
    private void Set_DamageObj(UnitData_SO unitData)
    {
        GameObject hpStat_OBJ = Instantiate(unitStatList.damageOBJ, unitStats);
        UI_UpgradeStatText uiDamage = hpStat_OBJ.GetComponent<UI_UpgradeStatText>();

        if (uiDamage != null)
        {
            uiDamage.value.text = unitData.damage.ToString();
            uiDamage.upgradeValue.text = $"+ {unitData.Get_Upgrade_Damage()}";
        }
    }
}
