using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingCannon : MonoBehaviour
{
    TargetFollowUnit followUnit;
    public Transform target;        

    public float attackSpeed = 2f;

    private RangedUnit rangedUnit;

    private void Awake()
    {
        followUnit = GetComponent<TargetFollowUnit>();
        rangedUnit = GetComponent<RangedUnit>();
    }

    private void Update()
    {
        target = followUnit.target;
        rangedUnit.target = target;

        Debug.Log(target == null);

        Vector3 dir = (target.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = lookRotation;

        Attack();
    }
    public void Attack()
    {
        attackSpeed -= Time.deltaTime;
        if (attackSpeed <= 0)
        {
            rangedUnit.Attack();
            attackSpeed = 2f;
        }
    }
}
