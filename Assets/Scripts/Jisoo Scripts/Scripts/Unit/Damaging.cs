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

    private void Start()
    {
        pv = gameObject.GetComponent<PhotonView>();

        if (pv != null && pv.IsMine)
        {
            unit = transform.root.GetComponent<Unit>();

            if (unit != null)
            {
                damage = unit.damage;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pv != null && pv.IsMine)
        {
            Debug.Log(other.gameObject.layer);
            Debug.Log($"¹º°¡ ¸Â¾Ò´Ù {other.gameObject.name}");
            Debug.Log($"{gameObject.name} ÀÌ ¹º°¡ ¸ÂÇû´Ù");
            if ((targetLayerMask | (1 << other.gameObject.layer)) != targetLayerMask)
            {
                print("a");
                if (rangeType == Range.Ranged)
                {
                    print("b");
                    pv.RPC("DestoryGob", RpcTarget.All);
                }
                return;
            }

            if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
            {
                print("c");
                if (unitType == Type.Magic)
                {
                    print("d");
                    damagable.GetDamage(damage);
                    //pv.RPC("RPC_SendDamage", RpcTarget.All, damage);

                    pv.RPC("DestoryGob", RpcTarget.All);

                    return;
                }

                if (rangeType == Range.Ranged)
                {
                    print("d");
                    if (other.gameObject.name.Equals(target.gameObject.name))
                    {
                        print("f");
                        damagable.GetDamage(damage);
                        //pv.RPC("RPC_SendDamage", RpcTarget.All, damage);
                        pv.RPC("DestoryGob", RpcTarget.All);
                    }

                    return;
                }
                print(damagable == null);
                damagable?.GetDamage(damage);
                //pv.RPC("RPC_SendDamage", RpcTarget.All, damage);
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
