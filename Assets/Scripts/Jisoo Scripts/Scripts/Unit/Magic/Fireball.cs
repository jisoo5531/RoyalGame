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

    private void Awake()
    {
        // InitializeUnitData(UnitSpawner.instance.selectedUnit);

        clickPos.y = -1;
        SpawnFireBall();
    }

    private void SpawnFireBall()
    {
        Projectile fireballProjectile = gameObject.AddComponent<Projectile>();
        fireballProjectile.targetPos = clickPos;
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
        throw new System.NotImplementedException();
    }
}
