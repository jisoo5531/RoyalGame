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

    private TargetFollowUnit targetFollowUnit;
    //public float distance;

    private void Awake()
    {
        targetFollowUnit = GetComponent<TargetFollowUnit>();
    }

    private void Update()
    {
        target = targetFollowUnit.target;
        //distance = Vector3.Distance(target.position, transform.position);
    }

    public void Attack()
    {
        if (target != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, pStart_trans);
            projectile.layer = gameObject.layer;
            Projectile projectileComponent = projectile.AddComponent<Projectile>();
            projectileComponent.targetPos = target.position;

            Damaging damagingComponent = projectile.AddComponent<Damaging>();
            damagingComponent.target = target;
            damagingComponent.rangeType = Range.Ranged;
            damagingComponent.damage = damage;
            damagingComponent.targetLayerMask = targetLayerMask;
        }
        
    }
}
