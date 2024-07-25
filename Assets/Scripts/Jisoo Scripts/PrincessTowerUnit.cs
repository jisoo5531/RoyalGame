using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PrincessTowerUnit : MonoBehaviour
{    
    private RangedUnit ranged;
    private float attackTime = 1.0f;

    private void Awake()
    {        
        ranged = GetComponent<RangedUnit>();
        ranged.damage = 50;
    }

    private void Update()
    {
        attackTime -= Time.deltaTime;
        if (attackTime <= 0f)
        {
            ranged.Attack();
            attackTime = 1f;
        }        
    }
}
