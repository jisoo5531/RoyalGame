using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : MonoBehaviour
{
    EpicToonFX.ETFXProjectileScript magicProjectile;


    public GameObject projectilePrefab;
    public Transform pStart_trans;
    public LayerMask targetLayerMask;
    public int damage;

    private void Awake()
    {
        if (projectilePrefab.TryGetComponent<EpicToonFX.ETFXProjectileScript>(out EpicToonFX.ETFXProjectileScript magic))
        {
            magic.magicTrans = pStart_trans;
        }        
    }

    public void Attack()
    {
        Transform target = GetComponent<TargetFollowUnit>().target;

        GameObject projectile = Instantiate(projectilePrefab, pStart_trans);

        Projectile projectileComponent = projectile.AddComponent<Projectile>();
        projectileComponent.forward = pStart_trans.forward;
        projectileComponent.target = target;

        Damaging damagingComponent = projectile.AddComponent<Damaging>();
        damagingComponent.target = target;
        damagingComponent.rangeType = Range.Ranged;
        damagingComponent.damage = damage;
        damagingComponent.targetLayerMask = targetLayerMask;
    }
}
