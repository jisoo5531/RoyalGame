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
            GameObject effect = Instantiate(blastEffect, transform.position + new Vector3(0, transform.localScale.y, 0), transform.rotation);
            effect.transform.localScale = transform.localScale;
            Destroy(gameObject);
        }
    }
}
