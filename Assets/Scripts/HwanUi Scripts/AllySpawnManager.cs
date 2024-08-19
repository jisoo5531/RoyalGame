using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllySpawnManager : MonoBehaviour
{
    public static AllySpawnManager Instance { get; private set; }

    public Transform allyFireballPos;
    public Transform enemyFireballPos;

    private float spawntime = 0f;

    private void Awake()
    {
        Instance = this;
    }
    public GameObject InitCreateUnit(string name, Vector3 spawnTrans, AllCardData ud, GameObject unitObj = null)
    {
        GameObject unit = null;
        if (ud.cardId != 3)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                unit = PhotonNetwork.Instantiate(name, spawnTrans, Quaternion.Euler(0, 180, 0));
            }
            else
            {
                unit = PhotonNetwork.Instantiate(name, spawnTrans, Quaternion.identity);
            }
            Renderer[] renderers = unit.GetComponentsInChildren<Renderer>();
            RendererChange(renderers);

            if (unit.TryGetComponent<UnitCanvasInfo>(out UnitCanvasInfo uci))
            {
                if (ud is UnitInfoData unitData)
                {
                    spawntime = unitData.spawnTime;
                }
                else if (ud is DEFENSETOWERInfoData towerData)
                {
                    spawntime = towerData.spawnTime;
                }
                uci.UIInit(ud.level, spawntime);
            }
        }
        else
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                unit = PhotonNetwork.Instantiate(name, enemyFireballPos.position, Quaternion.identity);
            }
            else
            {
                unit = PhotonNetwork.Instantiate(name, allyFireballPos.position, Quaternion.identity);
            }
            unit.transform.GetChild(0).GetComponent<Fireball>().targetPos = spawnTrans;
        }

        unit.UnitClassification(ud);

        if (ud.cardId != 3)
        {
            unit.layer = 11;
            foreach (Transform child in unit.transform)
            {
                child.gameObject.layer = 11;
            }
        }
        else
        {
            unit.layer = 13;
            foreach (Transform child in unit.transform)
            {
                child.gameObject.layer = 13;
            }
        }

        Destroy(unitObj);

        if (ud is UnitInfoData unitInfo)
        {
            if (unit.TryGetComponent<MovableUnit>(out MovableUnit mu))
            {
                mu.moveDelay = unitInfo.spawnTime;
                mu.isSpawn = true;
            }
        }
        else if (ud is DEFENSETOWERInfoData defenseTowerInfo)
        {
            if (unit.TryGetComponent<DeffenseTower>(out DeffenseTower dt))
            {
                dt.isSpawn = true;
            }
        }
        return unit;
    }

    private void RendererChange(Renderer[] renderers)
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = GameManager.instance.allyMaterial[1];
        }
    }
}
