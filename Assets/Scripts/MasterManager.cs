using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MasterManager : MonoBehaviourPunCallbacks
{
    public static MasterManager Instance;
    private bool isCrate = false;
    public List<GameObject> towers = new List<GameObject>();
    KingTower kingTower;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            if (Instance == null)
            {
                Instance = this;
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
        Transform[] towerPos = GameManager.Instance.enemyTowersTranform;
        for (int i = 0; i < GameManager.Instance.enemyTowers.Length; i++)
        {
            string name = GameManager.Instance.enemyTowers[i].name;
            GameObject tower = PhotonNetwork.Instantiate(name, towerPos[i].position, Quaternion.Euler(0, 180, 0));
            UnitCanvasInfo unitCanvas = tower.GetComponentInChildren<UnitCanvasInfo>();
            unitCanvas.unitCanvas.transform.position = GameManager.Instance.allyTowerHp[i].position;
            if (i != 0)
            {
                unitCanvas.unitCanvas.transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                unitCanvas.unitCanvas.transform.localEulerAngles = Vector3.zero;
            }
            unitCanvas.objIndex = i;
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
            GameManager.Instance.grid.CreateGrid();
            isCrate = true;
        }
    }

    [PunRPC]
    private void AddEnemyTower()
    {
        GameManager.Instance.currentEnemyTowers = GameObject.FindGameObjectsWithTag("EnemyTower");
        DetectEnemyManager.Instance.towerList = GameManager.Instance.currentEnemyTowers.ToList();
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
            towers[i].GetComponent<MeshRenderer>().material = GameManager.Instance.allyMaterial[0];

            Renderer[] child = towers[i].transform.GetChild(0).GetComponentsInChildren<Renderer>();

            foreach (Renderer mat in child)
            {
                mat.material = GameManager.Instance.allyMaterial[1];
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
            DetectEnemyManager.Instance.enemyList.Add(character);
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
            DetectEnemyManager.Instance.towerList.Add(tower);
        }
    }

    public void RemoveTower(GameObject tower)
    {
        towers.Remove(tower);
        if (kingTower.princessTowers.Contains(tower))
        {
            kingTower.princessTowers.Remove(tower);
        }
        ScoreManager.Instance.enemyCount++;
        ScoreManager.Instance.SettingScore();
    }
}
