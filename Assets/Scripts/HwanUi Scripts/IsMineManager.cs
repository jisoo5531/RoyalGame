using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Reflection;

public class IsMineManager : MonoBehaviourPunCallbacks
{
    public static IsMineManager instance;
    private bool isCrate = false;
    private GameObject[] towers;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        towers = new GameObject[GameManager.instance.enemyTowers.Length];

        if (PhotonNetwork.IsConnected && photonView.IsMine)
        {
            CreateTower();
        }

        GameManager.instance.grid.CreateGrid();
        GameObject[] slots = GameObject.FindGameObjectsWithTag("Spawn");
        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i].GetComponent<SpawnSlot>();
            slot.isMineManager = this;
        }
    }

    private void CreateTower()
    {
        Transform[] towerPos;
        if (PhotonNetwork.IsMasterClient)
        {
            towerPos = GameManager.instance.enemyTowersTranform;
            for (int i = 0; i < GameManager.instance.enemyTowers.Length; i++)
            {
                string name = GameManager.instance.enemyTowers[i].name;
                GameObject tower = PhotonNetwork.Instantiate(name, towerPos[i].position, Quaternion.identity);
                towers[i] = tower;
            }
        }
        else
        {
            towerPos = GameManager.instance.myTowersTranform;
            for (int i = 0; i < GameManager.instance.enemyTowers.Length; i++)
            {
                string name = GameManager.instance.enemyTowers[i].name;
                GameObject tower = PhotonNetwork.Instantiate(name, towerPos[i].position, Quaternion.identity);
                towers[i] = tower;
            }
        }
    }

    private void Update()
    {
        if (photonView.IsMine && !isCrate && towers.Length > 2)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                GameManager.instance.currentAllyTowers = towers;
                SettingAlly();
                DetectEnemyManager.instance.towerList = AddEnemtyTower(GameManager.instance.currentEnemyTowers).ToList();
            }
            else
            {
                GameManager.instance.currentEnemyTowers = towers;
                SettingAlly();
                DetectEnemyManager.instance.towerList = AddEnemtyTower(GameManager.instance.currentAllyTowers).ToList();
            }

            isCrate = true;
        }
    }

    private GameObject[] AddEnemtyTower(GameObject[] towers)
    {
        GameObject[] enemyTowers = GameObject.FindGameObjectsWithTag("EnemyTower");

        towers = enemyTowers;

        return towers;
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
        }

        Transform child = towers[0].transform;

        foreach (Transform mat in child.transform)
        {
            if (mat.TryGetComponent(out Renderer renderer))
            {
                renderer.material = GameManager.instance.allyMaterial[1];
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
