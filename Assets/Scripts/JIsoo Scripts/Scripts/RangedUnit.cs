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

    private void Update()
    {
        target = GetComponent<TargetFollowUnit>().target;
    }

    public void Attack()
    {
        //string pfbName = projectilePrefab.name;
        //GameObject projectile = PhotonNetwork.Instantiate(pfbName, pStart_trans.position, Quaternion.identity);
        //Debug.Log(projectile == null);
        //Projectile projectileComponent = projectile.AddComponent<Projectile>();
        //projectileComponent.targetPos = target.position;

        //Damaging damagingComponent = projectile.AddComponent<Damaging>();
        //damagingComponent.target = target;
        //damagingComponent.rangeType = Range.Ranged;
        //damagingComponent.damage = damage;
        //damagingComponent.targetLayerMask = targetLayerMask;
    }
}
