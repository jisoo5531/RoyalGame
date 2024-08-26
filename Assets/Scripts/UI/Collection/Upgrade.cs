using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using UnityEngine.TextCore.Text;

[System.Serializable]
public class Unittitle
{
    public TextMeshProUGUI unitname;
}
[System.Serializable]
public class Unitimage
{
    public Image unitimage;
    public Image cardcountfill;
    public TextMeshProUGUI costtext;
    public TextMeshProUGUI cardcounttext;
    public TextMeshProUGUI upgradeCost;
    public Image uparrow;
    public Button upgradeBtn;
}
[System.Serializable]
public class Gradeandtype
{
    public Image gradeandtypebackground;
    public TextMeshProUGUI gradetext;
    public TextMeshProUGUI typetext;
}

[System.Serializable]
public class Unitstat
{
    public Transform unitstats;
    public GameObject hpobj;
    public GameObject damageobj;
    public GameObject towerDamageobj;
    public GameObject attackspeedobj;
    public GameObject movespeedobj;
    public GameObject targetobj;
    public GameObject rangeobj;
    public GameObject radiusobj;
    public GameObject creationtimeobj;
    public GameObject lifetimeobj;
}
[System.Serializable]
public class unitdescription
{
    public TextMeshProUGUI desctext;
}

public class Upgrade : MonoBehaviour
{
    public Unittitle unittitle;
    public Unitimage unitlist;
    public Gradeandtype unitgnt;
    public unitdescription unitdesc;
    public Unitstat unitstatlist;

    public AllCardData unitdata;
    public Collection collection;

    private bool availableupgrade = false;
    public int damage;
    public int tower_damage;
    public int hp;
    public int upgradeCost;

    private void UpgradeUnit(bool isUpgrade)
    {
        int cost = (1000 * ((unitdata.level - 1) * 5)) + 1000;
        upgradeCost = cost;
        unitlist.upgradeCost.text = $"¾÷±×·¹ÀÌµå\n{cost}";


        if (isUpgrade && StartManager.m_Instance.gold >= cost)
        {
            unitlist.cardcountfill.ColorGreen();
            unitlist.uparrow.ColorGreen();

            unitlist.upgradeBtn.interactable = true;
        }
        else
        {
            unitlist.cardcountfill.ColorSky();
            unitlist.uparrow.ColorSky();

            unitlist.upgradeBtn.interactable = false;
        }
    }

    public void Setinfo(AllCardData unitdata, bool isCanUpgrade)
    {
        this.unitdata = unitdata;

        if (isCanUpgrade)
        {
            availableupgrade = true;
        }
        else
        {
            availableupgrade = false;
        }

        Settitle();

        Setunitimage();

        Setunitgradeandtype();

        Setdescription();

        Setcardcountfill();

        UpgradeUnit(isCanUpgrade);


        Setstats();
    }

    #region title

    private void Settitle()
    {
        unittitle.unitname.text = $"·¹º§ {unitdata.level}   {unitdata.cardName}";
    }

    #endregion

    #region unit

    private void Setunitimage()
    {
        unitlist.unitimage.sprite = unitdata.img;
        unitlist.costtext.text = unitdata.cost.ToString();
    }
    #endregion

    #region cardcount

    private void Setcardcountfill()
    {
        unitlist.cardcounttext.text = $"{unitdata.currentCardCount} / {unitdata.maxCardCount}";

        unitlist.cardcountfill.fillAmount = (float)unitdata.currentCardCount / unitdata.maxCardCount;
    }
        
    #endregion

    #region grade / type

    private void Setunitgradeandtype()
    {
        Setbackground();
        Set_gnt_text();
    }

    private void Setbackground()
    {
        switch (unitdata.grade)
        {
            case "ÀÏ¹Ý":
                unitgnt.gradeandtypebackground.ColorNormal();
                break;
            case "Èñ±Í":
                unitgnt.gradeandtypebackground.ColorRare();
                break;
            case "¿µ¿õ":
                unitgnt.gradeandtypebackground.ColorEpic();
                break;
            default:
                break;
        }
    }
    private void Set_gnt_text()
    {
        string gradetext = null;
        string typetext = null;
        switch (unitdata.grade)
        {
            case "ÀÏ¹Ý":
                gradetext = "ÀÏ¹Ý";
                break;
            case "Èñ±Í":
                gradetext = "Èñ±Í";
                break;
            case "¿µ¿õ":
                gradetext = "¿µ¿õ";
                break;
            default:
                break;
        }
        switch (unitdata.type)
        {
            case "À¯´Ö":
                typetext = "À¯´Ö";
                break;
            case "¹æ¾îÅ¸¿ö":
                typetext = "°Ç¹°";
                break;
            case "¸¶¹ý":
                typetext = "¸¶¹ý";
                break;
            default:
                break;
        }
        if (gradetext != null)
        {
            unitgnt.gradetext.text = gradetext;
        }
        if (typetext != null)
        {
            unitgnt.typetext.text = typetext;
        }
    }


    #endregion

    #region description

    private void Setdescription()
    {
        unitdesc.desctext.text = unitdata.desc;
    }

    #endregion

    #region stat

    private void Setstats()
    {
        foreach (Transform child in unitstatlist.unitstats)
        {
            Destroy(child.gameObject);
        }

        Set_hpobj();
        Set_damageobj();
        Set_TowerDamageobj();
        Set_attacksppedobj();
        Set_movesppedobj();
        Set_targetobj();
        Set_rangeobj();
        Set_creationtimeobj();
        Set_lifetimeobj();
    }
    private void Set_hpobj()
    {
        if (unitdata.type.Equals("¸¶¹ý"))
        {
            return;
        }

        GameObject hpstat_obj = Instantiate(unitstatlist.hpobj, unitstatlist.unitstats);
        UI_UpgradeStat uihp = hpstat_obj.GetComponent<UI_UpgradeStat>();

        if (uihp != null)
        {
            if (unitdata is UnitInfoData unitInfo)
            {
                uihp.value.text = unitInfo.hp.ToString();
                hp = Mathf.FloorToInt(unitInfo.hp * 0.1f);
            }
            else if (unitdata is DEFENSETOWERInfoData defenseTowerInfo)
            {
                uihp.value.text = defenseTowerInfo.hp.ToString();
                hp = Mathf.FloorToInt(defenseTowerInfo.hp * 0.1f);
            }

            if (availableupgrade)
            {
                uihp.SetUpgrade(hp);
            }
            else
            {
                uihp.SetNotUpgrade();
            }
        }
    }
    private void Set_damageobj()
    {
        GameObject damagestat_obj = Instantiate(unitstatlist.damageobj, unitstatlist.unitstats);
        UI_UpgradeStat uidamage = damagestat_obj.GetComponent<UI_UpgradeStat>();

        if (uidamage != null)
        {
            uidamage.value.text = unitdata.damage.ToString();

            damage = Mathf.FloorToInt(unitdata.damage * 0.1f);
            if (availableupgrade)
            {
                uidamage.SetUpgrade(damage);
            }
            else
            {
                uidamage.SetNotUpgrade();
            }
        }
    }


    private void Set_TowerDamageobj()
    {
        if (unitdata.type.Equals("¸¶¹ý"))
        {
            GameObject damagestat_obj = Instantiate(unitstatlist.towerDamageobj, unitstatlist.unitstats);
            UI_UpgradeStat uidamage = damagestat_obj.GetComponent<UI_UpgradeStat>();

            if (uidamage != null)
            {
                if (unitdata is MAGICInfoData magicInfo)
                {
                    uidamage.value.text = magicInfo.tower_Damage.ToString();
                    tower_damage = Mathf.FloorToInt(magicInfo.tower_Damage * 0.1f);
                }

                if (availableupgrade)
                {
                    uidamage.SetUpgrade(tower_damage);
                }
                else
                {
                    uidamage.SetNotUpgrade();
                }
            }
        }

    }
    private void Set_attacksppedobj()
    {
        if (unitdata.type.Equals("¸¶¹ý"))
        {
            return;
        }

        GameObject as_stat_obj = Instantiate(unitstatlist.attackspeedobj, unitstatlist.unitstats);
        UI_UpgradeStat ui_as = as_stat_obj.GetComponent<UI_UpgradeStat>();

        if (ui_as != null)
        {
            if (unitdata is UnitInfoData unitInfo)
            {
                ui_as.value.text = unitInfo.attackSpeed.ToString();
            }
            else if (unitdata is DEFENSETOWERInfoData defenseTowerInfo)
            {
                ui_as.value.text = defenseTowerInfo.attackSpeed.ToString();
            }
        }
    }

    private void Set_movesppedobj()
    {
        if (unitdata.type.Equals("¸¶¹ý") || unitdata.type.Equals("¹æ¾îÅ¸¿ö"))
        {
            return;
        }

        GameObject ms_stat_obj = Instantiate(unitstatlist.movespeedobj, unitstatlist.unitstats);
        UI_UpgradeStat ui_ms = ms_stat_obj.GetComponent<UI_UpgradeStat>();

        if (ui_ms != null)
        {
            if (unitdata is UnitInfoData unitInfo)
            {
                ui_ms.value.text = unitInfo.moveSpeed.ToString();
            }
        }
    }

    private void Set_targetobj()
    {
        if (unitdata.type.Equals("¸¶¹ý"))
        {
            return;
        }

        GameObject target_stat_obj = Instantiate(unitstatlist.targetobj, unitstatlist.unitstats);
        UI_UpgradeStat ui_target = target_stat_obj.GetComponent<UI_UpgradeStat>();

        if (ui_target != null)
        {
            if (unitdata is UnitInfoData unitInfo)
            {
                ui_target.value.text = unitInfo.target.ToString();
            }
            else if (unitdata is DEFENSETOWERInfoData defenseTowerInfo)
            {
                ui_target.value.text = defenseTowerInfo.target.ToString();
            }
        }
    }

    private void Set_rangeobj()
    {
        if (unitdata.type.Equals("¸¶¹ý"))
        {
            GameObject range_stat_obj = Instantiate(unitstatlist.radiusobj, unitstatlist.unitstats);
            UI_UpgradeStat ui_radius = range_stat_obj.GetComponent<UI_UpgradeStat>();

            if (ui_radius != null)
            {
                ui_radius.value.text = unitdata.range.ToString();
            }
        }
        else
        {
            GameObject range_stat_obj = Instantiate(unitstatlist.rangeobj, unitstatlist.unitstats);
            UI_UpgradeStat ui_range = range_stat_obj.GetComponent<UI_UpgradeStat>();

            if (ui_range != null)
            {
                ui_range.value.text = unitdata.range.ToString();
            }
        }
    }

    private void Set_creationtimeobj()
    {
        if (unitdata.type.Equals("¸¶¹ý"))
        {
            return;
        }

        GameObject creationtime_stat_obj = Instantiate(unitstatlist.creationtimeobj, unitstatlist.unitstats);
        UI_UpgradeStat ui_creationtime = creationtime_stat_obj.GetComponent<UI_UpgradeStat>();

        if (ui_creationtime != null)
        {
            if (unitdata is UnitInfoData unitInfo)
            {
                ui_creationtime.value.text = unitInfo.spawnTime.ToString();
            }
            else if (unitdata is DEFENSETOWERInfoData defenseTowerInfo)
            {
                ui_creationtime.value.text = defenseTowerInfo.spawnTime.ToString();
            }
        }
    }
    private void Set_lifetimeobj()
    {
        if (unitdata.type.Equals("À¯´Ö") || unitdata.type.Equals("¸¶¹ý"))
        {
            return;
        }
        GameObject lifetime_stat_obj = Instantiate(unitstatlist.lifetimeobj, unitstatlist.unitstats);
        UI_UpgradeStat ui_lifetime = lifetime_stat_obj.GetComponent<UI_UpgradeStat>();

        if (ui_lifetime != null)
        {
            if (unitdata is DEFENSETOWERInfoData defenseTowerInfo)
            {
                ui_lifetime.value.text = defenseTowerInfo.lifeTime.ToString();
            }
        }
    }

    #endregion
}
