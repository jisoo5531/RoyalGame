using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : Attackable
{
    private void Awake()
    {
        InitializeUnitData(UnitSpawner.instance.selectedUnit);
    }
    protected virtual void InitializeUnitData(UnitData_SO unit)
    {
        name = unit.unitName;
        damage = unit.damage;
        range = unit.range;

        //coolTime = unit.spawnTime;
    }

    public override void Damage(int damage)
    {
        throw new System.NotImplementedException();
    }
}
