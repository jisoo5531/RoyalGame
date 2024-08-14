using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NonMasterManager : MonoBehaviourPunCallbacks
{
    public static NonMasterManager instance;
    private bool isCrate = false;
    public List<GameObject> towers = new List<GameObject>();
    KingTower kingTower;

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
        if (photonView.IsMine)
        {
            if (PhotonNetwork.IsConnected)
            {
                CreateTower();
            }
        }
    }

    private void CreateTower()
    {
        Transform[] towerPos = GameManager.instance.myTowersTranform;
        for (int i = 0; i < GameManager.instance.enemyTowers.Length; i++)
        {
            string name = GameManager.instance.enemyTowers[i].name;
            GameObject tower = PhotonNetwork.Instantiate(name, towerPos[i].position, Quaternion.identity);
            tower.GetComponentInChildren<UnitCanvasInfo>().unitCanvas.transform.position = GameManager.instance.enemyTowerHp[i].position;
            towers.Add(tower);

        }
        photonView.RPC("AddEnemyTower", RpcTarget.Others);
    }

    private void Update()
    {
        if (photonView.IsMine && !isCrate && towers[1] != null)
        {
            kingTower = towers[0].GetComponent<KingTower>();
            for (int i = 1; i < towers.Count; i++)
            {
                kingTower.princessTowers.Add(towers[i]);
            }
            SettingAlly();
            GameManager.instance.grid.CreateGrid();
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
        for (int i = 0; i < towers.Count; i++)
        {
            towers[i].layer = 6;
            towers[i].tag = "AllyTower";

            foreach (Transform towerChild in towers[i].transform)
            {
                towerChild.gameObject.layer = 6;
            }
        }

        for (int i = 0; i < towers.Count; i++)
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
    public void AddTower(int id)
    {
        photonView.RPC("OnTowerCreated", RpcTarget.Others, id);
    }

    [PunRPC]
    public void OnTowerCreated(int viewID)
    {
        PhotonView towerView = PhotonView.Find(viewID);
        if (towerView != null)
        {
            GameObject tower = towerView.gameObject;
            DetectEnemyManager.instance.towerList.Add(tower);
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

    public void RemoveTower(int id, GameObject tower)
    {
        towers.Remove(tower);
        if(kingTower.princessTowers.Contains(tower))
        {
            kingTower.princessTowers.Remove(tower);
        }
        photonView.RPC("OnTowerRemove", RpcTarget.Others, id);
    }

    [PunRPC]
    public void OnTowerRemove(int viewID)
    {
        PhotonView towerView = PhotonView.Find(viewID);
        if (towerView != null)
        {
            GameObject tower = towerView.gameObject;
            DetectEnemyManager.instance.towerList.Remove(tower);
        }
    }
}
