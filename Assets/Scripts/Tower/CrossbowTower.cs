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
            GetComponent<UnitCanvasInfo>().HP = HP;
            GetComponent<UnitCanvasInfo>().maxHP = maxHP;
            deffenseUnit.InitData(range, damage);
        }
    }

    protected override void InitData()
    {
        maxHP = 2000;
        HP = maxHP;
        range = 33;
        damage = 60;
        isNotOnCannon = false;
    }
}
    