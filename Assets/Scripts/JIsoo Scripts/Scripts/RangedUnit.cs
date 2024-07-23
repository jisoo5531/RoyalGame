using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectile_trans;
    public LayerMask targetLayerMask;
    public int damage;
    

    public void Attack()
    {
        Transform target = GetComponent<TargetFollowUnit>().target;

        GameObject projectile = Instantiate(projectilePrefab, projectile_trans);
        Projectile projectileComponent = projectile.AddComponent<Projectile>();
        projectileComponent.forward = projectile_trans.forward;
        projectileComponent.target = target;

        Damaging damagingComponent = projectile.AddComponent<Damaging>();
        damagingComponent.target = target;
        damagingComponent.rangeType = Range.Ranged;
        damagingComponent.damage = damage;
        damagingComponent.targetLayerMask = targetLayerMask;
    }
}
