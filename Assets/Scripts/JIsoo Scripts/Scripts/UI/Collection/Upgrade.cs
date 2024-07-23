//using system.collections;
//using system.collections.generic;
//using unityengine;
//using unityengine.ui;
//using tmpro;

//[system.serializable]
//public class unittitle
//{
//    public textmeshprougui unitname;
//}
//[system.serializable]
//public class unitimage
//{
//    public image unitimage;
//    public image cardcountfill;
//    public textmeshprougui costtext;
//    public textmeshprougui cardcounttext;
//    public image uparrow;
//}
//[system.serializable]
//public class gradeandtype
//{
//    public image gradeandtypebackground;
//    public textmeshprougui gradetext;
//    public textmeshprougui typetext;
//}

//[system.serializable]
//public class unitstat
//{
//    public transform unitstats;
//    public gameobject hpobj;
//    public gameobject damageobj;
//    public gameobject attackspeedobj;
//    public gameobject movespeedobj;
//    public gameobject targetobj;
//    public gameobject rangeobj;
//    public gameobject creationtimeobj;
//    public gameobject lifetimeobj;
//}
//[system.serializable]
//public class unitdescription
//{
//    public textmeshprougui desctext;
//}

//public class upgrade : monobehaviour
//{
//    public unittitle unittitle;
//    public unitimage unitlist;
//    public gradeandtype unitgnt;
//    public unitdescription unitdesc;
//    public unitstat unitstatlist;

//    private characterdata unitdata;

//    private bool availableupgrade = false;



//    public void setinfo(characterdata unitdata)
//    {
//        this.unitdata = unitdata;

//        settitle();

//        setunitimage();

//        setunitgradeandtype();

//        setdescription();

//        setcardcountfill();

//        setstats();
//    }

//    #region title

//    private void settitle()
//    {
//        unittitle.unitname.text = $"·¹º§ {unitdata.unit_level} {unitdata.unitname}".tostring();
//    }

//    #endregion

//    #region unit

//    private void setunitimage()
//    {
//        unitlist.unitimage.sprite = unitdata.img;
//        unitlist.costtext.text = unitdata.cost.tostring();
//    }

//    #region cardcount

//    private void setcardcountfill()
//    {
//        collection collection = findobjectoftype<collection>();

//        //collection.settingui(unitdata, unitlist.cardcountfill, unitlist.cardcounttext, unitlist.costtext);

//        if (true == collection.checkavailableupgrade(unitlist.cardcountfill, unitlist.uparrow))
//        {
//            availableupgrade = true;
//        }
//        else
//        {
//            availableupgrade = false;
//        }
//    }

//    #endregion

//    #endregion

//    #region grade / type

//    private void setunitgradeandtype()
//    {
//        setbackground();
//        set_gnt_text();
//    }

//    private void setbackground()
//    {
//        switch (unitdata.grade)
//        {
//            case "ÀÏ¹Ý":
//                unitgnt.gradeandtypebackground.colornormal();
//                break;
//            case "Èñ±Í":
//                unitgnt.gradeandtypebackground.colorrare();
//                break;
//            case "¿µ¿õ":
//                unitgnt.gradeandtypebackground.colorepic();
//                break;
//            default:
//                break;
//        }
//    }
//    private void set_gnt_text()
//    {
//        string gradetext = null;
//        string typetext = null;
//        switch (unitdata.grade)
//        {
//            case "ÀÏ¹Ý":
//                gradetext = "ÀÏ¹Ý";
//                break;
//            case "Èñ±Í":
//                gradetext = "Èñ±Í";
//                break;
//            case "¿µ¿õ":
//                gradetext = "¿µ¿õ";
//                break;
//            default:
//                break;
//        }
//        switch (unitdata.type)
//        {
//            case type.unit:
//                typetext = "À¯´Ö";
//                break;
//            case type.deffense:
//                typetext = "°Ç¹°";
//                break;
//            case type.magic:
//                typetext = "¸¶¹ý";
//                break;
//            default:
//                break;
//        }
//        if (gradetext != null)
//        {
//            unitgnt.gradetext.text = gradetext;
//        }
//        if (typetext != null)
//        {
//            unitgnt.typetext.text = typetext;
//        }
//    }


//    #endregion

//    #region description

//    private void setdescription()
//    {
//        unitdesc.desctext.text = unitdata.unit_desc;
//    }

//    #endregion

//    #region stat

//    private void setstats()
//    {
//        foreach (transform child in unitstatlist.unitstats)
//        {
//            destroy(child.gameobject);
//        }

//        set_hpobj();
//        set_damageobj();
//        set_attacksppedobj();
//        set_movesppedobj();
//        set_targetobj();
//        set_rangeobj();
//        set_creationtimeobj();
//        set_lifetimeobj();
//    }
//    private void set_hpobj()
//    {
//        if (unitdata.type == type.magic)
//        {
//            return;
//        }

//        gameobject hpstat_obj = instantiate(unitstatlist.hpobj, unitstatlist.unitstats);
//        ui_upgradestat uihp = hpstat_obj.getcomponent<ui_upgradestat>();

//        if (uihp != null)
//        {
//            uihp.value.text = unitdata.maxhp.tostring();
//            uihp.upgradevalue.text = $"+ {unitdata.get_upgrade_hp()}";

//            if (availableupgrade)
//            {
//                uihp.setupgrade();
//            }
//            else
//            {
//                uihp.setnotupgrade();
//            }
//        }
//    }
//    private void set_damageobj()
//    {
//        gameobject damagestat_obj = instantiate(unitstatlist.damageobj, unitstatlist.unitstats);
//        ui_upgradestat uidamage = damagestat_obj.getcomponent<ui_upgradestat>();

//        if (uidamage != null)
//        {
//            uidamage.value.text = unitdata.damage.tostring();
//            uidamage.upgradevalue.text = $"+ {unitdata.get_upgrade_damage()}";

//            if (availableupgrade)
//            {
//                uidamage.setupgrade();
//            }
//            else
//            {
//                uidamage.setnotupgrade();
//            }
//        }
//    }
//    private void set_attacksppedobj()
//    {
//        if (unitdata.type == type.magic)
//        {
//            return;
//        }

//        gameobject as_stat_obj = instantiate(unitstatlist.attackspeedobj, unitstatlist.unitstats);
//        ui_upgradestat ui_as = as_stat_obj.getcomponent<ui_upgradestat>();

//        if (ui_as != null)
//        {
//            ui_as.value.text = unitdata.attackspeed.tostring();
//        }
//    }

//    private void set_movesppedobj()
//    {
//        if (unitdata.type == type.magic)
//        {
//            return;
//        }

//        gameobject ms_stat_obj = instantiate(unitstatlist.movespeedobj, unitstatlist.unitstats);
//        ui_upgradestat ui_ms = ms_stat_obj.getcomponent<ui_upgradestat>();

//        if (ui_ms != null)
//        {
//            ui_ms.value.text = unitdata.movespeed.tostring();
//        }
//    }

//    private void set_targetobj()
//    {
//        if (unitdata.type == type.magic)
//        {
//            return;
//        }

//        gameobject target_stat_obj = instantiate(unitstatlist.targetobj, unitstatlist.unitstats);
//        ui_upgradestat ui_target = target_stat_obj.getcomponent<ui_upgradestat>();

//        if (ui_target != null)
//        {
//            ui_target.value.text = unitdata.attacktarget.tostring();
//        }
//    }

//    private void set_rangeobj()
//    {
//        if (unitdata.type == type.magic)
//        {
//            return;
//        }

//        gameobject range_stat_obj = instantiate(unitstatlist.rangeobj, unitstatlist.unitstats);
//        ui_upgradestat ui_range = range_stat_obj.getcomponent<ui_upgradestat>();

//        if (ui_range != null)
//        {
//            ui_range.value.text = unitdata.range.tostring();
//        }
//    }

//    private void set_creationtimeobj()
//    {
//        gameobject creationtime_stat_obj = instantiate(unitstatlist.creationtimeobj, unitstatlist.unitstats);
//        ui_upgradestat ui_creationtime = creationtime_stat_obj.getcomponent<ui_upgradestat>();

//        if (ui_creationtime != null)
//        {
//            ui_creationtime.value.text = unitdata.spawntime.tostring();
//        }
//    }
//    private void set_lifetimeobj()
//    {
//        if (unitdata.type == type.unit || unitdata.type == type.magic)
//        {
//            return;
//        }
//        gameobject lifetime_stat_obj = instantiate(unitstatlist.lifetimeobj, unitstatlist.unitstats);
//        ui_upgradestat ui_lifetime = lifetime_stat_obj.getcomponent<ui_upgradestat>();

//        if (ui_lifetime != null)
//        {
//            ui_lifetime.value.text = unitdata.lifetime.tostring();
//        }
//    }

//    #endregion
//}
