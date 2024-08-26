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
        if (other.transform.root.gameObject.name.Equals(target.gameObject.name))
        {
            damagable?.GetDamage(damage);
            if (rangeType == Range.Ranged)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }
}
