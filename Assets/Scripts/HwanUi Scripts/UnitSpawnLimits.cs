using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnLimits : MonoBehaviour
{
    public GameObject masterLimits;
    public GameObject nonMasterLimits;

    public GameObject limitObj;

    private void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            limitObj = masterLimits;
        }
        else
        {
            limitObj = nonMasterLimits;
        }
    }

    public void DisableIndexTowerLimit(int index)
    {
        limitObj.transform.GetChild(index).gameObject.SetActive(false);
    }

    public void EnableTowerLimit()
    {
        limitObj.SetActive(true);
    }

    public void TowerDestroyEnable()
    {
        limitObj.SetActive(true);
        Invoke("DisableTower", 2f);
    }

    public void DisableTower()
    {
        limitObj.SetActive(false);
    }
}
