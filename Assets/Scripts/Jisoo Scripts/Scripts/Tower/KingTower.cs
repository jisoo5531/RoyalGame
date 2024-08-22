using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class KingTower : Tower
{
    public GameObject cannon;
    public List<GameObject> princessTowers = new List<GameObject>();

    public UnitCanvasInfo canvasInfo;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            InitData();
            deffenseUnit = cannon.GetComponent<DeffenseUnit>();
            canvasInfo = onTopUnit.GetComponent<UnitCanvasInfo>();
            onTopUnit.GetComponent<UnitCanvasInfo>().maxHP = maxHP;
            onTopUnit.GetComponent<UnitCanvasInfo>().HP = HP;
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
            if (!GameManager.instance.isGameEnd && isNotOnCannon && princessTowers.Count < 2 || !GameManager.instance.isGameEnd && isNotOnCannon && canvasInfo.HP < canvasInfo.maxHP)
            {
                photonView.RPC("AppearCannon", RpcTarget.All);
                deffenseUnit.ComponentInit();
                deffenseUnit.InitData(range, damage);
                isNotOnCannon = false;
            }
        }
    }

    [PunRPC]
    private void AppearCannon()
    {
        cannon.SetActive(true);
    }
}
