using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IsMineManager : MonoBehaviourPunCallbacks
{
    public static IsMineManager instance;
    public GameObject[] tower;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if (photonView.IsMine)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                tower = GameManager.instance.myTowers.ToArray();
                SettingAlly();
                DetectEnemyManager.instance.towerList = GameManager.instance.enemyTowers.ToList();
            }
            else
            {
                tower = GameManager.instance.enemyTowers.ToArray();
                SettingEnemy();
                DetectEnemyManager.instance.towerList = GameManager.instance.myTowers.ToList();
            }
            GameManager.instance.grid.CreateGrid();
            GameObject[] slots = GameObject.FindGameObjectsWithTag("Spawn");
            for (int i = 0; i < slots.Length; i++)
            {
                var slot = slots[i].GetComponent<SpawnSlot>();
                slot.isMineManager = this;
            }
        }
    }

    private void SettingAlly()
    {
        for (int i = 0; i < tower.Length; i++)
        {
            tower[i].layer = 6;

            foreach (Transform child in tower[i].transform)
            {
                child.gameObject.layer = 6;
            }
        }

        for (int i = 0; i < GameManager.instance.allyTowerMaterial.Length; i++)
        {
            GameManager.instance.allyTowerMaterial[i].material = GameManager.instance.allyMaterial[0];
        }

        for (int i = 0; i < GameManager.instance.allyUnitMaterial.Length; i++)
        {
            GameManager.instance.allyUnitMaterial[i].material = GameManager.instance.allyMaterial[1];
        }
    }

    //public void 

    private void SettingEnemy()
    {
        for (int i = 0; i < tower.Length; i++)
        {
            tower[i].layer = 6;

            foreach (Transform child in tower[i].transform)
            {
                child.gameObject.layer = 6;
            }
        }

        for (int i = 0; i < GameManager.instance.enemyTowerMaterial.Length; i++)
        {
            GameManager.instance.enemyTowerMaterial[i].material = GameManager.instance.allyMaterial[0];
        }

        for (int i = 0; i < GameManager.instance.enemyUnitMaterial.Length; i++)
        {
            GameManager.instance.enemyUnitMaterial[i].material = GameManager.instance.allyMaterial[1];
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
}
