using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Reflection;

public class MasterManager : MonoBehaviourPunCallbacks
{
    public static MasterManager instance;
    private bool isCrate = false;
    public GameObject[] towers;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
    private void Start()
    {
        towers = new GameObject[3];

        if (photonView.IsMine)
        {
            if (PhotonNetwork.IsConnected)
            {
                CreateTower();
            }

            GameManager.instance.grid.CreateGrid();
        }
    }

    private void CreateTower()
    {
        Transform[] towerPos = GameManager.instance.enemyTowersTranform;
        for (int i = 0; i < GameManager.instance.enemyTowers.Length; i++)
        {
            string name = GameManager.instance.enemyTowers[i].name;
            GameObject tower = PhotonNetwork.Instantiate(name, towerPos[i].position, Quaternion.Euler(0, 180, 0));
            tower.GetComponentInChildren<UnitCanvasInfo>().unitCanvas.transform.position = GameManager.instance.allyTowerHp[i].position;
            towers[i] = tower;
        }
    }

    private void Update()
    {
        if (photonView.IsMine && !isCrate && towers[1] != null)
        {
            GameManager.instance.currentAllyTowers = towers;
            SettingAlly();
            photonView.RPC("AddEnemyTower", RpcTarget.Others);
            isCrate = true;
        }
    }

    [PunRPC]
    private void AddEnemyTower()
    {
        GameManager.instance.currentEnemyTowers = GameObject.FindGameObjectsWithTag("EnemyTower");
        DetectEnemyManager.instance.towerList = GameManager.instance.currentEnemyTowers.ToList();
    }


    private void SettingAlly()
    {
        for (int i = 0; i < towers.Length; i++)
        {
            towers[i].layer = 6;
            towers[i].tag = "AllyTower";

            foreach (Transform towerChild in towers[i].transform)
            {
                towerChild.gameObject.layer = 6;
            }
        }

        for (int i = 0; i < towers.Length; i++)
        {
            towers[i].GetComponent<MeshRenderer>().material = GameManager.instance.allyMaterial[0];

            Renderer[] child = towers[i].transform.GetChild(0).GetComponentsInChildren<Renderer>();

            foreach (Renderer mat in child)
            {
                mat.material = GameManager.instance.allyMaterial[1];
            }
        }
    }

    public void AddUnit(int id)
    {
        photonView.RPC("OnCharacterCreated", RpcTarget.Others, id);
    }


    [PunRPC]
    public void OnCharacterCreated(int viewID)
    {
        PhotonView characterView = PhotonView.Find(viewID);
        if (characterView != null)
        {
            GameObject character = characterView.gameObject;
            DetectEnemyManager.instance.enemyList.Add(character);
        }
    }


    public void RemoveUnit(int id)
    {
        photonView.RPC("OnCharacterRemove", RpcTarget.Others, id);
    }

    [PunRPC]
    public void OnCharacterRemove(int viewID)
    {
        PhotonView characterView = PhotonView.Find(viewID);
        if (characterView != null)
        {
            GameObject character = characterView.gameObject;
            DetectEnemyManager.instance.enemyList.Remove(character);
        }
    }

    //public void DestroyUnit(int id)
    //{
    //    PhotonView characterView = PhotonView.Find(id);
    //    if (characterView != null)
    //    {
    //        GameObject character = characterView.gameObject;
    //        DetectEnemyManager.instance.enemyList.Remove(character);
    //    }
    //}


    //[PunRPC]
    //private void UnitDestroy(Vector3 trans)
    //{
    //    effect.transform.localScale = trans;
    //    Destroy(effect, 1f);
    //    Destroy(gameObject);
    //}
}
