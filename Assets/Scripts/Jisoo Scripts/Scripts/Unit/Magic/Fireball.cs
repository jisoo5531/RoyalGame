using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour, ICard, IAttackable
{
    public int cardLevel { get; set; }
    public int currentCardCount { get; set; }
    public int maxCardCount { get; set; }
    public int damage { get; set; }
    public float range { get; set; }
    public int cost { get; set; }
    public float spawnTime { get; set; }
    public float attackSpeed { get; set; }

    public Vector3 clickPos;
    private ClickMagic fireBall;

    private void Awake()
    {
        fireBall = FindObjectOfType<ClickMagic>();
        // InitializeUnitData(UnitSpawner.instance.selectedUnit);
        SendDamage(damage);
        
    }
    private void Start()
    {
        clickPos.y = -1;
        SpawnFireBall();
    }

    private void SpawnFireBall()
    {
        fireBall.SpawnFireBall(gameObject);
    }

    protected virtual void InitializeUnitData(UnitData_SO unit)
    {
        name = unit.unitName;
        damage = unit.damage;
        range = unit.range;

        //coolTime = unit.spawnTime;
    }


    public void SendDamage(int damage)
    {
        //GetComponent<Damaging>().damage = damage;
    }
}
