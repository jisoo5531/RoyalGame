using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaging : MonoBehaviourPunCallbacks
{
    public int damage;

    public Transform target;
    public Range rangeType;
    public bool isWait = true;
    private Dictionary<int, IDamagable> damagedTargetsCache = new Dictionary<int, IDamagable>();

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine && other.gameObject.layer != 11)
        {
            if(rangeType == Range.Melee)
            {
                if (isWait) return;
            }

            if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                PhotonView targetPV = other.transform.root.GetComponent<PhotonView>();
                int targetViewID = targetPV.ViewID;

                if (!damagedTargetsCache.ContainsKey(targetViewID))
                {
                    damagedTargetsCache[targetViewID] = damagable;
                }

                if (rangeType == Range.Ranged)
                {
                    if (other.gameObject.name.Equals(target.gameObject.name))
                    {
                        photonView.RPC("RPC_SendDamage", RpcTarget.Others, damage, targetViewID);
                        photonView.RPC("DestoryGob", RpcTarget.All);
                    }

                    return;
                }

                if (damagedTargetsCache.TryGetValue(targetViewID, out IDamagable cachedDamagable))
                {
                    if (other.gameObject.name.Equals(target.gameObject.name))
                    {
                        photonView.RPC("RPC_SendDamage", RpcTarget.Others, damage, targetViewID);
                    }
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
                    targetDamagable?.GetDamage(damage);
                }
                else
                {
                    targetDamagable = targetPV.GetComponentInChildren<IDamagable>();

                    if(targetDamagable != null)
                    {
                        damagedTargetsCache[targetViewID] = targetDamagable;
                        targetDamagable?.GetDamage(damage);
                    }
                }
            }
        }
    }

    [PunRPC]
    private void DestoryGob()
    {
        Destroy(gameObject);
    }
}
