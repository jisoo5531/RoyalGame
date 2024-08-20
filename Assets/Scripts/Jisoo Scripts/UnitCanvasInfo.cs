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

    #region public 변수
    public AllyImagePrefab allyPrefab;

    public int HP { get; set; }
    public int maxHP { get; set; }

    public GameObject blastEffect;

    public GameObject unitCanvas;
    public GameObject hpBarOBJ;
    public Image hpBarFill;
    public Image level;
    public TextMeshProUGUI hpBarvalue;
    public Image spawnTimeUI;
    public GameObject clock;
    public TextMeshProUGUI levelValue;

    public bool isTower;
    public bool isKingTower;
    public bool isPrince;

    public int objIndex;
    #endregion

    #region private 변수
    Tower tower;
    GameObject effect;
    private bool isHPBarOn = false;
    private float spawnTime;
    #endregion

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
            if(isKingTower)
            {
                tower = this.transform.parent.GetComponent<Tower>();
            }
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
        else if(gameObject.TryGetComponent<DeffenseTower>(out DeffenseTower tower))
        {
            tower.isWait = false;
        }
    }

    public void GetDamage(int damage)
    {
        if(this == null) return;

        this.HP -= damage;
        this.HP = Mathf.Max(HP, 0);
        photonView.RPC("RPC_damage", RpcTarget.All, damage, this.HP, this.maxHP);

        if (HP <= 0 && photonView.IsMine)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (isTower)
                {
                    int id = gameObject.transform.root.GetComponent<PhotonView>().ViewID;
                    MasterManager.instance.RemoveTower(id, this.transform.parent.gameObject);
                }
                else
                {
                    MasterManager.instance.RemoveUnit(GetComponent<PhotonView>().ViewID);
                }
            }
            else
            {
                if (isTower)
                {
                    int id = gameObject.transform.root.GetComponent<PhotonView>().ViewID;
                    NonMasterManager.instance.RemoveTower(id, this.transform.parent.gameObject);
                }
                else
                {
                    NonMasterManager.instance.RemoveUnit(GetComponent<PhotonView>().ViewID);
                }
            }
            Death(isTower);
        }
    }

    [PunRPC]
    private void RPC_damage(int damage, int currentHp, int currentMaxHp)
    {
        if (this == null) return;

        if (isTower)
        {
            if(isKingTower && tower != null && tower.isNotOnCannon)
            {
                tower.isNotOnCannon = false;
            }
        }
        else
        {
            if (!isHPBarOn)
            {
                OnHPBar();
            }
        }

        hpBarFill.fillAmount = (float)currentHp / currentMaxHp;
        if (isTower)
        {
            hpBarvalue.text = currentHp.ToString();
        }
    }

    private void Death(bool isTower)
    {
        if (isTower)
        {
            GameManager.instance.spawnLimits.DisableIndexTowerLimit(objIndex);
            effect =  PhotonNetwork.Instantiate(blastEffect.name, transform.position + new Vector3(0, 5, 0), transform.rotation);
            effect.transform.localScale = new Vector3(8, 8, 8);
            PhotonNetwork.Destroy(gameObject.transform.root.gameObject);
        }
        else
        {
            effect = PhotonNetwork.Instantiate(blastEffect.name, transform.position + new Vector3(0, transform.localScale.y, 0), transform.rotation);
            effect.transform.localScale = transform.localScale;
            PhotonNetwork.Destroy(gameObject);
        }
        Invoke("DestroyEffect", 1f);
    }

    public void DestroyEffect()
    {
        PhotonNetwork.Destroy(effect);
    }

    private void EnableTowerLimit()
    {
        GameManager.instance.spawnLimits.EnableTowerLimit();
    }

    private void OnHPBar()
    {
        isHPBarOn = true;
        hpBarOBJ.SetActive(true);        
    }
}
