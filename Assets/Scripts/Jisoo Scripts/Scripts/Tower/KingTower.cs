using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingTower : Tower
{
    private void Awake()
    {
        maxHP = 10000;
        HP = maxHP;

        onTopUnit.GetComponent<RoyalEnemyTest>().maxHP = maxHP;
    }
}
