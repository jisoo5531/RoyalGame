using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowTower : Tower
{
    private void Start()
    {
        maxHP = 2500;
        HP = maxHP;

        onTopUnit.GetComponent<UnitCanvasInfo>().maxHP = maxHP;
        onTopUnit.GetComponent<UnitCanvasInfo>().HP = HP;

    }
}
