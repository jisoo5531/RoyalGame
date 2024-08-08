using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KingTower : Tower
{
    public GameObject cannon;
    public GameObject[] princessTowers;

    private void Awake()
    {
        maxHP = 3500;
        HP = maxHP;
        isNotOnCannon = true;

        onTopUnit.GetComponent<UnitCanvasInfo>().maxHP = maxHP;
        onTopUnit.GetComponent<UnitCanvasInfo>().HP = HP;
    }

    private void Update()
    {
        if(isNotOnCannon || princessTowers.Length < 2)
        {
            //photonView.RPC("AppearCannon", RpcTarget.All);
            isNotOnCannon = false;
        }
    }

    [PunRPC]
    private void AppearCannon()
    {
        cannon.SetActive(true);

        cannon.GetComponent<Animator>().SetBool("isAppear", true);

    }
}
