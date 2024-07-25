using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTest : MonoBehaviour, ICard, IAttackable
{

    public int cardLevel { get; set; }
    public int currentCardCount { get; set; }
    public int maxCardCount { get; set; }
    public int damage { get; set; }
    public float range { get; set; }
    public int cost { get; set; }
    public float spawnTime { get; set; }
    public float attackSpeed { get; set; }

    public UnitData_SO unitData;
    public SphereCollider sphereCollider;

    private void Awake()
    {
        // InitializeUnitData(UnitSpawner.instance.selectedUnit);

        InitializeUnitData(unitData);
        SendDamage(damage);
    }


    private void InitializeUnitData(UnitData_SO unit)
    {
        name = unit.unitName;
        damage = unit.damage;
        range = unit.range;

        //coolTime = unit.spawnTime;
    }


    public void SendDamage(int damage)
    {
        GetComponent<Damaging>().damage = damage;
    }

    public void OnCollider()
    {
        ExplosionParticle();

        Debug.Log("µÆ³ª");
        sphereCollider.enabled = true;
        Debug.Log(sphereCollider.enabled == true);
        //sphereCollider.gameObject.SetActive(true);
        Destroy(gameObject, 0.5f);
    }
    private void ExplosionParticle()
    {
        ClickMagic clickMagic = FindAnyObjectByType<ClickMagic>();
        Vector3 position = clickMagic.clickPos;
        GameObject ex = Instantiate(clickMagic.explosion, position, clickMagic.explosion.transform.rotation);
        Destroy(ex, 1f);
    }
}
