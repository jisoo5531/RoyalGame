using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damagable : MonoBehaviour, IDamagable
{
    public string name;
    public int HP;
    public int maxHp;
    public int damage;
    public int moveSpeed;
    public int range;
    public float detectionRange;
    //public float coolTime;
    public Transform targetTransform;


    public virtual void GetDamage(int _damage)
    {
        HP -= _damage;
        PrintTest();
    }

    public virtual void PrintTest()
    {
        Debug.Log($"¿Ã∏ß : {name}, HP : {HP}");
    }
}
