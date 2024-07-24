using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedUnit : MonoBehaviourPunCallbacks
{    
    public GameObject projectilePrefab;
    public Transform pStart_trans;
    public LayerMask targetLayerMask;
    public int damage;
    public Transform target;
    private Unit unit;

    private void Start()
    {
        unit = this.transform.root.GetComponent<Unit>();
        damage = unit.damage;
    }
    private void Update()
    {
        target = GetComponent<TargetFollowUnit>().target;
    }

    public void Attack()
    {
        string pfbName = projectilePrefab.name;
        GameObject projectile = PhotonNetwork.Instantiate(pfbName, pStart_trans.position, Quaternion.identity);

        Projectile projectileComponent = projectile.AddComponent<Projectile>();        
        projectileComponent.targetPos = target.position;

        Damaging damagingComponent = projectile.AddComponent<Damaging>();
        damagingComponent.target = target;
        damagingComponent.rangeType = Range.Ranged;
        damagingComponent.damage = damage;
        damagingComponent.targetLayerMask = targetLayerMask;
    }
}
