using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour, ICard, IAttackable
{
    public int cardLevel { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int currentCardCount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int maxCardCount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int damage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float range { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public int cost { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float spawnTime { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float attackSpeed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Awake()
    {
        InitializeUnitData(UnitSpawner.instance.selectedUnit);
    }
    protected virtual void InitializeUnitData(UnitData unit)
    {
        name = unit.unitInfo.unitName;
        damage = unit.unitInfo.unitStat.damage;
        range = unit.unitInfo.unitRange.range;

        //coolTime = unit.spawnTime;
    }

    public void GetDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    public void SendDamage(int damage)
    {
        throw new System.NotImplementedException();
    }
}
