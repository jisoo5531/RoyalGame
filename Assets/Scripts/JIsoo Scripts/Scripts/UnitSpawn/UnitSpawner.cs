using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public static UnitSpawner instance { get; private set; }

    public AllCardData selectedUnit;
    public bool isElixirEnough = false;
    public bool spawnComplete = false;

    public Camera mainCamera;

    private bool isMaster = false;

    private void Awake()
    {        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        isMaster = PhotonNetwork.IsMasterClient;

        StartCoroutine(ClickSpawnUnit());
    }
    IEnumerator ClickSpawnUnit()
    {
        while (true)
        {
            yield return null;

            if (selectedUnit != null && isElixirEnough && Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Floor"))
                    {
                        string unitName = selectedUnit.prefab.name;
                        GameObject unitObj = AllySpawnManager.Instance.InitCreateUnit(unitName, hit.point, selectedUnit);
                        if (isMaster)
                        {
                            if (unitObj.TryGetComponent<DeffenseTower>(out DeffenseTower deffense))
                            {
                                MasterManager.instance.AddTower(unitObj.GetComponent<PhotonView>().ViewID);
                            }
                            else if (unitObj.TryGetComponent<MovableUnit>(out MovableUnit movable))
                            {
                                MasterManager.instance.AddUnit(unitObj.GetComponent<PhotonView>().ViewID);
                            }
                        }
                        else
                        {
                            if (unitObj.TryGetComponent<DeffenseTower>(out DeffenseTower deffense))
                            {
                                NonMasterManager.instance.AddTower(unitObj.GetComponent<PhotonView>().ViewID);
                            }
                            else if (unitObj.TryGetComponent<MovableUnit>(out MovableUnit movable))
                            {
                                NonMasterManager.instance.AddUnit(unitObj.GetComponent<PhotonView>().ViewID);
                            }
                        }
                        spawnComplete = true;
                        selectedUnit = null;
                        isElixirEnough = false;
                    }
                }
            }
        }
    }

    public void SelectUnit(AllCardData unit)
    {        
        selectedUnit = unit;
    }
}
