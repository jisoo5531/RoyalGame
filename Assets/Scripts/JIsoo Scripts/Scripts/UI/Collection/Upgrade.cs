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
public class Upgrade : MonoBehaviour
{            
    public UnitTitle unitTitle;
    public UnitImage unitList;
    public GradeAndType unitGnT;
    public UnitStat unitStatList;

    private UnitData_SO unitData;

    private bool availableUpgrade = false;

    

    public void SetInfo(UnitData_SO unitData)
    {
        this.unitData = unitData;

        SetTitle();

        SetUnitImage();

        SetUnitGradeAndType();

        SetCardCountFill();

        SetStats();        
    }

    #region Title

    private void SetTitle()
    {
        unitTitle.unitName.text = $"·¹º§ {unitData.unit_Level} {unitData.unitName}".ToString();
    }

    #endregion

    #region Unit

    private void SetUnitImage()
    {
        unitList.unitImage.sprite = unitData.iconSprite;
        unitList.costText.text = unitData.cost.ToString();
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
        switch (unitData.type)
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

    #region Stat

    public void SetStats()
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
        if (unitData.type == Type.Magic)
        {
            return;
        }

        GameObject hpStat_OBJ = Instantiate(unitStatList.hpOBJ, unitStatList.unitStats);
        UI_UpgradeStat uiHp = hpStat_OBJ.GetComponent<UI_UpgradeStat>();

        if (uiHp != null)
        {
            uiHp.value.text = unitData.maxHp.ToString();
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
            uiDamage.value.text = unitData.damage.ToString();
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
        if (unitData.type == Type.Magic)
        {
            return;
        }

        GameObject AS_Stat_OBJ = Instantiate(unitStatList.attackSpeedOBJ, unitStatList.unitStats);
        UI_UpgradeStat ui_AS = AS_Stat_OBJ.GetComponent<UI_UpgradeStat>();

        if (ui_AS != null)
        {
            ui_AS.value.text = unitData.attackSpeed.ToString();
        }
    }
    
    private void Set_MoveSppedObj()
    {
        if (unitData.type == Type.Magic)
        {
            return;
        }

        GameObject MS_Stat_OBJ = Instantiate(unitStatList.moveSpeedOBJ, unitStatList.unitStats);
        UI_UpgradeStat ui_MS = MS_Stat_OBJ.GetComponent<UI_UpgradeStat>();

        if (ui_MS != null)
        {
            ui_MS.value.text = unitData.moveSpeed.ToString();
        }
    }
    
    private void Set_TargetObj()
    {
        if (unitData.type == Type.Magic)
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
        if (unitData.type == Type.Magic)
        {
            return;
        }

        GameObject range_Stat_OBJ = Instantiate(unitStatList.rangeOBJ, unitStatList.unitStats);
        UI_UpgradeStat ui_Range = range_Stat_OBJ.GetComponent<UI_UpgradeStat>();

        if (ui_Range != null)
        {
            ui_Range.value.text = unitData.range.ToString();
        }
    }
    
    private void Set_CreationTimeObj()
    {
        GameObject creationTime_Stat_OBJ = Instantiate(unitStatList.creationTimeOBJ, unitStatList.unitStats);
        UI_UpgradeStat ui_CreationTIme = creationTime_Stat_OBJ.GetComponent<UI_UpgradeStat>();

        if (ui_CreationTIme != null)
        {
            ui_CreationTIme.value.text = unitData.spawnTime.ToString();
        }
    }
    private void Set_LifeTimeObj()
    {
        if (unitData.type == Type.Unit || unitData.type == Type.Magic)
        {
            return;
        }
        GameObject LifeTime_Stat_OBJ = Instantiate(unitStatList.LifeTimeOBJ, unitStatList.unitStats);
        UI_UpgradeStat ui_LifeTIme = LifeTime_Stat_OBJ.GetComponent<UI_UpgradeStat>();

        if (ui_LifeTIme != null)
        {
            ui_LifeTIme.value.text = unitData.lifeTime.ToString();
        }
    }

    #endregion
}
