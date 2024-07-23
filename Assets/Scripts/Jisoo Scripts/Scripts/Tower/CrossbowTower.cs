using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowTower : Tower
{
    private void Start()
    {
        maxHP = 5000;
        HP = maxHP;

        onTopUnit.GetComponent<RoyalEnemyTest>().maxHP = maxHP;
        onTopUnit.GetComponent<RoyalEnemyTest>().HP = HP;

    }
}
