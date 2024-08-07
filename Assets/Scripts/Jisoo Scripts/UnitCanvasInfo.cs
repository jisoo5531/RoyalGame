using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AllyImagePrefab
{
    public Sprite hpBarFill;
    public Sprite levelSprite;
}


public class UnitCanvasInfo : MonoBehaviourPunCallbacks, IDamagable
{
    public AllyImagePrefab allyPrefab;

    public int HP { get; set; }
    public int maxHP { get; set; }

    public GameObject blastEffect;

    public Canvas unitCanvas;
    public GameObject hpBarOBJ;
    public Image hpBarFill;
    public Image level;
    public TextMeshProUGUI hpBarvalue;
    public Image spawnTimeUI;
    public GameObject clock;
    public TextMeshProUGUI levelValue;

    GameObject effect;

    private bool isHPBarOn = false;
    private float spawnTime;

    public bool isTower;

    private void Start()
    {
        if (photonView.IsMine)
        {
            this.hpBarFill.sprite = allyPrefab.hpBarFill;
            
            if(!isTower)
            {
                spawnTimeUI.color = Color.cyan;
                this.level.sprite = allyPrefab.levelSprite;
                photonView.RPC("SpawnTime", RpcTarget.All);
            }
            Debug.Log(HP);
            unitCanvas.transform.localEulerAngles = Vector3.zero;
        }
    }

    public void UIInit(int level, float time)
    {
        photonView.RPC("RPC_UIINIT", RpcTarget.All, level, time);
    }

    [PunRPC]
    private void RPC_UIINIT(int level, float time)
    {
        levelValue.text = level.ToString();
        spawnTime = time;
    }

    [PunRPC]
    private void SpawnTime()
    {
        StartCoroutine(FillTimer());
    }

    IEnumerator FillTimer()
    {
        float elapsedTime = 0f;
        while (elapsedTime < spawnTime)
        {
            elapsedTime += Time.deltaTime;
            spawnTimeUI.fillAmount = elapsedTime / spawnTime;
            yield return null;
        }
        spawnTimeUI.fillAmount = 1f;
        clock.SetActive(false);
        level.gameObject.SetActive(true);
        if (gameObject.TryGetComponent<MovableUnit>(out MovableUnit mu))
        {
            mu.isWait = false;
        }
    }

    public void GetDamage(int damage)
    {
        this.HP -= damage;
        this.HP = Mathf.Max(HP, 0);
        photonView.RPC("RPC_damage", RpcTarget.All, damage, this.HP, this.maxHP);

        if (HP <= 0)
        {
            photonView.RPC("RPC_Death", RpcTarget.All);

            if (PhotonNetwork.IsMasterClient)
            {
                MasterManager.instance.RemoveUnit(GetComponent<PhotonView>().ViewID);
            }
            else
            {
                NonMasterManager.instance.RemoveUnit(GetComponent<PhotonView>().ViewID);
            }
        }


    }

    [PunRPC]
    private void RPC_damage(int damage, int currentHp, int currentMaxHp)
    {
        if(!isHPBarOn)
        {
            OnHPBar();
        }

        hpBarFill.fillAmount = (float)currentHp / currentMaxHp;
    }

    [PunRPC]
    private void RPC_Death()
    {
        effect = Instantiate(blastEffect, transform.position + new Vector3(0, transform.localScale.y, 0), transform.rotation);
        effect.transform.localScale = transform.localScale;
        Destroy(effect, 1f);
        Destroy(gameObject);
    }

    private void OnHPBar()
    {
        isHPBarOn = true;
        hpBarOBJ.SetActive(true);        
    }
}
