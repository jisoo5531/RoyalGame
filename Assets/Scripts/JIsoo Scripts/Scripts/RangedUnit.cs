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

    private TargetFollowUnit targetFollowUnit;

    private void Start()
    {
        targetFollowUnit = GetComponent<TargetFollowUnit>();
    }

    private void Update()
    {
        if(targetFollowUnit.target != null)
        {
            target = targetFollowUnit.target;
        }
    }

    public void Attack()
    {
        string pfbName = projectilePrefab.name;
        GameObject projectile = PhotonNetwork.Instantiate(pfbName, pStart_trans.position, Quaternion.Euler(-90, 0, transform.localEulerAngles.y));
        projectile.GetComponent<Projectile>().targetPos = target.position;

        Damaging damagingComponent = projectile.GetComponent<Damaging>();
        damagingComponent.target = target;
        damagingComponent.rangeType = Range.Ranged;
        damagingComponent.damage = damage;
    }
}
