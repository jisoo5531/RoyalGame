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
        GameObject projectile = Instantiate(projectilePrefab, projectile_trans);
        projectile.AddComponent<Projectile>().forward = projectile_trans.forward;

        Damaging damagingComponent = projectile.AddComponent<Damaging>();
        damagingComponent.rangeType = Range.Ranged;
        damagingComponent.damage = damage;
        damagingComponent.targetLayerMask = targetLayerMask;
    }
}
