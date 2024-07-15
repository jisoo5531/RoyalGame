using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    public float Hp = 100;

    private void Awake()
    {
        Hp = 100;
    }

    public void GetHit(float damage)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
