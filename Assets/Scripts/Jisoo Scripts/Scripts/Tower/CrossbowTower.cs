using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowTower : Tower
{
    private void Start()
    {
        if (photonView.IsMine)
        {
            InitData();
            deffenseUnit = GetComponentInChildren<DeffenseUnit>();
            onTopUnit.GetComponent<UnitCanvasInfo>().HP = HP;
            onTopUnit.GetComponent<UnitCanvasInfo>().maxHP = maxHP;
            deffenseUnit.InitData(range, damage);
        }
    }

    protected override void InitData()
    {
        maxHP = 300;
        HP = maxHP;
        range = 37;
        damage = 60;
        isNotOnCannon = false;
    }
}
    