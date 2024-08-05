using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviourPunCallbacks, IDamagable
{
    public int HP { get; set; }
    public int maxHP { get; set; }    

    public GameObject onTopUnit;

    public virtual void GetDamage(int damage)
    {
        onTopUnit.GetComponent<UnitCanvasInfo>().GetDamage(damage);

        Debug.Log($"{gameObject.name} ¸Â¾Ò´Ù");

        photonView.RPC("RPC_Death", RpcTarget.All, damage);
    }

    [PunRPC]
    protected void RPC_Death(int damage)
    {
        HP -= damage;

        if (HP <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Debug.Log(EffectManager.instance.deathEffect == null);
        GameObject effect = Instantiate(EffectManager.instance.deathEffect, transform.position + new Vector3(0, 5, 0), transform.rotation);
        effect.transform.localScale = new Vector3(8, 8, 8);
        Destroy(gameObject);
    }
}
