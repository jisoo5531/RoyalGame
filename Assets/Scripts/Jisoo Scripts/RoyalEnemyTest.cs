using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoyalEnemyTest : MonoBehaviour, IDamagable
{
    public int HP { get; set; }
    public int maxHP { get; set; }
    public GameObject blastEffect;

    private void Awake()
    {
        maxHP = 100;
        HP = maxHP;
    }

    public void GetDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Destroy(gameObject);
            GameObject effect = Instantiate(blastEffect, transform.position, transform.rotation);
            effect.transform.localScale = transform.localScale;
        }
    }
}
