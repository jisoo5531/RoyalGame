using Mysqlx.Crud;
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
        if (!GameManager.instance.isGameEnd && photonView.IsMine && other.gameObject.layer != 11)
        {
            if (rangeType == Range.Melee)
            {
                if (isWait) return;
            }


            if (!other.isTrigger)
            {
                GameObject childObj = null;

                if (other.transform.childCount > 0)
                {
                    childObj = other.transform.GetChild(0).gameObject;
                }

                if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
                {
                    CheckRange(damagable, other.gameObject);
                }
                else if (childObj != null && childObj.TryGetComponent<IDamagable>(out IDamagable childDamagable))
                {
                    CheckRange(childDamagable, childObj);
                }

            }
        }
    }

    private void CheckRange(IDamagable damagable, GameObject other)
    {
        PhotonView targetPV = other.GetComponent<PhotonView>();
        if (targetPV == null) return;

        int targetViewID = targetPV.ViewID;

        if (!damagedTargetsCache.ContainsKey(targetViewID))
        {
            damagedTargetsCache[targetViewID] = damagable;
        }

        if (other.transform.root.gameObject.name.Equals(target.gameObject.name))
        {
            if (rangeType == Range.Ranged)
            {
                photonView.RPC("RPC_SendDamage", RpcTarget.Others, damage, targetViewID);
                photonView.RPC("DestoryGob", RpcTarget.All);
            }
            else
            {
                photonView.RPC("RPC_SendDamage", RpcTarget.Others, damage, targetViewID);
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
            }
        }
    }

    [PunRPC]
    private void DestoryGob()
    {
        Destroy(gameObject);
    }
}
