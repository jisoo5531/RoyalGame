using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowTower : Tower
{
    private void Start()
    {
        InitData();
        onTopUnit.GetComponent<UnitCanvasInfo>().maxHP = maxHP;
        onTopUnit.GetComponent<UnitCanvasInfo>().HP = HP;

    }

    protected override void InitData()
    {
        maxHP = 2500;
        HP = maxHP;
        range = 26;
        damage = 90;
        isNotOnCannon = false;
    }
}
