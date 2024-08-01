using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingTower : Tower
{
    public GameObject cannon;
    public GameObject[] princessTowers;

    bool isOnCannon = false;
    private void Awake()
    {
        maxHP = 10000;
        HP = maxHP;

        //onTopUnit.GetComponent<RoyalEnemyTest>().maxHP = maxHP;
    }

    private void Update()
    {
        if (isOnCannon)
        {
            return;
        }
        foreach (GameObject tower in princessTowers)
        {
            if (tower == null)
            {
                isOnCannon = true;
                //AppearCannon();
            }
        }
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);

        if (false == isOnCannon)
        {
            AppearCannon();
        }
    }

    private void AppearCannon()
    {
        isOnCannon = true;
        cannon.SetActive(true);

        cannon.GetComponent<Animator>().SetTrigger("DoAppear");

    }
}
