using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : MonoBehaviour
{    
    public GameObject projectilePrefab;
    public Transform pStart_trans;
    public LayerMask targetLayerMask;
    public int damage;
    public Transform target;

    private void Update()
    {
        target = GetComponent<TargetFollowUnit>().target;
    }

    public void Attack()
    {        
        GameObject projectile = Instantiate(projectilePrefab, pStart_trans);

        Projectile projectileComponent = projectile.AddComponent<Projectile>();        
        projectileComponent.targetPos = target.position;

        Damaging damagingComponent = projectile.AddComponent<Damaging>();
        damagingComponent.target = target;
        damagingComponent.rangeType = Range.Ranged;
        damagingComponent.damage = damage;
        damagingComponent.targetLayerMask = targetLayerMask;
    }
}
