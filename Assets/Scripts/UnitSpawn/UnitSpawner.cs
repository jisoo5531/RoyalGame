using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    #region public º¯¼ö
    public static UnitSpawner Instance { get; private set; }

    public AllCardData selectedUnit;
    public bool isElixirEnough = false;
    public bool spawnComplete = false;
    public Camera mainCamera;
    #endregion

    private bool isMaster = false;

    private void Awake()
    {        
        if (Instance == null)
        {
            Instance = this;
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

            if (!GameManager.Instance.isGameEnd && selectedUnit != null && isElixirEnough && Input.GetMouseButtonDown(0))
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
                                MasterManager.Instance.AddTower(unitObj.GetComponent<PhotonView>().ViewID);
                            }
                            else if (unitObj.TryGetComponent<MovableUnit>(out MovableUnit movable))
                            {
                                MasterManager.Instance.AddUnit(unitObj.GetComponent<PhotonView>().ViewID);
                            }
                        }
                        else
                        {
                            if (unitObj.TryGetComponent<DeffenseTower>(out DeffenseTower deffense))
                            {
                                NonMasterManager.Instance.AddTower(unitObj.GetComponent<PhotonView>().ViewID);
                            }
                            else if (unitObj.TryGetComponent<MovableUnit>(out MovableUnit movable))
                            {
                                NonMasterManager.Instance.AddUnit(unitObj.GetComponent<PhotonView>().ViewID);
                            }
                        }
                        GameManager.Instance.spawnLimits.DisableTower();
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
