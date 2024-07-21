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
    public Image gradeAndTypeBackground;

    public TextMeshProUGUI costText;
    public TextMeshProUGUI cardCountText;

    public Transform unitStats;

    [Space(20)]
    public UnitStat unitStatList;

    private UnitData_SO unitData;

    public void SetInfo(UnitData_SO unitData)
    {
        this.unitData = unitData;

        SetTitle();

        SetUnitImage();

        SetUnitGradePanel();

        SetCardCountFill();

        SetStats();        
    }

    #region Title

    private void SetTitle()
    {
        unitName.text = $"·¹º§ {unitData.unit_Level} {unitData.unitName}".ToString();
    }

    #endregion

    #region Unit

    private void SetUnitImage()
    {
        unitImage.sprite = unitData.iconSprite;
        costText.text = unitData.cost.ToString();
    }

    #region CardCount

    private void SetCardCountFill()
    {
        Collection collection = FindObjectOfType<Collection>();

        collection.SettingUI(unitData, cardCountFill, cardCountText, costText);
        collection.CheckAvailableUpgrade(cardCountFill, upArrow);
    }

    #endregion

    #endregion

    #region Grade / Type

    private void SetUnitGradePanel()
    {
        SetBackground();
        

    }

    private void SetBackground()
    {
        switch (unitData.grade)
        {
            case Grade.Normal:
                gradeAndTypeBackground.ColorNormal();
                break;
            case Grade.Rare:
                gradeAndTypeBackground.ColorRare();
                break;
            case Grade.Epic:
                gradeAndTypeBackground.ColorEpic();
                break;
            default:
                break;
        }
    }

    #endregion

    #region Stat

    public void SetStats()
    {
        foreach (Transform child in unitStats)
        {
            Destroy(child.gameObject);
        }

        Set_HPObj();
        Set_DamageObj();
        Set_AttackSppedObj();
        Set_MoveSppedObj();
        Set_TargetObj();
        Set_RangeObj();
        Set_CreationTimeObj();
    }
    private void Set_HPObj()
    {
        GameObject hpStat_OBJ = Instantiate(unitStatList.hpOBJ, unitStats);
        UI_UpgradeStatText uiHp = hpStat_OBJ.GetComponent<UI_UpgradeStatText>();

        if (uiHp != null)
        {
            uiHp.value.text = unitData.maxHp.ToString();
            uiHp.upgradeValue.text = $"+ {unitData.Get_Upgrade_HP()}";
        }
    }
    private void Set_DamageObj()
    {
        GameObject DamageStat_OBJ = Instantiate(unitStatList.damageOBJ, unitStats);
        UI_UpgradeStatText uiDamage = DamageStat_OBJ.GetComponent<UI_UpgradeStatText>();

        if (uiDamage != null)
        {
            uiDamage.value.text = unitData.damage.ToString();
            uiDamage.upgradeValue.text = $"+ {unitData.Get_Upgrade_Damage()}";
        }
    }
    private void Set_AttackSppedObj()
    {
        GameObject AS_Stat_OBJ = Instantiate(unitStatList.attackSpeedOBJ, unitStats);
        UI_UpgradeStatText ui_AS = AS_Stat_OBJ.GetComponent<UI_UpgradeStatText>();

        if (ui_AS != null)
        {
            ui_AS.value.text = unitData.attackSpeed.ToString();
        }
    }
    
    private void Set_MoveSppedObj()
    {
        GameObject MS_Stat_OBJ = Instantiate(unitStatList.moveSpeedOBJ, unitStats);
        UI_UpgradeStatText ui_MS = MS_Stat_OBJ.GetComponent<UI_UpgradeStatText>();

        if (ui_MS != null)
        {
            ui_MS.value.text = unitData.moveSpeed.ToString();
        }
    }
    
    private void Set_TargetObj()
    {
        GameObject target_Stat_OBJ = Instantiate(unitStatList.targetOBJ, unitStats);
        UI_UpgradeStatText ui_Target = target_Stat_OBJ.GetComponent<UI_UpgradeStatText>();

        if (ui_Target != null)
        {
            ui_Target.value.text = unitData.attackTarget.ToString();
        }
    }
    
    private void Set_RangeObj()
    {
        GameObject range_Stat_OBJ = Instantiate(unitStatList.rangeOBJ, unitStats);
        UI_UpgradeStatText ui_Range = range_Stat_OBJ.GetComponent<UI_UpgradeStatText>();

        if (ui_Range != null)
        {
            ui_Range.value.text = unitData.range.ToString();
        }
    }
    
    private void Set_CreationTimeObj()
    {
        GameObject creationTime_Stat_OBJ = Instantiate(unitStatList.creationTimeOBJ, unitStats);
        UI_UpgradeStatText ui_CreationTIme = creationTime_Stat_OBJ.GetComponent<UI_UpgradeStatText>();

        if (ui_CreationTIme != null)
        {
            ui_CreationTIme.value.text = unitData.spawnTime.ToString();
        }
    }

    #endregion
}
