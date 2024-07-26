using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class IsMineManager : MonoBehaviourPunCallbacks
{
    public static IsMineManager instance;
    public GameObject[] tower;
    private bool isCrate = false;
    private GameObject[] towers;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        towers = new GameObject[GameManager.instance.enemyTowers.Length];
        if (photonView.IsMine)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < GameManager.instance.enemyTowers.Length; i++)
                {
                    string name = GameManager.instance.enemyTowers[i].name;
                    GameObject tower = PhotonNetwork.Instantiate(name, GameManager.instance.myTowersTranform[i].position, Quaternion.identity);
                    GameManager.instance.currentAllyTowers[i] = tower;
                }

            }
            else
            {
                for (int i = 0; i < GameManager.instance.enemyTowers.Length; i++)
                {
                    string name = GameManager.instance.enemyTowers[i].name;
                    GameObject tower = PhotonNetwork.Instantiate(name, GameManager.instance.enemyTowersTranform[i].position, Quaternion.Euler(0, 180, 0));
                    GameManager.instance.currentAllyTowers[i] = tower;
                }
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

    private void Update()
    {
        if (!isCrate && tower.Length < 3)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                tower = GameManager.instance.currentAllyTowers.ToArray();
                SettingAlly();
                DetectEnemyManager.instance.towerList = GameManager.instance.currentEnemyTowers.ToList();
            }
            else
            {
                tower = GameManager.instance.currentEnemyTowers.ToArray();
                SettingEnemy();
                DetectEnemyManager.instance.towerList = GameManager.instance.currentAllyTowers.ToList();
            }
            int targetLayer = LayerMask.NameToLayer("target");
            GameObject[] targets = FindObjectsInLayer(targetLayer);
            if (!PhotonNetwork.IsMasterClient)
            {
                GameManager.instance.currentEnemyTowers = targets;
            }
            else
            {
                GameManager.instance.currentAllyTowers = targets;
            }
            isCrate = true;
        }
    }

    private GameObject[] FindObjectsInLayer(int layer)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true);
        var filteredObjects = new List<GameObject>();

        foreach (var obj in allObjects)
        {
            if (obj.layer == layer)
            {
                filteredObjects.Add(obj);
            }
        }

        return filteredObjects.ToArray();
    }

    private void SettingAlly()
    {
        for (int i = 0; i < tower.Length; i++)
        {
            tower[i].layer = 6;

            foreach (Transform towerChild in tower[i].transform)
            {
                towerChild.gameObject.layer = 6;
            }
        }

        for (int i = 0; i < tower.Length; i++)
        {
            tower[i].GetComponent<MeshRenderer>().material = GameManager.instance.allyMaterial[0];
        }

        Transform child = tower[0].transform;

        foreach (Transform mat in child.transform)
        {
            if (mat.TryGetComponent<Renderer>(out Renderer renderer))
            {
                renderer.material = GameManager.instance.allyMaterial[1];
            }
        }
    }

    private void SettingEnemy()
    {
        for (int i = 0; i < tower.Length; i++)
        {
            tower[i].layer = 6;

            foreach (Transform towerChild in tower[i].transform)
            {
                towerChild.gameObject.layer = 6;
            }
        }

        for (int i = 0; i < tower.Length; i++)
        {
            tower[i].GetComponent<MeshRenderer>().material = GameManager.instance.allyMaterial[0];
        }

        Transform child = tower[0].transform;

        foreach (Transform mat in child.transform)
        {
            if (mat.TryGetComponent<Renderer>(out Renderer renderer))
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
