using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UnitTitle
{
    public TextMeshProUGUI unitName;
}
[System.Serializable]
public class UnitImage
{
    public Image unitImage;
    public Image cardCountFill;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI cardCountText;
    public Image upArrow;
}
[System.Serializable]
public class GradeAndType
{
    public Image gradeAndTypeBackground;
    public TextMeshProUGUI gradeText;
    public TextMeshProUGUI typeText;
}

[System.Serializable]
public class UnitStat
{
    public Transform unitStats;
    public GameObject hpOBJ;
    public GameObject damageOBJ;
    public GameObject attackSpeedOBJ;
    public GameObject moveSpeedOBJ;
    public GameObject targetOBJ;
    public GameObject rangeOBJ;
    public GameObject creationTimeOBJ;
    public GameObject LifeTimeOBJ;
}
[System.Serializable]
public class UnitDescription
{
    public TextMeshProUGUI descText;
}
[System.Serializable]
public class UpgradeButton
{
    public Image background;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI upgradeCost;
    public Image coinImage;
}

public class Upgrade : MonoBehaviour
{            
    public UnitTitle unitTitle;
    public UnitImage unitList;
    public GradeAndType unitGnT;
    public UnitDescription unitDesc;
    public UnitStat unitStatList;
    public UpgradeButton upgradeButton;


    private UnitData unitData;

    private bool availableUpgrade = false;

    

    public void SetInfo(UnitData unitData)
    {
        this.unitData = unitData;

        SetTitle();

        SetUnitImage();

        SetUnitGradeAndType();

        SetDescription();

        SetCardCountFill();

        SetStats();

        SetUpgradeButton();
    }

    #region Title

    private void SetTitle()
    {
        unitTitle.unitName.text = $"·¹º§ {unitData.cardInfo.unit_Level} {unitData.unitInfo.unitName}".ToString();
    }

    #endregion

    #region Unit

    private void SetUnitImage()
    {
        unitList.unitImage.sprite = unitData.iconSprite;
        unitList.costText.text = unitData.unitInfo.unitStat.cost.ToString();
    }

    #region CardCount

    private void SetCardCountFill()
    {
        Collection collection = FindObjectOfType<Collection>();

        collection.SettingUI(unitData, unitList.cardCountFill, unitList.cardCountText, unitList.costText);

        if (true == collection.CheckAvailableUpgrade(unitList.cardCountFill, unitList.upArrow))
        {
            availableUpgrade = true;            
        }      
        else
        {
            availableUpgrade = false;
        }
    }

    #endregion

    #endregion

    #region Grade / Type

    private void SetUnitGradeAndType()
    {
        SetBackground();
        Set_GnT_Text();
    }

    private void SetBackground()
    {
        switch (unitData.grade)
        {
            case Grade.Normal:
                unitGnT.gradeAndTypeBackground.ColorNormal();
                break;
            case Grade.Rare:
                unitGnT.gradeAndTypeBackground.ColorRare();
                break;
            case Grade.Epic:
                unitGnT.gradeAndTypeBackground.ColorEpic();
                break;
            default:
                break;
        }
    }
    private void Set_GnT_Text()
    {
        string gradeText = null;
        string typeText = null;
        switch (unitData.grade)
        {
            case Grade.Normal:
                gradeText = "ÀÏ¹Ý";
                break;
            case Grade.Rare:
                gradeText = "Èñ±Í";
                break;
            case Grade.Epic:
                gradeText = "¿µ¿õ";
                break;
            default:
                break;
        }
        switch (unitData.unitInfo.type)
        {
            case Type.Unit:
                typeText = "À¯´Ö";
                break;
            case Type.Deffense:
                typeText = "°Ç¹°";
                break;
            case Type.Magic:
                typeText = "¸¶¹ý";
                break;
            default:
                break;
        }
        if (gradeText != null)
        {
            unitGnT.gradeText.text = gradeText;
        }
        if (typeText != null)
        {
            unitGnT.typeText.text = typeText;
        }        
    }


    #endregion

    #region Description

    private void SetDescription()
    {
        unitDesc.descText.text = unitData.unitInfo.unit_Desc;
    }

    #endregion

    #region Stat

    private void SetStats()
    {
        foreach (Transform child in unitStatList.unitStats)
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
        Set_LifeTimeObj();
    }
    private void Set_HPObj()
    {
        if (unitData.unitInfo.type == Type.Magic)
        {
            return;
        }

        GameObject hpStat_OBJ = Instantiate(unitStatList.hpOBJ, unitStatList.unitStats);
        UI_UpgradeStat uiHp = hpStat_OBJ.GetComponent<UI_UpgradeStat>();

        if (uiHp != null)
        {
            uiHp.value.text = unitData.unitInfo.unitStat.maxHp.ToString();
            uiHp.upgradeValue.text = $"+ {unitData.Get_Upgrade_HP()}";

            if (availableUpgrade)
            {
                uiHp.SetUpgrade();
            }
            else
            {
                uiHp.SetNotUpgrade();
            }
        }
    }
    private void Set_DamageObj()
    {
        GameObject DamageStat_OBJ = Instantiate(unitStatList.damageOBJ, unitStatList.unitStats);
        UI_UpgradeStat uiDamage = DamageStat_OBJ.GetComponent<UI_UpgradeStat>();

        if (uiDamage != null)
        {
            uiDamage.value.text = unitData.unitInfo.unitStat.damage.ToString();
            uiDamage.upgradeValue.text = $"+ {unitData.Get_Upgrade_Damage()}";

            if (availableUpgrade)
            {
                uiDamage.SetUpgrade();
            }
            else
            {
                uiDamage.SetNotUpgrade();
            }
        }
    }
    private void Set_AttackSppedObj()
    {
        if (unitData.unitInfo.type == Type.Magic)
        {
            return;
        }

        GameObject AS_Stat_OBJ = Instantiate(unitStatList.attackSpeedOBJ, unitStatList.unitStats);
        UI_UpgradeStat ui_AS = AS_Stat_OBJ.GetComponent<UI_UpgradeStat>();

        if (ui_AS != null)
        {
            ui_AS.value.text = unitData.unitInfo.unitStat.attackSpeed.ToString();
        }
    }
    
    private void Set_MoveSppedObj()
    {
        if (unitData.unitInfo.type == Type.Magic)
        {
            return;
        }

        GameObject MS_Stat_OBJ = Instantiate(unitStatList.moveSpeedOBJ, unitStatList.unitStats);
        UI_UpgradeStat ui_MS = MS_Stat_OBJ.GetComponent<UI_UpgradeStat>();

        if (ui_MS != null)
        {
            ui_MS.value.text = unitData.unitInfo.unitStat.moveSpeed.ToString();
        }
    }
    
    private void Set_TargetObj()
    {
        if (unitData.unitInfo.type == Type.Magic)
        {
            return;
        }

        GameObject target_Stat_OBJ = Instantiate(unitStatList.targetOBJ, unitStatList.unitStats);
        UI_UpgradeStat ui_Target = target_Stat_OBJ.GetComponent<UI_UpgradeStat>();

        if (ui_Target != null)
        {
            ui_Target.value.text = unitData.attackTarget.ToString();
        }
    }
    
    private void Set_RangeObj()
    {
        if (unitData.unitInfo.type == Type.Magic)
        {
            return;
        }

        GameObject range_Stat_OBJ = Instantiate(unitStatList.rangeOBJ, unitStatList.unitStats);
        UI_UpgradeStat ui_Range = range_Stat_OBJ.GetComponent<UI_UpgradeStat>();

        if (ui_Range != null)
        {
            ui_Range.value.text = unitData.unitInfo.unitRange.range.ToString();
        }
    }
    
    private void Set_CreationTimeObj()
    {
        GameObject creationTime_Stat_OBJ = Instantiate(unitStatList.creationTimeOBJ, unitStatList.unitStats);
        UI_UpgradeStat ui_CreationTIme = creationTime_Stat_OBJ.GetComponent<UI_UpgradeStat>();

        if (ui_CreationTIme != null)
        {
            ui_CreationTIme.value.text = unitData.unitInfo.unitStat.spawnTime.ToString();
        }
    }
    private void Set_LifeTimeObj()
    {
        if (unitData.unitInfo.type == Type.Unit || unitData.unitInfo.type == Type.Magic)
        {
            return;
        }
        GameObject LifeTime_Stat_OBJ = Instantiate(unitStatList.LifeTimeOBJ, unitStatList.unitStats);
        UI_UpgradeStat ui_LifeTIme = LifeTime_Stat_OBJ.GetComponent<UI_UpgradeStat>();

        if (ui_LifeTIme != null)
        {
            ui_LifeTIme.value.text = unitData.unitInfo.unitStat.lifeTime.ToString();
        }
    }

    #endregion

    private void SetUpgradeButton()
    {
        if (availableUpgrade)
        {
            upgradeButton.background.ColorGreen();
            upgradeButton.upgradeText.ColorWhite();
            upgradeButton.upgradeCost.ColorWhite();
            upgradeButton.coinImage.ColorWhite();
        }
        else
        {
            upgradeButton.background.ColorNormal();
            upgradeButton.upgradeText.ColorNormal();
            upgradeButton.upgradeCost.ColorNormal();
            upgradeButton.coinImage.ColorNormal();
        }
    }
}
