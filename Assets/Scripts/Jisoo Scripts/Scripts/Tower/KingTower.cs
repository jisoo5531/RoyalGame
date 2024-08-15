using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KingTower : Tower
{
    public GameObject cannon;
    public List<GameObject> princessTowers = new List<GameObject>();

    private void Awake()
    {
        if (photonView.IsMine)
        {
            InitData();
            deffenseUnit = GetComponentInChildren<DeffenseUnit>();
            onTopUnit.GetComponent<UnitCanvasInfo>().maxHP = maxHP;
            onTopUnit.GetComponent<UnitCanvasInfo>().HP = HP;
            deffenseUnit.InitData(range, damage);
        }
    }
    protected override void InitData()
    {
        maxHP = 3500;
        HP = maxHP;
        range = 29;
        damage = 90;
        isNotOnCannon = true;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            if (!GameManager.instance.isGameEnd && isNotOnCannon && princessTowers.Count < 2 || isNotOnCannon && HP < maxHP)
            {
                photonView.RPC("AppearCannon", RpcTarget.All);
                isNotOnCannon = false;
            }
        }
    }

    [PunRPC]
    private void AppearCannon()
    {
        cannon.SetActive(true);

        cannon.GetComponent<Animator>().SetBool("isAppear", true);

    }
}
