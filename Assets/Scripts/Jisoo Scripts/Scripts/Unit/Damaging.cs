using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaging : MonoBehaviourPunCallbacks
{
    public int damage;

    public Transform target;
    public LayerMask targetLayerMask;
    public Range rangeType;
    public Type unitType;
    private Unit unit;
    PhotonView pv;
    private Dictionary<int, IDamagable> damagedTargetsCache = new Dictionary<int, IDamagable>();

    private void Start()
    {
        pv = gameObject.GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pv != null && pv.IsMine)
        {
            if ((targetLayerMask | (1 << other.gameObject.layer)) != targetLayerMask)
            {
                if (rangeType == Range.Ranged)
                {
                    pv.RPC("DestoryGob", RpcTarget.All);
                }
                return;
            }

            if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                PhotonView targetPV = other.transform.root.GetComponent<PhotonView>();
                int targetViewID = targetPV.ViewID;

                if (!damagedTargetsCache.ContainsKey(targetViewID))
                {
                    damagedTargetsCache[targetViewID] = damagable;
                }

                if (unitType == Type.Magic)
                {
                    damagable.GetDamage(damage);

                    pv.RPC("DestoryGob", RpcTarget.All);

                    return;
                }

                if (rangeType == Range.Ranged)
                {
                    if (other.gameObject.name.Equals(target.gameObject.name))
                    {
                        damagable.GetDamage(damage);
                        pv.RPC("DestoryGob", RpcTarget.All);
                    }

                    return;
                }
                
                if (damagedTargetsCache.TryGetValue(targetViewID, out IDamagable cachedDamagable))
                {
                    pv.RPC("RPC_SendDamage", RpcTarget.Others, damage, targetViewID);
                }
            }
        }
    }
    [PunRPC]
    public void RPC_SendDamage(int damage, int targetViewID)
    {
        if (damagedTargetsCache.TryGetValue(targetViewID, out IDamagable targetDamagable))
        {
            targetDamagable?.GetDamage(damage);
        }
        else
        {
            PhotonView targetPV = PhotonView.Find(targetViewID);
            if (targetPV != null)
            {
                targetDamagable = targetPV.GetComponent<IDamagable>();
                if (targetDamagable != null)
                {
                    damagedTargetsCache[targetViewID] = targetDamagable;
                    targetDamagable.GetDamage(damage);
                }
            }
        }
    }

    [PunRPC]
    private void DestoryGob()
    {
        Destroy(gameObject);
    }

    //[PunRPC]
    //private void RPC_SendDamage(int damage)
    //{
    //    if (TryGetComponent<IDamagable>(out IDamagable damagable))
    //    {
    //        damagable.GetDamage(damage);
    //    }
    //}
}
