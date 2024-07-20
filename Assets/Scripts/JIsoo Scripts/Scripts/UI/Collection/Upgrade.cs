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
    public GameObject attackSpeedOBJ;
    public GameObject moveSpeedOBJ;
    public GameObject targetOBJ;
    public GameObject rangeOBJ;
    public GameObject creationTimeOBJ;
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
        Set_AttackSppedObj(unitData);
        Set_MoveSppedObj(unitData);
        Set_TargetObj(unitData);
        Set_RangeObj(unitData);
        Set_CreationTimeObj(unitData);
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
        GameObject DamageStat_OBJ = Instantiate(unitStatList.damageOBJ, unitStats);
        UI_UpgradeStatText uiDamage = DamageStat_OBJ.GetComponent<UI_UpgradeStatText>();

        if (uiDamage != null)
        {
            uiDamage.value.text = unitData.damage.ToString();
            uiDamage.upgradeValue.text = $"+ {unitData.Get_Upgrade_Damage()}";
        }
    }
    private void Set_AttackSppedObj(UnitData_SO unitData)
    {
        GameObject AS_Stat_OBJ = Instantiate(unitStatList.attackSpeedOBJ, unitStats);
        UI_UpgradeStatText ui_AS = AS_Stat_OBJ.GetComponent<UI_UpgradeStatText>();

        if (ui_AS != null)
        {
            ui_AS.value.text = unitData.attackSpeed.ToString();
        }
    }
    
    private void Set_MoveSppedObj(UnitData_SO unitData)
    {
        GameObject MS_Stat_OBJ = Instantiate(unitStatList.moveSpeedOBJ, unitStats);
        UI_UpgradeStatText ui_MS = MS_Stat_OBJ.GetComponent<UI_UpgradeStatText>();

        if (ui_MS != null)
        {
            ui_MS.value.text = unitData.moveSpeed.ToString();
        }
    }
    
    private void Set_TargetObj(UnitData_SO unitData)
    {
        GameObject target_Stat_OBJ = Instantiate(unitStatList.targetOBJ, unitStats);
        UI_UpgradeStatText ui_Target = target_Stat_OBJ.GetComponent<UI_UpgradeStatText>();

        if (ui_Target != null)
        {
            ui_Target.value.text = unitData.attackTarget.ToString();
        }
    }
    
    private void Set_RangeObj(UnitData_SO unitData)
    {
        GameObject range_Stat_OBJ = Instantiate(unitStatList.rangeOBJ, unitStats);
        UI_UpgradeStatText ui_Range = range_Stat_OBJ.GetComponent<UI_UpgradeStatText>();

        if (ui_Range != null)
        {
            ui_Range.value.text = unitData.range.ToString();
        }
    }
    
    private void Set_CreationTimeObj(UnitData_SO unitData)
    {
        GameObject creationTime_Stat_OBJ = Instantiate(unitStatList.creationTimeOBJ, unitStats);
        UI_UpgradeStatText ui_CreationTIme = creationTime_Stat_OBJ.GetComponent<UI_UpgradeStatText>();

        if (ui_CreationTIme != null)
        {
            ui_CreationTIme.value.text = unitData.spawnTime.ToString();
        }
    }
    
}
