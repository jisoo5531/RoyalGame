using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damagable : MonoBehaviour//, ICard//, IDamagable
{
    public string name;
    public int HP;
    public int maxHp;
    public int damage;
    //public int moveSpeed;
    public int range;
    public float detectionRange;
    //public float coolTime;
    public Transform targetTransform;

    //public string Name { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    //public int CardLevel { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    //public int CurrentCardCount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    //public int MaxCardCount { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    //public float Damage { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    //public float Range { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public abstract void GetDamage(int damage);

    public virtual void PrintTest()
    {
        Debug.Log($"¿Ã∏ß : {name}, HP : {HP}");
    }
}
